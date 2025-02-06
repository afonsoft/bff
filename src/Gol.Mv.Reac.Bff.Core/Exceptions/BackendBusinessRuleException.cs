namespace Eaf.Template.Bff.Core.Exceptions
{
    public class BackendBusinessRuleException : InvoluntaryExchangeApiBaseException
    {
        public BackendBusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}