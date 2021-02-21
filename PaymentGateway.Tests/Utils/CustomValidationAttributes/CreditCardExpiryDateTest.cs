using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PaymentGateway.Utils.CustomValidationAttributes;

namespace PaymentGateway.Tests.Utils.CustomValidationAttributes
{
    [TestFixture]

    public class CreditCardExpiryDateAttributeTest : CardExpiryDateAttribute
    {
        [TestCase(((object)"0a/45788"))]
        [TestCase(null)]
        public void IsValidForInvalidFormate(Object input)
        {
            ErrorMessage = "This expiry must be in future and in the formate: MM/YYYY";
            ValidationResult result = IsValid(input, new ValidationContext(new { }));
            Assert.AreEqual(ErrorMessage, result.ErrorMessage);
        }


        [Test]
        public void IsValidExpiredMonth()
        {
            ErrorMessage = "This expiry must be in future and in the formate: MM/YYYY";
            var result = IsValid(
                $"{DateTime.MinValue.Month}/{DateTime.MinValue.Year}",
                new ValidationContext(new { }));
            Assert.AreEqual(ErrorMessage, result.ErrorMessage);
        }

        [Test]
        public void IsValidValidAndInFuture()
        {
            var result = IsValid(
                $"{DateTime.Now.AddDays(30).Month:00}/{DateTime.Now.AddDays(30).Year}",
                new ValidationContext(new { }));
            Assert.IsNull(result);
        }
    }
}