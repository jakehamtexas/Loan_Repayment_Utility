<<<<<<< HEAD
﻿using System;
using System.IO;

namespace LoanRepaymentUtility
{
    class Program
    {
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string persistedDataFilePath = baseDirectory + "preserve.txt";

        static void Main(string[] args)
        {
            Loan loan = new Loan(persistedDataFilePath);
            if (File.Exists(persistedDataFilePath))
            {
                InitializeLoanMembersFromPersistedData(loan);
            }

            while (true)
            {
                InputCycle(loan);
            }
        }

        private static void InitializeLoanMembersFromPersistedData(Loan loan)
        {
            loan.LoadData();
            loan.ShowCurrentBalance();
            loan.IsInitialized = true;
        }

        private static void InputCycle(Loan loan)
        {
            if (loan.IsInitialized)
            {
                DisplayRegularPromptAndOptions(loan);
            }
            else
            {
                DisplayConfigurPrompt(loan);
            }
        }

        private static void DisplayRegularPromptAndOptions(Loan loan)
        {
            Console.WriteLine("Enter 'config' keyword to configure loan terms. Otherwise, enter a payment. Use CTRL^C to exit.");
            string input = Console.ReadLine();
            if (input == "config")
            {
                loan.ConfigureTerms();
            }
            else if (Validate.IsValidDollarAmount(input))
            {
                MakePayment(input, loan);
            }
        }

        private static void MakePayment(string amount, Loan loan)
        {
            loan.SetPayment(amount);
            loan.ApplyPayment();
            loan.ShowCurrentBalance();
            loan.UpdateLastCalculatedDate();
            loan.Log(baseDirectory);
            loan.PersistData();
        }

        private static void DisplayConfigurPrompt(Loan loan)
        {
            loan.ConfigureTerms();
        }
    }
}
=======
﻿using System;
using System.IO;

namespace LoanRepaymentUtility
{
    class Program
    {
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string preservedDataFilePath = baseDirectory + "preserve.txt";

        static void Main(string[] args)
        {
            Loan loan = new Loan(preservedDataFilePath);
            if (File.Exists(preservedDataFilePath))
            {
                InitializeLoanMembersFromPreservedData(loan);
            }

            while (true)
            {
                InputCycle(loan);
            }
        }

        private static void InitializeLoanMembersFromPreservedData(Loan loan)
        {
            loan.LoadData();
            loan.ShowCurrentBalance();
            loan.IsInitialized = true;
        }

        private static void InputCycle(Loan loan)
        {
            if (loan.IsInitialized)
            {
                DisplayRegularPromptAndOptions(loan);
            }
            else
            {
                DisplayConfigurPrompt(loan);
            }
        }

        private static void DisplayRegularPromptAndOptions(Loan loan)
        {
            Console.WriteLine("Enter 'config' keyword to configure loan terms. Otherwise, enter a payment. Use CTRL^C to exit.");
            string input = Console.ReadLine();
            if (input == "config")
            {
                loan.ConfigureTerms();
            }
            else if (Validate.IsValidDollarAmount(input))
            {
                MakePayment(input, loan);
            }
        }

        private static void MakePayment(string amount, Loan loan)
        {
            loan.SetPayment(amount);
            loan.ApplyPayment();
            loan.ShowCurrentBalance();
            loan.UpdateLastCalculatedDate();
            loan.Log(baseDirectory);
            loan.PreserveData();
        }

        private static void DisplayConfigurPrompt(Loan loan)
        {
            loan.ConfigureTerms();
        }
    }
}
>>>>>>> 21fff0f6295d471cb83564aaaac281a02e9dc797
