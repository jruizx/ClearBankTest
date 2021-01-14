namespace ClearBank.DeveloperTest.Types
{
    public class BacsPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool Validate(Account account, decimal amount)
        {
            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}