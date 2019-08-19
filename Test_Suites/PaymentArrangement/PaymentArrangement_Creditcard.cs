using System;
using Payquest_Testing;
using OpenQA.Selenium;
using NUnit;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;


namespace Test_Suites.TEST
{

    [TestFixture]
    public class PaymentArrangement_Creditcard
    {
        private static Class1 accessor = new Class1();

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 6 and TrancheID=697";

        #endregion Queries ----------------------------------------------------

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =6  and TrancheID=697 ";

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



        [Test]

        public void CreatePaymentarrangementWizard_Creditcard()
        {
            DebtID = GetDebtID();
            debtorID = GetDebtorID();

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtID));
            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", DebtID));

            IWebElement AddpaymentWizard = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.addArrangementViaWizard($event)']", DebtID));
            accessor.ClickElement(AddpaymentWizard);

            IWebElement nextbttn = accessor.GetElementByXPath(string.Format("//button[@ng-click='arrangementCtrl.nextPage()']"));
            accessor.ClickElement(nextbttn);

            IWebElement paymentmethod = accessor.GetElementByID("paymentMethodID");
            accessor.ClickElement(paymentmethod);

            accessor.SetSelectedOption(paymentmethod, "Debit/Credit Card");



            accessor.ClickElement(nextbttn);

            var addcreditcard = accessor.GetElementByXPath("//*[@id='creditCardPage']/div[2]/div[1]/div/label");

            addcreditcard.Click();

            var mastercard = accessor.GetElementByXPath("//*[@id='creditCardPage']/div[2]/div[2]/div[1]/div[2]/div[2]/div/label");
            mastercard.Click();

            var cardnumber = accessor.GetElementByXPath("//*[@id='creditCardNumber']");
            cardnumber.Click();
            accessor.SetElementValue(accessor.GetElementByXPath("//*[@id='creditCardNumber']"), "5163200000000008");

            var card = accessor.GetElementByXPath("//*[@id='expiryDate']");
            card.Click();
            accessor.SetElementValue(accessor.GetElementByXPath("//*[@id='expiryDate']"), "08/2020");

            var Cardname = accessor.GetElementByXPath("//*[@id='nameOnCard']");
            Cardname.Click();
            accessor.SetElementValue(accessor.GetElementByXPath("//*[@id='nameOnCard']"), "TEST");


            accessor.ClickElement(nextbttn);
            accessor.ClickElement(nextbttn);

            var next = accessor.GetElementByXPath("//*[@id='emailCorrespondencePage']/div[2]/div/div/label");
            next.Click();

            var emailID = accessor.GetElementByID("email");
            accessor.SetElementValue(emailID, "test@server.com");

            accessor.ClickElement(nextbttn);
            accessor.ClickElement(nextbttn);


            IWebElement Createpayment = accessor.GetElementByID("paWizardFinishBtn");
            accessor.ClickElement(Createpayment);

            System.Threading.Thread.Sleep(2000);
            accessor.Save();

            System.Threading.Thread.Sleep(5000);
            accessor.RefreshPage();

            System.Threading.Thread.Sleep(5000);

            var debt = string.Format("debt{0}DebtStatusID", DebtID);
            accessor.GetElementByID(debt);

            var actualdebtstatus = accessor.GetElementValue(debt, 5);

            Assert.AreEqual("number:10", actualdebtstatus);

            accessor.Close();







        }

    }
}
