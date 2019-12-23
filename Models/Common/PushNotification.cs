using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace HangOut.Models.Common
{
    public class PushNotification
    {
        
        public PushNotification()
        {
            
    }
        public static string SendNotification(List<string> deviceRegIds, string message, string title)
        {
            string SERVER_API_KEY = "";
            var SENDER_ID = "";
            string regIds = string.Join("", "", deviceRegIds);
            WebRequest tRequest;
            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
          string  postData ="{ \"registration_ids\": [ \"" + regIds + "\" ], " +"\"data\": {\"tickerText\":\"" + title + "\", " +"\"contentTitle\":\"" + title + "\", " +"\"message\": \"" + message + "\"}}";
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();
            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }


    }
}