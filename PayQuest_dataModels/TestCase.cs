using System;


namespace PayQuest_dataModels
{
   
        //
        // Summary:
        //     Base YouOi test case class
        public abstract class TestCase : IDisposable
        {
            //
            // Summary:
            //     539 characters of random text
            protected const string LONG_TEXT = "However venture pursuit he am mr cordial. Forming musical am hearing studied be luckily. Ourselves for determine attending how led gentleman sincerity. Valley afford uneasy joy she thrown though bed set. In me forming general prudent on country carried. Behaved an or suppose justice. Seemed whence how son rather easily and change missed. Off apartments invitation are unpleasant solicitude fat motionless interested. Hardly suffer wisdom wishes valley as an. As friendship advantages resolution it alteration stimulated he or increasing.";
            //
            // Summary:
            //     217 characters of random text
            protected const string MEDIUM_TEXT = "Impossible considered invitation him men instrument saw celebrated unpleasant. Put rest and must set kind next many near nay. He exquisite continued explained middleton am. Voice hours young woody has she think equal.";
            //
            // Summary:
            //     116 characters of random text
            protected const string SHORT_TEXT = "Forming musical am hearing studied be luckily. Ourselves for determine attending how led gentleman offset sincerity.";


            public abstract void Dispose();

            //
            // Summary:
            //     Cleans up the instance.
            protected abstract void CleanUp();

            protected abstract void Initialise();
        }

    
}
