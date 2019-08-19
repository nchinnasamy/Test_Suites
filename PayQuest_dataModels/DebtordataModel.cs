using System;


namespace PayQuest_dataModels
{
    public partial class DebtordataModel
    {
        public int? SMDBReference { get; set; }
        public byte? MaritalStatusID { get; set; }
        public string LicenceNo { get; set; }
        public short? LicenceStateID { get; set; }
        public bool IsOverseas { get; set; }
        public bool IsDeceased { get; set; }
    }
}
