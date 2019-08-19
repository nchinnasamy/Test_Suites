using System;


namespace PayQuest_dataModels
{
    public partial class Gender
    {

        public byte GenderID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string DefaultImage { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public short UpdatedByUserID { get; set; }
    }
}

