using System;

namespace Eaf.Template.Bff.Core.Exceptions
{
    public class BackendInfrasctructureException : InvoluntaryExchangeApiBaseException
    {
        public BackendInfrasctructureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}