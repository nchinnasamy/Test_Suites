using Dapper;
using NUnit.Framework;
using Payquest_Testing;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Protractor;
using OpenQA.Selenium;

namespace Test_Suites.PaymentArrangement
{


    [TestFixture]
    
    

    public class PaymentArrangement_wizard_Bankaccount
    {
        private static Class1 accessor = new Class1();

        #region Queries------------------------------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=697";

        #endregion Queires-----------------------------------------------------------------------------

        #region Queries---------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 8 and TrancheID=697";

        #endregion Queries----------------------------------------------------------------------------------

        #region Queries------------------------------------------------------------------------------------------

        private const string RANDOM_ARRANGEMENT_QUERY = @"SELECT TOP(1) ArrangementID  FROM Debt.Debt ddd JOIN Pay.Arrangement d ON ddd.DebtID = d.DebtID WHERE ddd.DebtStatusID= 10 and TrancheID=697 and d.CommencementDate='2019-06-18' ORDER BY ArrangementID DESC";

        #endregion Queries-------------------------------------------------------------------------------------------- 


       



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

        private static long arrangementID = 1;

        private static long GetarrangementID()
         {
          using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
          {
           conn.Open();
        
            return conn.QuerySingle(RANDOM_ARRANGEMENT_QUERY).ArrangementID;

          }
          }
        [OneTimeTearDown]

        public static void close()
        {

            accessor.driver.Close();

        }

        [Test]

        public void CreatePaymentArrangementWizard_Directdebit()
        {
            DebtID = GetDebtID();
          //  arrangementID = GetarrangementID();

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, DebtID));
            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", DebtID));


            IWebElement addPaymentWizard = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.addArrangementViaWizard($event)']", DebtID));
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
            var debt = string.Format("debt{0}DebtStatusID", DebtID);
            accessor.GetElementByID(debt);

            var actualdebtstatus = accessor.GetElementValue(debt, 5);

            Assert.AreEqual("number:10", actualdebtstatus);

            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", DebtID));

            
            arrangementID = GetarrangementID();  


            IWebElement ExpandCreditcardpanel = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']", DebtID, -1));
            ExpandCreditcardpanel.Click();

            System.Threading.Thread.Sleep(2000);

            IWebElement ExpandpaymentArrangemnetPanel = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}Arrangement{1}']", DebtID, arrangementID));
            ExpandpaymentArrangemnetPanel.Click();

            var actualamount = accessor.GetElementValue(string.Format("debt{0}Arrangement{1}Amount", DebtID, arrangementID),5);
           // Assert.AreEqual("100", actualamount);

            accessor.Close();





        }


    }

    }

