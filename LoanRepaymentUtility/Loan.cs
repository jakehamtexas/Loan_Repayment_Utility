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
                return Principal + InterestQuantity;
            }
        }

        public decimal InterestQuantity { get; set; }
        public decimal Principal { get; set; }
        public decimal Payment { get; set; }

        public int LoanTermInMonths { get; set; }
        public decimal InterestRatePerYear { get; set; }

        public DateTime LastInterestCalculation { get; set; } = DateTime.MinValue;

        private string persistedDataFilePath;
        private List<string> persisted = new List<string>();

        private string loggablePayment;
        private const string logFilePath = "log.txt";

        private const int daysInAYear = 365;

        private const string dateFormat = "MM/dd/yyyy";
        private const string currencyFormat = "C";

        public Loan(string persistedDataFilePath)
        {
            this.persistedDataFilePath = persistedDataFilePath;
        }

        public Loan()
        {
        }

        public void LoadData()
        {
            SetPersistedProperties(persistedDataFilePath);
            SetNewInterestQuantity();
        }

        public void ShowCurrentBalance()
        {
            Console.WriteLine(GetBalanceMessage());
        }

        public void ConfigureTerms()
        {
            SetPropertyByName(nameof(Principal), "Principal Amount");
            SetPropertyByName(nameof(InterestRatePerYear), "Interest Rate (annual, in decimal form)");
            SetPropertyByName(nameof(LoanTermInMonths), "Loan Term (in months)");

            IsInitialized = true;
        }

        public void SetPayment(string payment)
        {
            Payment = int.Parse(payment);
            loggablePayment = Payment.ToString(currencyFormat);
        }

        public void ApplyPayment()
        {
            if (Payment >= InterestQuantity)
            {
                Payment -= InterestQuantity;
                InterestQuantity = 0;
                if (Payment > 0)
                {
                    Principal -= Payment;
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
            string loggableToday = DateTime.Today.ToString(dateFormat);
            string logEntry = string.Format("Payment made on {0}: {1}; {2}", loggableToday, loggablePayment, GetBalanceMessage());
            using (StreamWriter sw = File.AppendText(dir + logFilePath))
            {
                sw.WriteLine(logEntry + Environment.NewLine);
            }
        }

        public void PersistData()
        {
            if (persisted.Count > 0)
                persisted.Clear();
            AddPropertiesToPersistedDataCollection();
            string persistedDataAsCSVRow = persisted.Aggregate((x, y) => x + ", " + y);
            using (StreamWriter sw = new StreamWriter(persistedDataFilePath))
            {
                sw.WriteLine(persistedDataAsCSVRow);
            }
            
        }

        private void SetPersistedProperties(string persistedDataFilePath)
        {
            persisted = GetPersistedData(persistedDataFilePath);
            if (persisted.Count > 0)
            {
                Principal = decimal.Parse(persisted[0]);
                InterestQuantity = decimal.Parse(persisted[1]);
                LoanTermInMonths = int.Parse(persisted[2]);
                InterestRatePerYear = decimal.Parse(persisted[3]);
                LastInterestCalculation = DateTime.Parse(persisted[4]);
            }
        }

        private List<string> GetPersistedData(string persistedDataFilePath)
        {
            List<string> row = new List<string>();
            using (StreamReader sr = new StreamReader(persistedDataFilePath))
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
            return Principal * percentageInterestOverSpanOfDays;
        }

        private string GetBalanceMessage()
        {
            return String.Format("Balance To Date: {0}", TotalBalance.ToString(currencyFormat));
        }


        private void SetPropertyByName(string property, string userPrompt)
        {
            System.Reflection.PropertyInfo propertyInfo = typeof(Loan).GetProperty(property);
            while (true)
            {
                try
                {
                    propertyInfo.SetValue(this, GetValueFromUser(userPrompt, propertyInfo.PropertyType));
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private object GetValueFromUser(string userPrompt, Type propertyType)
        { 
            if (propertyType == typeof(decimal) && decimal.TryParse(ShowPromptAndGetInput(userPrompt), out decimal decimalAmount))
            {
                return decimalAmount;
            }
            else if (propertyType == typeof(int) && int.TryParse(ShowPromptAndGetInput(userPrompt), out int intAmount))
            {
                return intAmount;
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

        private void AddPropertiesToPersistedDataCollection()
        {
            persisted.Add(Principal.ToString());
            persisted.Add(InterestQuantity.ToString());
            persisted.Add(LoanTermInMonths.ToString());
            persisted.Add(InterestRatePerYear.ToString());
            persisted.Add(LastInterestCalculation.ToString(dateFormat));
        }
    }
}