using Dapper;
using NUnit.Framework;
using OpenQA.Selenium;
using Payquest_Testing;

using System;
using System.Configuration;
using System.Data.SqlClient;
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using AventStack.ExtentReports.Reporter;
using System.IO;

namespace Test_Suites.Legals
{
    public class Payment_Arrangement_Statistics
    {

        private static Class1 accessor = new Class1();
        

        #region Queries------------------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 9 and TrancheID=1222";

        #endregion Queries-----------------------------------------------------------------------------------------------------------------------

        #region Queries-----------------------------------------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 9 and TrancheID=1222";

        #endregion Queries----------------------------------------------------------------------------------------------------------------------------------------------

        private static long DebtorID = -1;

        private static long GetDebtorID()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                return conn.QuerySingle(RANDOM_DEBTOR_QUERY).DebtorEntityID;

            }

        }


        private static long DebtId = -1;

        private static long GetDebtID()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))

            {

                conn.Open();

                return conn.QuerySingle(RANDOM_DEBT_QUERY).debtID;
            }
        }





        protected ExtentReports _extent;
        protected ExtentTest _test;

        [OneTimeSetUp]

        public void Onetimesetup()
        {

            try
            {

                //To create report directory and add HTML report into it

                _extent = new ExtentReports();

                var dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\PROD", "");
                DirectoryInfo di = Directory.CreateDirectory(dir + "Test_Execution_Reports");
                var htmlReporter = new ExtentHtmlReporter(dir + "Test_Execution_Reports" + "\\Automation_Report" + ".html");
                var extent = new ExtentReports();
                _extent.AttachReporter(htmlReporter);

            }

            catch (Exception e)
            {
                throw (e);
            }

        }
        [SetUp]
        public void BeforeTest()
        {
            try
            {
                _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        [TearDown]
        public void AfterTest()
        {
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                var stacktrace = "" + TestContext.CurrentContext.Result.StackTrace + "";
                var errorMessage = TestContext.CurrentContext.Result.Message;
                Status logstatus;
                switch (status)
                {
                    case TestStatus.Skipped:
                        logstatus = Status.Skip;
                        _test.Log(logstatus, "Test ended with " + logstatus);
                        break;
                    default:
                        logstatus = Status.Pass;
                        _test.Log(logstatus, "Test ended with " + logstatus);
                        break;
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
        }


        [OneTimeTearDown]
        public void AfterClass()
        {
            try
            {
                _extent.Flush();
            }
            catch (Exception e)
            {
                throw (e);
            }
            accessor.driver.Quit();
        }
        [Test]

        public void paymentArrangement()
        {
            

            DebtId = GetDebtID();

            accessor.Open(string.Format("{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtId));

            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", DebtId));


            //IWebElement addPaymentWizard = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.addArrangementViaWizard($event)']", debtID));
            IWebElement paymentwizard = accessor.GetElementByXPath(string.Format("//button[@ng-click='arrangementsCtrl.addArrangementViaWizard($event)']"));
            paymentwizard.Click();


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

            var card = accessor.GetElementByXPath("[@id='expiryDate']");
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

            var debt = string.Format("debt{0}DebtStatusID", DebtId);
            accessor.GetElementByID(debt);

            var actualdebtstatus = accessor.GetElementValue(debt, 5);

            Assert.AreEqual("number:10", actualdebtstatus);

            accessor.Open(string.Format(@"{0}/Statistics", accessor.BaseURL));
            System.Threading.Thread.Sleep(5000);

    

            accessor.Close();

          



        }


        [Test]

        public void Oneoff_statistics()
        {

            DebtorID = GetDebtorID();

            DebtId = GetDebtID();

            accessor.Open(string.Format("{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtId));

            accessor.ClickTab(string.Format("#debtor{0}BankDetails", DebtorID));

            IWebElement addcreditcard = accessor.GetElementByXPath(string.Format("//button[@ng-click='accountCtrl.model.addCreditCard($event)']"));
            addcreditcard.Click();

            System.Threading.Thread.Sleep(2000);

            var addmastercard = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}CreditCardNumber", DebtorID, -1));
            addmastercard.Click();
            accessor.SetElementValue(addmastercard, "5163200000000008");

            var expirydate = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}ExpiryDate", DebtorID, -1));
            expirydate.Click();
            accessor.SetElementValue(expirydate, "08/2020");

            var nameoncard = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}NameOnCard", DebtorID, -1));
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


            accessor.Open(string.Format(@"{0}/Statistics", accessor.BaseURL));
            IWebElement oneoff = accessor.GetElementByID("cashChart");
            oneoff.Click();
            System.Threading.Thread.Sleep(5000);

            accessor.Close();


        }


        [Test]

        public void Oneoff_Transaction()
        {

            DebtorID = GetDebtorID();

            DebtId = GetDebtID();

            accessor.Open(string.Format("{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtId));

            accessor.ClickTab(string.Format("#debtor{0}BankDetails", DebtorID));

            IWebElement addcreditcard = accessor.GetElementByXPath(string.Format("//button[@ng-click='accountCtrl.model.addCreditCard($event)']"));
            addcreditcard.Click();

            System.Threading.Thread.Sleep(2000);

            var addmastercard = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}CreditCardNumber", DebtorID, -1));
            addmastercard.Click();
            accessor.SetElementValue(addmastercard, "5163200000000008");

            var expirydate = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}ExpiryDate", DebtorID, -1));
            expirydate.Click();
            accessor.SetElementValue(expirydate, "08/2020");

            var nameoncard = accessor.GetElementByID(string.Format("debtor{0}CreditCard{1}NameOnCard", DebtorID, -1));
            nameoncard.Click();
            accessor.SetElementValue(nameoncard, "TEST");

            accessor.Save();

            IWebElement processcreditcard = accessor.GetElementByXPath(string.Format("//button[@ng-click='accountCtrl.processCreditCard($event, creditCard)']"));
            processcreditcard.Click();

            var process = accessor.GetElementByID("existingCardCVN");
            accessor.SetElementValue(process, "050");

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


            accessor.Open(string.Format(@"{0}/Statistics", accessor.BaseURL));
            IWebElement oneoff = accessor.GetElementByID("dailyCollectedRevenueChart");
            oneoff.Click();

            System.Threading.Thread.Sleep(5000);
          
            accessor.Close();


        }


    }
}
