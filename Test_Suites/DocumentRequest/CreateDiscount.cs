using Dapper;
using NUnit.Framework;
using Payquest_Testing;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Protractor;
using OpenQA.Selenium;


namespace Test_Suites.DocumentRequest
{
    [TestFixture]

    public class CreateDiscount
    {
        private static Class1 accessor = new Class1();

        #region Queries----------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=691";

        #endregion Queries---------------------------------------------------------------------------------------------------------------

        #region Queries-----------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=691";

        #endregion Queries-----------------------------------------------------------------------------------------------------------------

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


        [OneTimeTearDown]

        public static void Teardown()
        {

            accessor.driver.Close();

        }

        [Test]

        public void CreateDiscountOffer()
        {
            DebtId = GetDebtID();

            DebtorID = GetDebtorID();

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtId));

            IWebElement Addemail=accessor.GetElementByXPath(string.Format("//button[@ng-click='debtorCtrl.model.addEmailAddress($event)']"));
            Addemail.Click();

            IWebElement emailId = accessor.GetElement_model(string.Format("email.Address"));
            emailId.SendKeys("test@server.com");

            accessor.Save();
            System.Threading.Thread.Sleep(3000);
            accessor.RefreshPage();

            var sendemail = accessor.GetElementByID(string.Format("debtor{0}SendEmail", DebtorID));
            sendemail.Click();





        }



    }
}
