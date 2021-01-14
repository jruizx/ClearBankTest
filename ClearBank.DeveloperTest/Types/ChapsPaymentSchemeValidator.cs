namespace ClearBank.DeveloperTest.Types
{
    public class ChapsPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool Validate(Account account, decimal amount)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            {
                return false;
            }

            return account.Status == AccountStatus.Live;
        }
    }
}