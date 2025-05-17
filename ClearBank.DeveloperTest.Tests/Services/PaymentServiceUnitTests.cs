using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NSubstitute;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services;
[TestFixture]
public class PaymentServiceUnitTests
{
	private IDataStore _dataStore;

	private PaymentService _sut;
	
	[SetUp]
	public void SetUp()
	{
		_dataStore = Substitute.For<IDataStore>();
		_sut = new PaymentService(_dataStore);
	}

	[Test]
	public void GivenMakePayment_WhenAccountIsNull_ThenPaymentNotSuccessful()
	{
		// Arrange
		var request = new MakePaymentRequest()
		{
			DebtorAccountNumber = "",
			PaymentScheme = PaymentScheme.Bacs
		};

		// Act
		var result = _sut.MakePayment(request);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.False);
	}

	[TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
	[TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
	[TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
	public void GivenValidPayment_WhenAccountSupportsPayment_ThenPaymentSuccessful(PaymentScheme paymentScheme, AllowedPaymentSchemes allowedPaymentSchemes)
	{
		// Arrange
		var request = new MakePaymentRequest()
		{
			DebtorAccountNumber = "",
			PaymentScheme = paymentScheme,
			Amount = 10,

		};
		var account = new Account
		{
			Balance = 100,
			AllowedPaymentSchemes = allowedPaymentSchemes,
			Status = AccountStatus.Live
		};

		_dataStore.GetAccount(Arg.Any<string>()).Returns(account);

		// Act
		var result = _sut.MakePayment(request);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.True);

		_dataStore.Received(1).UpdateAccount(Arg.Any<Account>());
	}
}
