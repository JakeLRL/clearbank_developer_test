using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Extensions
{
	public static class PaymentValidationExtensions
	{
		public static bool IsPaymentAllowed(this Account account, MakePaymentRequest request)
		{
			return request.PaymentScheme switch
			{
				PaymentScheme.Bacs =>
					account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs),

				PaymentScheme.FasterPayments =>
					account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
					account.Balance >= request.Amount,

				PaymentScheme.Chaps =>
					account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
					account.Status == AccountStatus.Live,

				_ => false
			};
		}
	}
}