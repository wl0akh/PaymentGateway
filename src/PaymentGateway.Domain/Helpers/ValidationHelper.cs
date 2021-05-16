using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Utils.Helpers
{
    /// <summary>
    /// Helper class to mask CardNumber 
    /// </summary>
    public class ValidationHelper
    {
        private Dictionary<string, string> _error = new Dictionary<string, string>();

        public Dictionary<string, string> Error { get => _error; }
        /// <summary>
        /// method to validate payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns> bool</returns>
        public bool isValid(Payment payment)
        {
            var context = new ValidationContext(payment, null, null);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(payment, context, validationResults, true);

            validationResults.ForEach(item => { this._error.Add(item.MemberNames.ToList()[0], item.ErrorMessage); });
            return isValid;
        }
    }
}