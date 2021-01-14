namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }

        public bool ValidatePayment(PaymentScheme paymentScheme, decimal amount)
        {
            var validator = PaymentSchemeValidatorFactory.GetValidator(paymentScheme);
            return validator.Validate(this, amount);
        }
    }
}
