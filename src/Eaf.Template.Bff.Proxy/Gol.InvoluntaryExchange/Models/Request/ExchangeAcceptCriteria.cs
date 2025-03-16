namespace Gol.InvoluntaryExchange.Models.Request
{
    public class ExchangeAcceptCriteria
    {
        public ExchangeAcceptCriteria()
        {
            DepartureStation = string.Empty;
            PassengerLastName = string.Empty;
            RecordLocator = string.Empty;
        }

        public string DepartureStation { get; set; }
        public string PassengerLastName { get; set; }
        public string RecordLocator { get; set; }
    }
}