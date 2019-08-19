using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Payquest_Testing;




namespace Test_Suites.PaymentTransaction
{

    [TestFixture]
    public class PaymentTransaction
    {
        #region Constants -----------------------------------------------------

        private const string ADD_BTN_XPATH_QUERY = "//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.model.addArrangement($event)']";

        #endregion Constants --------------------------------------------------

        private static Class1 accessor = new Class1();

        private TrancheTestcase tranche = null;
        private DebtTestcase debt = null;
        private DebtorDetailTestcase debtor = null;
        private DebtDebtorsDetailTestCase debtDebtorsDetail = null;


        #region Queries------------------------------------------------------------------------------------------

        private const string RANDOM_ARRANGEMENT_QUERY = @"SELECT TOP(1) ArrangementID  FROM Debt.Debt ddd JOIN Pay.Arrangement d ON ddd.DebtID = d.DebtID WHERE ddd.DebtStatusID= 10 and TrancheID=697 and d.CommencementDate='2019-03-18' ORDER BY ArrangementID DESC";

        #endregion Queries-------------------------------------------------------------------------------------------- 

        

       /* private void CleanupPaymentArrangement(string tablename,string  columnname,int debtID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenericConnection"].ConnectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("DELETE from" + " "+ tablename +" "+ "where" + " "+ columnname +" = "+ debtID,conn))
                {
                    
                    command.ExecuteNonQuery();
                }
              
            }
        }
        */

        [Test]

        public void TestInit()
        {
            tranche = new TrancheTestcase(TrancheTestcase.TestCase.Create,708);

            debt = new DebtTestcase(DebtTestcase.TestCase.New, tranche.TrancheID);

            debtor = new DebtorDetailTestcase(DebtorDetailTestcase.TestCase.BasicMale);
            debtDebtorsDetail = new DebtDebtorsDetailTestCase(DebtDebtorsDetailTestCase.TestCase.PrimaryDebtor, debt.DebtID, debtor.debtorID, 1);


            //accessor.Open(string.Format(@"{0}DebtDebtorDetails/debt/{1}", accessor.BaseURL, debt.DebtID));

            //accessor.WaitForElementToBeDisplayed(string.Format("Debt{0}", debt.DebtID), 10); // Main debt tab 
            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, debt.DebtID));

            accessor.ClickTab(string.Format("#debt{0}PaymentArrangementsTab", debt.DebtID)); 


            IWebElement addPaymentWizard = accessor.GetElementByXPath(string.Format("//div[@collapsible-panel='debt{0}PaymentArrangementsPanel']//button[@ng-click='arrangementsCtrl.addArrangementViaWizard($event)']", debt.DebtID));
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
            System.Threading.Thread.Sleep(2000);
            accessor.RefreshPage();



            
        }

        
    }
}
