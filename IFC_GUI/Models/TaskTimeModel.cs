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
        public string DataOrigin { get; set; }
        public string UserDefinedDataOrigin { get; set; }

        // IfcTasktTime attributes
        public string DurationType { get; set; }
        public string ScheduleDuration { get; set; }
        public string ScheduleStart { get; set; }
        public string ScheduleFinish { get; set; }
        public string EarlyStart { get; set; }
        public string EarlyFinish { get; set; }
        public string LateStart { get; set; }
        public string LateFinish { get; set; }
        public string FreeFloat { get; set; }
        public string TotalFloat { get; set; }
        public bool IsCritical { get; set; }
        public string StatusTime { get; set; }
        public string ActualDuration { get; set; }
        public string ActualStart { get; set; }
        public string ActualFinish { get; set; }
        public string RemainingTime { get; set; }
        public string Completion { get; set; }
        public TaskTimeModel()
        {
            
        }

        public TaskTimeModel(string name, string dataorigin, string userdefineddataorigin, string durationtype, string scheduleduration, string schedulestart, string schedulefinish, string earlystart, string earlyfinish, string latestart, string latefinish,
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
