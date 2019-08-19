using NUnit.Framework;
using Payquest_Testing;
using System;


namespace Test_Suites.PaymentArrangement
{

    [TestFixture]
    public class MultiDebtId_paymentArrangement
    {

        private static Class1 accessor = new Class1();

        private TrancheTestcase tranche = null;
        private DebtTestcase debt = null;
        private DebtorDetailTestcase  debtor1= null;
        private DebtorDetailTestcase debtor2 = null;
        private DebtDebtorsDetailTestCase DebtorDetail1 = null;
        private DebtDebtorsDetailTestCase DebtorDetail2 = null;




        [Test]

        public void MultiDebtID_paymentarrangement()
        {

            tranche = new TrancheTestcase(TrancheTestcase.TestCase.Create, 708);
            debt = new DebtTestcase(DebtTestcase.TestCase.New, tranche.TrancheID);
            debtor1 = new DebtorDetailTestcase(DebtorDetailTestcase.TestCase.BasicFemale);
            debtor2 = new DebtorDetailTestcase(DebtorDetailTestcase.TestCase.BasicMale);
            DebtorDetail1 = new DebtDebtorsDetailTestCase(DebtDebtorsDetailTestCase.TestCase.PrimaryDebtor, debt.DebtID, debtor1.debtorID, 1);
            DebtorDetail2 = new DebtDebtorsDetailTestCase(DebtDebtorsDetailTestCase.TestCase.PrimaryDebtor, debt.DebtID, debtor2.debtorID, 2);

            accessor.Open(string.Format(@"{0}/DebtDebtorDetails/Debt/{1}", accessor.BaseURL, debt.DebtID));
            accessor.Close();



        }

    }
}
