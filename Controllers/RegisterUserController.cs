using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.ENT;
using WebApiOauth2.Helper_Code.Classes;
using WebApiOauth2.Models;


using System.Threading.Tasks;
using System.Web;

namespace WebApiOauth2.Controllers
{

    [RoutePrefix("api/Registration")]
    public class RegisterUserController : ApiController
    {
        [HttpPost]
        [Route("UserSignUp")]
        public JsonStandardResponse RegisterUser([FromBody] WebApiOauth2.Models.RequestModels.RequestModelRegisterUsers requestobj)
        {
            string userName = requestobj.userName;
            userName = (String.IsNullOrEmpty(userName)) ? "" : userName;
            string passWord = requestobj.passWord;
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
            string pictureUrl = requestobj.pictureUrl;
            pictureUrl = (String.IsNullOrEmpty(pictureUrl)) ? "" : pictureUrl;
            string phoneNo = requestobj.phoneNo;
            phoneNo = (String.IsNullOrEmpty(phoneNo)) ? "" : phoneNo;


            Users userobject = new Users();
            userobject.userName = userName;
            userobject.passWord = passWord;
            userobject.fullName = fullName;
            //userobject.emirateID = emirateID;
            //userobject.licenseNo = licenseNo;
            userobject.emailAddr = emailAddr;
            userobject.addressHome = addressHome;
            userobject.pictureUrl = pictureUrl;
            userobject.phoneNo = phoneNo;

            JsonStandardResponse result = null;
            try
            {
                if (userobject.userName == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Username cannot be empty!"
                    };
                    return result;
                }

                if (userobject.passWord == "")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Password cannot be empty!"
                    };
                    return result;
                }

                if (userobject.passWord.Length < 8)
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = "Password length must be must be equal or greater than 8 characters!"
                    };
                    return result;
                }

                //Verification.
                string Status = new UserProfile().RegisterUser(userobject, Constants.GetConnectionString());
                if (Status != "Success")
                {
                    result = new JsonStandardResponse
                    {
                        status = "error",
                        data = "",
                        message = Status
                    };
                }
                else
                {
                    result = new JsonStandardResponse
                    {
                        status = "success",
                        data = "",
                        message = "Registration Successful!"
                    };
                }
            }
            catch (Exception ex)
            {
                result = new JsonStandardResponse
                {
                    status = "error",
                    data = "",
                    message = ex.Message
                };
                //new BusinessLogic().CreateLog(1, ex.Message, ex.HResult.ToString(), new Commons().BaseUrl + "AccidentReporting/CheckVehicleDetails");

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

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
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
                            filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                        }
                    }

                    var message1 = string.Format("Image Created Successfully.");
                    dict.Add("status", "success");
                    dict.Add("data", "/Userimage/" + postedFile.FileName);
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
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);

            }
            //return result;
        }

    }
}