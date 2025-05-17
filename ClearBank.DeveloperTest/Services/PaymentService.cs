using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;

namespace ClearBank.DeveloperTest.Services
{
	public class PaymentService(IAccountDataStore accountDataStore) : IPaymentService
	{
		private readonly IAccountDataStore _accountDataStore = accountDataStore;

		public MakePaymentResult MakePayment(MakePaymentRequest request)
		{
			var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

			if (account == null || !account.IsPaymentAllowed(request))
			{
				return new MakePaymentResult { Success = false };
			}

			account.Balance -= request.Amount;
			_accountDataStore.UpdateAccount(account);

			return new MakePaymentResult { Success = true };
		}
	}
}