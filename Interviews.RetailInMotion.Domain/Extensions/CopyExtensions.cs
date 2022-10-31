using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Extensions
{
    internal static class CopyExtensions
    {
        public static T CopyOf<T>(this T source)
        {
            var serialied = System.Text.Json.JsonSerializer.Serialize(source);
            return System.Text.Json.JsonSerializer.Deserialize<T>(serialied);
        }
    }
}
