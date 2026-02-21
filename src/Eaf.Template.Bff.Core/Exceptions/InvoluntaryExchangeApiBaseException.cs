using System;

namespace Eaf.Template.Bff.Core.Exceptions
{
    public abstract class InvoluntaryExchangeApiBaseException : Exception
    {
        public InvoluntaryExchangeApiBaseException(string message, Exception innerException) : base(message, innerException)
        {
            TimeStamp = DateTime.UtcNow;
        }

        public DateTime? TimeStamp { get; set; }
    }
}