using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PaymentGateway.Utils.CustomValidationAttributes
{
    /// <summary>
    /// CardExpiryDateAttribute class for validate Expiry field
    /// </summary>
    public class CardExpiryDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validation method to validate Card Expiry it should be in formate "MM/YYYY
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //REGEX TO ONLY ALLOW The EXPIRY IN FORMATE MM/YYYY
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