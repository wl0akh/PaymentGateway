using System;

namespace PaymentGateway.Helpers
{
    /// <summary>
    /// Helper class to mask CardNumber 
    /// </summary>
    public class MaskHelper
    {
        /// <summary>
        /// Mask method to mask CardNumber
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns> string of Masked cardNumber</returns>
        public static string Mask(string cardNumber)
        {
            var lastDigits = cardNumber.Length < 4 ? cardNumber : cardNumber.Substring(cardNumber.Length - 4, 4);
            var remainingDigitsLength = cardNumber.Length - 4;
            string remainingDigits = "";
            for (int i = 0; i < remainingDigitsLength; i++)
            {
                remainingDigits += "*";
            }
            return remainingDigits + lastDigits;
        }
    }
}