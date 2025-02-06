using FluentValidation;
using Gol.InvoluntaryExchange.Models.Request;

namespace Gol.InvoluntaryExchange.Validator
{
    public class ExchangeAcceptRequestValidation : AbstractValidator<ExchangeAcceptRequest>
    {
        public ExchangeAcceptRequestValidation()
        {
            RuleFor(x => x.criteria.DepartureStation)
                .NotEmpty()
                .WithMessage("DepartureStation is required");

            RuleFor(x => x.criteria.PassengerLastName)
                .NotEmpty()
                .WithMessage("PassengerLastName is required");

            RuleFor(x => x.criteria.RecordLocator)
                .NotEmpty()
                .WithMessage("RecordLocator is required");
        }
    }
}