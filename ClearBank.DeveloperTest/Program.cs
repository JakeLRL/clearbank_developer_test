using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

// Determine data store type from config
var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

var services = new ServiceCollection()
	.AddSingleton<IAccountDataStore>(provider =>
	dataStoreType == "Backup"
		? new BackupAccountDataStore()
		: new AccountDataStore()
	).AddSingleton<IPaymentService, PaymentService>();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Usage (optional):
var paymentService = serviceProvider.GetService<IPaymentService>();
var request = new MakePaymentRequest
{
	Amount = 10,
	DebtorAccountNumber = "1",
	PaymentScheme = PaymentScheme.Bacs
};

var result = paymentService.MakePayment(request);

Console.WriteLine($"Payment Success = {result.Success}");