using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web;
using System.IO;
using System.Drawing;
using WebApiOauth2.ENT;
using WebApiOauth2.Helper_Code.Classes;
using WebApiOauth2.DAL.Utils;
using System.Configuration;
using WebApiOauth2.ENT.Announcement;
using WebApiOauth2.ENT.Document;

namespace WebApiOauth2.Controllers
{
    [Authorize]
    [RoutePrefix("api/WebApi")]
    public class WebApiController : ApiController
    {
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("sendnotification")]
        //public string sendnotification(string To, string _title, string _message)
        //{
        //    var a = new Notifications();
        //    var result = a.SendNotification(To, _title, _message,"","");
        //    if (result.Response != null)
        //    {
        //        return result.Response;
        //    }
        //    else
        //    {
        //        return result.Error.Message;
        //    }


          
            
        //}

        // GET api/WebApi
        [HttpGet]
        [Route("GetDataValues")]
        public IEnumerable<string> GetDataValues()
        {
            return new string[] { "Hello REST API", "I am Authorized" };
        }

        // GET api/WebApi/5
        [HttpGet]
        [Route("GetDataValue/{id}")]
        public string GetDataValue(int id)
        {
            return "Hello Authorized API with ID = " + id;
        }

        

        [HttpPost]
        [Route("UpdateUserProfile")]
        public JsonStandardResponse UpdateUserProfile([FromBody] WebApiOauth2.Models.RequestModels.RequestModelUpdateUserProfile requestobj)
        {
           string userName = requestobj.userName;
           userName = (String.IsNullOrEmpty(userName)) ? "" : userName;
           string passWord = requestobj.newpassWord;
           passWord = (String.IsNullOrEmpty(passWord)) ? "" : passWord;

           string fullName = requestobj.fullName;
           fullName = (String.IsNullOrEmpty(fullName)) ? "" : fullName;
           //string emirateID = requestobj.emirateID;
           //emirateID = (String.IsNullOrEmpty(emirateID)) ? "" : emirateID;
           //string licenseNo = requestobj.licenseNo;
           //licenseNo = (String.IsNullOrEmpty(licenseNo)) ? "" : licenseNo;
           string emailAddr = requestobj.emailAddr;
           emailAddr = (String.IsNullOrEmpty(emailAddr)) ? "" : emailAddr;
           string addressHome = requestobj.addressHome;
           addressHome = (String.IsNullOrEmpty(addressHome)) ? "" : addressHome;

           //string pictureUrl = requestobj.pictureUrl;
           //pictureUrl = (String.IsNullOrEmpty(pictureUrl)) ? "" : pictureUrl;

           string phoneNo = requestobj.phoneNo;
           phoneNo = (String.IsNullOrEmpty(phoneNo)) ? "" : phoneNo;



            string deviceDetails = requestobj.deviceDetails;
            deviceDetails = (String.IsNullOrEmpty(deviceDetails)) ? "" : deviceDetails;
            string deviceUDID = requestobj.deviceUDID;
            deviceUDID = (String.IsNullOrEmpty(deviceUDID)) ? "" : deviceUDID;
            string deviceTYPE = requestobj.deviceTYPE;
            deviceTYPE = (String.IsNullOrEmpty(deviceTYPE)) ? "" : deviceTYPE;
            string mobileDatetime = requestobj.mobileDatetime;
            mobileDatetime = (String.IsNullOrEmpty(mobileDatetime)) ? "" : mobileDatetime;
            string serviceTYPE = requestobj.serviceTYPE;
            serviceTYPE = (String.IsNullOrEmpty(serviceTYPE)) ? "" : serviceTYPE;

            string fileObject = requestobj.base64FileObject;
            fileObject = (String.IsNullOrEmpty(fileObject)) ? "" : fileObject;

            Users userobject = new Users();
            userobject.userName = userName;
            userobject.passWord = passWord;
            userobject.fullName = fullName;
            //userobject.emirateID = emirateID;
            //userobject.licenseNo = licenseNo;
            userobject.emailAddr = emailAddr;
            userobject.addressHome = addressHome;

            //userobject.pictureUrl = pictureUrl;

            userobject.phoneNo = phoneNo;

            userobject.deviceDetails = deviceDetails;
            userobject.deviceUDID = deviceUDID;
            userobject.deviceTYPE = deviceTYPE;
            userobject.mobileDatetime = mobileDatetime;
            userobject.serviceTYPE = serviceTYPE;


           bool flag = false;
           JsonStandardResponse result = null;
           try
           {
               DateTime dateTime;
               try
               {
                   dateTime = DateTime.ParseExact(userobject.mobileDatetime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
               }
               catch (FormatException)
               {
                   result = new JsonStandardResponse
                   {
                       status = "error",
                       data = "",
                       message = "invalid datetime format required is MM-dd-yyyy HH:mm:ss"
                   };
                   return result;
               }

               if (requestobj.deviceUDID == "" || requestobj.userName == "")
               {
                   result = new JsonStandardResponse
                   {
                       status = "error",
                       data = "",
                       message = "device udid or user name cannot be empty!"
                   };
                   return result;
               }

               //Login Status Verification.
               Users obj = new UserProfile().checkUserLoginStatus(userobject, Constants.GetConnectionString());
               if (obj == null)
               {
                   result = new JsonStandardResponse
                   {
                       status = "error",
                       data = "",
                       message = "no session found against device udid and user name!"
                   };
                   return result;
               }

               if (requestobj.serviceTYPE.ToLower() == "updateuserprofile")
               {
                   string filename = "";

                   #region Base 64 Image Processing Work
                   if (fileObject != "")
                   {
                       int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB
                       IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                       string fileextension = WebApiOauth2.ENT.Utilities.GetMimeType(fileObject).Extension;
                       if (!AllowedFileExtensions.Contains(fileextension))
                       {
                           result = new JsonStandardResponse
                           {
                               status = "error",
                               data = "",
                               message = "Please Upload image of type .jpg,.png."
                           };
                           return result;
                           //var message = string.Format("Please Upload image of type .jpg,.png.");
                           //dict.Add("error", message);
                           //return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                       }
                       byte[] bytes = Convert.FromBase64String(fileObject);
                       if (bytes.Length > MaxContentLength)
                       {
                           result = new JsonStandardResponse
                           {
                               status = "error",
                               data = "",
                               message = "Please Upload a file upto 1 mb."
                           };
                           return result;
                           //var message = string.Format("Please Upload a file upto 1 mb.");
                           //dict.Add("error", message);
                           //return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                       }
                       Image image;
                       using (MemoryStream ms = new MemoryStream(bytes))
                       {
                           image = Image.FromStream(ms);
                           filename = Helper_Code.Classes.Constants.AppendTimeStamp("userimage" + fileextension);
                           string filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + filename);
                           if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Userimage")))
                               Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Userimage"));
                           image.Save(filePath);
                       }
                   }

                   #endregion

                   string ApplicationURL = ConfigurationManager.AppSettings["ApplicationURL"];
                   userobject.pictureUrl = ((filename != "") ? (ApplicationURL + "/Userimage/" + filename) : "");
                   flag = new UserProfile().UpdateUserProfile(userobject, Constants.GetConnectionString());
               }
               else if (requestobj.serviceTYPE.ToLower() == "updateuserpassword")
               {
                   if (requestobj.oldpassWord.Length < 8 || requestobj.newpassWord.Length < 8)
                   {
                       result = new JsonStandardResponse
                       {
                           status = "error",
                           data = "",
                           message = "Old Password and New Password length must be equal or greater than 8 characters!"
                       };
                       return result;
                   }
                   if (requestobj.oldpassWord == requestobj.newpassWord)
                   {
                       result = new JsonStandardResponse
                       {
                           status = "error",
                           data = "",
                           message = "Old Password and New Password cannot be same!"
                       };
                       return result;
                   }
                   flag = new UserProfile().UpdateUserProfile(userobject, Constants.GetConnectionString());
               }
               else if (requestobj.serviceTYPE.ToLower() == "updateuserimage")
               {
                   #region Base 64 Image Processing Work

                   if (fileObject == "")
                   {
                       result = new JsonStandardResponse
                       {
                           status = "error",
                           data = "",
                           message = "base64FileObject cannot be empty!"
                       };
                       return result;
                   }
                   int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB
                   IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                   string fileextension = WebApiOauth2.ENT.Utilities.GetMimeType(fileObject).Extension;
                   string filename = "";
                   if (!AllowedFileExtensions.Contains(fileextension))
                   {
                       result = new JsonStandardResponse
                       {
                           status = "error",
                           data = "",
                           message = "Please Upload image of type .jpg,.png."
                       };
                       return result;
                       //var message = string.Format("Please Upload image of type .jpg,.png.");
                       //dict.Add("error", message);
                       //return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                   }
                   byte[] bytes = Convert.FromBase64String(fileObject);
                   if (bytes.Length > MaxContentLength)
                   {
                       result = new JsonStandardResponse
                       {
                           status = "error",
                           data = "",
                           message = "Please Upload a file upto 1 mb."
                       };
                       return result;
                       //var message = string.Format("Please Upload a file upto 1 mb.");
                       //dict.Add("error", message);
                       //return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                   }
                   Image image;
                   using (MemoryStream ms = new MemoryStream(bytes))
                   {
                       image = Image.FromStream(ms);
                       filename = Helper_Code.Classes.Constants.AppendTimeStamp("userimage" + fileextension);
                       string filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + filename);
                       if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Userimage")))
                           Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Userimage"));
                       image.Save(filePath);
                   }

                   #endregion

                   string ApplicationURL = ConfigurationManager.AppSettings["ApplicationURL"];
                   userobject.pictureUrl = ApplicationURL + "/Userimage/" + filename;
                   flag = new UserProfile().UpdateUserProfile(userobject, Constants.GetConnectionString());
               }
               else
               {
                   result = new JsonStandardResponse
                   {
                       status = "error",
                       data = "",
                       message = "invalid request!"
                   };
                   return result;
               }

               Users userobj = new UserProfile().checkUserLoginStatus(userobject, Constants.GetConnectionString());

               result = new JsonStandardResponse
               {
                   status = (flag)?"success":"error",
                   data = (flag) ? userobj : null,
                   message = (flag) ? "Request Successful!" : "Request Failed!"
               };
               new BusinessLogic().CreateLog("UserProfileUpdate", "UserProfile", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/WebApi/UpdateUserProfile", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

           }
           catch (Exception ex)
           {
               result = new JsonStandardResponse
               {
                   status = "error",
                   data = "",
                   message = ex.Message
               };
               new BusinessLogic().CreateLog("UserProfileUpdate", "UserProfile", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/WebApi/UpdateUserProfile", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

           }
           return result;
        }


        [HttpPost]
        [Route("GetUserDetails")]
        public JsonStandardResponse GetUserDetails([FromBody] WebApiOauth2.Models.RequestModels.RequestModelGetUserDetails requestobj)
        {
            string userID = requestobj.userID;
            userID = (String.IsNullOrEmpty(userID)) ? "" : userID;

            string userName = requestobj.userName;
            userName = (String.IsNullOrEmpty(userName)) ? "" : userName;

            Users userobject = new Users();
            userobject.ID = userID;
            userobject.userName = userName;

            JsonStandardResponse result = null;
            try
            {
                if (requestobj.userID == "" || requestobj.userName == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "user id or user name cannot be empty!"
                    };
                    return result;
                }

                //Login Status Verification.
                Users obj = new UserProfile().getUserByUserNameAndUserID(userobject, Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no session found against user id and user name!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetUserDetails", "GetUser", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/WebApi/GetUserDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetUserDetails", "GetUser", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/WebApi/GetUserDetails", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [Route("UploadUserImage")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            //JsonStandardResponse result = null;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                var filePath = "";
                var filename = "";
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 2 MB  
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        filename = Helper_Code.Classes.Constants.AppendTimeStamp(postedFile.FileName);
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + filename);
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Userimage")))
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Userimage"));
                            postedFile.SaveAs(filePath);
                        }
                    }

                    var message1 = string.Format("Image Created Successfully.");
                    dict.Add("status", "success");
                    dict.Add("data", "/Userimage/" + filename);
                    dict.Add("message", message1);
                    return Request.CreateResponse(HttpStatusCode.Created, dict);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                //var res = string.Format("some Message");
                //dict.Add("errorStackTrace", ex.StackTrace);
                //dict.Add("errorMessage", ex.Message);
                dict.Add("error", "error occured");
                dict.Add("exceptionMessage", ex.Message);
                dict.Add("exceptionDetail", ex.InnerException);
                
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);

            }
            //return result;
        }

        [HttpGet]
        [Route("GetAnnouncements")]
        public JsonStandardResponse GetAnnouncements()
        {
            JsonStandardResponse result = null;
            try
            {
                List<AnnouncementObject> listobj = new UserProfile().getAnnouncements(Constants.GetConnectionString());
                if (listobj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no announcements found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = listobj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetAnnouncements", "GetAnnouncements", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/WebApi/GetAnnouncements", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetAnnouncements", "GetAnnouncements", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/WebApi/GetAnnouncements", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }

        [HttpGet]
        [Route("GetDocuments")]
        public JsonStandardResponse GetDocuments()
        {
            JsonStandardResponse result = null;
            try
            {
                List<DocumentObject> obj = new UserProfile().getDocuments(Constants.GetConnectionString());
                if (obj == null)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "no document found!"
                    };
                    return result;
                }
                result = new JsonStandardResponse
                {
                    status = "success",
                    data = obj,
                    message = "Request Successful!"
                };
                new BusinessLogic().CreateLog("GetDocuments", "GetDocuments", "0", "webapi", result.message, ((result.status == "success") ? "1" : "0"), "api/Event/GetDocuments", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                new BusinessLogic().CreateLog("GetDocuments", "GetDocuments", "0", "webapi", ex.Message, ex.HResult.ToString(), "api/Event/GetDocuments", Request.Headers.Authorization.Parameter, Constants.GetConnectionString());

            }
            return result;
        }


        /*
        // POST api/WebApi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/WebApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/WebApi/5
        public void Delete(int id)
        {
        }
         * 
         * */


    }
}
