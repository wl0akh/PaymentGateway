using NUnit.Framework;
using PaymentGateway.Utils.Helpers;

namespace PaymentGateway.Tests.Utils.Helpers
{
    [TestFixture]
    public class MaskCardNumberTests
    {
        // TEST MASKING FOR ALL VALID LENGTH CARD NUMBERS
        [TestCase("5500000000000004", "************0004")]
        [TestCase("500000000000004", "***********0004")]
        [TestCase("50000000000004", "**********0004")]
        [TestCase("5000000000004", "*********0004")]
        [TestCase("500000000004", "********0004")]
        // TEST MASKING FOR ALL INVALID LENGTH CARD NUMBERS
        [TestCase("50000000004", "*******0004")]
        [TestCase("0004", "0004")]
        public void MaskValidLengthTest(string cardnumber, string masked)
        {
            var result = MaskHelper.Mask(cardnumber);
            Assert.AreEqual(masked, result);
        }
    }
}