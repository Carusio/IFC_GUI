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
        // PnYnMnDTnHnMnS, where nY represents the number of years, nM the number of months, nD the number of days, 
        // 'T' is the date/time separator, nH the number of hours, nM the number of minutes and nS the number of seconds
        public string ScheduleDuration { get; set; }
        // the next attributes should be in Format YYYY-MM-DDThh:mm:ss where "YYYY" represent the year, "MM" the month and "DD" the day, preceded by an optional leading "-" sign to indicate a negative year number
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
            // this forces the Comboboxes in the view to show the empty default view
            DataOrigin = -1;
            DurationType = -1;
        }

        public TaskTimeModel(string name, string dataorigin, string userdefineddataorigin, string durationtype, string scheduleduration, string schedulestart, string schedulefinish, string earlystart, string earlyfinish, string latestart, string latefinish,
            string freefloat, string totalfloat, string iscritical, string statustime, string actualduration, string actualstart, string actualfinish, string remainingtime, string completion)
        {
            Name = name;
            switch (dataorigin)
            {
                case "MEASURED":
                    DataOrigin = 0;
                    break;
                case "PREDICTED":
                    DataOrigin = 1;
                    break;
                case "SIMULATED":
                    DataOrigin = 2;
                    break;
                case "USERDEFINED":
                    DataOrigin = 3;
                    break;
                case "NOTEDEFINED":
                    DataOrigin = 4;
                    break;
                default:
                    DataOrigin = -1;
                    break;
            }

            UserDefinedDataOrigin = userdefineddataorigin;
            switch (durationtype)
            {
                case "ELAPSEDTIME":
                    DurationType = 0;
                    break;
                case "WORKTIME":
                    DurationType = 1;
                    break;
                case "NOTDEFINED":
                    DurationType = 2;
                    break;
                default:
                    DurationType = -1;
                    break;
            }
            ScheduleDuration = scheduleduration;
            ScheduleStart = schedulestart;
            ScheduleFinish = schedulefinish;
            EarlyStart = earlystart;
            EarlyFinish = earlyfinish;
            LateStart = latestart;
            LateFinish = latefinish;
            FreeFloat = freefloat;
            TotalFloat = totalfloat;

            switch (iscritical)
            {
                case "TRUE":
                    IsCritical = true;
                    break;
                case "FALSE":
                    IsCritical = false;
                    break;
                default:
                    IsCritical = false;
                    break;
            }
            StatusTime = statustime;
            ActualDuration = actualduration;
            ActualStart = actualstart;
            ActualFinish = actualfinish;
            RemainingTime = remainingtime;
            Completion = completion;
        }
    } 
}
