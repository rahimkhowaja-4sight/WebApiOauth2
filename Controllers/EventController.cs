using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.DAL.Utils;
using WebApiOauth2.ENT;
using WebApiOauth2.ENT.EventManagement;
using WebApiOauth2.Helper_Code.Classes;
using WebApiOauth2.Models;

namespace WebApiOauth2.Controllers
{
    [Authorize]
    [RoutePrefix("api/Events")]
    public class EventController : ApiController
    {

        string ApplicationURL = ConfigurationManager.AppSettings["ApplicationURL"];


        [HttpGet]
        [Route("GetAllEvents")]
        public JsonStandardResponse GetAllEvents()
        {
            JsonStandardResponse result = null;
            try
            {
                List<EventObject> obj = new EventManagement().getAllEvents(Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no events found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllEvents", "GetEvents", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetAllEvents", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllEvents", "GetEvents", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetAllEvents", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetAllEvents_New")]
        public JsonStandardResponse GetAllEvents_New([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventsList requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                List<EventObject> obj = new EventManagement().getAllEventsNew(requestobj.UserID,Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no events found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllEvents", "GetEvents", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetAllEvents", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllEvents", "GetEvents", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetAllEvents", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }



        [HttpPost]
        [Route("GetEventDetailByUser")]
        public JsonStandardResponse GetEventDetailByUser([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventsList requestobj)
        {
            JsonStandardResponse result = null;
            try
            {

                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for GetEventDetailByUser api"
                    };
                    return result;
                }
                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching event detail!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    //var message = new EventManagement().checkUserOrTeamByID(requestobj.EventType, "User", Constants.GetConnectionString());
                   
                    //    result = new JsonStandardResponse
                    //    {
                    //        status = "error",
                    //        data = "",
                    //        message = "invalid UserID doesn't exists!"
                    //    };
                    //    return result;
                    
                }



                #endregion


                  List<EventManagementDetail> obj = new EventManagement().getEventDetailByUserID(requestobj.UserID, requestobj.EventID, requestobj.EventType, requestobj.EventCode,requestobj.FirstName
                      ,requestobj.LastName,requestobj.Email,requestobj.PhoneNumber,requestobj.RegStatus, Constants.GetConnectionString());
                    if (obj == null)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "no data found!"
                        };
                        return result;
                    }
                    result = new JsonStandardResponse
                    {
                        status = "success",
                        data = obj,
                        message = "Request Successful!"
                    };
                
              


              
                new BusinessLogic().CreateLog("GetEventDetailByUser", "GetEventDetailByUser", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Events/GetEventDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetEventDetailByUser", "GetEventDetailByUser", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Events/GetEventDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }




        [HttpPost]
        [Route("GetAgendaDetailByUser")]
        public JsonStandardResponse GetAgendaDetailByUser([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventFeatures requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                List<EventAgendaDetail> obj = new EventManagement().getEventAgendaDetailByUser(requestobj.UserID, requestobj.EventId, requestobj.FeatureId, requestobj.EventCode, requestobj.SessionId, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no session found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAgendaDetailByUser", "GetAgendaDetailByUser", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetAgendaDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAgendaDetailByUser", "GetAgendaDetailByUser", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetAgendaDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetSpeakerDetailByUser")]
        public JsonStandardResponse GetSpeakerDetailByUser([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventFeatures requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                List<EventSpeakers> obj = new EventManagement().getEventSpeakerDetailByUser(requestobj.UserID, requestobj.EventId, requestobj.FeatureId, requestobj.EventCode, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no events found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetSpeakerDetailByUser", "GetSpeakerDetailByUser", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetSpeakerDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetSpeakerDetailByUser", "GetSpeakerDetailByUser", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetSpeakerDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }



        [HttpPost]
        [Route("BookSessionByUser")]
        public JsonStandardResponse BookSessionByUser([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventFeatures requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                var obj = new EventManagement().BookSessionByUser(requestobj.UserID, requestobj.EventId, requestobj.FeatureId, requestobj.SessionId, requestobj.SubSessionId, requestobj.UserID, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no result found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj.Item2,
                    message = obj.Item2
                };
                new BusinessLogic().CreateLog("BookSessionByUser", "BookSessionByUser", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/BookSessionByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("BookSessionByUser", "BookSessionByUser", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/BookSessionByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("BookSessionForUsers")]
        public JsonStandardResponse BookSessionForUsers([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventFeatures requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for BookSessionForUsers api"
                    };
                    return result;
                }
                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for Book Session!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    var message = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!message)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.UserIDs == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserIDs is mandatory for Book Session!"
                    };
                    return result;
                }
                if (requestobj.UserIDs.Count != 0)
                {
                    foreach (string user in requestobj.UserIDs)
                    {
                        int temp;
                        if (!int.TryParse(user, out temp))
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid data for UserId in UserIDs should be in number form!"
                            };
                            return result;
                        }
                        bool flag = new TaskManagement().checkUserOrTeamByID(user, "User", Constants.GetConnectionString());
                        if (!flag)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid UserId in UserIDs! doesn't exists"
                            };
                            return result;
                        }
                    }
                }

                #endregion

                List<BookedSessions> listObjects = new List<BookedSessions>();
                foreach (string user in requestobj.UserIDs)
                {
                    string userName = ""; 
                    string responseMsg = "";
                    var responseObj = new EventManagement().BookSessionByUser(user, requestobj.EventId, requestobj.FeatureId, requestobj.SessionId, requestobj.SubSessionId, requestobj.UserID, Constants.GetConnectionString());
                    if (responseObj != null)
                    {
                        userName = responseObj.Item1;
                        responseMsg = responseObj.Item2;
                    }
                    BookedSessions obj = new BookedSessions();
                    obj.UserID = user;
                    obj.UserName = userName;
                    obj.Message = responseMsg;
                    listObjects.Add(obj);
                }

                result = new JsonStandardResponse
                {
                    status = "success",
                    data = listObjects,
                    message = "Processed"
                };
                new BusinessLogic().CreateLog("BookSessionForUsers", "BookSessionForUsers", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/BookSessionForUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("BookSessionForUsers", "BookSessionForUsers", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/BookSessionForUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetDocumentDetailByUser")]
        public JsonStandardResponse GetDocumentDetailByUser([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventFeatures requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                List<EventDocumentDetail> obj = new EventManagement().getEventDocumentByUser(requestobj.UserID, requestobj.EventId, requestobj.FeatureId, requestobj.EventCode, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetDocumentDetailByUser", "GetDocumentDetailByUser", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetDocumentDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetDocumentDetailByUser", "GetDocumentDetailByUser", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetDocumentDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetTipsDetailByUser")]
        public JsonStandardResponse GetTipsDetailByUser([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventFeatures requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                List<EventTipDetail> obj = new EventManagement().getEventTipsByUser(requestobj.UserID, requestobj.EventId, requestobj.FeatureId, requestobj.EventCode, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetTipsDetailByUser", "GetTipsDetailByUser", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetTipsDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetTipsDetailByUser", "GetTipsDetailByUser", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetTipsDetailByUser", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetSurveyDetailsByEventID")]
        public JsonStandardResponse GetSurveyDetailsByEventID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetSurveyDetailsByEventID requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for GetSurveyDetailsByEventID api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }
                    
                }

                if (requestobj.EventId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventId is mandatory for fetching survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getEventByID(requestobj.EventId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventId doesn't exists!"
                        };
                        return result;
                    }
                    
                }

                #endregion

                List<EventSurveyDetails> obj = new EventManagement().getSurveyDetailsByEventID(requestobj.UserID, requestobj.EventId, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetSurveyDetailsByEventID", "GetSurveyDetailsByEventID", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetSurveyDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetSurveyDetailsByEventID", "GetSurveyDetailsByEventID", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetSurveyDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("PostSurveyDetails")]
        public JsonStandardResponse PostSurveyDetails([FromBody] WebApiOauth2.Models.RequestModels.RequestModelPostSurveyDetails requestobj)
        {
            PostSurveyDetails obj = new PostSurveyDetails();
            obj.SurveyResponse = new List<SurveyResponse>();

            bool flag = false;
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for PostSurveyDetails api"
                    };
                    return result;
                }

                if (requestobj.SessionUserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SessionUserID is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.SessionUserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for SessionUserID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().checkUserOrTeamByID(requestobj.SessionUserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid SessionUserID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for posting survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getEventByID(requestobj.EventID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.SurveyID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SurveyID is mandatory for posting survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.SurveyID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for SurveyID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getSurveyByID(requestobj.SurveyID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid SurveyID doesn't exists!"
                        };
                        return result;
                    }

                }

                flag = new EventManagement().getEventAndSurveyRelationByIDs(requestobj.SurveyID, requestobj.EventID, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid SurveyID and EventID relation doesn't exists!"
                    };
                    return result;
                }

                if (requestobj.SurveyDateTime == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SurveyDateTime is mandatory for posting survey details!"
                    };
                    return result;
                }
                else
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.SurveyDateTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid date format for SurveyDateTime required is MM-dd-yyyy HH:mm:ss!"
                        };
                        return result;
                    }
                }

                if (requestobj.SurveyResponse == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SurveyResponse is mandatory for posting survey details!"
                    };
                    return result;
                }
                if (requestobj.SurveyResponse.Count == 0)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SurveyResponse cannot be empty for posting survey details!"
                    };
                    return result;
                }
                else
                {
                    foreach (var value in requestobj.SurveyResponse)
                    {
                        if (value.QuestionID == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "QuestionID is mandatory in SurveyResponse for posting survey details!"
                            };
                            return result;
                        }
                        else
                        {
                            int temp;
                            if (!int.TryParse(value.QuestionID, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for QuestionID in SurveyResponse should be in number form!"
                                };
                                return result;
                            }
                            flag = new EventManagement().getSurveyQuestionByID(value.QuestionID, Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid QuestionID in SurveyResponse! doesn't exists"
                                };
                                return result;
                            }
                        }

