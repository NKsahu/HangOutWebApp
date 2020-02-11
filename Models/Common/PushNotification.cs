using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Web;

namespace HangOut.Models.Common
{
    public class PushNotification
    {

        public PushNotification()
        {

        }
        public static string SendNotification(string[] deviceRegIds, string message, string title, Int64 OID = 0)
        {
            string SERVER_API_KEY = "AAAA5_sPHX8:APA91bHDAXzfpWGrIXMebCCIySxJo7WY-t8ID4mylmgd-ZHRp65Ybbuk_HW0YZ_nOQkPYjUN83Y9OYv1Gh7WY6Kd8GEJ-xK3xaLz8Zt9BHwz59Ba4P6cwHX4XFd1f2krQYOEuV9hSy94";
            var SENDER_ID = "996349517183";
            string regIds = string.Join(",", deviceRegIds);
            // var applicationID_ServerKey = "AAAA5_sPHX8:APA91bHDAXzfpWGrIXMebCCIySxJo7WY-t8ID4mylmgd-ZHRp65Ybbuk_HW0YZ_nOQkPYjUN83Y9OYv1Gh7WY6Kd8GEJ-xK3xaLz8Zt9BHwz59Ba4P6cwHX4XFd1f2krQYOEuV9hSy94";
            //var senderId = "996349517183";
            //string deviceId = DeviceID;
            System.Net.WebRequest tRequest = System.Net.WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var data = new
            {
                to = "/topics/" + regIds,//"/topics/AgroIndia",//deviceId,
                data = new
                {
                    body = message,
                    title = title,
                    icon = "myicon",
                    sound = "default",//"default",
                    OrderID = OID
                }
            };

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(data);
            System.Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            tRequest.ContentLength = byteArray.Length;

            using (System.IO.Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                try
                {
                    using (System.Net.WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (System.IO.Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (System.IO.StreamReader tReader = new System.IO.StreamReader(dataStreamResponse))
                            {
                                System.String sResponseFromServer = tReader.ReadToEnd();
                                return sResponseFromServer;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return "";
                }

            }
        }


        public static string BrowserNotification(string[] Topics, string message, string title, Int64 OID = 0)
        {
            string SERVER_API_KEY = "AAAA5_sPHX8:APA91bHDAXzfpWGrIXMebCCIySxJo7WY-t8ID4mylmgd-ZHRp65Ybbuk_HW0YZ_nOQkPYjUN83Y9OYv1Gh7WY6Kd8GEJ-xK3xaLz8Zt9BHwz59Ba4P6cwHX4XFd1f2krQYOEuV9hSy94";
            var SENDER_ID = "996349517183";
            // string regIds = string.Join(",", deviceRegIds);
            foreach (var regIds in Topics)
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = "/topics/" + regIds,//"/topics/AgroIndia",//deviceId,
                    notification = new
                    {
                        body = message,
                        title = title,
                        icon = "~/Image/app_icon.png",
                        sound = "~/Image/noticefict.mpeg",//"default",
                        OrderID = OID
                    }
                };
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(data);
                System.Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                tRequest.ContentLength = byteArray.Length;
                using (System.IO.Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    try
                    {
                        using (System.Net.WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (System.IO.Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (System.IO.StreamReader tReader = new System.IO.StreamReader(dataStreamResponse))
                                {
                                    System.String sResponseFromServer = tReader.ReadToEnd();
                                    //return sResponseFromServer;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                       // return "";
                    }

                }
            }
            return "";
        } 
        public static int NewOrderMsg(string[] tpics,Int64 OID,int TicketNo=0)
        {
            string title = "New Order Placed";
            string msg = "Order Plced Order No: " + OID;
            if (TicketNo > 0)
            {
                msg+= " & Ticket No: " + TicketNo;
            }
            try
            {
                BrowserNotification(tpics, msg, title, OID);
            }
            catch(Exception e)
            {

            }
            
            return 0;
        }
    }
}
