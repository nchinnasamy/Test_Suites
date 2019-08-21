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



    public class Transaction 

    {
        private static Class1 accessor = new Class1();
       


        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 10 and TrancheID=391";

        #endregion Queries ----------------------------------------------------

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =10  and TrancheID=391 ";

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
        public static void CreditcardTransaction()
        {

            debtorID = GetDebtorID();
            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debtor/{1}", accessor.BaseURL, debtorID));
            accessor.ClickTab(string.Format("#Debtor{0}", debtorID));
            accessor.WaitForElementToBeDisplayed(string.Format("Debtor{0}", debtorID), 10); // Main debtor tab

            accessor.ClickTab(string.Format("#debtor{0}BankDetails", debtorID));

          


            //accessor.WaitForElementXpathToBeDisplayed(radioLabelXpath, 3);
            // accessor.ClickElement(accessor.GetElementByXPath(radioLabelXpath));
            System.Threading.Thread.Sleep(2000);

            var Mastercard = string.Format("debtor{0}CreditCard{1}CreditCardNumber", debtorID, -1);
            accessor.WaitForElementToBeDisplayed(Mastercard, 5);
            accessor.SetElementValue(accessor.GetElement(Mastercard), "5163200000000008");

            var Expirydate = string.Format("debtor{0}CreditCard{1}ExpiryDate", debtorID, -1);
            accessor.WaitForElementToBeDisplayed(Expirydate, 5);
            accessor.SetElementValue(accessor.GetElement(Expirydate), "08/2020");

            var nameoncard = string.Format("debtor{0}CreditCard{1}NameOnCard", debtorID, -1);
            accessor.WaitForElementToBeDisplayed(nameoncard, 5);
            accessor.SetElementValue(accessor.GetElement(nameoncard),"TEST");
            accessor.Save();

            accessor.Close();



        }

    }
}