                        if (value.AnswerID == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "AnswerID is mandatory in SurveyResponse for posting survey details!"
                            };
                            return result;
                        }
                        else
                        {
                            int temp;
                            if (!int.TryParse(value.AnswerID, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for AnswerID in SurveyResponse should be in number form!"
                                };
                                return result;
                            }
                            flag = new EventManagement().getSurveyAnswerByID(value.AnswerID, Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid AnswerID in SurveyResponse! doesn't exists"
                                };
                                return result;
                            }
                        }
                        
                        SurveyResponse surveyResponse = new SurveyResponse();
                        surveyResponse.QuestionID = value.QuestionID;
                        surveyResponse.AnswerID = value.AnswerID;
                        surveyResponse.Other = value.Other;
                        obj.SurveyResponse.Add(surveyResponse);
                    }
                }

                #endregion
                
                obj.SessionUserID = requestobj.SessionUserID;
                obj.EventID = requestobj.EventID;
                obj.SurveyID = requestobj.SurveyID;
                obj.SurveyDateTime = requestobj.SurveyDateTime;

                flag = new EventManagement().PostSurveyDetails(obj, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Request Failed!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = "",
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("PostSurveyDetails", "PostSurveyDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/PostSurveyDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("PostSurveyDetails", "PostSurveyDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/PostSurveyDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetMapsByEvent")]
        public JsonStandardResponse GetMapsByEvent([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetEventFeatures requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                List<EventMapDetail> obj = new EventManagement().getMapsByEvent(requestobj.UserID, requestobj.EventId, requestobj.FeatureId, requestobj.EventCode, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetMapsByEvent", "GetMapsByEvent", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetMapsByEvent", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetMapsByEvent", "GetMapsByEvent", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetMapsByEvent", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetVotingDetailsByEventID")]
        public JsonStandardResponse GetVotingDetailsByEventID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetVotingDetailsByEventID requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for GetSurveyDetailsByEventID api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.EventId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventId is mandatory for fetching survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getEventByID(requestobj.EventId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventId doesn't exists!"
                        };
                        return result;
                    }

                }

                #endregion

                List<EventVotingDetails> obj = new EventManagement().getVotingDetailsByEventID(requestobj.UserID, requestobj.EventId, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetVotingDetailsByEventID", "GetVotingDetailsByEventID", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetVotingDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetVotingDetailsByEventID", "GetVotingDetailsByEventID", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetVotingDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("PostVotingDetails")]
        public JsonStandardResponse PostVotingDetails([FromBody] WebApiOauth2.Models.RequestModels.RequestModelPostVotingDetails requestobj)
        {
            PostVotingDetails obj = new PostVotingDetails();
            obj.VotingResponse = new List<VotingResponse>();

            bool flag = false;
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for PostVotingDetails api"
                    };
                    return result;
                }

                if (requestobj.SessionUserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SessionUserID is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.SessionUserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for SessionUserID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().checkUserOrTeamByID(requestobj.SessionUserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid SessionUserID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for posting voting details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getEventByID(requestobj.EventID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.VotingID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "VotingID is mandatory for posting survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.VotingID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for VotingID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getVotingByID(requestobj.VotingID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid VotingID doesn't exists!"
                        };
                        return result;
                    }

                }

                flag = new EventManagement().getEventAndVotingRelationByIDs(requestobj.VotingID, requestobj.EventID, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid SurveyID and VotingID relation doesn't exists!"
                    };
                    return result;
                }

                if (requestobj.VotingDateTime == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "VotingDateTime is mandatory for posting voting details!"
                    };
                    return result;
                }
                else
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.VotingDateTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid date format for VotingDateTime required is MM-dd-yyyy HH:mm:ss!"
                        };
                        return result;
                    }
                }

                if (requestobj.VotingResponse == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "VotingResponse is mandatory for posting voting details!"
                    };
                    return result;
                }
                if (requestobj.VotingResponse.Count == 0)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "VotingResponse cannot be empty for posting voting details!"
                    };
                    return result;
                }
                else
                {
                    foreach (var value in requestobj.VotingResponse)
                    {
                        if (value.QuestionID == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "QuestionID is mandatory in VotingResponse for posting voting details!"
                            };
                            return result;
                        }
                        else
                        {
                            int temp;
                            if (!int.TryParse(value.QuestionID, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for QuestionID in VotingResponse should be in number form!"
                                };
                                return result;
                            }
                            flag = new EventManagement().getVotingQuestionByID(value.QuestionID, Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid QuestionID in VotingResponse! doesn't exists"
                                };
                                return result;
                            }
                        }

                        if (value.AnswerID == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "AnswerID is mandatory in VotingResponse for posting voting details!"
                            };
                            return result;
                        }
                        else
                        {
                            int temp;
                            if (!int.TryParse(value.AnswerID, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for AnswerID in VotingResponse should be in number form!"
                                };
                                return result;
                            }
                            flag = new EventManagement().getVotingAnswerByID(value.AnswerID, Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid AnswerID in VotingResponse! doesn't exists"
                                };
                                return result;
                            }
                        }

                        VotingResponse surveyResponse = new VotingResponse();
                        surveyResponse.QuestionID = value.QuestionID;
                        surveyResponse.AnswerID = value.AnswerID;
                        surveyResponse.Other = value.Other;
                        obj.VotingResponse.Add(surveyResponse);
                    }
                }

                #endregion

                obj.SessionUserID = requestobj.SessionUserID;
                obj.EventID = requestobj.EventID;
                obj.VotingID = requestobj.VotingID;
                obj.VotingDateTime = requestobj.VotingDateTime;

                flag = new EventManagement().PostVotingDetails(obj, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Request Failed!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = "",
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("PostVotingDetails", "PostVotingDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/PostVotingDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("PostVotingDetails", "PostVotingDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/PostVotingDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetAttendeesByEventID")]
        public JsonStandardResponse GetAttendeesByEventID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAttendeesByEventID requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for GetSurveyDetailsByEventID api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.EventId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventId is mandatory for fetching survey details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getEventByID(requestobj.EventId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventId doesn't exists!"
                        };
                        return result;
                    }

                }

                #endregion

                List<AttendeesDetails> obj = new EventManagement().getAttendeesByEventID(requestobj.UserID, requestobj.EventId, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAttendeesByEventID", "GetAttendeesByEventID", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetAttendeesByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAttendeesByEventID", "GetAttendeesByEventID", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetAttendeesByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("UpdateAttendeeDetails")]
        public JsonStandardResponse UpdateAttendeeDetails([FromBody] WebApiOauth2.Models.RequestModels.RequestUpdateAttendeeDetails requestobj)
        {
            UpdateAttendeeDetails obj = new UpdateAttendeeDetails();

            bool flag = false;
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for UpdateAttendeeDetails api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for updating attendee details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for updating attendee details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getEventByID(requestobj.EventID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.AttendeeID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "AttendeeID is mandatory for updating attendee details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.AttendeeID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for AttendeeID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getAttendeeByID(requestobj.AttendeeID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid AttendeeID doesn't exists!"
                        };
                        return result;
                    }

                }


                #endregion

                obj.UserID = requestobj.UserID;
                obj.EventID = requestobj.EventID;
                obj.AttendeeID = requestobj.AttendeeID;
                obj.Name = requestobj.Name;
                obj.Organization = requestobj.Organization;
                obj.JobDescription = requestobj.JobDescription;
                obj.PhoneNumber = requestobj.PhoneNumber;

                flag = new EventManagement().UpdateAttendeeDetails(obj, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Request Failed!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = "",
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("UpdateAttendeeDetails", "UpdateAttendeeDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/UpdateAttendeeDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("UpdateAttendeeDetails", "UpdateAttendeeDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/UpdateAttendeeDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetTriviaDetailsByEventID")]
        public JsonStandardResponse GetTriviaDetailsByEventID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetTriviaDetailsByEventID requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for GetTriviaDetailsByEventID api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching trivia details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.EventId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventId is mandatory for fetching trivia details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getEventByID(requestobj.EventId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventId doesn't exists!"
                        };
                        return result;
                    }

                }

                #endregion

                List<EventTriviaMasterDetails> obj = new EventManagement().getTriviaMasterDetailsByEventID(requestobj.UserID, requestobj.EventId, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetTriviaDetailsByEventID", "GetTriviaDetailsByEventID", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetTriviaDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetTriviaDetailsByEventID", "GetTriviaDetailsByEventID", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetTriviaDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("JoinOrAssignTrivia")]
        public JsonStandardResponse JoinOrAssignTrivia([FromBody] WebApiOauth2.Models.RequestModels.RequestModelJoinOrAssignTrivia requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for JoinOrAssignTrivia api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for joining or assigning trivia!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.AssignedtoUserID != null)
                {
                    int temp;
                    if (!int.TryParse(requestobj.AssignedtoUserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for AssignedtoUserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.AssignedtoUserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid AssignedtoUserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.EventId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventId is mandatory for joining or assigning trivia!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getEventByID(requestobj.EventId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventId doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.TriviaId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TriviaId is mandatory for joining or assigning trivia!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.TriviaId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for TriviaId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getTriviaByID(requestobj.TriviaId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid TriviaId doesn't exists!"
                        };
                        return result;
                    }

                }

                #endregion

                if (requestobj.AssignedtoUserID != null)
                {
                    bool flag = new EventManagement().AssignTriviaByUserID(requestobj.UserID, requestobj.AssignedtoUserID, requestobj.EventId, requestobj.TriviaId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "Already Assigned!",
                            message = "Request Failed!"
                        };
                        return result;
                    }
                    result = new JsonStandardResponse
                    {
                        status = "success",
                        data = "Trivia Assigned!",
                        message = "Request Successful!"
                    };
                }
                else
                {
                    List<EventTriviaDetails> obj = new EventManagement().getTriviaDetailsByEventID(requestobj.UserID, requestobj.EventId, requestobj.TriviaId, Constants.GetConnectionString());
                    if (obj == null)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "Trivia Already Assigned OR Started!",
                            message = "Request Failed!"
                        };
                        return result;
                    }
                    result = new JsonStandardResponse
                    {
                        status = "success",
                        data = obj,
                        message = "Request Successful!"
                    };
                }
                new BusinessLogic().CreateLog("JoinOrAssignTrivia", "JoinOrAssignTrivia", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/JoinOrAssignTrivia", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("JoinOrAssignTrivia", "JoinOrAssignTrivia", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/JoinOrAssignTrivia", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("PostTriviaDetails")]
        public JsonStandardResponse PostTriviaDetails([FromBody] WebApiOauth2.Models.RequestModels.RequestModelPostTriviaDetails requestobj)
        {
            PostTriviaDetails obj = new PostTriviaDetails();
            obj.TriviaResponse = new List<TriviaResponse>();

            bool flag = false;
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for PostTriviaDetails api"
                    };
                    return result;
                }

                if (requestobj.SessionUserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SessionUserID is mandatory for posting trivia details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.SessionUserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for SessionUserID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().checkUserOrTeamByID(requestobj.SessionUserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid SessionUserID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for posting trivia details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getEventByID(requestobj.EventID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.TriviaID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TriviaID is mandatory for posting trivia details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.TriviaID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for TriviaID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getTriviaByID(requestobj.TriviaID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid TriviaID doesn't exists!"
                        };
                        return result;
                    }

                }

                flag = new EventManagement().getTriviaRelationID(requestobj.TriviaRelationID, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid TriviaRelationID doesn't exists!"
                    };
                    return result;
                }

                if (requestobj.TriviaResponse == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TriviaResponse is mandatory for posting trivia details!"
                    };
                    return result;
                }
                if (requestobj.TriviaResponse.Count == 0)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TriviaResponse cannot be empty for posting trivia details!"
                    };
                    return result;
                }
                else
                {
                    foreach (var value in requestobj.TriviaResponse)
                    {
                        if (value.QuestionID == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "QuestionID is mandatory in TriviaResponse for posting trivia details!"
                            };
                            return result;
                        }
                        else
                        {
                            int temp;
                            if (!int.TryParse(value.QuestionID, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for QuestionID in TriviaResponse should be in number form!"
                                };
                                return result;
                            }
                            flag = new EventManagement().getTriviaQuestionByID(value.QuestionID, Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid QuestionID in TriviaResponse! doesn't exists"
                                };
                                return result;
                            }
                        }

                        //if (value.AnswerID == null)
                        //{
                        //    result = new JsonStandardResponse
                        //    {
                        //        status = "error",
                        //        data = "",
                        //        message = "AnswerID is mandatory in TriviaResponse for posting trivia details!"
                        //    };
                        //    return result;
                        //}
                        //else
                        if (value.AnswerID != null)
                        {
                            int temp;
                            if (!int.TryParse(value.AnswerID, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for AnswerID in SurveyResponse should be in number form!"
                                };
                                return result;
                            }
                            flag = new EventManagement().getTriviaAnswerByID(value.AnswerID, Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid AnswerID in TriviaResponse! doesn't exists"
                                };
                                return result;
                            }
                        }

                        TriviaResponse triviaResponse = new TriviaResponse();
                        triviaResponse.QuestionID = value.QuestionID;
                        triviaResponse.AnswerID = ((value.AnswerID == null) ? "0" : value.AnswerID);
                        triviaResponse.Other = ((value.Other == null) ? "" : value.Other);
                        obj.TriviaResponse.Add(triviaResponse);
                    }
                }

                #endregion

                obj.SessionUserID = requestobj.SessionUserID;
                obj.EventID = requestobj.EventID;
                obj.TriviaID = requestobj.TriviaID;
                obj.TriviaRelationID = requestobj.TriviaRelationID;

                flag = new EventManagement().PostTriviaDetails(obj, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Request Failed!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = "",
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("PostTriviaDetails", "PostTriviaDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/PostTriviaDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("PostTriviaDetails", "PostTriviaDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/PostTriviaDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        [HttpPost]
        [Route("GetChallengeDetailsByEventID")]
        public JsonStandardResponse GetChallengeDetailsByEventID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetChallengeDetailsByEventID requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for GetChallengeDetailsByEventID api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching challenge details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.EventId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventId is mandatory for fetching challenge details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getEventByID(requestobj.EventId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventId doesn't exists!"
                        };
                        return result;
                    }

                }

                #endregion

                List<EventChallengeMasterDetails> obj = new EventManagement().getChallengeMasterDetailsByEventID(requestobj.UserID, requestobj.EventId, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no data found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetChallengeDetailsByEventID", "GetChallengeDetailsByEventID", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetChallengeDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetChallengeDetailsByEventID", "GetChallengeDetailsByEventID", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetChallengeDetailsByEventID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("JoinOrAssignChallenge")]
        public JsonStandardResponse JoinOrAssignChallenge([FromBody] WebApiOauth2.Models.RequestModels.RequestModelJoinOrAssignChallenge requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for JoinOrAssignChallenge api"
                    };
                    return result;
                }

                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for joining or assigning challenge!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid UserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.AssignedtoUserID != null)
                {
                    int temp;
                    if (!int.TryParse(requestobj.AssignedtoUserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for AssignedtoUserID should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new TaskManagement().checkUserOrTeamByID(requestobj.AssignedtoUserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid AssignedtoUserID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.EventId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventId is mandatory for joining or assigning challenge!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getEventByID(requestobj.EventId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventId doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.ChallengeId == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "ChallengeId is mandatory for joining or assigning challenge!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.ChallengeId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for ChallengeId should be in number form!"
                        };
                        return result;
                    }


                    bool flag = new EventManagement().getChallengeByID(requestobj.ChallengeId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid ChallengeId doesn't exists!"
                        };
                        return result;
                    }

                }

                #endregion

                if (requestobj.AssignedtoUserID != null)
                {
                    bool flag = new EventManagement().AssignChallengeByUserID(requestobj.UserID, requestobj.AssignedtoUserID, requestobj.EventId, requestobj.ChallengeId, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "Already Assigned!",
                            message = "Request Failed!"
                        };
                        return result;
                    }
                    result = new JsonStandardResponse
                    {
                        status = "success",
                        data = "Challenge Assigned!",
                        message = "Request Successful!"
                    };
                }
                else
                {
                    List<EventChallengeDetails> obj = new EventManagement().getChallengeDetailsByEventID(requestobj.UserID, requestobj.EventId, requestobj.ChallengeId, Constants.GetConnectionString());
                    if (obj == null)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "Challenge Already Assigned OR Started!",
                            message = "Request Failed!"
                        };
                        return result;
                    }
                    result = new JsonStandardResponse
                    {
                        status = "success",
                        data = obj,
                        message = "Request Successful!"
                    };
                }
                new BusinessLogic().CreateLog("JoinOrAssignChallenge", "JoinOrAssignChallenge", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/JoinOrAssignChallenge", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("JoinOrAssignChallenge", "JoinOrAssignChallenge", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/JoinOrAssignChallenge", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("PostChallengeDetails")]
        public JsonStandardResponse PostChallengeDetails([FromBody] WebApiOauth2.Models.RequestModels.RequestModelPostChallengeDetails requestobj)
        {
            PostChallengeDetails obj = new PostChallengeDetails();
            obj.ChallengeResponse = new List<ChallengeResponse>();

            bool flag = false;
            JsonStandardResponse result = null;
            try
            {
                #region Validations
                if (requestobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid data for PostChallengeDetails api"
                    };
                    return result;
                }

                if (requestobj.SessionUserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SessionUserID is mandatory for posting challenge details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.SessionUserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for SessionUserID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().checkUserOrTeamByID(requestobj.SessionUserID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid SessionUserID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for posting challenge details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.EventID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for EventID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getEventByID(requestobj.EventID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid EventID doesn't exists!"
                        };
                        return result;
                    }

                }

                if (requestobj.ChallengeID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "ChallengeID is mandatory for posting challenge details!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.ChallengeID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for ChallengeID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getTriviaByID(requestobj.ChallengeID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid TriviaID doesn't exists!"
                        };
                        return result;
                    }

                }

                flag = new EventManagement().getChallengeRelationID(requestobj.ChallengeRelationID, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "invalid ChallengeRelationID doesn't exists!"
                    };
                    return result;
                }

                if (requestobj.ChallengeResponse == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "ChallengeResponse is mandatory for posting challenge details!"
                    };
                    return result;
                }
                if (requestobj.ChallengeResponse.Count == 0)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "ChallengeResponse cannot be empty for posting challenge details!"
                    };
                    return result;
                }
                else
                {
                    foreach (var value in requestobj.ChallengeResponse)
                    {
                        if (value.StepID == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "StepID is mandatory in ChallengeResponse for posting challenge details!"
                            };
                            return result;
                        }
                        else
                        {
                            int temp;
                            if (!int.TryParse(value.StepID, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for StepID in ChallengeResponse should be in number form!"
                                };
                                return result;
                            }
                            flag = new EventManagement().getChallengeStepByID(value.StepID, Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid StepID in ChallengeResponse! doesn't exists"
                                };
                                return result;
                            }
                        }

                        if (value.ScannedBarCodeText == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "ScannedBarCodeText is mandatory in ChallengeResponse for posting challenge details!"
                            };
                            return result;
                        }
                        else
                        {
                            if (value.ScannedBarCodeText.Trim() == "")
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = "",
                                    message = "invalid data for ScannedBarCodeText in ChallengeResponse should not be empty!"
                                };
                                return result;
                            }
                        }

                        ChallengeResponse challengeResponse = new ChallengeResponse();
                        challengeResponse.StepID = value.StepID;
                        challengeResponse.ScannedBarCodeText = value.ScannedBarCodeText;
                        obj.ChallengeResponse.Add(challengeResponse);
                    }
                }

                #endregion

                obj.SessionUserID = requestobj.SessionUserID;
                obj.EventID = requestobj.EventID;
                obj.ChallengeID = requestobj.ChallengeID;
                obj.ChallengeRelationID = requestobj.ChallengeRelationID;

                flag = new EventManagement().PostChallengeDetails(obj, Constants.GetConnectionString());
                if (!flag)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Request Failed!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = "",
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("PostChallengeDetails", "PostChallengeDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/PostChallengeDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("PostChallengeDetails", "PostChallengeDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/PostChallengeDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }
    }
}
