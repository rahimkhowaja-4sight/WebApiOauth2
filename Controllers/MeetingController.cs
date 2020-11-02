using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.DAL.Utils;
using WebApiOauth2.ENT;
using WebApiOauth2.ENT.MeetingManagement;
using WebApiOauth2.Helper_Code.Classes;
using WebApiOauth2.Models;

namespace WebApiOauth2.Controllers
{
    [Authorize]
    [RoutePrefix("api/Meetings")]
    public class MeetingController : ApiController
    {
        string ApplicationURL = ConfigurationManager.AppSettings["ApplicationURL"];

        [HttpPost]
        [Route("GetAllMeetings")]
        public JsonStandardResponse GetAllMeetings([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAllMeetingsList requestobj)
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
                        message = "invalid data for GetAllMeetings api"
                    };
                    return result;
                }
                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching meeting list!"
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

                if (requestobj.TaskID != null)
                {
                    int temp;
                    if (!int.TryParse(requestobj.TaskID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for TaskID should be in number form!"
                        };
                        return result;
                    }
                    bool flag = new TaskManagement().getTaskByID(requestobj.TaskID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid TaskID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.EventID != null)
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
                    //flag = new TaskManagement().getTaskByID(requestobj.TaskID, Constants.GetConnectionString());
                    //if (!flag)
                    //{
                    //    result = new JsonStandardResponse
                    //    {
                    //        status = "error",
                    //        data = "",
                    //        message = "invalid EventID doesn't exists!"
                    //    };
                    //    return result;
                    //}
                }



                #endregion

