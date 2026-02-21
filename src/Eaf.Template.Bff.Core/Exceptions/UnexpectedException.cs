using System;

namespace Eaf.Template.Bff.Core.Exceptions
{
    public class UnexpectedException : InvoluntaryExchangeApiBaseException
    {
        public UnexpectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}