using Dapper;
using NUnit.Framework;
using OpenQA.Selenium;
using Payquest_Testing;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Test_Suites.Legals
{
    public class Add_NewLegalreferal
    {
        private static Class1 accessor = new Class1();

        #region Queries------------------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=2056";

        #endregion Queries-----------------------------------------------------------------------------------------------------------------------

        #region Queries-----------------------------------------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=2056";

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


        [OneTimeTearDown]

        public void Teardown()
        {

            accessor.driver.Close();
        }

      

       

    }
}
