using System.Web.Http;
using HangOut.Models;
using Newtonsoft.Json.Linq;

namespace HangOut.Controllers
{
    public class WebApiController : ApiController
    {
        public JObject GetLogin(string Obj)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(Obj);

            vw_HG_UsersDetails LoginExist = Objuser.Checkvw_HG_UsersDetails();

            return new JObject(LoginExist);

        }

        public JObject GetRegistration(string Obj)
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


            return new JObject(Objuser);

        }

    }
}
