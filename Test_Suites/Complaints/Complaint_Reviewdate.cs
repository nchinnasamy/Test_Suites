using Dapper;
using NUnit.Framework;
using Payquest_Testing;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Protractor;
using OpenQA.Selenium;

namespace Test_Suites.Complaints
{
   public  class Complaint_Reviewdate
    {
        private static Class1 accessor = new Class1();

        #region Queries----------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 18 and TrancheID=2056";

        #endregion Queries---------------------------------------------------------------------------------------------------------------

        #region Queries-----------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 18 and TrancheID=2056";

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

        [TearDown]

        public void Teardown()
        {
            accessor.driver.Close();

        }

        [Test]

        public void DebtManagement_Assigneduser()
        {

            DebtId = GetDebtID();

            DebtorID = GetDebtorID();

            accessor.Open(string.Format(@"{0}/Complaints", accessor.BaseURL));

            IWebElement referenceno = accessor.GetElementByID("debtID");
            referenceno.SendKeys(DebtId.ToString());

            System.Threading.Thread.Sleep(3000);

            var search = accessor.GetElementByID("complaintSearchBtn");
            search.Click();

            var complaint = accessor.GetElementByXPath("//table/tbody/tr[1]");
            complaint.Click();

           
          
           // Complaint_notes.SendKeys("TEST");


            var Update = accessor.GetElementByXPath("//button[@ng-click='complaintCtrl.update()']");
            Update.Click();


        }

        [Test]

        public void Reviewedby()
        {



        }

    }
}
