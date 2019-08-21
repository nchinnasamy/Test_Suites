using System;
using Payquest_Testing;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using OpenQA.Selenium;

namespace Test_Suites.DocumentRequest
{
    [TestFixture]
    public class RaiseDocumentRequest
    {
        private static Class1 accessor = new Class1();

        #region Queries------------------------------------------------------------------------

        private const string RANDOM_DEBOTR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=697";

        #endregion Queries----------------------------------------------------------------------------

        #region Queries-------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=697";

        #endregion Queries-----------------------------------------------------------------------------

        #region Queries------------------------------------------------------------------------

        private const string RANDOM_DOCUMENTREQUESTID= @"select documentRequestID from   Debt.DocumentRequest where debtID={0}";

        #endregion Queries-----------------------------------------------------------------------------


        #region Queries-------------------------------------------------------------------------------------

        private const string RANDOM_CONTACT_HOLD = @"select HoldID from Debt.Hold where DebtorEntityID={0}";

        #endregion Queries----------------------------------------------------------------------------------



        private static long debtorID = -1;

        

        private static long GetdebtorID()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                return conn.QuerySingle(RANDOM_DEBOTR_QUERY).DebtorEntityID;
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

        private static  long documentID = -1;

        private static long GetDocumentRequestID()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Genericconnection"].ConnectionString))
            {
                conn.Open();
                var query = string.Format(RANDOM_DOCUMENTREQUESTID, DebtID);
                var result = conn.QuerySingle(query);
                return result.documentRequestID;
            }

        }


        private static long HoldId = -1;

        private static long GetHoldID()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Genericconnection"].ConnectionString))
            {

                conn.Open();
                var Query = string.Format(RANDOM_CONTACT_HOLD, debtorID);
                var result = conn.QuerySingle(Query);
                return result.HoldID;
            }
        }

        [OneTimeTearDown]

        public void Teardown()
        {
            accessor.Close();
        }

        [Test]
        [TestCase( "Complaint")]
        [TestCase("More Info")]

        public void CreateDocumentRequest(string Reason)
        {

            DebtID = GetDebtID();
            debtorID = GetdebtorID();
            
            
            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtID));

            var Fileaction = accessor.GetElementByID("debtDebtorFileAction");
            Fileaction.Click();


            var DocumentRequest = accessor.GetElementByXPath("//*[@id='debtDebtorFileAction']/ul/li[4]/a");
            DocumentRequest.Click();

            var SelectAccount = accessor.GetElementByID("documentTypeID");
            SelectAccount.Click();


            accessor.SetElementValue(accessor.GetElementByID("documentTypeID"), "Account Statement");

            var DocumentReason = accessor.GetElementByID("documentRequestReasonID");
            DocumentReason.Click();

            accessor.SetElementValue(accessor.GetElementByID("documentRequestReasonID"), "Complaint");

            var forwardresponse = accessor.GetElementByID("forwardToEntityID");
            forwardresponse.Click();

            accessor.SetElementByClick(accessor.GetElementByID("forwardToEntityID"));

            var submit = accessor.GetElementByXPath("/html/body/div[1]/div/div/form/div[4]/button[2]");
            submit.Click();
            System.Threading.Thread.Sleep(2000);
            accessor.Save();
            System.Threading.Thread.Sleep(2000);
            accessor.RefreshPage();

            var DebtstatusId = accessor.GetElementValue(string.Format("debt{0}DebtStatusID", DebtID),2);

            Assert.AreEqual(DebtstatusId, "number:25");

            accessor.ClickTab(string.Format("#debt{0}DocumentRequest",DebtID));

            documentID = GetDocumentRequestID();

            var Document=accessor.GetPanel(string.Format("debt{0}DocRequest{1}", DebtID, documentID));
            
            

            var DocumentType = string.Format("debt{0}DocRequest{1}DocumentTypeID", DebtID, documentID);
            var DocumentType_value = accessor.GetElementValue(DocumentType, 2);
            var DocumentReasonID = string.Format("debt{0}DocRequest{1}DocumentRequestReasonID", DebtID, documentID);
            var DocumnetReasonID_value = accessor.GetElementValue(DocumentReasonID, 2);

            Assert.AreEqual(DocumentType_value, "number:2");
            Assert.AreEqual(DocumnetReasonID_value, "number:1");

            accessor.ClickTab(string.Format("#debtor{0}Contact",debtorID));

            var Contactpanel = accessor.GetPanel(string.Format("debtor{0}DebtorContact",debtorID));
           

            var ExpandContactHold = accessor.GetPanel(string.Format("debtor{0}DebtorHolds", debtorID));
           
            

            HoldId = GetHoldID(); 

            var Expand_HOLDID = accessor.GetPanel(string.Format("debtorHold{0}", HoldId));
          

            

        }
    }
}
