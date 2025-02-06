using System;

namespace Eaf.Template.Bff.Host.Swagger
{
    public enum HeaderResponseType
    { String, Number }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ProducesResponseHeaderAttribute : Attribute
    {
        public ProducesResponseHeaderAttribute(string name, int statusCode)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            StatusCode = statusCode;
            Type = HeaderResponseType.String;
        }

        public string Name { get; set; }
        public int StatusCode { get; set; }
        public HeaderResponseType Type { get; set; }
        public string Description { get; set; }
    }
}