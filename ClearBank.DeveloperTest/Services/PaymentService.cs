using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore accountDataStore;

        public PaymentService(IAccountDataStore accountDataStore)
        {
            this.accountDataStore = accountDataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();

            if (account == null || !account.ValidatePayment(request.PaymentScheme, request.Amount)) return result;

            result.Success = true;
            
            account.Balance -= request.Amount;
            
            accountDataStore.UpdateAccount(account);

            return result;
        }
    }
}
