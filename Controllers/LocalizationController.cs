using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.Helper_Code.Classes;
using WebApiOauth2.Models;

namespace WebApiOauth2.Controllers
{
    [RoutePrefix("api/LocalizationController")]
    public class LocalizationController : ApiController
    {
        [HttpGet]
        [Route("ListLocalization")]
        public JsonStandardResponse ListLocalization()
        {
            JsonStandardResponse result = null;
            try
            {
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = new Localization().ListLocalization(Constants.GetConnectionString()),
                    message = ""
                };
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                //new BusinessLogic().CreateLog(1, ex.Message, ex.HResult.ToString(), "LocalizationController/ListLocalization");
            }
            return result;
        }

        [HttpPost]
        [Route("ListLocalizationByScreenName")]
        public JsonStandardResponse ListLocalizationByScreenName([FromBody] WebApiOauth2.Models.RequestModels.ListLocalizationByScreenName requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = new Localization().ListLocalizationByScreenName(requestobj.ScreenName, Constants.GetConnectionString()),
                    message = ""
                };
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                //new BusinessLogic().CreateLog(1, ex.Message, ex.HResult.ToString(), "LocalizationController/ListLocalization");
            }
            return result;
        }
    }
}