                List<MeetingObject> obj = new MeetingManagement().getAllMeetings(
                    (requestobj.UserID == null || requestobj.UserID == "") ? "0" : requestobj.UserID,
                    (requestobj.TaskID == null || requestobj.TaskID == "") ? "0" : requestobj.TaskID,
                    (requestobj.EventID == null || requestobj.EventID == "") ? "0" : requestobj.EventID, 
                    Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no meetings found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllMeetings", "GetMeetings", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Meetings/GetAllMeetings", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllMeetings", "GetMeetings", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Meetings/GetAllMeetings", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetAllUsersAndTeamsByUserID")]
        public JsonStandardResponse GetAllUsersAndTeamsByUserID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAllUsersAndTeamsByUserID requestobj)
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
                        message = "invalid data for GetAllUsersAndTeamsByUserID api"
                    };
                    return result;
                }
                if (requestobj.UserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching users and teams list!"
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



                #endregion

                List<MeetingManagementUsersAndTeams> obj = new MeetingManagement().getAllUsersAndTeamsByUserID(requestobj.UserID, Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("GetAllUsers", "GetAllUsers", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Meetings/GetAllUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllUsers", "GetAllUsers", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Meetings/GetAllUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("AddUpdateMeeting")]
        public JsonStandardResponse AddUpdateMeeting([FromBody] WebApiOauth2.Models.RequestModels.RequestModelAddUpdateMeeting requestobj)
        {
            MeetingObject obj = new MeetingObject();
            obj.AttachmentFiles = new List<string>();
            obj.AssignedUsersOrTeams = new List<AssignedUsersOrTeams>();

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
                        message = "invalid data for AddUpdateMeeting api"
                    };
                    return result;
                }
                if (requestobj.MeetingID != null)
                {
                    if (requestobj.MeetingID != "")
                    {
                        int temp;
                        if (!int.TryParse(requestobj.MeetingID, out temp))
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid data for MeetingID should be in number form!"
                            };
                            return result;
                        }
                        flag = new MeetingManagement().getMeetingByID(requestobj.MeetingID, Constants.GetConnectionString());
                        if (!flag)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid MeetingID doesn't exists!"
                            };
                            return result;
                        }
                    }
                }

                if (requestobj.TaskID != null)
                {
                    if (requestobj.TaskID != "")
                    {
                        int temp;
                        if (!int.TryParse(requestobj.TaskID, out temp))
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid data for TaskID should be in number form!"
                            };
                            return result;
                        }
                        flag = new TaskManagement().getTaskByID(requestobj.TaskID, Constants.GetConnectionString());
                        if (!flag)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid TaskID doesn't exists!"
                            };
                            return result;
                        }
                    }
                }

                if (requestobj.EventID != null)
                {
                    if (requestobj.EventID != "")
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
                        //flag = new TaskManagement().getTaskByID(requestobj.TaskID, Constants.GetConnectionString());
                        //if (!flag)
                        //{
                        //    result = new JsonStandardResponse
                        //    {
                        //        status = "error",
                        //        data = "",
                        //        message = "invalid EventID doesn't exists!"
                        //    };
                        //    return result;
                        //}
                    }
                }

                if (requestobj.MeetingTitleEng == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Title in English is mandatory for meeting!"
                    };
                    return result;
                }

                if (requestobj.MeetingTitleArb == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Title in Arabic is mandatory for meeting!"
                    };
                    return result;
                }

                if (requestobj.MeetingAgendaEng == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "MeetingAgenda in English is mandatory for meeting!"
                    };
                    return result;
                }

                if (requestobj.MeetingAgendaArb == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "MeetingAgenda in Arabic is mandatory for meeting!"
                    };
                    return result;
                }

                if (requestobj.MeetingTypeEng == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "MeetingType in English is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    if (requestobj.MeetingTypeEng != "Virtual" && requestobj.MeetingTypeEng != "Place")
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "MeetingType would be 'Virtual' or 'Place' for meeting!"
                        };
                        return result;
                    }
                }

                if (requestobj.MeetingTypeEng == "Virtual")
                {
                    if (requestobj.VirtualLink == null)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "VirtualLink is mandatory for meeting type virtual!"
                        };
                        return result;
                    }
                }

                if (requestobj.MeetingTypeEng == "Place")
                {
                    if (
                        (requestobj.MeetingLocation == null) 
                        ||
                        (requestobj.MeetingLocationCoordinates == null)
                        )
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "MeetingLocation and MeetingLocationCoordinates are mandatory for meeting type place!"
                        };
                        return result;
                    }
                    if (requestobj.MeetingLocationCoordinates != "")
                    {
                        var match = Regex.Match(requestobj.MeetingLocationCoordinates, @"^(\s*(-|\+)?\d+(?:\.\d+)?\s*,\s*)+(-|\+)?\d+(?:\.\d+)?\s*$", RegexOptions.IgnoreCase);
                        if (!match.Success)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "MeetingLocationCoordinates should be like this {lat},{lng} !"
                            };
                            return result;
                        }
                        
                    }
                }

                if (requestobj.MeetingDate == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "MeetingDate is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.MeetingDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid date format for MeetingDate required is MM/dd/yyyy!"
                        };
                        return result;
                    }
                }

                if (requestobj.StartTime == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "StartTime is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.StartTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid time format for StartTime required is HH:mm:ss!"
                        };
                        return result;
                    }
                }

                if (requestobj.EndTime == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EndTime is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.EndTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid time format for EndTime required is HH:mm:ss!"
                        };
                        return result;
                    }
                }

                if (requestobj.AssignPersonMOMID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "AssignPersonMOMID is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.AssignPersonMOMID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for AssignPersonMOMID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().checkUserOrTeamByID(requestobj.AssignPersonMOMID, "User", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid AssignPersonMOMID doesn't exists!"
                        };
                        return result;
                    }
                }


                if (requestobj.AssignedUsersOrTeams == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "AssignedUsersOrTeams is mandatory for meeting!"
                    };
                    return result;
                }
                if (requestobj.AssignedUsersOrTeams.Count == 0)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "AssignedUsersOrTeams cannot be empty for meeting!"
                    };
                    return result;
                }
                else
                {
                    foreach (var value in requestobj.AssignedUsersOrTeams)
                    {
                        if (value.Type == null)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "Type in AssignedUsersOrTeams is mandatory for meeting!"
                            };
                            return result;
                        }
                        if (value.Type != "User" && value.Type != "Team")
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid data for Type in AssignedUsersOrTeams should be in 'User' or 'Team'!"
                            };
                            return result;
                        }
                        int temp;
                        if (!int.TryParse(value.ID, out temp))
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid data for ID in AssignedUsersOrTeams should be in number form!"
                            };
                            return result;
                        }
                        flag = new TaskManagement().checkUserOrTeamByID(value.ID, value.Type, Constants.GetConnectionString());
                        if (!flag)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid ID in AssignedUsersOrTeams! doesn't exists"
                            };
                            return result;
                        }
                        AssignedUsersOrTeams userorteam = new AssignedUsersOrTeams();
                        userorteam.ID = value.ID;
                        userorteam.Type = value.Type;
                        obj.AssignedUsersOrTeams.Add(userorteam);
                    }
                }

                //if (requestobj.MOMItems == "" && requestobj.MeetingID != "")
                //{
                //    result = new JsonStandardResponse
                //    {
                //        status = "error",
                //        data = "",
                //        message = "MOMItems is mandatory for meeting!"
                //    };
                //    return result;
                //}

                //if (requestobj.MOMItems == "" && requestobj.MeetingID != "")
                //{
                //    result = new JsonStandardResponse
                //    {
                //        status = "error",
                //        data = "",
                //        message = "MOMItems is mandatory for meeting!"
                //    };
                //    return result;
                //}

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

                if (requestobj.MeetingID != null)
                {
                    if (requestobj.IsActive == null)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "IsActive is mandatory for meeting!"
                        };
                        return result;
                    }
                    else
                    {
                        if (requestobj.IsActive != "0" && requestobj.IsActive != "1")
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "IsActive would be '0' or '1' for meeting!"
                            };
                            return result;
                        }
                    }
                }

                if (requestobj.MOMDate != null)
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.MOMDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid date format for MOMDate required is MM/dd/yyyy!"
                        };
                        return result;
                    }
                }

                #endregion

                if (requestobj.base64FileObjects != null)
                {
                    if (requestobj.base64FileObjects.Count != 0)
                    {
                        foreach (string base64FileObject in requestobj.base64FileObjects)
                        {
                            if (base64FileObject != null)
                            {
                                #region Base 64 File Processing Work
                                if (base64FileObject != "")
                                {
                                    int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB
                                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png", ".pdf" };
                                    string fileextension = WebApiOauth2.ENT.Utilities.GetMimeType(base64FileObject).Extension;
                                    if (!AllowedFileExtensions.Contains(fileextension))
                                    {
                                        result = new JsonStandardResponse
                                        {
                                            status = "error",
                                            data = "",
                                            message = "One of the file is invalid! Please Upload image of type .jpg,.png,.pdf"
                                        };
                                        return result;
                                        //var message = string.Format("Please Upload image of type .jpg,.png.");
                                        //dict.Add("error", message);
                                        //return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                    }
                                    byte[] bytes = Convert.FromBase64String(base64FileObject);
                                    if (bytes.Length > MaxContentLength)
                                    {
                                        result = new JsonStandardResponse
                                        {
                                            status = "error",
                                            data = "",
                                            message = "One of the file size is over the limit! Please Upload a file upto 1 mb."
                                        };
                                        return result;
                                        //var message = string.Format("Please Upload a file upto 1 mb.");
                                        //dict.Add("error", message);
                                        //return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                    }
                                }

                                #endregion
                            }
                        }
                        foreach (string base64FileObject in requestobj.base64FileObjects)
                        {
                            if (base64FileObject != null)
                            {
                                #region Base 64 File Saving Work
                                if (base64FileObject != "")
                                {
                                    string fileextension = WebApiOauth2.ENT.Utilities.GetMimeType(base64FileObject).Extension;
                                    byte[] bytes = Convert.FromBase64String(base64FileObject);
                                    string filename = Helper_Code.Classes.Constants.AppendTimeStamp("attachment" + fileextension);
                                    string filePath = HttpContext.Current.Server.MapPath("~/Attachments/" + filename);
                                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Attachments")))
                                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Attachments"));

                                    System.IO.FileStream stream = new FileStream(@filePath, FileMode.CreateNew);
                                    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                                    writer.Write(bytes, 0, bytes.Length);
                                    writer.Close();


                                    obj.AttachmentFiles.Add(((filename != "") ? (ApplicationURL + "/Attachments/" + filename) : ""));
                                }

                                #endregion
                            }
                        }
                    }
                }

                obj.MeetingID = (requestobj.MeetingID == null || requestobj.MeetingID == "") ? "0" : requestobj.MeetingID;
                obj.TaskID = requestobj.TaskID;
                obj.EventID = requestobj.EventID;
                obj.MeetingTitleEng = requestobj.MeetingTitleEng;
                obj.MeetingTitleArb = requestobj.MeetingTitleArb;
                obj.MeetingAgendaEng = requestobj.MeetingAgendaEng;
                obj.MeetingAgendaArb = requestobj.MeetingAgendaArb;
                obj.MeetingTypeEng = requestobj.MeetingTypeEng;
                obj.MeetingTypeArb = requestobj.MeetingTypeArb;
                obj.VirtualLink = requestobj.VirtualLink;
                obj.MeetingLocation = requestobj.MeetingLocation;
                obj.MeetingLocationMap = requestobj.MeetingLocationCoordinates;
                obj.MeetingDate = requestobj.MeetingDate;
                obj.StartTime = requestobj.StartTime;
                obj.EndTime = requestobj.EndTime;
                obj.AssignPersonMOMID = requestobj.AssignPersonMOMID;
                obj.SessionUserID = requestobj.SessionUserID;
                obj.IsActive = requestobj.IsActive;
                obj.MOMItems = requestobj.MOMItems;
                obj.MOMDate = requestobj.MOMDate;

                flag = new MeetingManagement().AddUpdateMeeting(obj, Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("AddUpdateTask", "AddUpdateTask", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Meetings/AddUpdateTask", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("AddUpdateTask", "AddUpdateTask", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Meetings/AddUpdateTask", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("UpdateMeetingStatus")]
        public JsonStandardResponse UpdateMeetingStatus([FromBody] WebApiOauth2.Models.RequestModels.RequestModelUpdateMeetingStatus requestobj)
        {
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
                        message = "invalid data for AddUpdateMeeting api"
                    };
                    return result;
                }
                if (requestobj.SessionUserID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "SessionUserID is mandatory for fetching meeting list!"
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
                if (requestobj.MeetingID != null)
                {
                    if (requestobj.MeetingID != "")
                    {
                        int temp;
                        if (!int.TryParse(requestobj.MeetingID, out temp))
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid data for MeetingID should be in number form!"
                            };
                            return result;
                        }
                        flag = new MeetingManagement().getMeetingByID(requestobj.MeetingID, Constants.GetConnectionString());
                        if (!flag)
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = "",
                                message = "invalid MeetingID doesn't exists!"
                            };
                            return result;
                        }
                    }
                }
                if (requestobj.IsActive == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "IsActive is mandatory for meeting!"
                    };
                    return result;
                }
                else
                {
                    if (requestobj.IsActive != "0" && requestobj.IsActive != "1")
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "IsActive would be '0' or '1' for meeting!"
                        };
                        return result;
                    }
                }
                #endregion
                flag = new MeetingManagement().UpdateMeetingStatus(requestobj.SessionUserID, requestobj.MeetingID, requestobj.IsActive, Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("UpdateMeetingStatus", "UpdateMeetingStatus", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Meetings/UpdateMeetingStatus", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("UpdateMeetingStatus", "UpdateMeetingStatus", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Meetings/UpdateMeetingStatus", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetAllParticipantsListByMeetingID")]
        public JsonStandardResponse GetAllParticipantsListByMeetingID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAllParticipantsListForMeeting requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.MeetingID == null || requestobj.MeetingID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TaskID is mandatory for fetching paticipants list!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.MeetingID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for MeetingID should be in number form!"
                        };
                        return result;
                    }
                    bool flag = new MeetingManagement().getMeetingByID(requestobj.MeetingID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid MeetingID doesn't exists!"
                        };
                        return result;
                    }
                }



                #endregion

                List<AssignedUsers> obj = new MeetingManagement().getAllParticipantsListByMeetingID(requestobj.MeetingID, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no participants found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllParticipantsListByMeetingID", "GetAllParticipants", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Meetings/GetAllParticipantsListByMeetingID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllParticipantsListByMeetingID", "GetAllParticipants", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Meetings/GetAllParticipantsListByMeetingID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetMeetingDetailsByMeetingID")]
        public JsonStandardResponse GetMeetingDetailsByMeetingID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetMeetingDetails requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.MeetingID == null || requestobj.MeetingID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "MeetingID is mandatory for fetching details of meeting!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.MeetingID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for MeetingID should be in number form!"
                        };
                        return result;
                    }
                    bool flag = new MeetingManagement().getMeetingByID(requestobj.MeetingID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid MeetingID doesn't exists!"
                        };
                        return result;
                    }
                }



                #endregion

                MeetingObject obj = new MeetingManagement().getMeetingDetails(requestobj.MeetingID, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no meeting details found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetMeetingDetailsByMeetingID", "GetMeetingDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Meetings/GetMeetingDetailsByMeetingID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetMeetingDetailsByMeetingID", "GetMeetingDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Meetings/GetMeetingDetailsByMeetingID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

    }
}
