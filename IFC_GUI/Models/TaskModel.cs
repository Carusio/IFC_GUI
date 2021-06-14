using System.Collections.Generic;

namespace IFC_GUI.Models
{
    // Task Model based on IfcTask class from the IFC-Standard
    public class TaskModel : ObjectModel
    {
        //IfcTaskModel attributes
        public string Identification { get; set; }
        public string LongDescription { get; set; }
        public string Status { get; set; }
        public string WorkMethod { get; set; }
        public bool IsMilestone { get; set; }
        public string Priority { get; set; }
        public TaskTimeModel TaskTime { get; set; } = new TaskTimeModel();
        public int PredefinedType { get; set; }

        //Relationship attributes - sequential
        public List<string> IsPredecessorTo { get; set; } = new List<string>();
        public List<string> IsSuccessorFrom { get; set; } = new List<string>();

        public TaskModel() : base ()
        {
            // necessary so the combobox shows the default value
            PredefinedType = -1;
        }

        public TaskModel(string globalid, string name, string description, string objecttype, string identification, string longdescription, string status,
            string workmethod, bool ismilestone, string priority, TaskTimeModel tasktime, string predefinedtype, List<string> isnestedby, List<string> nests, List<string> ispredecessorto, List<string> issuccessorfrom) :
            base(globalid, name, description, objecttype, isnestedby, nests)
        {
            Identification = identification;
            LongDescription = longdescription;
            Status = status;
            WorkMethod = workmethod;
            IsMilestone = ismilestone;
            Priority = priority;
            TaskTime = tasktime;

            switch (predefinedtype)
            {
                case "ATTENDANCE":
                    PredefinedType = 0;
                    break;
                case "CONSTRUCTION":
                    PredefinedType = 1;
                    break;
                case "DEMOLITION":
                    PredefinedType = 2;
                    break;
                case "DISMANTLE":
                    PredefinedType = 3;
                    break;
                case "DISPOSAL":
                    PredefinedType = 4;
                    break;
                case "INSTALLATION":
                    PredefinedType = 5;
                    break;
                case "LOGISTIC":
                    PredefinedType = 6;
                    break;
                case "MAINTENANCE":
                    PredefinedType = 7;
                    break;
                case "MOVE":
                    PredefinedType = 8;
                    break;
                case "OPERATION":
                    PredefinedType = 9;
                    break;
                case "REMOVAL":
                    PredefinedType = 10;
                    break;
                case "RENOVATION":
                    PredefinedType = 11;
                    break;
                case "USERDEFINED":
                    PredefinedType = 12;
                    break;
                case "NOTDEFINED":
                    PredefinedType = 13;
                    break;
                default:
                    PredefinedType = -1;
                    break;
            }

            if (ispredecessorto == null)
            {
                IsPredecessorTo.Add("");
            }
            else
            {
                IsPredecessorTo = ispredecessorto;
            }
            if (issuccessorfrom == null)
            {
                IsSuccessorFrom.Add("");
            }
            else
            {
                IsSuccessorFrom = issuccessorfrom;
            }
        }
    }
}
