using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test.Business.HelperUtility
{
    public static class HelperUtility
    {
        public static List<T> ConvertJsonToObject<T>(this string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                try
                {
                    string json = file.ReadToEnd();
                    var serializerSettings = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    //return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path), serializerSettings) ?? new List<T>();

                    return JsonSerializer.Deserialize<List<T>>(json, serializerSettings)??new List<T>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}
