using System;
using NUnit.Framework;
using Payquest_Testing;
using OpenQA.Selenium;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using OpenQA.Selenium.Support.UI;

namespace Test_Suites.TEST
{



    [TestFixture]

    public class Login_test : Class1

    {
        private Class1 accessor = new Class1();
       // private int DebtID = 55386988;
        //private int DebtorID = 3207577;

       

        //[Description("Navigate to the home page and verify the home link's text.")]

        //  [TestCaseSource(typeof(Class1), "GetBrowsers")]


        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=391";

        #endregion Queries ----------------------------------------------------

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =8  and TrancheID=391 ";

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

        private static long DebtID = -1;

        private static long GetDebtID()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                return conn.QuerySingle(RANDOM_DEBT_QUERY).debtID;
            }
        }


       
        

        public void CreateNewCreditCard2(long debtorID, string cardNumber, string nameOnCard, int index = -1)
        {

            debtorID = GetDebtorID();

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debtor/{1}", accessor.BaseURL, debtorID));
            accessor.ClickTab(string.Format("#Debtor{0}", debtorID));
            accessor.WaitForElementToBeDisplayed(string.Format("Debtor{0}", debtorID), 10); // Main debtor tab

            accessor.ClickTab(string.Format("#debtor{0}BankDetails", debtorID));

          

            System.Threading.Thread.Sleep(2000);

            // Get the add address button
            IWebElement addButton = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debtor{0}CreditCards']//button[@ng-click='accountCtrl.model.addCreditCard($event)']", debtorID));
            accessor.ClickElement(addButton);

           // var radioLabelXpath = string.Format("//label[@for='{0}']", string.Format("debtor{0}CreditCard{1}CreditCardTypeIDMaster", debtorID, index));

          
          // accessor.WaitForElementXpathToBeDisplayed(radioLabelXpath, 3);
           //accessor.ClickElement(accessor.GetElementByXPath(radioLabelXpath));


            var Mastercard = string.Format("debtor{0}CreditCard{1}CreditCardNumber", debtorID, index);
            accessor.WaitForElementToBeDisplayed(Mastercard, 3);
            accessor.SetElementValue(accessor.GetElement(Mastercard), cardNumber);

            var Expirydate = string.Format("debtor{0}CreditCard{1}ExpiryDate", debtorID, index);
            accessor.WaitForElementToBeDisplayed(Expirydate, 3);
            accessor.SetElementValue(accessor.GetElement(Expirydate), "08/2020");

            var nameoncard = string.Format("debtor{0}CreditCard{1}NameOnCard", debtorID, index); 
            accessor.WaitForElementToBeDisplayed(nameoncard, 3);
            accessor.SetElementValue(accessor.GetElement(nameoncard), nameOnCard); 
            accessor.Save();
        }




        
        private void CreateNewBankaccount(long debtorID,string bsb,string accountnumber,string name, int index = -1)
        {  


            
            accessor.ClickTab(string.Format("#debtor{0}BankDetails", DebtID));

            


            IWebElement addBankbutton = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debtor{0}BankAccounts']//button[@ng-click='accountCtrl.model.addBankAccount($event)']", debtorID));
            accessor.ClickElement(addBankbutton);

            accessor.WaitForElementToBeDisplayed(string.Format("debtor{0}BankAccount{1}BSB", debtorID,index), 3); 

            var BSB = string.Format("debtor{0}BankAccount{1}BSB", debtorID, index);
            accessor.WaitForElementToBeDisplayed(BSB, 3);
            accessor.SetElementValue(accessor.GetElement(BSB),bsb); 

            var account = string.Format("debtor{0}BankAccount{1}AccountNumber", debtorID, index);
            accessor.WaitForElementToBeDisplayed(account, 3);
            accessor.SetElementValue(accessor.GetElement(account), accountnumber); 

            var Accountname = string.Format("debtor{0}BankAccount{1}AccountName", debtorID, index);
            accessor.WaitForElementToBeDisplayed(Accountname, 3);
            accessor.SetElementValue(GetElement(Accountname), name); 

        }

        


        [Test]

        public void CreateBpayPayment()
        {
            debtorID = GetDebtorID();

           DebtID = GetDebtID();

           

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtID));
            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", DebtID));
          


            IWebElement Addpayment = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.model.addArrangement($event)']", DebtID));
            accessor.ClickElement(Addpayment);
            System.Threading.Thread.Sleep(2000);

            var customer = string.Format("debt{0}Arrangement{1}DebtorEntityID", DebtID, -1);
            accessor.WaitForElementToBeDisplayed(customer, 3);
            accessor.SetElementByClick(accessor.GetElement(customer));

            var paymentmethod = string.Format("debt{0}Arrangement{1}PaymentMethodID", DebtID, -1);
            accessor.WaitForElementToBeDisplayed(paymentmethod, 3);
            accessor.SetElementValue(accessor.GetElement(paymentmethod), "Bpay");

            System.Threading.Thread.Sleep(2000);

            accessor.Save();

            System.Threading.Thread.Sleep(2000);

            accessor.RefreshPage();
            accessor.Close();

        }

        [Test]

        public void Createdepositpayment()
        {
            DebtID = GetDebtID();
            debtorID = GetDebtorID();


            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtID));
            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", DebtID));
          

            
            IWebElement Addpayment = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.model.addArrangement($event)']", DebtID));
            accessor.ClickElement(Addpayment);

            System.Threading.Thread.Sleep(2000);

            var customer = string.Format("debt{0}Arrangement{1}DebtorEntityID", DebtID, -1);
            accessor.WaitForElementToBeDisplayed(customer, 3);
            accessor.SetElementByClick(accessor.GetElement(customer));

            
            var paymentmethod = string.Format("debt{0}Arrangement{1}PaymentMethodID", DebtID, -1);
            accessor.WaitForElementToBeDisplayed(paymentmethod, 3);
            accessor.SetElementValue(accessor.GetElement(paymentmethod), "Deposit");

            var amount = string.Format("debt{0}Arrangement{1}Amount", DebtID, -1);
            accessor.WaitForElementToBeDisplayed(amount, 3);
            accessor.SetNewElement(accessor.GetElement(amount), "555");


            IWebElement OriginalAmount = accessor.GetElement(amount);
            string ActualAmount = OriginalAmount.GetAttribute("value");


            System.Threading.Thread.Sleep(2000);

            accessor.Save();

            System.Threading.Thread.Sleep(2000);

            accessor.RefreshPage();
            accessor.Close();



            // Assert.AreEqual("100",ActualAmount);

           

        }
        


    }

}
