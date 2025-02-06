using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gol.InvoluntaryExchange.Models.Response
{
    public class ExchangeAcceptResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public Response Response { get; set; }

        public Error Error { get; set; }
    }

    public class Error
    {
        public string Message { get; set; }
        public string Code { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("recordLocator")]
        public string RecordLocator { get; set; }

        [JsonPropertyName("passengers")]
        public List<Passenger> Passengers { get; set; }

        [JsonPropertyName("revalidatedItinerary")]
        public RevalidatedItinerary RevalidatedItinerary { get; set; }
    }

    public class Passenger
    {
        [JsonPropertyName("nameId")]
        public string NameId { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
    }

    public class RevalidatedItinerary
    {
        [JsonPropertyName("segments")]
        public List<Segment> Segments { get; set; }
    }

    public class Segment
    {
        [JsonPropertyName("airlineCode")]
        public string AirlineCode { get; set; }

        [JsonPropertyName("flightNumber")]
        public string FlightNumber { get; set; }

        [JsonPropertyName("separtureAirport")]
        public string DepartureAirport { get; set; }

        [JsonPropertyName("separtureDateTime")]
        public DateTime DepartureDateTime { get; set; }

        [JsonPropertyName("arrivalAirport")]
        public string ArrivalAirport { get; set; }

        [JsonPropertyName("arrivalDateTime")]
        public DateTime ArrivalDateTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}