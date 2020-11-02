using System;
using System.Collections.Generic;
using System.Configuration;
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
    [RoutePrefix("api/Wall")]
    public class WallController : ApiController
    {
        string ApplicationURL = ConfigurationManager.AppSettings["ApplicationURL"];

        [HttpPost]
        [Route("GetAllWallPosts")]
        public JsonStandardResponse GetAllWallPosts([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetAllWallPosts requestobj)
        {
            WallPost obj = new WallPost();
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
                        message = "invalid data for GetAllWallPosts api"
                    };
                    return result;
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for getting data of wall posting!"
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

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for getting data of wall posting!"
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

                if (requestobj.PostID == null || requestobj.PostID == "")
                {
                    requestobj.PostID = "0";
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostByID(requestobj.PostID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PageNumber == null || requestobj.PageNumber == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PageNumber is mandatory for getting data of wall posting!"
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
                            message = "invalid data for PageNumber should be in number form!"
                        };
                        return result;
                    }
                }


                #endregion

                obj.UserId = requestobj.UserID;
                obj.EventID = requestobj.EventID;
                obj.ID = requestobj.PostID;
                obj.PageNumber = requestobj.PageNumber;
                List<EventWallPostDetail> resultobj = new WallManagement().getEventWallPostDetails(obj, Constants.GetConnectionString());
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
                    data = resultobj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAllWallPosts", "GetAllWallPosts", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Wall/GetAllWallPosts", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAllWallPosts", "GetAllWallPosts", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Wall/GetAllWallPosts", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            return result;
        }

        [HttpPost]
        [Route("WallAddPost")]
        public JsonStandardResponse WallAddPost([FromBody] WebApiOauth2.Models.RequestModels.RequestModelWallAddPost requestobj)
        {
            WallPost obj = new WallPost();
            obj.AttachmentFiles = new List<string>();
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
                        message = "invalid data for WallAddPost api"
                    };
                    return result;
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for wall posting!"
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

                if (requestobj.FeatureID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "FeatureID is mandatory for wall posting!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.FeatureID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for FeatureID should be in number form!"
                        };
                        return result;
                    }


                    flag = new EventManagement().getFeatureByID(requestobj.FeatureID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid FeatureID doesn't exists or invalid for wall posting!"
                        };
                        return result;
                    }

                }

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for wall posting!"
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
                    flag = new WallManagement().checkUserStatusAllowedWallPosting(requestobj.UserID, requestobj.EventID, requestobj.FeatureID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "wall posting not allowed!"
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
                                    string filename = Helper_Code.Classes.Constants.AppendTimeStamp("wallattachment" + fileextension);
                                    string filePath = HttpContext.Current.Server.MapPath("~/WallPostAttachments/" + filename);
                                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/WallPostAttachments")))
                                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/WallPostAttachments"));

                                    System.IO.FileStream stream = new FileStream(@filePath, FileMode.CreateNew);
                                    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                                    writer.Write(bytes, 0, bytes.Length);
                                    writer.Close();


                                    obj.AttachmentFiles.Add(((filename != "") ? (ApplicationURL + "/WallPostAttachments/" + filename) : ""));
                                }

                                #endregion
                            }
                        }
                    }
                }

                obj.UserId = requestobj.UserID;
                obj.EventID = requestobj.EventID;
                obj.FeatureID = requestobj.FeatureID;
                obj.Post_Content_Eng = requestobj.Post_Content_Eng;
                obj.Post_Content_Arb = requestobj.Post_Content_Arb;
                flag = new WallManagement().WallAddPost(obj, Constants.GetConnectionString());
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
                new BusinessLogic().CreateLog("WallAddPost", "WallAddPost", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Wall/WallAddPost", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("WallAddPost", "WallAddPost", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Wall/WallAddPost", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            return result;
        }

        [HttpPost]
        [Route("WallPostLikeDislike")]
        public JsonStandardResponse WallPostLikeDislike([FromBody] WebApiOauth2.Models.RequestModels.RequestModelPostLikeDislike requestobj)
        {
            WallPost obj = new WallPost();
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
                        message = "invalid data for WallPostLikeDislike api"
                    };
                    return result;
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for wall post liking!"
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

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for wall post liking!"
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

                if (requestobj.PostID == null || requestobj.PostID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PostID is mandatory for wall post liking!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostByID(requestobj.PostID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PostAttachmentID == null || requestobj.PostAttachmentID == "")
                {
                    requestobj.PostAttachmentID = "0";
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostAttachmentID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostAttachmentID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostAttachmentByID(requestobj.PostAttachmentID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostAttachmentID doesn't exists!"
                        };
                        return result;
                    }
                }

                #endregion

                obj.UserId = requestobj.UserID;
                obj.EventID = requestobj.EventID;
                obj.ID = requestobj.PostID;
                obj.PostAttachmentID = requestobj.PostAttachmentID;
                EventWallPostLikeDetail resultobj = new WallManagement().WallPostLikeDislike(obj, Constants.GetConnectionString());
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
                    data = resultobj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("WallPostLikeDislike", "WallPostLikeDislike", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Wall/WallPostLikeDislike", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("WallPostLikeDislike", "WallPostLikeDislike", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Wall/WallPostLikeDislike", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            return result;
        }

        [HttpPost]
        [Route("WallPostComment")]
        public JsonStandardResponse WallPostComment([FromBody] WebApiOauth2.Models.RequestModels.RequestModelWallPostComment requestobj)
        {
            WallPost obj = new WallPost();
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
                        message = "invalid data for WallPostComment api"
                    };
                    return result;
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for wall comment posting!"
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

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for wall comment posting!"
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

                if (requestobj.PostID == null || requestobj.PostID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PostID is mandatory for wall comment posting!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostByID(requestobj.PostID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PostAttachmentID == null || requestobj.PostAttachmentID == "")
                {
                    requestobj.PostAttachmentID = "0";
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostAttachmentID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostAttachmentID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostAttachmentByID(requestobj.PostAttachmentID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostAttachmentID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PostCommentEng == null || requestobj.PostCommentEng == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PostCommentEng is mandatory for wall comment posting!"
                    };
                    return result;
                }

                if (requestobj.PostCommentArb == null || requestobj.PostCommentArb == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PostCommentArb is mandatory for wall comment posting!"
                    };
                    return result;
                }

                #endregion

                obj.UserId = requestobj.UserID;
                obj.EventID = requestobj.EventID;
                obj.ID = requestobj.PostID;
                obj.PostAttachmentID = requestobj.PostAttachmentID;
                obj.PostCommentEng = requestobj.PostCommentEng;
                obj.PostCommentArb = requestobj.PostCommentArb;
                //List<CommentNode> resultobj = new WallManagement().WallPostComment(obj, Constants.GetConnectionString());
                EventWallPostComment resultobj = new WallManagement().WallPostComment(obj, Constants.GetConnectionString());
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
                    data = resultobj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("WallPostComment", "WallPostComment", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Wall/WallPostComment", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("WallPostComment", "WallPostComment", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Wall/WallPostComment", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            return result;
        }

        [HttpPost]
        [Route("GetPostComment")]
        public JsonStandardResponse GetPostComment([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetPostComment requestobj)
        {
            WallPost obj = new WallPost();
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
                        message = "invalid data for GetPostComment api"
                    };
                    return result;
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for getting wall comments!"
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

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for getting wall comments!"
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

                if (requestobj.PostID == null || requestobj.PostID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PostID is mandatory for getting wall comments!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostByID(requestobj.PostID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PostAttachmentID == null || requestobj.PostAttachmentID == "")
                {
                    requestobj.PostAttachmentID = "0";
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostAttachmentID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostAttachmentID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostAttachmentByID(requestobj.PostAttachmentID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostAttachmentID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PageNumber == null || requestobj.PageNumber == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PageNumber is mandatory for getting wall comments!"
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
                            message = "invalid data for PageNumber should be in number form!"
                        };
                        return result;
                    }
                }

                #endregion

                obj.UserId = requestobj.UserID;
                obj.EventID = requestobj.EventID;
                obj.ID = requestobj.PostID;
                obj.PostAttachmentID = requestobj.PostAttachmentID;
                obj.PageNumber = requestobj.PageNumber;
                List<CommentNode> resultobj = new WallManagement().GetPostComment(obj, Constants.GetConnectionString());
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
                    data = resultobj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetPostComment", "GetPostComment", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Wall/GetPostComment", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetPostComment", "GetPostComment", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Wall/GetPostComment", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            return result;
        }

        [HttpPost]
        [Route("GetPostLike")]
        public JsonStandardResponse GetPostLike([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetPostLikes requestobj)
        {
            WallPost obj = new WallPost();
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
                        message = "invalid data for GetPostLike api"
                    };
                    return result;
                }

                if (requestobj.EventID == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "EventID is mandatory for getting wall likes!"
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

                if (requestobj.UserID == null || requestobj.UserID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "UserID is mandatory for getting wall likes!"
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

                if (requestobj.PostID == null || requestobj.PostID == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PostID is mandatory for getting wall likes!"
                    };
                    return result;
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostByID(requestobj.PostID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PostAttachmentID == null || requestobj.PostAttachmentID == "")
                {
                    requestobj.PostAttachmentID = "0";
                }
                else
                {
                    int temp;
                    if (!int.TryParse(requestobj.PostAttachmentID, out temp))
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid data for PostAttachmentID should be in number form!"
                        };
                        return result;
                    }
                    flag = new WallManagement().getPostAttachmentByID(requestobj.PostAttachmentID, Constants.GetConnectionString());
                    if (!flag)
                    {
                        result = new JsonStandardResponse
                        {
                            status = "error",
                            data = "",
                            message = "invalid PostAttachmentID doesn't exists!"
                        };
                        return result;
                    }
                }

                if (requestobj.PageNumber == null || requestobj.PageNumber == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "PageNumber is mandatory for getting wall likes!"
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
                            message = "invalid data for PageNumber should be in number form!"
                        };
                        return result;
                    }
                }

                #endregion

                obj.UserId = requestobj.UserID;
                obj.EventID = requestobj.EventID;
                obj.ID = requestobj.PostID;
                obj.PostAttachmentID = requestobj.PostAttachmentID;
                obj.PageNumber = requestobj.PageNumber;
                List<LikeReactionNode> resultobj = new WallManagement().GetPostLike(obj, Constants.GetConnectionString());
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
                    data = resultobj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetPostLike", "GetPostLike", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Wall/GetPostLike", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetPostLike", "GetPostLike", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Wall/GetPostLike", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            return result;
        }
    }
}
