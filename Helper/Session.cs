using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace BoxBuildproj.Helpers  // ✅ Organized into a Helpers folder (rename if needed)
{
    public static class SessionExtensions
    {
        // ✅ Store an object as JSON in session
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // ✅ Retrieve an object from session and deserialize it
        public static T? GetObjectFromJson<T>(this ISession session, string key) where T : class
        {
            var value = session.GetString(key);
            return value == null ? null : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
