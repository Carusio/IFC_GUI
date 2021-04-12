using DynamicData;
using IFC_GUI.Models;
using IFC_GUI.ViewModels;
using IFC_GUI.ViewModels.NodeViewModels;
using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.ProcessExtension;
using Xbim.IO;

namespace IFC_GUI.DataAccess
{
    class IfcDataHandling
    {
        //TODO: delete all Console Writeline
        public static List<TaskModel> OpenIfcData(string fileName)
        {
            List<TaskModel> allTaskModelsList = new List<TaskModel>();

            using (var model = IfcStore.Open(fileName))
            {
                // get all tasks in the model (using IFC4 interface of IfcTask this will work both for IFC2x3 and IFC4)
                var allTasks = model.Instances.OfType<IIfcTask>();

                // iterate through every existing task in allTasks
                foreach (var currentTask in allTasks)
                {
                    // set the nests attribute of the currentTask
                    List<string> nestedtaskguids = new List<string>();

                    // get all IIfcRelNests objects, where the relating object is the currentTask
                    var allRelObjectsNestedByTask = model.Instances.Where<IIfcRelNests>(rel => rel.RelatingObject == currentTask);

                    // iterate through every IIfcRelNests objects, which are nestes by the currentTask
                    //if (allRelObjectsNestedByTask != null && allRelObjectsNestedByTask.Any())
                    //{
                    foreach (var relobject in allRelObjectsNestedByTask)
                    {
                        // TODO: probably only IfcTask
                        // get all relatedObjects of type IIfcTask from the IIfcRelNests object
                        var nestedtasks = relobject.RelatedObjects.OfType<IIfcTask>();
                        //if (nestedtasks != null && nestedtasks.Any())
                        //{
                        // iterate through every nestedtask in nestedtasks
                        foreach (var nestedtask in nestedtasks)
                        {
                            // add the GUID of the nestedtask to the nestedtaskguids list
                            nestedtaskguids.Add(nestedtask.GlobalId);
                        }
                        //}
                        //else { Console.WriteLine("No Nested Tasks"); }
                    }
                    //}
                    //else { Console.WriteLine("No RelObjectsNestedByThisTask"); }

                    // set the isnestedby attribute of the currentTask
                    List<string> isnestedbytaskguids = new List<string>();

                    // get all IIfcRelNests objects, where the relatedobjects list contains the currentTask
                    var allRelObjectsNestATask = model.Instances.Where<IIfcRelNests>(rel => rel.RelatedObjects.Contains(currentTask));


                    //if (allRelObjectsNestATask != null && allRelObjectsNestATask.Any())
                    //{
                    // get and add all GUIDs of the tasks, where currentTask is nestedby
                    foreach (var relobject in allRelObjectsNestATask)
                    {
                        isnestedbytaskguids.Add(relobject.RelatingObject.GlobalId);
                    }
                    //}
                    //else { Console.WriteLine("No RelObjectsNestThisTask"); }

                    // set the ispredesseorto attribute of the currentTask
                    List<string> ispredecessortoobjectsguids = new List<string>();
                    // check if the currentTask is Predecessor to another task
                    if (currentTask.IsPredecessorTo != null && currentTask.IsPredecessorTo.Any())
                    {
                        // iterate through the list of IIfcRelSequence objects, where the currentTask is the relatingProcess (predecessor)
                        foreach (var ifcispredecessortoObject in currentTask.IsPredecessorTo)
                        {
                            // check if the IIfcRelSequence object has a RelatedProcess
                            if (ifcispredecessortoObject.RelatedProcess != null)
                            {
                                // add the GUID of the RelatedProcess (successor task) to the ispredessortoobjectsguids list
                                ispredecessortoobjectsguids.Add(ifcispredecessortoObject.RelatedProcess.GlobalId);
                            }
                        }
                    }
                    else { Console.WriteLine("No IsPredecessorTo Object"); }

                    // same for the isSuccessorFrom attribute
                    List<string> issuccessorfromobjectsguids = new List<string>();
                    // check if the currentTask is Successor from another task
                    if (currentTask.IsSuccessorFrom != null && currentTask.IsSuccessorFrom.Any())
                    {
                        // iterate through the list of IIfcRelSequence objects, where the currentTask is the relatedProcess (successor)
                        foreach (var ifcissuccessorfromObject in currentTask.IsSuccessorFrom)
                        {
                            // check if the IIfcRelSequence object has a RelatingProcess
                            if (ifcissuccessorfromObject.RelatingProcess != null)
                            {
                                // add the GUID of the RelatingProcess (predecessor task) to the issuccessorfromobjectsguids list
                                issuccessorfromobjectsguids.Add(ifcissuccessorfromObject.RelatingProcess.GlobalId);
                            }
                        }
                    }
                    else { Console.WriteLine("No IsSuccessorFrom Object"); }

                    // TODO: get Tasktime right
                    // create a TaskModel object for the currentTask
                    TaskModel currentTaskModel = new TaskModel(currentTask.GlobalId, currentTask.Name, currentTask.Description, currentTask.ObjectType, currentTask.Identification, currentTask.LongDescription,
                        currentTask.Status, currentTask.WorkMethod, currentTask.IsMilestone, currentTask.Priority.ToString(), null, currentTask.PredefinedType.ToString(),
                        nestedtaskguids, isnestedbytaskguids, ispredecessortoobjectsguids, issuccessorfromobjectsguids);

                    // add the currentTaskModel to the allTaskModelsList
                    allTaskModelsList.Add(currentTaskModel);
                }
            }
            return allTaskModelsList;
        }

