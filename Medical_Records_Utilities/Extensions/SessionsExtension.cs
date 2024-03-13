using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Medical_Records_Utilities.Extensions
{
    public static class SessionsExtension
    {
        public static List<int> lstActions = new List<int>();

        public static void SetObject(this ISession session, string key, object value)
        {
            string data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            string data = session.GetString(key);
            if (data == null)
            {
                return default;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
    }
}
