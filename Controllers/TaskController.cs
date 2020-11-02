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
using System.Web.Script.Serialization;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.DAL.Utils;
using WebApiOauth2.ENT;
using WebApiOauth2.ENT.TaskManagement;
using WebApiOauth2.Helper_Code.Classes;
using WebApiOauth2.Models;

namespace WebApiOauth2.Controllers
{
    [Authorize]
    [RoutePrefix("api/Tasks")]
    public class TaskController : ApiController
    {
        string ApplicationURL = ConfigurationManager.AppSettings["ApplicationURL"];

        [HttpPost]
        [Route("GetAllTasks")]
        public JsonStandardResponse GetAllTasks([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAllTasksList requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching task list!"
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

                if (requestobj.TeamID != null)
                {
                    if(requestobj.TeamID != "")
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
                }

                #endregion

                List<TaskObject> obj = new TaskManagement().getAllTasks(requestobj.UserID, requestobj.TeamID, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no tasks found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllTasks", "GetTasks", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/GetAllTasks", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllTasks", "GetTasks", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/GetAllTasks", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetAllTasksWithStats")]
        public JsonStandardResponse GetAllTasksWithStats([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAllTasksList requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for fetching task list!"
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


                if (requestobj.TeamID != null)
                {
                    if (requestobj.TeamID != "")
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
                }


                #endregion

                TaskObjectWithStats obj = new TaskManagement().getAllTasksWithStats(requestobj.UserID, requestobj.TeamID, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no tasks found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllTasksWithStats", "GetTasksWithStats", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/GetAllTasksWithStats", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllTasksWithStats", "GetTasksWithStats", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/GetAllTasksWithStats", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpGet]
        [Route("GetAllKeyValues")]
        public JsonStandardResponse GetAllKeyValues()
        {
            JsonStandardResponse result = null;
            try
            {
                List<TaskManagementKeyValues> obj = new TaskManagement().getAllKeyValues(Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("GetAllKeyValues", "GetAllKeyValues", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/GetAllKeyValues", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllKeyValues", "GetAllKeyValues", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/GetAllKeyValues", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpGet]
        [Route("GetAllUsersAndTeams")]
        public JsonStandardResponse GetAllUsersAndTeams()
        {
            JsonStandardResponse result = null;
            try
            {
                List<TaskManagementUsersAndTeams> obj = new TaskManagement().getAllUsersAndTeams(Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("GetAllUsers", "GetAllUsers", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/GetAllUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllUsers", "GetAllUsers", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/GetAllUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

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

                if (requestobj.UserID == null || requestobj.UserID == "")
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

                List<TaskManagementUsersAndTeams> obj = new TaskManagement().getAllUsersAndTeamsByUserID(requestobj.UserID, Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("GetAllUsers", "GetAllUsers", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/GetAllUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllUsers", "GetAllUsers", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/GetAllUsers", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("AddUpdateTask")]
      
     //[AllowAnonymous]
        public  JsonStandardResponse AddUpdateTask([FromBody] WebApiOauth2.Models.RequestModels.RequestModelAddUpdateTask requestobj)
        {
            TaskObject obj = new TaskObject();
            obj.AttachmentFiles = new List<string>();
            obj.NotifyUsers = new List<string>();
            obj.AssignedUsersOrTeams = new List<AssignedUsersOrTeams>();

            bool flag = false;
            JsonStandardResponse result = null;
            
            try
            {
                #region Validations

                if (requestobj.Title_Eng == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = null,
                        message = "Title is mandatory for task!"
                    };
                    return result;
                }
                if (requestobj.TaskDescription_Eng == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = null,
                        message = "Description is mandatory for task!"
                    };
                    return result;
                }
                if (requestobj.TaskID != "")
                {
                    int temp;
                    if (!int.TryParse(requestobj.TaskID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
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
                            data = null,
                            message = "invalid TaskID doesn't exists!"
                        };
                        return result;
                    }
                }
                if (requestobj.ParentTaskID != "")
                {
                    int temp;
                    if (!int.TryParse(requestobj.ParentTaskID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data =null,
                            message = "invalid data for ParentTaskID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().getTaskByID(requestobj.ParentTaskID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data =null,
                            message = "invalid ParentTaskID doesn't exists!"
                        };
                        return result;
                    }
                }
                if (requestobj.TaskPriorityId != "")
                {
                    int temp;
                    if (!int.TryParse(requestobj.TaskPriorityId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
                            message = "invalid data for TaskPriorityId should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().getKeyValueOfTaskManagementByID(requestobj.TaskPriorityId, "Priority", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
                            message = "invalid TaskPriorityId doesn't exists!"
                        };
                        return result;
                    }
                }
                if (requestobj.TaskStatusId != "")
                {
                    int temp;
                    if (!int.TryParse(requestobj.TaskStatusId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
                            message = "invalid data for TaskStatusId should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().getKeyValueOfTaskManagementByID(requestobj.TaskStatusId, "Status", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data =null,
                            message = "invalid TaskStatusId doesn't exists!"
                        };
                        return result;
                    }
                }
                if (requestobj.TaskTypeId != "")
                {
                    int temp;
                    if (!int.TryParse(requestobj.TaskTypeId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
                            message = "invalid data for TaskTypeId should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().getKeyValueOfTaskManagementByID(requestobj.TaskTypeId, "Type", Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
                            message = "invalid TaskTypeId doesn't exists!"
                        };
                        return result;
                    }
                }
                if (requestobj.SessionUserID != "")
                {
                    int temp;
                    if (!int.TryParse(requestobj.SessionUserID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
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
                            data = null,
                            message = "invalid SessionUserID doesn't exists!"
                        };
                        return result;
                    }
                }
                if (requestobj.StartDate != "")
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
                            message = "invalid date format for StartDate required is MM/dd/yyyy!"
                        };
                        return result;
                    }
                }
                if (requestobj.EndDate != "")
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = DateTime.ParseExact(requestobj.EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = null,
                            message = "invalid date format for EndDate required is MM/dd/yyyy!"
                        };
                        return result;
                    }
                }

                if (requestobj.AssignedUsersOrTeams == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = null,
                        message = "AssignedUsersOrTeams is mandatory for task!"
                    };
                    return result;
                }
                if (requestobj.AssignedUsersOrTeams.Count == 0)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = null,
                        message = "AssignedUsersOrTeams cannot be empty for task!"
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
                                data = null,
                                message = "Type in AssignedUsersOrTeams is mandatory for task!"
                            };
                            return result;
                        }
                        if (value.Type != "User" && value.Type != "Team")
                        {
                            result = new JsonStandardResponse
                            {
                                status = "error",
                                data = null,
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
                                data = null,
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
                                data = null,
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

                if (requestobj.NotifyUsers != null)
                {
                    if (requestobj.NotifyUsers.Count != 0)
                    {
                        foreach (string user in requestobj.NotifyUsers)
                        {
                            int temp;
                            if (!int.TryParse(user, out temp))
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = null,
                                    message = "invalid data for UserId in NotifyUsers should be in number form!"
                                };
                                return result;
                            }
                            flag = new TaskManagement().checkUserOrTeamByID(user, "User", Constants.GetConnectionString());
                            if (!flag)
                            {
                                result = new JsonStandardResponse
                                {
                                    status = "error",
                                    data = null,
                                    message = "invalid UserId in NotifyUsers! doesn't exists"
                                };
                                return result;
                            }
                            obj.NotifyUsers.Add(user);
                        }
                    }
                }

                #endregion

                if (requestobj.base64FileObjects != null)
                {
                    if (requestobj.base64FileObjects.Count != 0)
                    {
                        foreach(string base64FileObject in requestobj.base64FileObjects)
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
                                            data =null,
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
                                            data = null,
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

                obj.TaskID = (requestobj.TaskID == null || requestobj.TaskID == "") ? "0" : requestobj.TaskID;
                obj.ParentTaskID = requestobj.ParentTaskID;
                obj.Title_Eng = requestobj.Title_Eng;
                obj.Title_Arb = requestobj.Title_Arb;
                obj.TaskDescription_Eng = requestobj.TaskDescription_Eng;
                obj.TaskDescription_Arb = requestobj.TaskDescription_Arb;
                obj.Progress = requestobj.Progress;
                obj.TaskStatusId = requestobj.TaskStatusId;
                obj.TaskPriorityId = requestobj.TaskPriorityId;
                obj.TaskTypeId = requestobj.TaskTypeId;
                obj.SessionUserID = requestobj.SessionUserID;
                obj.StartDate = requestobj.StartDate;
                obj.EndDate = requestobj.EndDate;

                flag = new TaskManagement().AddUpdateTask(obj,Constants.GetConnectionString());

                if (flag)
                {
                     var notisender = new NotificationController();
                    if (obj.TaskID == "0")
                    {
                        if (requestobj.AssignedUsersOrTeams[0].Type =="Team")
                        {
                            var objres = new WebApiOauth2.Models.RequestModels.NotificationResponse
                            {
                                id = "",
                                action = "New Task For Team",
                                module = "Task"
                            };
                            notisender.sendnotification(objres);
                        }
                        else
                        {
                            var objres = new WebApiOauth2.Models.RequestModels.NotificationResponse
                            {
                                id = "",
                                action = "New Task For User",
                                module = "Task"
                            };
                            notisender.sendnotification(objres);
                        }
                       
                       
                    }
                    else
                    {
                        if (requestobj.AssignedUsersOrTeams[0].Type == "Team")
                        {
                            var objres = new WebApiOauth2.Models.RequestModels.NotificationResponse
                            {
                                id = "",
                                action = "Update Task For Team",
                                module = "Task"
                            };
                            notisender.sendnotification(objres);
                        }
                        else
                        {
                            var objres = new WebApiOauth2.Models.RequestModels.NotificationResponse
                            {
                                id = "",
                                action = "Update Task For User",
                                module = "Task"
                            };
                            notisender.sendnotification(objres);
                        }
                    }
                }

                List<string> ids=new List<string>();
                string type="";

                foreach (var item in requestobj.AssignedUsersOrTeams)
              	{
                    ids.Add(item.ID);
		            type=item.Type;
	           }

             
              
               
                
                if (!flag) 
                          {   
                    result = new JsonStandardResponse
                    { 
               
             status = "success",
             data = "",
                message = "Request Successful!",
              
              
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {

                  
                    status = "success",
              data="",
                    message = "Request Successful!",
                   

                 
                };
                new BusinessLogic().CreateLog("AddUpdateTask", "AddUpdateTask", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/AddUpdateTask", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = null,
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("AddUpdateTask", "AddUpdateTask", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/AddUpdateTask", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

      

        [HttpPost]
        [Route("GetAllParticipantsListByTaskID")]
        public JsonStandardResponse GetAllParticipantsListByTaskID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAllParticipantsListForTask requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.TaskID == null || requestobj.TaskID == "")
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



                #endregion

                List<AssignedUsers> obj = new TaskManagement().getAllParticipantsListByTaskID(requestobj.TaskID, Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("GetAllParticipantsListByTaskID", "GetAllParticipants", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/GetAllParticipantsListByTaskID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllParticipantsListByTaskID", "GetAllParticipants", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/GetAllParticipantsListByTaskID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("GetTaskDetailsByTaskID")]
        public JsonStandardResponse GetTaskDetailsByTaskID([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetTaskDetails requestobj)
        {
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.TaskID == null || requestobj.TaskID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TaskID is mandatory for fetching details of task!"
                    };
                    return result;
                }
                else
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



                #endregion

                TaskObject obj = new TaskManagement().getTaskDetails(requestobj.TaskID, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no task details found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetTaskDetailsByTaskID", "GetTaskDetails", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/GetTaskDetailsByTaskID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetTaskDetailsByTaskID", "GetTaskDetails", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/GetTaskDetailsByTaskID", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpPost]
        [Route("AddTaskComment")]
        public JsonStandardResponse AddTaskComment([FromBody] WebApiOauth2.Models.RequestModels.RequestModelAddTaskComment requestobj)
        {
            TaskLog obj = new TaskLog();
            obj.AttachmentFiles = new List<string>();
            bool flag = false;
            JsonStandardResponse result = null;
            try
            {
                #region Validations

                if (requestobj.TaskID == null || requestobj.TaskID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "TaskID is mandatory for inserting task comment!"
                    };
                    return result;
                }
                else
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

                if (requestobj.UserId == null || requestobj.UserId == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for inserting task comment!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.UserId, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for UserID should be in number form!"
                        };
                        return result;
                    }
                    flag = new TaskManagement().checkUserOrTeamByID(requestobj.UserId, "User", Constants.GetConnectionString());
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

                if (requestobj.Comment == null || requestobj.Comment == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Comment is mandatory for inserting task comment!"
                    };
                    return result;
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

                obj.UserId = requestobj.UserId;
                obj.TaskID = requestobj.TaskID;
                obj.Comment = requestobj.Comment;
                flag = new TaskManagement().AddTaskComment(obj, Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("AddTaskComment", "AddTaskComment", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Tasks/AddTaskComment", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("AddTaskComment", "AddTaskComment", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Tasks/AddTaskComment", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            return result;
        }


        
    }
}
