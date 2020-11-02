//-----------------------------------------------------------------------
// <copyright file="AppOAuthProvider.cs" company="None">
//     Copyright (c) Allow to distribute this code.
// </copyright>
// <author>Asma Khalid</author>
//-----------------------------------------------------------------------

namespace WebApiOauth2.Helper_Code.OAuth2
{
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using WebApiOauth2.Helper_Code.Classes;
    using WebApiOauth2.DAL.CallingMethods;
    using WebApiOauth2.ENT;
    using System.Globalization;
    using WebApiOauth2.Models;
    using WebApiOauth2.Controllers;
    using WebApiOauth2.DAL.Utils;
    using Newtonsoft.Json;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Application OAUTH Provider class.
    /// </summary>
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {
        #region Private Properties

        /// <summary>
        /// Public client ID property.
        /// </summary>
        private readonly string _publicClientId;

        /// <summary>
        /// Database Store property.
        /// </summary>
        

        #endregion

        #region Default Constructor method.

        /// <summary>
        /// Default Constructor method.
        /// </summary>
        /// <param name="publicClientId">Public client ID parameter</param>
        public AppOAuthProvider(string publicClientId)
        {
            //TODO: Pull from configuration
            if (publicClientId == null)
            {
                throw new ArgumentNullException(NameOf.nameof(() => publicClientId));
            }

            // Settings.
            _publicClientId = publicClientId;
        }

        #endregion

        #region Grant resource owner credentials override method.

        /// <summary>
        /// Grant resource owner credentials overload method.
        /// </summary>
        /// <param name="context">Context parameter</param>
        /// <returns>Returns when task is completed</returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Initialization.
            string usernameVal = (String.IsNullOrEmpty(context.UserName)) ? "" : context.UserName;
            string passwordVal = (String.IsNullOrEmpty(context.Password)) ? "" : context.Password;
            //var user = this.databaseManager.LoginByUsernamePassword(usernameVal, passwordVal).ToList();

            IFormCollection parameters = await context.Request.ReadFormAsync();
            string deviceDetails = parameters.Get("DeviceDetails");
            deviceDetails = (String.IsNullOrEmpty(deviceDetails)) ? "" : deviceDetails;
            string deviceUDID = parameters.Get("DeviceUDID");
            deviceUDID = (String.IsNullOrEmpty(deviceUDID)) ? "" : deviceUDID;
            string deviceTYPE = parameters.Get("DeviceTYPE");
            deviceTYPE = (String.IsNullOrEmpty(deviceTYPE)) ? "" : deviceTYPE;
            string mobileDatetime = parameters.Get("MobileDateTime");
            mobileDatetime = (String.IsNullOrEmpty(mobileDatetime)) ? "" : mobileDatetime;
            string fcmToken = parameters.Get("FcmToken");
            fcmToken = (String.IsNullOrEmpty(fcmToken)) ? "" : fcmToken;

            string serviceTYPE = parameters.Get("ServiceTYPE");
            serviceTYPE = (String.IsNullOrEmpty(serviceTYPE)) ? "" : serviceTYPE;

            Users userobject = new Users();
            userobject.userName = usernameVal;
            userobject.passWord = passwordVal;
            userobject.deviceDetails = deviceDetails;
            userobject.deviceUDID = deviceUDID;
            userobject.deviceTYPE = deviceTYPE;
            userobject.mobileDatetime = mobileDatetime;
            userobject.fcmToken = fcmToken;
            userobject.serviceTYPE = serviceTYPE;

            string userID = "";
            string isLead = "";
            string teamIDs = "";

            JsonStandardResponse sendOtpResponse = null;

            if (userobject.serviceTYPE.ToLower() == "login")
            {

                /*if (userobject.userName != "admin" || userobject.passWord != "admin")
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }*/
                if (userobject.passWord.Length < 8)
                {
                    context.SetError("invalid_grant", "Password length must be must be equal or greater than 8 characters.");
                    return;
                }

                DateTime dateTime;
                try
                {
                    dateTime = DateTime.ParseExact(userobject.mobileDatetime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    context.SetError("invalid_grant", "invalid datetime format required is MM-dd-yyyy HH:mm:ss");
                    return;
                }

                //Login from credentials
                //Verification.
                //string Status = new UserProfile().VerifyUserCredentials(userobject, Constants.GetConnectionString());
                string Status = new UserProfile().VerifyUserCredentialsFromAD(userobject, Constants.GetConnectionString());
                if (Status != "Success")
                {
                    context.SetError("invalid_grant", Status);
                    return;
                }

                new UserProfile().InsertUserFcmToken(userobject, Constants.GetConnectionString());

                userobject.passWord = "";

            }
            else if (userobject.serviceTYPE.ToLower() == "adminlogin")
            {

                /*if (userobject.userName != "admin" || userobject.passWord != "admin")
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }*/
                if (userobject.passWord.Length < 8)
                {
                    context.SetError("invalid_grant", "Password length must be must be equal or greater than 8 characters.");
                    return;
                }

                DateTime dateTime;
                try
                {
                    dateTime = DateTime.ParseExact(userobject.mobileDatetime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    context.SetError("invalid_grant", "invalid datetime format required is MM-dd-yyyy HH:mm:ss");
                    return;
                }

                //Login from credentials
                //Verification.
                string Status = new UserProfile().VerifyUserCredentials(userobject, Constants.GetConnectionString());
                //string Status = new UserProfile().VerifyUserCredentialsFromAD(userobject, Constants.GetConnectionString());
                if (Status != "Success")
                {
                    context.SetError("invalid_grant", Status);
                    return;
                }

                new UserProfile().InsertUserFcmToken(userobject, Constants.GetConnectionString());

                userobject.passWord = "";

            }
            else if (userobject.serviceTYPE.ToLower() == "refreshtoken")
            {
                //Checks DeviceUDID whether it is logged in or not
                if (userobject.deviceUDID == "" || userobject.userName == "")
                {
                    context.SetError("invalid_grant", "device udid or user name cannot be empty!");
                    return;
                }

                //Login Status Verification.
                Users obj = new UserProfile().checkUserLoginStatus(userobject, Constants.GetConnectionString());
                if (obj == null)
                {
                    context.SetError("invalid_grant", "no session found against device udid and user name!");
                    return;
                }
            }
            else
            {
                context.SetError("invalid_grant", "Invalid Request!");
                return;
            }

            Users userobj = new UserProfile().checkUserLoginStatus(userobject, Constants.GetConnectionString());
            if (userobj != null)
            {
                userobject.ID = userobj.ID;
                Users obj = new UserProfile().getUserByUserNameAndUserID(userobject, Constants.GetConnectionString());
                userID = obj.ID;
                isLead = obj.isLead;
                //teamDetailsJson = new JavaScriptSerializer().Serialize(obj.teams);
                teamIDs = string.Join(",", obj.teams.Select(x => x.ID).ToArray());
            }

            var claims = new List<Claim>();
            //claims.Add(new Claim("serviceTYPE", userobject.serviceTYPE.ToLower()));
            //claims.Add(new Claim("userName", usernameVal));
            IDictionary<string, string> data = new Dictionary<string, string>
                                               {
                                                   { "serviceTYPE", userobject.serviceTYPE.ToLower()},
                                                   { "userName", usernameVal },
                                                   { "userID", userID },
                                                   { "isLead", isLead },
                                                   { "teamIDs", teamIDs }
                                                   //{ "UserDetails", JsonConvert.SerializeObject(new UserProfile().checkUserLoginStatus(userobject, Constants.GetConnectionString()))}
                                               };
            
            // Setting Claim Identities for OAUTH 2 protocol.
            ClaimsIdentity oAuthClaimIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesClaimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthClaimIdentity, new AuthenticationProperties(data));
            // Grant access to authorize user.
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesClaimIdentity);
        }

