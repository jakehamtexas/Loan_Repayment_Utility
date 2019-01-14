using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace LoanRepaymentUtility
{
    public class Loan
    {
        public bool IsInitialized { get; set; }
        public decimal TotalBalance
        {
            get
            {
                return Principle + InterestQuantity;
            }
            set
            {
                TotalBalance = value;
            }
        }

        public decimal InterestQuantity { get; set; }
        public decimal Principle { get; set; }
        public decimal Payment { get; set; }

        public int LoanTermInMonths { get; set; }
        public decimal InterestRatePerYear { get; set; }

        public DateTime LastInterestCalculation { get; set; }

        private string preservedDataFilePath;
        private List<string> preservedData = new List<string>();

        private string loggablePayment;
        const string logFilePath = "log.txt";

        const int daysInAYear = 365;

        public Loan(string preservedDataFilePath)
        {
            this.preservedDataFilePath = preservedDataFilePath;
        }

        public Loan()
        {
        }

        public void LoadData()
        {
            SetPreservedProperties(preservedDataFilePath);
            SetNewInterestQuantity();
        }

        public void ShowCurrentBalance()
        {
            Console.WriteLine(GetBalanceMessage());
        }

        public void ConfigureTerms()
        {
            while (true)
            {
                try
                {
                    Principle = GetPropertyFromUserText("Principal Amount");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (true)
            {
                try
                {
                    InterestRatePerYear = GetPropertyFromUserText("Interest Rate (annual, in decimal form)");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (true)
            {
                try
                {
                    LoanTermInMonths = (int)GetPropertyFromUserText("Loan Term (in months)");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IsInitialized = true;
        }

        public void SetPayment(string payment)
        {
            Payment = int.Parse(payment);
            loggablePayment = Payment.ToString("C");
        }

        public void ApplyPayment()
        {
            if (Payment >= InterestQuantity)
            {
                Payment -= InterestQuantity;
                InterestQuantity = 0;
                if (Payment > 0)
                {
                    Principle -= Payment;
                }
            }
            else
            {
                InterestQuantity -= Payment;
            }
        }

        public void UpdateLastCalculatedDate()
        {
            LastInterestCalculation = DateTime.Today.Date;
        }

        public void Log(string dir)
        {
            string logEntry = String.Format("Payment made on {0}: {1}; {2}", DateTime.Today.ToString("MM/dd/yyyy"), loggablePayment, GetBalanceMessage());
            using (StreamWriter sw = File.AppendText(dir + logFilePath))
            {
                sw.WriteLine(logEntry + Environment.NewLine);
            }
        }

        public void PreserveData()
        {
            if (preservedData.Count > 0)
                preservedData.Clear();
            AddPropertiesToPreservedDataCollection();
            string preservedDataAsCSVRow = preservedData.Aggregate((x, y) => x + ", " + y);
            using (StreamWriter sw = new StreamWriter(preservedDataFilePath))
            {
                sw.WriteLine(preservedDataAsCSVRow);
            }
            
        }

        private string GetBalanceMessage()
        {
            return String.Format("Balance To Date: {0}", TotalBalance.ToString("C"));
        }

        private decimal GetPropertyFromUserText(string property)
        {
            if (decimal.TryParse(ShowPromptAndGetInput(property), out decimal amount))
            {
                return amount;
            }
            else
            {
                throw new Exception("Invalid input!");
            }
        }

        private string ShowPromptAndGetInput(string inputQuery)
        {
            Console.Write(String.Format("Enter a value for {0}: ", inputQuery));
            return Console.ReadLine();
        }

        private void SetPreservedProperties(string preservedDataFilePath)
        {
            preservedData = GetPreservedData(preservedDataFilePath);
            if (preservedData.Count > 0)
            {
                Principle = decimal.Parse(preservedData[0]);
                InterestQuantity = decimal.Parse(preservedData[1]);
                LoanTermInMonths = int.Parse(preservedData[2]);
                InterestRatePerYear = decimal.Parse(preservedData[3]);
                LastInterestCalculation = DateTime.Parse(preservedData[4]);
            }
        }

        private List<string> GetPreservedData(string preservedDataFilePath)
        {
            List<string> row = new List<string>();
            using (StreamReader sr = new StreamReader(preservedDataFilePath))
            {
                row.AddRange(sr.ReadLine().Split(','));
            }
            return row;
        }

        private void SetNewInterestQuantity()
        {
            InterestQuantity += GetInterestSinceLastCalculation(DateTime.Today);
        }

        private decimal GetInterestSinceLastCalculation(DateTime current)
        {
            int daysSinceLastCalculation = (current - LastInterestCalculation).Days;
            decimal percentageInterestOverSpanOfDays = (InterestRatePerYear * daysSinceLastCalculation) / daysInAYear;
            return Principle*percentageInterestOverSpanOfDays;
        }

        private void AddPropertiesToPreservedDataCollection()
        {
            preservedData.Add(Principle.ToString());
            preservedData.Add(InterestQuantity.ToString());
            preservedData.Add(LoanTermInMonths.ToString());
            preservedData.Add(InterestRatePerYear.ToString());
            preservedData.Add(LastInterestCalculation.ToString("MM/dd/yyyy"));
        }
    }
}