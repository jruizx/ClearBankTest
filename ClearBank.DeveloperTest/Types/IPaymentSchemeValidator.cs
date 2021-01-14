namespace ClearBank.DeveloperTest.Types
{
    public interface IPaymentSchemeValidator
    {
        bool Validate(Account account, decimal amount);
    }
}