        #endregion

        #region Token endpoint override method.

        /// <summary>
        /// Token endpoint override method
        /// </summary>
        /// <param name="context">Context parameter</param>
        /// <returns>Returns when task is completed</returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                // Adding.
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            // Return info.
            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            //context.AccessToken
            string username = context.AdditionalResponseParameters["userName"].ToString();
            string authenticationkey = context.AccessToken;
            string issuedDateTime = context.AdditionalResponseParameters[".issued"].ToString();
            string expiredDateTime = context.AdditionalResponseParameters[".expires"].ToString();
            DateTime _issuedDateTime = DateTime.ParseExact(issuedDateTime, "ddd, dd MMM yyyy HH:mm:ss GMT", System.Globalization.CultureInfo.InvariantCulture);
            DateTime _expiredDateTime = DateTime.ParseExact(expiredDateTime, "ddd, dd MMM yyyy HH:mm:ss GMT", System.Globalization.CultureInfo.InvariantCulture);
            //_issuedDateTime.ToString("MM-dd-yyyy HH:mm:ss")
            //log access token

            new UserProfile().InsertUserTokenRegister(username, authenticationkey, _issuedDateTime.ToString("MM-dd-yyyy HH:mm:ss"), _expiredDateTime.ToString("MM-dd-yyyy HH:mm:ss"), Constants.GetConnectionString());

            if(context.AdditionalResponseParameters.ContainsKey("serviceTYPE"))
            {
                string serviceTYPE = context.AdditionalResponseParameters["serviceTYPE"].ToString();
                new BusinessLogic().CreateLog(serviceTYPE, serviceTYPE, "0", "webapi", "", "1", "/AuthenticationTokenService", authenticationkey, Constants.GetConnectionString());
            }
            //context.AdditionalResponseParameters.Add("UserDetailsObject", JsonConvert.DeserializeObject<Users>(context.AdditionalResponseParameters["UserDetails"].ToString()));

            return base.TokenEndpointResponse(context);
        }

        #endregion

        #region Validate Client authntication override method

        /// <summary>
        /// Validate Client authntication override method
        /// </summary>
        /// <param name="context">Contect parameter</param>
        /// <returns>Returns validation of client authentication</returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                // Validate Authoorization.
                context.Validated();
            }

            // Return info.
            return Task.FromResult<object>(null);
        }

        #endregion

        #region Validate client redirect URI override method

        /// <summary>
        /// Validate client redirect URI override method
        /// </summary>
        /// <param name="context">Context parmeter</param>
        /// <returns>Returns validation of client redirect URI</returns>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            // Verification.
            if (context.ClientId == _publicClientId)
            {
                // Initialization.
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                // Verification.
                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    // Validating.
                    context.Validated();
                }
            }

            // Return info.
            return Task.FromResult<object>(null);
        }

        #endregion

        
    }
}