using NUnit.Framework;
using LoanRepaymentUtility;
namespace LoanRepaymentUtilityTest
{
    class LoanTest
    {
        private Loan testLoan = new Loan();

        [SetUp]
        public void Setup()
        {
            testLoan.Principal = 1000;
            testLoan.InterestQuantity = 50;
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsGreaterThanInteret_ReducesPrinciple()
        {
            decimal testInput = 100;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.Principal;
            decimal expected = 950;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsGreaterThanInteret_InterestIsZero()
        {
            decimal testInput = 100;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.InterestQuantity;
            decimal expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsEqualToInterest_PrincipleDoesNotChange()
        {
            decimal testInput = testLoan.InterestQuantity;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.Principal;
            decimal expected = 1000;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsEqualToInteret_InterestIsZero()
        {
            decimal testInput = testLoan.InterestQuantity;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.InterestQuantity;
            decimal expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsLessThanInteret_PrincipleDoesNotChange()
        {
            decimal testInput = testLoan.InterestQuantity - 25;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.Principal;
            decimal expected = 1000;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsLessThanInteret_InterestReducesByPayment()
        {
            decimal testInput = testLoan.InterestQuantity - 25;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.InterestQuantity;
            decimal expected = 25;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsEqualToBalance_InterestIsZero()
        {
            decimal testInput = testLoan.Principal + testLoan.InterestQuantity;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.InterestQuantity;
            decimal expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsEqualToBalance_PrincipleIsZero()
        {
            decimal testInput = testLoan.Principal + testLoan.InterestQuantity;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.Principal;
            decimal expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyPaymentTest_PaymentIsGreaterThanBalance_BalanceIsNegative()
        {
            decimal testInput = testLoan.Principal + testLoan.InterestQuantity + 1;
            testLoan.Payment = testInput;

            testLoan.ApplyPayment();
            decimal actual = testLoan.TotalBalance;
            decimal expected = -1;

            Assert.AreEqual(expected, actual);
        }
    }
}
