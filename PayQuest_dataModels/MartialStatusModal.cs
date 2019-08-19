using System;


namespace PayQuest_dataModels
{
    public class MartialStatusModal
    {
        
            public byte MaritalStatusID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Active { get; set; }
            public System.DateTime UpdatedDate { get; set; }
            public short UpdatedByUserID { get; set; }
        
    }
}
