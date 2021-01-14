namespace ClearBank.DeveloperTest.Types
{
    public class NullPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool Validate(Account account, decimal amount)
        {
            return false;
        }
    }
}