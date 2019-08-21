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



    public class Payment_MultidebtID : Class1
    {
        private static Class1 accessor = new Class1();


        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =10 and TrancheID=852";

        #endregion Queries ----------------------------------------------------

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =10 and TrancheID=852";
        private readonly double actualdebtstatus;

        #endregion Queries ----------------------------------------------------

        private static long debtID = -1;

        private static long GetDebtID()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                return conn.QuerySingle(RANDOM_DEBT_QUERY).debtID;
            }

        }

        [Test]

        public void CreatePaymentArrangementWizard_Directdebit()
        {
            debtID = GetDebtID();
            

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, debtID));
            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", debtID));


            IWebElement addPaymentWizard = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.addArrangementViaWizard($event)']", debtID));
            addPaymentWizard.Click();

            IWebElement part = accessor.GetElementByXPath("//*[@id='arrangementTypePage']/div[1]/div[3]/div/div/label/strong");
            part.Click();

            IWebElement nextbttn = accessor.GetElementByXPath(string.Format("//button[@ng-click='arrangementCtrl.nextPage()']"));
            accessor.ClickElement(nextbttn);

            IWebElement paymentmethod = accessor.GetElementByID("paymentMethodID");
            accessor.ClickElement(paymentmethod);

            accessor.SetSelectedOption(paymentmethod, "Direct Debit");
            accessor.ClickElement(nextbttn);

            var bankaccount = accessor.GetElementByXPath("//*[@id='bankAccountPage']/div[2]/div[1]/div/div/label");
            bankaccount.Click();

            var BSB = accessor.GetElementByID("BSB");
            BSB.Click();
            accessor.SetNewElement(BSB, "980600");

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

            

            Assert.AreEqual("number:10", actualdebtstatus);

            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", debtID));


            accessor.Close();





        }



    }

}