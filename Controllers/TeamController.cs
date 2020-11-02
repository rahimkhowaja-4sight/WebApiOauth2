using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.DAL.Utils;
using WebApiOauth2.ENT;
using WebApiOauth2.Helper_Code.Classes;
using WebApiOauth2.Models;

namespace WebApiOauth2.Controllers
{
    [Authorize]
    [RoutePrefix("api/Teams")]
    public class TeamController : ApiController
    {
        [HttpGet]
        [Route("GetAllTeams")]
        public JsonStandardResponse GetAllTeams()
        {
            JsonStandardResponse result = null;
            try
            {
                List<Teams> obj = new TeamManagement().getAllTeams(Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no teams found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllTeams", "GetTeams", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Teams/GetAllTeams", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllTeams", "GetTeams", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Teams/GetAllTeams", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetTeamDetails")]
        public JsonStandardResponse GetTeamDetails([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetTeamDetails requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.TeamID == null || requestobj.TeamID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TeamID is mandatory for fetching team details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.TeamID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for TeamID should be in number form!"
                        };
                        return result;
                    }
                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.TeamID, "Team", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid TeamID doesn't exists!"
                        };
                        return result;
                    }
                }



                #endregion

                TeamDetails obj = new TeamManagement().getTeamDetails(requestobj.TeamID, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no team details found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetTeamDetails", "GetTeamDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Teams/GetTeamDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetTeamDetails", "GetTeamDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Teams/GetTeamDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

    }
}
