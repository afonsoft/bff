using Eaf.Template.Bff.Core.Models;
using Newtonsoft.Json;
using System.Text;

namespace Eaf.Template.Bff.Core.Extensions
{
    public static class HttpExtensions
    {

        /// <summary>
        /// Set a object Body in <see cref="HttpClient"/> create a <see cref="StringContent"/> with SerializeObject
        /// </summary>
        /// <returns>return a <see cref="HttpContent"/></returns>
        public static HttpContent SetBody<T>(this HttpClient httpClient, T body)
        {
            var content = new StringContent(
               JsonConvert.SerializeObject(body),
               Encoding.UTF8,
               "application/json");

            return content;
        }
    }
}