using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services;
[TestFixture]
public class PaymentServiceTests
{
	private PaymentService _sut;
	
	[SetUp]
	public void SetUp()
	{
		_sut = new PaymentService();
	}

	[Test]
	public void GivenMakePayment_WhenAccountIsNull_ThenIsNotSuccessful()
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
}
