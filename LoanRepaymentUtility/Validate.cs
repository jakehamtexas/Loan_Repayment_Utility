using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanRepaymentUtility
{
    public class Validate
    {
        public static bool IsValidDollarAmount(string myString)
        {
            if (decimal.TryParse(myString, out decimal amount))
            {
                bool AmountIsNotNegativeAndHasCorrectNumberDecimalDigits = amount > 0 && !(amount * 100 % 1 > 0);
                if (AmountIsNotNegativeAndHasCorrectNumberDecimalDigits)
                {
                    return true;
                }
            }
            return GiveFalseAndWriteUserPrompt();
        }

        private static bool GiveFalseAndWriteUserPrompt()
        {
            Console.WriteLine("Not a valid payment!");
            Console.WriteLine();
            return false;
        }
    }
}
