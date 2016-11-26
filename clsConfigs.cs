using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timeMachine
{
    class clsConfigs
    {

        public const string gsStaticRefs = "staticRefs.xml";

        //public const string gsOutputLocation = @"C:\Temp\" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + ".csv";

        //This should probs be changed to the Timesheet_ is left off. What's the best way to handle the filename...??
        public const string gsOutputLocation = @"E:\TimeMachine\Today\Timesheet_";

        public const string gsSummaryLocation = @"E:\TimeMachine\Historical\Summaries\";

        //public const string gsExtRefFile = @"E:\Temp\report.csv";
        public const string gsExtRefFile = @"E:\TimeMachine\report.csv";

        //For production, change E drive to C:
        public const string gsFileDir = @"E:\TimeMachine";

    }
}