        // update an existing ifcproject and the included ifctasks
        public static void UpdateIfcData(string fileName, List<TaskModel>allTaskModels)
        {            
            using (var model = IfcStore.Open(fileName))
            {
                using (var txn = model.BeginTransaction("Tasks modification"))
                {
                    foreach (var taskmodel in allTaskModels)
                    {
                        var theTask = model.Instances.FirstOrDefault<IIfcTask>(t => t.GlobalId == taskmodel.GlobalId);

                        if (theTask != null)
                        {
                            theTask.Name = taskmodel.Name;
                            theTask.Description = taskmodel.Description;
                            theTask.ObjectType = taskmodel.ObjectType;
                            theTask.Identification = taskmodel.Identification;
                            theTask.LongDescription = taskmodel.LongDescription;
                            theTask.Status = taskmodel.Status;
                            theTask.WorkMethod = taskmodel.WorkMethod;
                            theTask.IsMilestone = taskmodel.IsMilestone;

                            // TODO: transform integer to ifcinteger and add TaskTime
                            // theTask.Priority = taskmodel.Priority;
                            // theTask.TaskTime = null;
                            theTask.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;
                        }
                        else
                        {
                            model.Instances.New<IfcTask>(t =>
                            {
                                t.GlobalId = Guid.NewGuid();
                                t.Name = taskmodel.Name;
                                t.Description = taskmodel.Description;
                                t.ObjectType = taskmodel.ObjectType;
                                t.Identification = taskmodel.Identification;
                                t.LongDescription = taskmodel.LongDescription;
                                t.Status = taskmodel.Status;
                                t.WorkMethod = taskmodel.WorkMethod;
                                t.IsMilestone = taskmodel.IsMilestone;

                                //TODO: transform integer to ifcinteger and add TaskTime
                                //theTask.Priority = taskmodel.Priority;
                                //t.TaskTime = null;
                                t.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;
                            });
                        }
                    }

                    // commit changes
                    txn.Commit();
                }
            }
        }

