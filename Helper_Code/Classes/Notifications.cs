using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using WebApiOauth2.Controllers;
using WebApiOauth2.Models;


namespace WebApiOauth2.Helper_Code.Classes
{
    public  class Notifications
    {

         public Notifications()
    {
        // TODO: Add constructor logic here
    }


    public bool Successful
    {
        get;
        set;
    }

    public string Response
    {
        get;
        set;
    }

    public string module
    {

        get;
        set;
    }

    public string action { get; set; }

    public Exception Error
    {
        get;
        set;
    }
    public Boolean SendNotification(notificationresonse body)
    {
        bool r = false;
        Notifications result = new Notifications();

         try
        {
            result.Successful = true;

            
                      
           // var value = message;
            var requestUri = "https://fcm.googleapis.com/fcm/send";

            WebRequest webRequest = WebRequest.Create(requestUri);
            webRequest.Method = "POST";
            webRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA67gsm4w:APA91bEQGKD7YjXBhztuVzWsRpX7m_JcSB9o9rOkiQTD4m4HwQ5NdYRv6psBg1XZTEeigFkD-nQkQ24aa4GYxGyTMd5K_Gu_WdXZwuo3G2kJIR4oY6wfSZ2Zg7IdYdbsXjNZBEoNsJbi"));
            webRequest.Headers.Add(string.Format("Sender: id={0}", "1012407245708"));
            webRequest.ContentType = "application/json";

            //if (body.data.screenName == "TeamDetail")
            //{

            //}
                
                //var data = new
                //{
                //    // to = YOUR_FCM_DEVICE_ID, // Uncoment this if you want to test for single device
                //    to =To
                //    ,
                //    notification = new
                //    {
                //        title = module,
                //        body = _message,
                //        key_1 = action
                //        //icon="myicon"
                //    },
                //    data = new
                //    {
                //        screenName = "",
                //        teamID= ""  
 

                //    }
                //};
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(body);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                webRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = webResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                var a = serializer.Deserialize<ResponseCheck>(sResponseFromServer);
                                result.Response = sResponseFromServer;
                                result.module = module;
                                result.action = action;
                                if (a.success==1)
                                {
                                    r = true;

                                }
                                else
                                {
                                    r=false;
                                }

                            }
                        }
                    }
                }
               


         }
        catch(Exception ex)
        {
            result.Successful = false;
            result.Response = null;
            result.action = null;

            result.module=null;

            result.Error = ex;
            r = false;
          
        }
        return r;
    }
        
    }

    class ResponseCheck
    {

        public string multicast_id { get; set; }
        public int success { get; set; }

        public int failure { get; set; }


        public int canonical_ids { get; set; }

        public result result { get; set; }
    
    }

    class result
    {

        public string message_id { get; set; }
    }

}

