using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Game_Net
{
    public class QueryStringBuilder
    {
        public static string ToQueryString<T>(T obj)
        {
            var properties = typeof(T).GetProperties();
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    query[prop.Name] = value.ToString();
                }
            }

            return query.ToString(); 
        }
    }
}
