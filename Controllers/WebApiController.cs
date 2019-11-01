using System.Web.Mvc;
using HangOut.Models;
using Newtonsoft.Json.Linq;

namespace HangOut.Controllers
{
    public class WebApiController : Controller
    {
        public JObject GetLogin(string Obj)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(Obj);

            vw_HG_UsersDetails LoginExist = Objuser.Checkvw_HG_UsersDetails();
            if (LoginExist == null)
            {
                LoginExist = new vw_HG_UsersDetails();
            }
            return JObject.FromObject(LoginExist);

        }

        public JObject PostRegistration(string Obj)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(Obj);

            vw_HG_UsersDetails Exist = Objuser.Checkvw_HG_UsersDetails();
            if(Exist!=null)
            {
                Objuser = null;
            }else
            {

                Objuser.UserCode = Objuser.save();
            }


            return JObject.FromObject(Objuser);

        }

    }
}
