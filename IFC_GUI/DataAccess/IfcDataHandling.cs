using DynamicData;
using IFC_GUI.Models;
using IFC_GUI.ViewModels;
using IFC_GUI.ViewModels.NodeViewModels;
using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc4.DateTimeResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.ProcessExtension;
using Xbim.IO;

namespace IFC_GUI.DataAccess
{
    class IfcDataHandling
    {
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
                    // --------------set the isNestedBy attribute of the currentTask
                    List<string> nestedtaskguids = new List<string>();

                    // get all IIfcRelNests objects, where the relating object is the currentTask
                    var allRelObjectsNestedByTask = model.Instances.Where<IIfcRelNests>(rel => rel.RelatingObject == currentTask);

                    // iterate through every IIfcRelNests objects, which are nestes by the currentTask
                    foreach (var relobject in allRelObjectsNestedByTask)
                    {
                        // get all relatedObjects of type IIfcTask from the IIfcRelNests object
                        var nestedtasks = relobject.RelatedObjects.OfType<IIfcTask>();

                        // iterate through every nestedtask in nestedtasks
                        foreach (var nestedtask in nestedtasks)
                        {
                            // add the GUID of the nestedtask to the nestedtaskguids list
                            nestedtaskguids.Add(nestedtask.GlobalId);
                        }
                    }

                    // --------------set the nests attribute of the currentTask
                    List<string> isnestedbytaskguids = new List<string>();

                    // get all IIfcRelNests objects, where the relatedobjects list contains the currentTask
                    var allRelObjectsNestATask = model.Instances.Where<IIfcRelNests>(rel => rel.RelatedObjects.Contains(currentTask));

                    // get and add all GUIDs of the tasks, where currentTask is nestedby
                    foreach (var relobject in allRelObjectsNestATask)
                    {
                        isnestedbytaskguids.Add(relobject.RelatingObject.GlobalId);
                    }

                    // --------------set the ispredesseorto attribute of the currentTask
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

                    // --------------same for the isSuccessorFrom attribute
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

                    TaskTimeModel taskTimeModelOfCurrentTask;
                    // TODO: get Tasktime right
                    if (currentTask.TaskTime != null)
                    {
                        var ifcTaskTimeOfCurrentTask = currentTask.TaskTime;
                        taskTimeModelOfCurrentTask = new TaskTimeModel(ifcTaskTimeOfCurrentTask.Name, (int)ifcTaskTimeOfCurrentTask.DataOrigin, ifcTaskTimeOfCurrentTask.UserDefinedDataOrigin, (int)ifcTaskTimeOfCurrentTask.DurationType, 
                            ifcTaskTimeOfCurrentTask.ScheduleDuration, ifcTaskTimeOfCurrentTask.ScheduleStart, ifcTaskTimeOfCurrentTask.ScheduleFinish, ifcTaskTimeOfCurrentTask.EarlyStart, ifcTaskTimeOfCurrentTask.EarlyFinish, 
                            ifcTaskTimeOfCurrentTask.LateStart, ifcTaskTimeOfCurrentTask.LateFinish, ifcTaskTimeOfCurrentTask.FreeFloat, ifcTaskTimeOfCurrentTask.TotalFloat, (bool)ifcTaskTimeOfCurrentTask.IsCritical, 
                            ifcTaskTimeOfCurrentTask.StatusTime, ifcTaskTimeOfCurrentTask.ActualDuration, ifcTaskTimeOfCurrentTask.ActualStart, ifcTaskTimeOfCurrentTask.ActualFinish, ifcTaskTimeOfCurrentTask.RemainingTime,
                            ifcTaskTimeOfCurrentTask.Completion.ToString());
                    }
                    else
                    {
                        taskTimeModelOfCurrentTask = new TaskTimeModel();
                    }