        //create a new ifcproject and the ifctasks
        public static void NewIfcData(string fileName, List<TaskModel> allTaskModels)
        {
            using (var model = IfcStore.Create(XbimSchemaVersion.Ifc4, XbimStoreType.InMemoryModel))
            {
                using (var txn = model.BeginTransaction("New IfcFile"))
                {
                    //there should always be one project in the model
                    var project = model.Instances.New<IfcProject>(p => p.Name = "Basic Creation");
                    //our shortcut to define basic default units
                    project.Initialize(ProjectUnits.SIUnitsUK);

                    foreach (var taskmodel in allTaskModels)
                    {
                        model.Instances.New<IfcTask>(t =>
                        {
                            t.GlobalId = taskmodel.GlobalId;
                            t.Name = taskmodel.Name;
                            t.Description = taskmodel.Description;
                            t.ObjectType = taskmodel.ObjectType;
                            t.Identification = taskmodel.Identification;
                            t.LongDescription = taskmodel.LongDescription;
                            t.Status = taskmodel.Status;
                            t.WorkMethod = taskmodel.WorkMethod;
                            t.IsMilestone = taskmodel.IsMilestone;

                            //TODO: transform integer to ifcinteger and add TaskTime
                            //theTask.Priority = taskmodel.Priority;
                            //t.TaskTime = null;
                            t.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;
                        });
                    }

                    //commit changes
                    txn.Commit();
                }
                model.SaveAs(fileName);
            }
        }

        public static void RecursiveNestingTaskModelToTaskNode(List<TaskModel> allTaskModels, BreadcrumbBarViewModel crumbBar, NetworkBreadcrumb crumbNetwork, string parentTaskModel, bool recursive)
        {
            List<TaskModel> allTaskModelsOnCurrentLevel = new List<TaskModel>();
            List<TaskModel> allTaskModelsOnLowerLevelThanCurrent = new List<TaskModel>();

            // create taskNode foreach TaskModel
            foreach (TaskModel tm in allTaskModels)
            {
                // check on which level the task is and add the task to a list
                if ((!tm.IsNestedBy.Any() && parentTaskModel == "") || (tm.IsNestedBy.Any() && tm.IsNestedBy.First() == parentTaskModel)) // can only be nested by at most one element
                {
                    crumbNetwork.Network.Nodes.Add(new IfcTaskNodeViewModel(tm));
                    allTaskModelsOnCurrentLevel.Add(tm);
                }
                else
                {
                    allTaskModelsOnLowerLevelThanCurrent.Add(tm);
                }
            }

            // create Connections foreach taskNode
            foreach (var taskNode in crumbNetwork.Network.Nodes.Items.OfType<IfcTaskNodeViewModel>())
            {
                // iterate through the GUIDs of predecessors of the current taskNode 
                foreach (var guid in taskNode.TaskModel.IsSuccessorFrom)
                {
                    // add a connection to the Network if the GUID of the current predecessor matches the GUID of an existing node in this Network
                    foreach (var node in crumbNetwork.Network.Nodes.Items.OfType<IfcTaskNodeViewModel>())
                    {
                        if (node.TaskModel.GlobalId == guid)
                        {
                            var connection = new IfcConnectionViewModel(crumbNetwork.Network, taskNode.Input, node.Output);
                            crumbNetwork.Network.Connections.Add(connection);
                        }
                    }
                }
            }

            // create a subnetwork foreach task on the current Level, that nests other tasks
            foreach (TaskModel tm in allTaskModelsOnCurrentLevel)
            {
                if (tm.Nests.Any() && recursive)
                {
                    var subnetwork = new NetworkBreadcrumb
                    {
                        Name = tm.Name,
                        Network = new NetworkViewModel()
                    };
                    crumbBar.ActivePath.Add(subnetwork);
                    RecursiveNestingTaskModelToTaskNode(allTaskModelsOnLowerLevelThanCurrent, crumbBar, subnetwork, tm.GlobalId, true);
                }
            }
        }
        public static bool CheckFileExtension(string filename)
        {
            bool fileExtensionBool;
            //check if file type is allowed
            switch (System.IO.Path.GetExtension(filename))
            {
                case ".ifc":
                    fileExtensionBool = false;
                    break;
                case ".IFC":
                    fileExtensionBool = false;
                    break;
                case ".ifczip":
                    fileExtensionBool = false;
                    break;
                case ".ifcxml":
                    fileExtensionBool = false;
                    break;
                case ".xbim":
                    fileExtensionBool = false;
                    break;
                default:
                    fileExtensionBool = true;
                    break;
            }
            return !fileExtensionBool;
        }
        public static string generateNewIfcGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
