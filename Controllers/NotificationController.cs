using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApiOauth2.DAL.CallingMethods;
using WebApiOauth2.Helper_Code.Classes;


namespace WebApiOauth2.Controllers
{
    [Authorize]
    [RoutePrefix("api/Notification")]
    public  class NotificationController : ApiController
    {
        NotificationSender s = new NotificationSender();

        Notifications notficate = new Notifications();
        [HttpPost]
        [Route("sendnotification")]
       // [AllowAnonymous]
        public void sendnotification([FromBody] WebApiOauth2.Models.RequestModels.NotificationResponse requestobj)
        {

            var serializer = new JavaScriptSerializer();
            

           

            var result = s.getfcm(requestobj.id, requestobj.action,requestobj.module, Constants.GetConnectionString());

            for (int i = 0; i < result.FCM_Token.Count; i++)
            {
                var body = serializer.Deserialize<notificationresonse>(result.body[i]);

                var res = notficate.SendNotification(body);
                
            }

            
            
        }


    }

  public  class notificationresonse
    {


        public string to { get; set; }
        public string collapse_key { get; set; }
        public notification notification { get; set; }
        public data data { get; set; }


    }



    public class notification
    {

        public string body { get; set; }

        public string title { get; set; }
    }


    public class data
    {
        public string UserID { get; set; }
        public string screenName { get; set; }


        public string teamID { get; set; }
        public string taskID { get; set; }

        public string eventIds { get; set; }
        public string featureId { get; set; }
        public string eventCodes { get; set; }
    }

}
