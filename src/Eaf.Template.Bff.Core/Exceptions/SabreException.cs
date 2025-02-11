using System;

namespace Eaf.Template.Bff.Core.Exceptions
{
    public class SabreException
    {
        public string Status { get; set; }
        public string Type { get; set; }
        public string ErrorCode { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
    }
}