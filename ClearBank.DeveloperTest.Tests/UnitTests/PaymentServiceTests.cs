using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.UnitTests
{
    public class PaymentServiceTests
    {
        private AccountDataStoreFactory accountDataStoreFactory;
        private Account account;
        private MakePaymentRequest request;
        private readonly Mock<IAccountDataStore> accountDataStore = new Mock<IAccountDataStore>();

        [SetUp]
        public void BeforeEach()
        {
            accountDataStoreFactory = new AccountDataStoreFactory();
            account = new Account
            {
                Balance = 10
            };
            request = new MakePaymentRequest { Amount = 10};
            accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(account);
        }

        [Test]
        public void MakePaymentShouldFailWhenTheAccountDoesNotAllowThePaymentSchemaRequested()
        {
            var serviceUnderTest = new PaymentService(accountDataStoreFactory.GetAccountDataStore());

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void MakePaymentShouldFailWhenTheRetrievedAccountIsNull()
        {
            accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns((Account)null);
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void MakePaymentShouldSuccessWhenBacsPaymentAreRequestedAndTheAccountAllowBacsPayments()
        {
            request.PaymentScheme = PaymentScheme.Bacs;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(true, response.Success);
        }

        [Test]
        public void MakePaymentShouldFailWhenChapsPaymentAreRequestedAndTheAccountIsNotLive()
        {
            request.PaymentScheme = PaymentScheme.Chaps;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            account.Status = AccountStatus.InboundPaymentsOnly;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void MakePaymentShouldFailWhenChapsPaymentAreRequestedAndTheAccountDoesNotAllowChapsPayments()
        {
            request.PaymentScheme = PaymentScheme.Chaps;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            account.Status = AccountStatus.InboundPaymentsOnly;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void MakePaymentShouldSuccessWhenChapsPaymentAreRequestedAndTheAccountIsLiveAndTheAccountAllowsChapsPayments()
        {
            request.PaymentScheme = PaymentScheme.Chaps;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(true, response.Success);
        }

        [Test]
        public void MakePaymentShouldFailWhenFasterPaymentAreRequestedAndTheAccountDoesNotAllowFasterPayments()
        {
            request.PaymentScheme = PaymentScheme.FasterPayments;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void MakePaymentShouldFailWhenFasterPaymentAreRequestedAndTheAccountBalanceIsLowerThanTheAmountRequested()
        {
            account.Balance = 0;
            request.PaymentScheme = PaymentScheme.FasterPayments;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void MakePaymentShouldSuccessWhenFasterPaymentAreRequestedTheAccountBalanceIsGreaterOrEqualToTheAmountRequestedAndTheAccountAllowFasterPayments()
        {
            request.PaymentScheme = PaymentScheme.FasterPayments;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            var response = serviceUnderTest.MakePayment(request);

            Assert.AreEqual(true, response.Success);
        }

        [Test]
        public void ASuccessPaymentShouldReduceTheBalanceFromTheAccount()
        {
            request.PaymentScheme = PaymentScheme.FasterPayments;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            serviceUnderTest.MakePayment(request);

            Assert.AreEqual(0, account.Balance);
        }

        [Test]
        public void ASuccessPaymentShouldUpdateTheNewAccountToTheDataStore()
        {
            request.PaymentScheme = PaymentScheme.FasterPayments;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            var serviceUnderTest = new PaymentService(accountDataStore.Object);

            serviceUnderTest.MakePayment(request);

            accountDataStore.Verify(x => x.UpdateAccount(account), Times.Once);
        }
    }
}
