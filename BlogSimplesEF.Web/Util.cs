using System.Text.Json;

namespace BlogSimplesEF.Web
{
    public static class Util
    {
        public static string GetJSONResultado(object objToConverter)
        {
            return JsonSerializer.Serialize(objToConverter);
        }
    }
}
