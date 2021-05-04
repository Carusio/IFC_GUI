using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.Models
{
    // TaskTimeModel based on IfcTaskTime class
    public class TaskTimeModel
    {
        // IfcSchedulingTime attributes
        public string Name { get; set; }
        public int DataOrigin { get; set; }
        public string UserDefinedDataOrigin { get; set; }

        // IfcTasktTime attributes
        public int DurationType { get; set; }
        public string ScheduleDuration { get; set; } // PnYnMnDTnHnMnS, where nY represents the number of years, nM the number of months, nD the number of days, 
                                                     // 'T' is the date/time separator, nH the number of hours, nM the number of minutes and nS the number of seconds
        // the next attributes should be in Format
        //YYYY-MM-DDThh:mm:ss where "YYYY" represent the year, "MM" the month and "DD" the day, preceded by an optional leading "-" sign to indicate a negative year number
        public string ScheduleStart { get; set; } //YYYY-MM-DDThh:mm:ss
        public string ScheduleFinish { get; set; } //YYYY-MM-DDThh:mm:ss
        public string EarlyStart { get; set; } //YYYY-MM-DDThh:mm:ss
        public string EarlyFinish { get; set; } //YYYY-MM-DDThh:mm:ss
        public string LateStart { get; set; } //YYYY-MM-DDThh:mm:ss
        public string LateFinish { get; set; } //YYYY-MM-DDThh:mm:ss
        public string FreeFloat { get; set; } // PnYnMnDTnHnMnS
        public string TotalFloat { get; set; } // PnYnMnDTnHnMnS
        public bool IsCritical { get; set; }
        public string StatusTime { get; set; } //YYYY-MM-DDThh:mm:ss
        public string ActualDuration { get; set; } // PnYnMnDTnHnMnS
        public string ActualStart { get; set; } //YYYY-MM-DDThh:mm:ss
        public string ActualFinish { get; set; } //YYYY-MM-DDThh:mm:ss
        public string RemainingTime { get; set; } // PnYnMnDTnHnMnS
        public string Completion { get; set; }
        public TaskTimeModel()
        {
            
        }

        public TaskTimeModel(string name, int dataorigin, string userdefineddataorigin, int durationtype, string scheduleduration, string schedulestart, string schedulefinish, string earlystart, string earlyfinish, string latestart, string latefinish,
            string freefloat, string totalfloat, bool iscritical, string statustime, string actualduration, string actualstart, string actualfinish, string remainingtime, string completion)
        {
            Name = name;
            DataOrigin = dataorigin;
            UserDefinedDataOrigin = userdefineddataorigin;
            DurationType = durationtype;
            ScheduleDuration = scheduleduration;
            ScheduleStart = schedulestart;
            ScheduleFinish = schedulefinish;
            EarlyStart = earlystart;
            EarlyFinish = earlyfinish;
            LateStart = latestart;
            LateFinish = latefinish;
            FreeFloat = freefloat;
            TotalFloat = totalfloat;
            IsCritical = iscritical;
            StatusTime = statustime;
            ActualDuration = actualduration;
            ActualStart = actualstart;
            ActualFinish = actualfinish;
            RemainingTime = remainingtime;
            Completion = completion;
        }
    }
}