                    // --------------create a TaskModel object for the currentTask
                    /*TaskModel currentTaskModel = new TaskModel(currentTask.GlobalId, currentTask.Name, currentTask.Description, currentTask.ObjectType, currentTask.Identification, currentTask.LongDescription,
                        currentTask.Status, currentTask.WorkMethod, currentTask.IsMilestone, currentTask.Priority.ToString(), null, currentTask.PredefinedType.ToString(),
                        nestedtaskguids, isnestedbytaskguids, ispredecessortoobjectsguids, issuccessorfromobjectsguids);*/
                    TaskModel currentTaskModel = new TaskModel(currentTask.GlobalId, currentTask.Name, currentTask.Description, currentTask.ObjectType, currentTask.Identification, currentTask.LongDescription,
                            currentTask.Status, currentTask.WorkMethod, currentTask.IsMilestone, currentTask.Priority.ToString(), taskTimeModelOfCurrentTask, currentTask.PredefinedType.ToString(),
                            nestedtaskguids, isnestedbytaskguids, ispredecessortoobjectsguids, issuccessorfromobjectsguids);

                    // add the currentTaskModel to the allTaskModelsList
                    allTaskModelsList.Add(currentTaskModel);
                }
            }
            return allTaskModelsList;
        }

        // update an existing ifcproject and the included ifctasks and create the new IfcTasks
        public static void UpdateIfcData(string fileName, List<TaskModel>allTaskModels, string wantedfileExtension)
        {
            using (var model = IfcStore.Open(fileName))
            {
                using (var txn = model.BeginTransaction("Task modification"))
                {
                    // Remove all taskmodels from the IFC File which are not in @allTaskModels (i.e., they did exist before but were manually removed using the node editor)
                    // --- BEGIN
                    /* 
                     * 1.) Get all IFC Tasks in the IFC File
                     * 2.) Delete all IFC Tasks from the IFC File which are not in @allTaskModels
                    */
                    // --- END
                    var allIfcTasks = model.Instances.OfType<IfcTask>();

                    List<IfcTask> ifcsTasksToRemove = new List<IfcTask>();

                    foreach (var ifcTask in allIfcTasks)
                    {
                        bool contains = false;
                        foreach (var taskModel in allTaskModels)
                        {
                            if (taskModel.GlobalId == ifcTask.GlobalId.ToString())
                            {
                                contains = true;
                                break;
                            }
                        }
                        if (!contains)
                        {
                            ifcsTasksToRemove.Add(ifcTask);
                        }
                    }

                    foreach (var ifcTask in ifcsTasksToRemove)
                    {
                        model.Delete(ifcTask);
                    }

                    // Add new Taskmodels
                    // --- BEGIN
                    /*
                     * 1.) For every TaskModel in @allTaskModel
                     *     a) check if IFC Tasks exist: True: update simple attribute which do not relate to other TaskModels; False: create new IFC Task and set simple attributes
                     * 2.) For every TaskModel in @allTaskModel
                     *     a) Set attributes which relate to other TaskModels, i.e., Nests, IsNestedBy, IsSuccessorFrom, IsPredecessorTo.
                     */
                    // --- END
                    foreach (var taskmodel in allTaskModels)
                    {
                        var theIfcTask = model.Instances.FirstOrDefault<IIfcTask>(t => t.GlobalId == taskmodel.GlobalId);

                        if (theIfcTask != null)
                        {
                            // TODO: schlüssel gibt es schon, bei ifcxml format
                            /*if(theIfcTask.Name.Value != taskmodel.Name)
                            {
                                theIfcTask.Name = taskmodel.Name;
                            }*/
                            theIfcTask.Name = taskmodel.Name;
                            theIfcTask.Description = taskmodel.Description;
                            theIfcTask.ObjectType = taskmodel.ObjectType;

                            theIfcTask.Identification = taskmodel.Identification;
                            theIfcTask.LongDescription = taskmodel.LongDescription;
                            theIfcTask.Status = taskmodel.Status;
                            theIfcTask.WorkMethod = taskmodel.WorkMethod;
                            theIfcTask.IsMilestone = taskmodel.IsMilestone;
                            theIfcTask.Priority = Convert.ToInt32(taskmodel.Priority);

                            if (theIfcTask.TaskTime != null)
                            {
                                theIfcTask.TaskTime.Name = taskmodel.TaskTime.Name;
                                theIfcTask.TaskTime.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                theIfcTask.TaskTime.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                theIfcTask.TaskTime.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                theIfcTask.TaskTime.ScheduleDuration = taskmodel.TaskTime.ScheduleDuration;
                                theIfcTask.TaskTime.ScheduleStart = taskmodel.TaskTime.ScheduleStart;
                                theIfcTask.TaskTime.ScheduleFinish = taskmodel.TaskTime.ScheduleFinish;
                                theIfcTask.TaskTime.EarlyStart = taskmodel.TaskTime.EarlyStart;
                                theIfcTask.TaskTime.EarlyFinish = taskmodel.TaskTime.EarlyFinish;
                                theIfcTask.TaskTime.LateStart = taskmodel.TaskTime.LateStart;
                                theIfcTask.TaskTime.LateFinish = taskmodel.TaskTime.LateFinish;
                                theIfcTask.TaskTime.FreeFloat = taskmodel.TaskTime.FreeFloat;
                                theIfcTask.TaskTime.TotalFloat = taskmodel.TaskTime.TotalFloat;
                                theIfcTask.TaskTime.IsCritical = taskmodel.TaskTime.IsCritical;
                                theIfcTask.TaskTime.StatusTime = taskmodel.TaskTime.StatusTime;
                                theIfcTask.TaskTime.ActualDuration = taskmodel.TaskTime.ActualDuration;
                                theIfcTask.TaskTime.ActualStart = taskmodel.TaskTime.ActualStart;
                                theIfcTask.TaskTime.ActualFinish = taskmodel.TaskTime.ActualStart;
                                theIfcTask.TaskTime.RemainingTime = taskmodel.TaskTime.RemainingTime;
                                //theIfcTask.TaskTime.Completion = taskmodel.TaskTime.Completion;
                            }
                            else
                            {
                                var newIfcTaskTime = model.Instances.New<IfcTaskTime>(tt =>
                                {
                                    tt.Name = taskmodel.TaskTime.Name;
                                    tt.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                    tt.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                    tt.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                    tt.ScheduleDuration = taskmodel.TaskTime.ScheduleDuration;
                                    tt.ScheduleStart = taskmodel.TaskTime.ScheduleStart;
                                    tt.ScheduleFinish = taskmodel.TaskTime.ScheduleFinish;
                                    tt.EarlyStart = taskmodel.TaskTime.EarlyStart;
                                    tt.EarlyFinish = taskmodel.TaskTime.EarlyFinish;
                                    tt.LateStart = taskmodel.TaskTime.LateStart;
                                    tt.LateFinish = taskmodel.TaskTime.LateFinish;
                                    tt.FreeFloat = taskmodel.TaskTime.FreeFloat;
                                    tt.TotalFloat = taskmodel.TaskTime.TotalFloat;
                                    tt.IsCritical = taskmodel.TaskTime.IsCritical;
                                    tt.StatusTime = taskmodel.TaskTime.StatusTime;
                                    tt.ActualDuration = taskmodel.TaskTime.ActualDuration;
                                    tt.ActualStart = taskmodel.TaskTime.ActualStart;
                                    tt.ActualFinish = taskmodel.TaskTime.ActualStart;
                                    tt.RemainingTime = taskmodel.TaskTime.RemainingTime;
                                    //tt.Completion = taskmodel.TaskTime.Completion;
                                });
                            }

                            // TODO: add TaskTime
                            // theTask.TaskTime = null;
                            theIfcTask.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;                          
                        }
                        else
                        {
                            var theIfcnewTask = model.Instances.New<IfcTask>(t =>
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
                                t.Priority = Convert.ToInt32(taskmodel.Priority);

                                var newIfcTaskTime = model.Instances.New<IfcTaskTime>(tt =>
                                {
                                    tt.Name = taskmodel.TaskTime.Name;
                                    tt.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                    tt.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                    tt.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                    tt.ScheduleDuration = taskmodel.TaskTime.ScheduleDuration;
                                    tt.ScheduleStart = taskmodel.TaskTime.ScheduleStart;
                                    tt.ScheduleFinish = taskmodel.TaskTime.ScheduleFinish;
                                    tt.EarlyStart = taskmodel.TaskTime.EarlyStart;
                                    tt.EarlyFinish = taskmodel.TaskTime.EarlyFinish;
                                    tt.LateStart = taskmodel.TaskTime.LateStart;
                                    tt.LateFinish = taskmodel.TaskTime.LateFinish;
                                    tt.FreeFloat = taskmodel.TaskTime.FreeFloat;
                                    tt.TotalFloat = taskmodel.TaskTime.TotalFloat;
                                    tt.IsCritical = taskmodel.TaskTime.IsCritical;
                                    tt.StatusTime = taskmodel.TaskTime.StatusTime;
                                    tt.ActualDuration = taskmodel.TaskTime.ActualDuration;
                                    tt.ActualStart = taskmodel.TaskTime.ActualStart;
                                    tt.ActualFinish = taskmodel.TaskTime.ActualStart;
                                    tt.RemainingTime = taskmodel.TaskTime.RemainingTime;
                                    //tt.Completion = taskmodel.TaskTime.Completion;
                                });
                                //TODO: add TaskTime
                                //t.TaskTime = null;
                                t.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;
                            });
                        }
                    }
                    // TODO: check if this can be done before commit or if we need two commits

                    foreach (var taskmodel in allTaskModels)
                    {
                        var theIfcTask = model.Instances.FirstOrDefault<IfcTask>(t => t.GlobalId == taskmodel.GlobalId);

                        // ifcTask already exists
                        if (theIfcTask != null)
                        {
                            // update NestedBy
                            if (!theIfcTask.IsNestedBy.Any())
                            {
                                theIfcTask.IsNestedBy.Append(model.Instances.New<IfcRelNests>(rel => {
                                    rel.RelatingObject = theIfcTask;
                                }));
                            }
                            
                            var theIfcTaskFirstRelNest = theIfcTask.IsNestedBy.FirstOrDefault();

                            if (theIfcTaskFirstRelNest == null)
                            {
                                // This should not happen!
                                throw new Exception();
                            }

                            foreach (var nestedguid in taskmodel.IsNestedBy)
                            {
                                var nestedIfcTask = model.Instances.FirstOrDefault<IfcTask>(task => task.GlobalId.ToString() == nestedguid);
                                if (!theIfcTaskFirstRelNest.RelatedObjects.Contains(nestedIfcTask))
                                {
                                    theIfcTaskFirstRelNest.RelatedObjects.Add(nestedIfcTask);
                                }                                
                            }

                            // update IsPredeccessorTo
                            var allifcRelSequenceObjects = model.Instances.Where<IfcRelSequence>(seq => seq.RelatingProcess == theIfcTask);

                            List<IfcTask> allSuccessorsOfCurrentTask = new List<IfcTask>();

                            foreach (var relSequenceObject in allifcRelSequenceObjects)
                            {
                                allSuccessorsOfCurrentTask.Add((IfcTask)relSequenceObject.RelatedProcess);
                            }

                            foreach (var successorguid in taskmodel.IsPredecessorTo)
                            {
                                var successorOfCurrentTask = model.Instances.FirstOrDefault<IfcTask>(Task => Task.GlobalId.ToString() == successorguid);
                                
                                if (!allSuccessorsOfCurrentTask.Contains(successorOfCurrentTask))
                                {
                                    model.Instances.New<IfcRelSequence>(rel => 
                                    {
                                        rel.RelatingProcess = theIfcTask;
                                        rel.RelatedProcess = successorOfCurrentTask;
                                    });
                                }
                            }
                        }
                        // ifcTask does not exist
                        else
                        {
                            // This should not happen because we just did create all TaskModels in @allTaskModels!
                            throw new Exception();
                        }
                    }
                    // commit changes
                    txn.Commit();
                }
                
                // change the fileextension to the wanted fileextension
                string filePathWithoutExtension = System.IO.Path.ChangeExtension(fileName, null);
                fileName = filePathWithoutExtension + wantedfileExtension;
                model.SaveAs(fileName);
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

                    // Add new Taskmodels
                    // --- BEGIN
                    /*
                     * 1.) For every TaskModel in @allTaskModel
                     *     a) check if IFC Tasks exist: True: update simple attribute which do not relate to other TaskModels; False: create new IFC Task and set simple attributes
                     * 2.) For every TaskModel in @allTaskModel
                     *     a) Set attributes which relate to other TaskModels, i.e., Nests, IsNestedBy, IsSuccessorFrom, IsPredecessorTo.
                     */
                    // --- END
                    foreach (var taskmodel in allTaskModels)
                    {
                        var theIfcTask = model.Instances.FirstOrDefault<IIfcTask>(t => t.GlobalId == taskmodel.GlobalId);

                        if (theIfcTask != null)
                        {
                            theIfcTask.Name = taskmodel.Name;
                            theIfcTask.Description = taskmodel.Description;
                            theIfcTask.ObjectType = taskmodel.ObjectType;

                            theIfcTask.Identification = taskmodel.Identification;
                            theIfcTask.LongDescription = taskmodel.LongDescription;
                            theIfcTask.Status = taskmodel.Status;
                            theIfcTask.WorkMethod = taskmodel.WorkMethod;
                            theIfcTask.IsMilestone = taskmodel.IsMilestone;
                            theIfcTask.Priority = Convert.ToInt32(taskmodel.Priority);

                            // TODO: add TaskTime
                            // theTask.TaskTime = null;
                            theIfcTask.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;
                        }
                        else
                        {
                            var theIfcnewTask = model.Instances.New<IfcTask>(t =>
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
                                if (!(taskmodel.Priority == null || taskmodel.Priority == ""))
                                {
                                    t.Priority = Convert.ToInt32(taskmodel.Priority);
                                }

                                var newIfcTaskTime = model.Instances.New<IfcTaskTime>(tt =>
                                {
                                    tt.Name = taskmodel.TaskTime.Name;
                                    tt.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                    tt.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                    tt.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                    tt.ScheduleDuration = taskmodel.TaskTime.ScheduleDuration;
                                    tt.ScheduleStart = taskmodel.TaskTime.ScheduleStart;
                                    tt.ScheduleFinish = taskmodel.TaskTime.ScheduleFinish;
                                    tt.EarlyStart = taskmodel.TaskTime.EarlyStart;
                                    tt.EarlyFinish = taskmodel.TaskTime.EarlyFinish;
                                    tt.LateStart = taskmodel.TaskTime.LateStart;
                                    tt.LateFinish = taskmodel.TaskTime.LateFinish;
                                    tt.FreeFloat = taskmodel.TaskTime.FreeFloat;
                                    tt.TotalFloat = taskmodel.TaskTime.TotalFloat;
                                    tt.IsCritical = taskmodel.TaskTime.IsCritical;
                                    tt.StatusTime = taskmodel.TaskTime.StatusTime;
                                    tt.ActualDuration = taskmodel.TaskTime.ActualDuration;
                                    tt.ActualStart = taskmodel.TaskTime.ActualStart;
                                    tt.ActualFinish = taskmodel.TaskTime.ActualStart;
                                    tt.RemainingTime = taskmodel.TaskTime.RemainingTime;
                                    //tt.Completion = taskmodel.TaskTime.Completion;
                                });
                                //TODO: add TaskTime
                                //t.TaskTime = null;
                                t.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;

                            });
                        }
                    }

                    // TODO: check if this can be done before commit or if we need two commits

                    foreach (var taskmodel in allTaskModels)
                    {
                        var theIfcTask = model.Instances.FirstOrDefault<IfcTask>(t => t.GlobalId == taskmodel.GlobalId);

                        // ifcTask already exists
                        if (theIfcTask != null)
                        {
                            // update NestedBy
                            if (!theIfcTask.IsNestedBy.Any())
                            {
                                theIfcTask.IsNestedBy.Append(model.Instances.New<IfcRelNests>(rel => {
                                    rel.RelatingObject = theIfcTask;
                                }));
                            }

                            var theIfcTaskFirstRelNest = theIfcTask.IsNestedBy.FirstOrDefault();

                            if (theIfcTaskFirstRelNest == null)
                            {
                                // This should not happen!
                                throw new Exception();
                            }

                            foreach (var nestedguid in taskmodel.IsNestedBy)
                            {
                                var nestedIfcTask = model.Instances.FirstOrDefault<IfcTask>(task => task.GlobalId.ToString() == nestedguid);
                                if (!theIfcTaskFirstRelNest.RelatedObjects.Contains(nestedIfcTask))
                                {
                                    theIfcTaskFirstRelNest.RelatedObjects.Add(nestedIfcTask);
                                }
                            }

                            // update IsPredeccessorTo
                            var allifcRelSequenceObjects = model.Instances.Where<IfcRelSequence>(seq => seq.RelatingProcess == theIfcTask);

                            List<IfcTask> allSuccessorsOfCurrentTask = new List<IfcTask>();

                            foreach (var relSequenceObject in allifcRelSequenceObjects)
                            {
                                allSuccessorsOfCurrentTask.Add((IfcTask)relSequenceObject.RelatedProcess);
                            }

                            foreach (var successorguid in taskmodel.IsPredecessorTo)
                            {
                                var successorOfCurrentTask = model.Instances.FirstOrDefault<IfcTask>(Task => Task.GlobalId.ToString() == successorguid);

                                if (!allSuccessorsOfCurrentTask.Contains(successorOfCurrentTask))
                                {
                                    model.Instances.New<IfcRelSequence>(rel =>
                                    {
                                        rel.RelatingProcess = theIfcTask;
                                        rel.RelatedProcess = successorOfCurrentTask;
                                    });
                                }
                            }
                        }
                        else
                        {
                            // This should not happen because we just did create all TaskModels in @allTaskModels!
                            // Maybe need additional commit if this is thrown.
                            throw new Exception();
                        }
                    }

                    //commit changes
                    txn.Commit();
                }
                model.SaveAs(fileName);
            }
        }

        public static void RecursiveNestingTaskModelToTaskNode(List<TaskModel> allTaskModels, BreadcrumbBarViewModel crumbBar, NetworkBreadcrumb crumbNetwork, string parentTaskModelGuid, bool recursive)
        {
            List<TaskModel> allTaskModelsOnCurrentLevel = new List<TaskModel>();
            List<TaskModel> allTaskModelsOnLowerLevelThanCurrent = new List<TaskModel>();

            // create taskNode foreach TaskModel
            foreach (TaskModel tm in allTaskModels)
            {
                // check on which level the task is and add the task to a list
                if ((!tm.Nests.Any() && parentTaskModelGuid == "") || (tm.Nests.Any() && tm.Nests.First() == parentTaskModelGuid)) // can only be nested by at most one element
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
            /*foreach (TaskModel tm in allTaskModelsOnCurrentLevel)
            {
                if (tm.IsNestedBy.Any() && recursive)
                {
                    var subnetwork = new NetworkBreadcrumb
                    {
                        Name = tm.Name,
                        Network = new NetworkViewModel()
                    };
                    crumbBar.ActivePath.Add(subnetwork);
                    RecursiveNestingTaskModelToTaskNode(allTaskModelsOnLowerLevelThanCurrent, crumbBar, subnetwork, tm.GlobalId, true);
                }
            }*/
        }

        public static bool CheckFileExtension(string filename)
        {
            bool fileExtensionBool;
            // check if file type is allowed
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
                default:
                    fileExtensionBool = true;
                    break;
            }
            return !fileExtensionBool;
        }
        public static string generateNewIfcGUID()
        {
            Xbim.Ifc4.UtilityResource.IfcGloballyUniqueId b = Guid.NewGuid();
            //return Xbim.Ifc4.UtilityResource.IfcGloballyUniqueId.AsPart21(Guid.NewGuid()).ToString().Trim('\'');
            return b.ToString().Trim('\'');
        }
    }
}
