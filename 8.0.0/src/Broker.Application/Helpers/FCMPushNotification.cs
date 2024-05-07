using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Broker.Helpers
{
    public class FCMPushNotification
    {
        public FCMPushNotification()
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
        public Exception Error
        {
            get;
            set;
        }

        /*public FCMPushNotification SendNotification( FcmNotificationInput input)
        {
            FCMPushNotification result = new FCMPushNotification();
            try
            {
                result.Successful = true;
                result.Error = null;
                var requestUri = "https://fcm.googleapis.com/fcm/send";
                WebRequest webRequest = WebRequest.Create(requestUri);
                webRequest.Method = "POST";
                webRequest.Headers.Add(string.Format("Authorization: key={0}", new FcmApplication().key));
                webRequest.Headers.Add(string.Format("Sender: id={0}", new FcmApplication().SenderId));
                webRequest.ContentType = "application/json";
                var userInfo = new
                {
                    to = input.RegistrationTokens[0],
                    data = new
                    {
                        title = input.Title,
                        body = input.Body,
                        message = input.Body,
                        sound = "whistle",
                        vibration = "true",
                        show_in_foreground = true,
                    },
                    content_available = true,
                    priority = "high",
                    notification = new
                    {
                        title = input.Title,
                        body = input.Body,
                        text = input.Body,
                        message = input.Body,
                        sound = "whistle",
                        vibration = "true",
                        show_in_foreground = true,
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(userInfo);
                var byteArray = Encoding.UTF8.GetBytes(json);
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
                                result.Response = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }
            return result;
        }*/

        public FCMPushNotification SendNotification(FcmNotificationInput input)
        {
            FCMPushNotification result = new FCMPushNotification();
            try
            {
                result.Successful = true;
                result.Error = null;
                var requestUri = "https://fcm.googleapis.com/fcm/send";

                //var registrationToken = input.RegistrationToken.Replace("\r\n", "");
                var registrationTokens = input.RegistrationTokens?.ToArray();

                WebRequest webRequest = WebRequest.Create(requestUri);

                webRequest.Method = "POST";
                webRequest.Headers.Add(string.Format("Authorization: key={0}", new FcmApplication().key));
                webRequest.Headers.Add(string.Format("Sender: id={0}", new FcmApplication().SenderId));
                webRequest.ContentType = "application/json";
                var userInfo = new
                {
                    registration_ids = registrationTokens,
                   // to = registrationToken,
                  
                    data = new
                    {
                        title = input.Title,
                        body = input.Body,

                        AdvertisementId = input.AdId.HasValue ? input.AdId.Value : 0,

                        BrokerId = input.BrokerId.HasValue ? input.BrokerId.Value : 0,
                        SeekerId = input.SeekerId.HasValue ? input.SeekerId.Value : 0,
                        OwnerId = input.OwnerId.HasValue ? input.OwnerId.Value : 0,
                        CompanyId = input.CompanyId.HasValue ? input.CompanyId.Value : 0,

                        vibration = "true"
                    },
                    content_available = true,
                    priority = "high",
                    notification = new
                    {
                        title = input.Title,
                        body = input.Body,
                        AdvertisementId = input.AdId.HasValue ? input.AdId.Value : 0,
                        BrokerId = input.BrokerId.HasValue ? input.BrokerId.Value : 0,
                        SeekerId = input.SeekerId.HasValue ? input.SeekerId.Value : 0,
                        OwnerId = input.OwnerId.HasValue ? input.OwnerId.Value : 0,
                        CompanyId = input.CompanyId.HasValue ? input.CompanyId.Value : 0,
                        vibration = "true"
                    }
                };
                string json = JsonConvert.SerializeObject(userInfo, Formatting.Indented);
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
                                result.Response = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }
            return result;
        }

    }

    public enum FcmApplicationEnum
    {
        Client = 0,
    }
    public class FcmApplication
    {
        public FcmApplication()
        {
           key = "AAAA-8ZJK6M:APA91bGpBRfhlL59Keq5AIsTiqoZlaoI7e33XgpaqXnbR7Ho6kRg-vMI6eiJi41pju7kDARl1cdUTmUjPDmcphkgSStvQFt8PBUMRzsXc3pXULnhokeFc8eoSrmxdqFe5htMBXqRCjQP";
           SenderId = "1081363475363";
   
            

        }
        public string key { get; set; }
        public string SenderId { get; set; }
    }

    public class FcmNotificationInput
    {
        public List<string> RegistrationTokens { get; set; }
        public string RegistrationToken { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public long? AdId { get; set; }

        public long? BrokerId { get; set; }
        public long? SeekerId { get; set; }
        public long? OwnerId { get; set; }
        public long? CompanyId { get; set; }
    }
}
