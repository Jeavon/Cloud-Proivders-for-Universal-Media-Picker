using System;
using System.Web.Script.Serialization;
using umbraco.BusinessLogic;

namespace Sitereactor.CloudProviders.Extensions
{
    public static class JsonExtensions
    {
        public static string SerializeToJson(this object toSerialize)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(toSerialize);
        }

        public static T DeserializeJsonTo<T>(this string toDeserialize) where T : new()
        {
            if (!string.IsNullOrEmpty(toDeserialize))
            {
                try
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<T>(toDeserialize);
                }
                catch (Exception ex)
                {
                    Log.Add(LogTypes.Error, -1, string.Concat("JsonExtensions: Execption thrown: ", ex.Message));
                }
            }

            return new T();
        }
    }
}
