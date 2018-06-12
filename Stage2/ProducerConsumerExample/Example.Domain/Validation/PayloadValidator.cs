using Example.Contract.Command;
using FluentValidation;

namespace Example.Domain.Validation
{
    public class PayloadValidator : AbstractValidator<IPayload>
    {
		public PayloadValidator()
		{
			RuleFor(payload => payload.NoneNegtiveValue).NotEmpty().GreaterThan(0);
		}
    }
}
