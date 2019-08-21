using Payquest_Testing;
using System;


namespace Test_Suites.LegalReferal
{
    public class RaiseLegalReferal
    {
        private static Class1 accessor = new Class1();

        #region Queries-----------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBTOR_QUERY = @"SELECT TOP(1) ddd.DebtorEntityID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 5 and TrancheID=400";

        #endregion Queries---------------------------------------------------------------------------------------------------------------------


        #region Queries---------------------------------------------------------------------------------------------------------------------------

        private const string RANDOM_DEBT_QUERY = @"SELECT TOP(1) ddd.debtID FROM Debt.DebtDebtorDetail ddd JOIN Debt.Debt  d ON ddd.DebtID = d.DebtID WHERE d.DebtStatusID = 5 and TrancheID=400";

        #endregion Queries--------------------------------------------------------------------------------------------------------------------------------




    }
}
