using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using NUnit.Framework;
using OpenQA.Selenium;
using Payquest_Testing;

namespace Test_Suites.PaymentArrangement
{

    [TestFixture]

    public class PaymentArrangement_Code
    {
        private static Class1 accessor = new Class1();

        #region Queries---------------------------------------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 2 and TrancheID=1367";

        #endregion Queries---------------------------------------------------------------------------------------

        #region Queries---------------------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.DebtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 2 and TrancheID=1367";

        #endregion Queries---------------------------------------------------------------------------------------------

        #region Queries------------------------------------------------------------------------------------------

        private const string RANDOM_ARRANGEMENT_QUERY = @"SELECT TOP(1) ArrangementID  FROM Debt.Debt ddd JOIN Pay.Arrangement d ON ddd.DebtID = d.DebtID WHERE ddd.DebtStatusID= 10 and TrancheID=1367 and d.CommencementDate='2019-06-18' ORDER BY ArrangementID DESC";

        #endregion Queries-------------------------------------------------------------------------------------------- 

        private static long Debtorid = -1;



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

                return conn.QuerySingle(RANDOM_DEBT_QUERY).DebtID;
            }
        }

        private static long arrangementID = 1;

        private static long GetarrangementID()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                return conn.QuerySingle(RANDOM_ARRANGEMENT_QUERY).ArrangementID;

            }
        }


        [OneTimeSetUp]

        public static void Open()
        {
            debtID = GetDebtID();
            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, debtID));

        }

        [OneTimeTearDown]

        public static void close()
        {

            accessor.driver.Close();
            
        }

        [Test]
        [TestCase ("Dishonoured")]
        [TestCase("Discounted")]

        public void paymentarrangementCode(string text)
        {
            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", debtID));

            IWebElement paymentarrangementWizard = accessor.GetElementByXPath("//button[@ng-click='arrangementsCtrl.addArrangementViaWizard($event)']");
            paymentarrangementWizard.Click();

             IWebElement discount = accessor.GetElementByXPath("//*[@id='arrangementTypePage']/div[1]/div[5]/div/div/label");
            discount.Click();

            IWebElement nextbttn = accessor.GetElementByXPath(string.Format("//button[@ng-click='arrangementCtrl.nextPage()']"));
            accessor.ClickElement(nextbttn);

            var discountoffer = accessor.GetElementByID("discountedTotalAmount");
            discountoffer.Click();
            discountoffer.SendKeys("500"); 



            var nextbuttondiscount = accessor.GetElementByID("paWizardNextBtn");
            nextbuttondiscount.Click();
            accessor.ClickElement(nextbuttondiscount);

            IWebElement paymentmethod = accessor.GetElementByID("paymentMethodID");
            accessor.ClickElement(paymentmethod);

            accessor.SetSelectedOption(paymentmethod, "Direct Debit");
            accessor.ClickElement(nextbttn);

            var bankaccount = accessor.GetElementByXPath("//*[@id='bankAccountPage']/div[2]/div[1]/div/div/label");
            bankaccount.Click();

            var BSB = accessor.GetElementByID("BSB");
            BSB.Click();
            accessor.SetNewElement(BSB, "980400");

            var accountnumber = accessor.GetElementByID("accountNumber");
            accountnumber.Click();
            accessor.SetElementValue(accountnumber, "123456");

            var accountname = accessor.GetElementByID("accountName");
            accountname.Click();
            accessor.SetElementValue(accountname, "TEST");

            accessor.ClickElement(nextbttn);
            accessor.ClickElement(nextbttn);

            var next = accessor.GetElementByXPath("//*[@id='emailCorrespondencePage']/div[2]/div/div/label");
            next.Click();

            var emailID = accessor.GetElementByID("email");
            accessor.SetElementValue(emailID, "test@server.com");

            accessor.ClickElement(nextbttn);

            var nextbutton = accessor.GetElementByID("paWizardNextBtn");
            nextbutton.Click();
            accessor.ClickElement(nextbttn);

            IWebElement Createpayment = accessor.GetElementByID("paWizardFinishBtn");
            accessor.ClickElement(Createpayment);

            System.Threading.Thread.Sleep(2000);
            accessor.Save();

            System.Threading.Thread.Sleep(5000);
            accessor.RefreshPage();
            var debt = string.Format("debt{0}DebtStatusID", debtID);
            accessor.GetElementByID(debt);

            var actualdebtstatus = accessor.GetElementValue(debt, 5);

            Assert.AreEqual("number:10", actualdebtstatus);

            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", debtID));
            IWebElement ExpandCreditcardpanel = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']", debtID, -1));
            ExpandCreditcardpanel.Click();

            System.Threading.Thread.Sleep(2000);

            arrangementID = GetarrangementID();
            IWebElement ExpandpaymentArrangemnetPanel = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}Arrangement{1}']", debtID, arrangementID));
            ExpandpaymentArrangemnetPanel.Click();

            var dishonoured = accessor.GetElementByID(string.Format("debt{0}Arrangement{1}ArrangementCompletionCodeID", debtID, arrangementID));
            dishonoured.Click();
            dishonoured.SendKeys(text);
            System.Threading.Thread.Sleep(2000);
            accessor.Save();


        }

    }
}
