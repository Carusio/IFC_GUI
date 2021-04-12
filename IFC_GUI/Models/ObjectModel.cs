using IFC_GUI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.Models
{
    // Object Model based on IfcObject class
    public class ObjectModel
    {
        //standard IfcObject attributes
        public string GlobalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ObjectType { get; set; }

        //Relationship attributes - Nestings
        public List<string> Nests { get; set; } = new List<string>();
        public List<string> IsNestedBy { get; set; } = new List<string>();

        public ObjectModel()
        {
            GlobalId = IfcDataHandling.generateNewIfcGUID();
        }

        public ObjectModel(string globalid, string name, string description, string objecttype, List<string> nests, List<string> isnestedby)
        {
            GlobalId = globalid;
            Name = name;
            Description = description;
            ObjectType = objecttype;

            if (nests == null)
            {
                Nests.Add("");
            }
            else
            {
                Nests = nests;
            }
            if (isnestedby == null)
            {
                IsNestedBy.Add("");
            }
            else
            {
                IsNestedBy = isnestedby;
            }
        }
    }  
}
