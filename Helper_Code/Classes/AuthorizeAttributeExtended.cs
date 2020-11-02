using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebApiOauth2.DAL.Utils;

namespace WebApiOauth2.Helper_Code.Classes
{
    public class AuthorizeAttributeExtended : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var tokenHasExpired = false;
            var owinContext = OwinHttpRequestMessageExtensions.GetOwinContext(actionContext.Request);
            if (owinContext != null)
            {
                tokenHasExpired = owinContext.Environment.ContainsKey("oauth.token_expired");
            }
            string authtokenKey = "";
            if (owinContext.Request.Headers.ContainsKey("Authorization"))
            {
                authtokenKey = owinContext.Request.Headers.Get("Authorization").Replace("Bearer ", "");
            }
            
            if (tokenHasExpired)
            {
                string requestPath = owinContext.Request.Environment["owin.RequestPath"].ToString();
                string[] pathComponents = requestPath.Split('/');

                new BusinessLogic().CreateLog(pathComponents[pathComponents.Length - 1], pathComponents[pathComponents.Length - 1], "0", "webapi", "Token Expired", "0", requestPath, authtokenKey, Constants.GetConnectionString());
                actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                    new
                    {
                        error = "invalid_token",
                        error_message = "The Token has expired"
                    });
            }
            else
            {
                actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                    new
                    {
                        error = "invalid_request",
                        error_message = "The Token is invalid"
                    });
            }
        }
    }

    public class AuthenticationFailureMessage : HttpResponseMessage
    {
        public AuthenticationFailureMessage(string reasonPhrase, HttpRequestMessage request, object responseMessage)
            : base(HttpStatusCode.Unauthorized)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

            Content = new ObjectContent<object>(responseMessage, jsonFormatter);
            RequestMessage = request;
            ReasonPhrase = reasonPhrase;
        }
    }
}