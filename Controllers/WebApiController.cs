using System.Web.Http;
using HangOut.Models;
using Newtonsoft.Json.Linq;

namespace HangOut.Controllers
{
    public class WebApiController : ApiController
    {
        public JObject GetLogin(string ObjLogin)
        {

            vw_HG_UsersDetails Objuser = Newtonsoft.Json.JsonConvert.DeserializeObject<vw_HG_UsersDetails>(ObjLogin);

            vw_HG_UsersDetails LoginExist = Objuser.Checkvw_HG_UsersDetails();

            return new JObject(LoginExist);

        }

    }
}
