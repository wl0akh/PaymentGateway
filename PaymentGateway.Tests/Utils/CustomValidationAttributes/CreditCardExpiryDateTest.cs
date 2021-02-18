using System;
using System.ComponentModel.DataAnnotations;
using Moq;
using NUnit.Framework;
using PaymentGateway.Utils.CustomValidationAttributes;

namespace PaymentGateway.Tests.Utils.CustomValidationAttributes
{
    [TestFixture]

    public class CreditCardExpiryDateAttributeTest : CreditCardExpiryDateAttribute
    {
        [Test]
        public void IsValidForInvalidFormate()
        {
            ErrorMessage = "This expiry must be in future and in the formate: MM/YYYY";
            ValidationResult result = IsValid(((object)"0a/45788"), new ValidationContext(new { }));
            Assert.AreEqual(ErrorMessage, result.ErrorMessage);
        }

        [Test]
        public void IsValidExpiredIsNull()
        {
            ErrorMessage = "This expiry must be in future and in the formate: MM/YYYY";
            var result = IsValid(null, new ValidationContext(new { }));
            Assert.AreEqual(ErrorMessage, result.ErrorMessage);
        }

        [Test]
        public void IsValidExpiredMonth()
        {
            ErrorMessage = "This expiry must be in future and in the formate: MM/YYYY";
            var passedDate = DateTime.MinValue;
            var result = IsValid($"{passedDate.Month}/{passedDate.Year}", new ValidationContext(new { }));
            Assert.AreEqual(ErrorMessage, result.ErrorMessage);
        }

        [Test]
        public void IsValidValidAndInFuture()
        {
            var futureDate = DateTime.Now.AddDays(30);
            var result = IsValid($"{futureDate.Month:00}/{futureDate.Year}", new ValidationContext(new { }));
            Assert.IsNull(result);
        }
    }
}