namespace ClearBank.DeveloperTest.Types
{
    public class FasterPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool Validate(Account account, decimal amount)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                return false;
            }

            return account.Balance >= amount;
        }
    }
}