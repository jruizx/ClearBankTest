namespace ClearBank.DeveloperTest.Types
{
    public static class PaymentSchemeValidatorFactory
    {
        public static IPaymentSchemeValidator GetValidator(PaymentScheme paymentScheme)
        {
            switch (paymentScheme)
            {
                case PaymentScheme.Bacs:
                    return new BacsPaymentSchemeValidator();

                case PaymentScheme.FasterPayments:
                    return new FasterPaymentSchemeValidator();

                case PaymentScheme.Chaps:
                    return new ChapsPaymentSchemeValidator();
                default:
                    return new NullPaymentSchemeValidator();
            }
        }
    }
}