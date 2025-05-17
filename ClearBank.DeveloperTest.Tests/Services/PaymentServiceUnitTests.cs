using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NSubstitute;
using NUnit.Framework;
using System;

namespace ClearBank.DeveloperTest.Tests.Services;
[TestFixture]
public class PaymentServiceUnitTests
{
	private static readonly Random _random = new();

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

		_dataStore.Received(0).UpdateAccount(Arg.Any<Account>());
	}

	[TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
	[TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
	[TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
	public void GivenValidPayment_WhenAccountSupportsPayment_ThenPaymentSuccessful(PaymentScheme paymentScheme, AllowedPaymentSchemes allowedPaymentSchemes)
	{
		// Arrange
		var request = ValidPaymentRequest(paymentScheme);

		var account = ValidAccount(allowedPaymentSchemes);

		_dataStore.GetAccount(Arg.Any<string>()).Returns(account);

		// Act
		var result = _sut.MakePayment(request);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.True);

		_dataStore.Received(1).UpdateAccount(Arg.Any<Account>());
	}

	[TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.FasterPayments)]
	[TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Chaps)]
	[TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Bacs)]
	public void GivenValidPayment_WhenAccountDoesNotSupportPayment_ThenPaymentNotSuccessful(PaymentScheme paymentScheme, AllowedPaymentSchemes allowedPaymentSchemes)
	{
		// Arrange
		var request = ValidPaymentRequest(paymentScheme);

		var account = ValidAccount(allowedPaymentSchemes);

		_dataStore.GetAccount(Arg.Any<string>()).Returns(account);

		// Act
		var result = _sut.MakePayment(request);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.False);

		_dataStore.Received(0).UpdateAccount(Arg.Any<Account>());
	}

	[Test]
	public void GivenFasterPayment_WhenAmountGreaterThanBalance_ThenPaymentNotSuccessful()
	{
		// Arrange
		var request = ValidPaymentRequest(PaymentScheme.FasterPayments);

		var account = ValidAccount(AllowedPaymentSchemes.FasterPayments);
		account.Balance = _random.Next(0, 9);

		_dataStore.GetAccount(Arg.Any<string>()).Returns(account);

		// Act
		var result = _sut.MakePayment(request);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.False);

		_dataStore.Received(0).UpdateAccount(Arg.Any<Account>());
	}

	[TestCase(AccountStatus.InboundPaymentsOnly)]
	[TestCase(AccountStatus.Disabled)]
	public void GivenChapsPayment_WhenAccountStatusNotLive_ThenPaymentNotSuccessful(AccountStatus accountStatus)
	{
		// Arrange
		var request = ValidPaymentRequest(PaymentScheme.Chaps);

		var account = ValidAccount(AllowedPaymentSchemes.Chaps);
		account.Status = accountStatus;

		_dataStore.GetAccount(Arg.Any<string>()).Returns(account);

		// Act
		var result = _sut.MakePayment(request);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Success, Is.False);

		_dataStore.Received(0).UpdateAccount(Arg.Any<Account>());
	}

	private static MakePaymentRequest ValidPaymentRequest(PaymentScheme paymentScheme)
	{
		return new MakePaymentRequest()
		{
			DebtorAccountNumber = "",
			PaymentScheme = paymentScheme,
			Amount = _random.Next(10,20)
		};
	}

	private static Account ValidAccount(AllowedPaymentSchemes allowedPaymentSchemes)
	{
		return new Account
		{
			Balance = _random.Next(100, 200),
			AllowedPaymentSchemes = allowedPaymentSchemes,
			Status = AccountStatus.Live
		};
	}
}
