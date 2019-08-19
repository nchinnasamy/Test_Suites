using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using NUnit.Framework;
using OpenQA.Selenium;
using Payquest_Testing;

namespace Test_Suites.PaymentTransaction
{
    [TestFixture]
    
    

    public class PaymentOneoff:Class1
    {
        private static Class1 accessor = new Class1();


        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =8 and TrancheID=852";

        #endregion Queries ----------------------------------------------------

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =8 and TrancheID=852 ";

        #endregion Queries ----------------------------------------------------

        


        private static long debtorID = -1;

        private static long GetDebtorID()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                return conn.QuerySingle(RANDOM_DEBTOR_QUERY).DebtorEntityID;
                
            }

            
        }

        private static long debtID = -1;

        private static long GetDebtID()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                return conn.QuerySingle(RANDOM_DEBT_QUERY).debtID;
            }

        }

       
                
        

        [OneTimeSetUp]

        public void Start()
        {
          
            debtID = GetDebtID();
            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL,debtID));
            
        }

        [OneTimeTearDown]

        public void Teardown()
        {

            accessor.Close();
        }
       

       [Test]
        

        public void CreateOneoff()
        {

            debtorID = GetDebtorID();
            accessor.ClickTab(string.Format("#debtor{0}BankDetails", debtorID));

            IWebElement addcreditcard = accessor.GetElementByXPath(string.Format("//button[@ng-click='accountCtrl.model.addCreditCard($event)']"));
            addcreditcard.Click();

            System.Threading.Thread.Sleep(2000);

            var addmastercard = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}CreditCardNumber", debtorID, -1));
            addmastercard.Click();
            accessor.SetElementValue(addmastercard, "5163200000000008");

            var expirydate = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}ExpiryDate", debtorID, -1));
            expirydate.Click();
            accessor.SetElementValue(expirydate, "08/2020");

            var nameoncard = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}NameOnCard", debtorID, -1));
            nameoncard.Click();
            accessor.SetElementValue(nameoncard, "TEST");

            accessor.Save();

            IWebElement processcreditcard = accessor.GetElementByXPath(string.Format("//button[@ng-click='accountCtrl.processCreditCard($event, creditCard)']"));
            processcreditcard.Click();

            var process = accessor.GetElementByID("existingCardCVN");
            accessor.SetElementValue(process, "070"); 

            IWebElement next = accessor.GetElementByXPath(string.Format("//button[@ng-click='processCreditCardPaymentCtrl.nextPage()']"));
            next.Click();

            var amount = accessor.GetElementByID("amount");
            accessor.SetElementValue(amount, "100"); 

            next.Click();
            next.Click();

            IWebElement processbutton = accessor.GetElementByXPath(string.Format("//button[@ng-click='processCreditCardPaymentCtrl.finish()']"));
            processbutton.Click();

            IWebElement successfully = accessor.GetElementByXPath(string.Format("//button[@ng-click='processCreditCardPaymentCtrl.finish()']"));
            successfully.Click();

          

        }
    }
}
