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
        public List<string> Nests { get; set; } = new List<string>();       // list of tasks(guids) that nest this task. It determines that this object definition is a part within an ordered whole/part decomposition relationship. 
                                                                            // An object occurrence or type can only be part of a single decomposition
        public List<string> IsNestedBy { get; set; } = new List<string>();  // list of tasks(guids) that are nested by this task. It determines that this object definition is the whole within an ordered whole/part decomposition relationship.
                                                                            // An object or object type can be nested by several other objects 

        public ObjectModel()
        {
            GlobalId = IfcDataHandling.GenerateNewIfcGUID();
        }

        public ObjectModel(string globalid, string name, string description, string objecttype, List<string> isnestedby, List<string> nests)
        {
            GlobalId = globalid;
            Name = name;
            Description = description;
            ObjectType = objecttype;

            if (isnestedby == null)
            {
                IsNestedBy.Add("");
            }
            else
            {
                IsNestedBy = isnestedby;
            }
            if (nests == null)
            {
                Nests.Add("");
            }
            else
            {
                Nests = nests;
            }
        }
    }  
}
