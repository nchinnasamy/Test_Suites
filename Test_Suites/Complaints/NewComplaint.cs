using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using NUnit.Framework;
using Payquest_Testing;
using OpenQA.Selenium;


namespace Test_Suites.Complaints
{
    [TestFixture]


    public class NewComplaint:Class1

    {
        private static Class1 accessor = new Class1();
        

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 6 and TrancheID=925";

        #endregion Queries ----------------------------------------------------

        #region Queries -------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID =6  and TrancheID=925 ";

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


        [OneTimeSetUp]

        public void openURL()
        {
            DebtID = GetDebtID();

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtID));

        }
        [Test]

        public void ComplaintInitialize()
        {
            debtorID = GetDebtorID();
            DebtID = GetDebtID();

            

            accessor.ClickTab(string.Format("#debtor{0}Complaints", debtorID));

            IWebElement addComplaint = accessor.GetElementByXPath(string.Format("//div[@id='debtor{0}Complaints']//button[@ng-click='complaintCtrl.model.addNewComplaint($event)']", debtorID));
            accessor.ClickElement(addComplaint);

            var Madeby = string.Format("debtor{0}Complaint{1}ComplainerEntityID", debtorID, -1);
            accessor.WaitForElementToBeDisplayed(Madeby, 5);
            accessor.SetElementByClick(accessor.GetElementByID(Madeby));

            var ComplaintType = string.Format("debtor{0}Complaint{1}ComplaintCategoryID", debtorID, -1);
            accessor.WaitForElementToBeDisplayed(ComplaintType, 5);
            accessor.SetElementValue(accessor.GetElement(ComplaintType), "Liability Issues");

            System.Threading.Thread.Sleep(5000);
            var actual=accessor.GetElementValue(ComplaintType,10);

            //Assert.AreEqual("number:5",actual);

            accessor.ClickTab(string.Format("#debtor{0}Complaint{1}ReviewTab", debtorID, -1));

            //IWebElement reviewedby = accessor.GetElementByXPath(string.Format("//div[@id='debtor{0}Complaints']//button[@ng-click='complaintCtrl.userSearch(complaint, 'ReviewedByUserID')']", debtorID));
            // accessor.ClickElement(reviewedby);


            accessor.Save(); 

        }

        [TestCase("Complaint")]
        [TestCase("FOS")]
        [TestCase("ASIC")]
        [TestCase("Internal - Major")]
        [TestCase("Internal - Minor")]
        [TestCase("Dispute")]


        public void CreateComplaint(string text)
        {
            debtorID = GetDebtorID();

            var Fileaction=accessor.GetElementByID("debtDebtorFileAction");
            Fileaction.Click();

            var Complaint = accessor.GetElementByXPath(string.Format("//*[@id='debtDebtorFileAction']/ul/li[2]/a"));
            Complaint.Click();

            var ComplaintType = accessor.GetElementByID("complaintType");
            accessor.SetSelectedOption(ComplaintType, text);

            var madeby = accessor.GetElementByID("complaintMadeBy");
            accessor.SetElementByClick(madeby);

            System.Threading.Thread.Sleep(5000);

            var RaiseComplaint = accessor.GetElementByXPath("//*[@id='txtComplaintNotes']/div[2]");
            RaiseComplaint.Click();
            

            var ComplaintNotes = accessor.GetElementByXPath(".//div[@ng-model='html']");
            ComplaintNotes.SendKeys("Test to raise a complaint");

            IWebElement submit = accessor.GetElementByXPath("//button[@ng-click='newComplaintCtrl.sendComplaint()']");
            submit.Click();
            System.Threading.Thread.Sleep(5000);
            accessor.Save();
            

        }


    }
}
