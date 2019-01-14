using NUnit.Framework;
using LoanRepaymentUtility;
namespace Tests
{
    public class ValidateTest
    {
        [Test]
        public void IsValidDollarAmount_Zero_ReturnsFalse()
        {
            string testInput = "0";
            bool actual = Validate.IsValidDollarAmount(testInput);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValidDollarAmount_Negative_ReturnsFalse()
        {
            string testInput = "-3";
            bool actual = Validate.IsValidDollarAmount(testInput);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValidDollarAmount_TooManyDecimals_ReturnsFalse()
        {
            string testInput = "5.001";
            bool actual = Validate.IsValidDollarAmount(testInput);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValidDollarAmount_NaN_ReturnsFalse()
        {
            string testInput = "hello";
            bool actual = Validate.IsValidDollarAmount(testInput);
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValidDollarAmount_TooFewDecimals_ReturnsTrue()
        {
            string testInput = "0.1";
            bool actual = Validate.IsValidDollarAmount(testInput);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValidDollarAmount_CorrectWithNoDecimal_ReturnsTrue()
        {
            string testInput = "1";
            bool actual = Validate.IsValidDollarAmount(testInput);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValidDollarAmount_CorrectWithDecimal_ReturnsTrue()
        {
            string testInput = "1.00";
            bool actual = Validate.IsValidDollarAmount(testInput);
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }
    }
}