using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Net;
using System.Configuration;

namespace KilnTool.Kiln
{
    public class KilnConfig
    {
        public static string BaseUrl => ConfigurationManager.AppSettings["kiln.root"];
        public static string UserName => ConfigurationManager.AppSettings["kiln.user"];
        public static string Password => ConfigurationManager.AppSettings["kiln.password"];
    }

    public class KilnApi
    {
        public string Url(string api, NameValueCollection parameters) => $"{KilnConfig.BaseUrl}{api}{ToQueryString(parameters)}";

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        private T DownloadJson<T>(string api, NameValueCollection parameters)
        {
            using (var client = new WebClient())
            {
                var str = client.DownloadString(Url(api, parameters));

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            }
        }

        public string Auth(string sUser, string sPassword)
        {
            var q = new NameValueCollection {
                { "sUser", sUser },
                { "sPassword", sPassword }
            };

            return DownloadJson<string>("Auth/Login", q);
        }

        public string Auth()
        {
            return Auth(KilnConfig.UserName, KilnConfig.Password);
        }

        public KilnProject[] GetProjects(string token)
        {
            return DownloadJson<KilnProject[]>("Project", new NameValueCollection { { "token", token } });
        }

        public KilnProject[] GetProjects()
        {
            return GetProjects(Auth());
        }
    }
}
