using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PaymentGateway.Utils.CustomValidationAttributes
{
    public class CreditCardExpiryDateAttribute : ValidationAttribute
    {
        public CreditCardExpiryDateAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex rgx = new Regex(@"^(0[1-9]|1[0-2])\/([0-9]{4})$");
            string dateString = (string)value;
            if (dateString != null && rgx.IsMatch(dateString))
            {
                var dateTime = DateTime.ParseExact(dateString, "MM/yyyy", CultureInfo.InvariantCulture);
                if (dateTime >= DateTime.Now)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage);
        }

    }
}