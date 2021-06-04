using DynamicData;
using IFC_GUI.Models;
using IFC_GUI.ViewModels;
using IFC_GUI.ViewModels.NodeViewModels;
using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc4.DateTimeResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.ProcessExtension;
using Xbim.IO;

namespace IFC_GUI.DataAccess
{
    public class IfcDataHandling
    {
        public static List<TaskModel> OpenIfcData(string fileName)
        {
            List<TaskModel> allTaskModelsList = new List<TaskModel>();
            try { 
            using (var model = IfcStore.Open(fileName))
            {
                // get all tasks in the model (using IFC4 interface of IfcTask this will work both for IFC2x3 and IFC4)
                var allTasks = model.Instances.OfType<IIfcTask>();

                // iterate through every existing task in allTasks
                foreach (var currentTask in allTasks)
                {
                    List<string> nestedtaskguids = ReadNestedTasks(model, currentTask);

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

                    // generate the TaskTimeModel for the TaskModel
                    TaskTimeModel taskTimeModelOfCurrentTask;
                    if (currentTask.TaskTime != null)
                    {
                        var ifcTaskTimeOfCurrentTask = currentTask.TaskTime;
                        taskTimeModelOfCurrentTask = new TaskTimeModel(ifcTaskTimeOfCurrentTask.Name, ifcTaskTimeOfCurrentTask.DataOrigin.ToString(), ifcTaskTimeOfCurrentTask.UserDefinedDataOrigin, ifcTaskTimeOfCurrentTask.DurationType.ToString(),
                            ifcTaskTimeOfCurrentTask.ScheduleDuration.ToString(), ifcTaskTimeOfCurrentTask.ScheduleStart.ToString(), ifcTaskTimeOfCurrentTask.ScheduleFinish.ToString(), ifcTaskTimeOfCurrentTask.EarlyStart.ToString(), ifcTaskTimeOfCurrentTask.EarlyFinish.ToString(),
                            ifcTaskTimeOfCurrentTask.LateStart.ToString(), ifcTaskTimeOfCurrentTask.LateFinish.ToString(), ifcTaskTimeOfCurrentTask.FreeFloat.ToString(), ifcTaskTimeOfCurrentTask.TotalFloat.ToString(), ifcTaskTimeOfCurrentTask.IsCritical.ToString(),
                            ifcTaskTimeOfCurrentTask.StatusTime.ToString(), ifcTaskTimeOfCurrentTask.ActualDuration.ToString(), ifcTaskTimeOfCurrentTask.ActualStart.ToString(), ifcTaskTimeOfCurrentTask.ActualFinish.ToString(), ifcTaskTimeOfCurrentTask.RemainingTime.ToString(),
                            ifcTaskTimeOfCurrentTask.Completion.ToString());
                    }
                    else
                    {
                        taskTimeModelOfCurrentTask = new TaskTimeModel();
                    }

                    // create a TaskModel object for the currentTask
                    TaskModel currentTaskModel = new TaskModel(currentTask.GlobalId, currentTask.Name, currentTask.Description, currentTask.ObjectType, currentTask.Identification, currentTask.LongDescription,
                            currentTask.Status, currentTask.WorkMethod, currentTask.IsMilestone, currentTask.Priority.ToString(), taskTimeModelOfCurrentTask, currentTask.PredefinedType.ToString(),
                            nestedtaskguids, isnestedbytaskguids, ispredecessortoobjectsguids, issuccessorfromobjectsguids);

                    // add the currentTaskModel to the allTaskModelsList
                    allTaskModelsList.Add(currentTaskModel);
                }
            }
                return allTaskModelsList;

            }
            catch (FileLoadException fle)
            {
                throw fle;
            }
        }

        private static List<string> ReadNestedTasks(IfcStore model, IIfcTask currentTask)
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
            return nestedtaskguids;
        }



        // update an existing ifcproject and the included ifctasks and create new IfcTasks, if the project does not contain them already
        public static void UpdateIfcData(string fileName, List<TaskModel>allTaskModels, string wantedfileExtension)
        {
            // --- BEGIN
            /*
             * Workaround is needed, because the .ifcxml format cannot be updated/saved if values changed
             * 
             * 1. open file and save file temporary file in .ifc format
             * 2. open temporary file, update values and save temporary file in .ifc format again
             * 3. open temporary file and save it in wanted file format
             * 4. delete temporary .ifc file
             * 
             */
            // --- END
            string filePathWithoutExtension = System.IO.Path.ChangeExtension(fileName, null);
            string filePathTemporary = filePathWithoutExtension + "_temp321654987.ifc";
            using (var model = IfcStore.Open(fileName))
            {
                model.SaveAs(filePathTemporary);
            }

            using (var model = IfcStore.Open(filePathTemporary))
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
                            theIfcTask.Name = taskmodel.Name;
                            theIfcTask.Description = taskmodel.Description;
                            theIfcTask.ObjectType = taskmodel.ObjectType;
                            theIfcTask.Identification = taskmodel.Identification;
                            theIfcTask.LongDescription = taskmodel.LongDescription;
                            theIfcTask.Status = taskmodel.Status;
                            theIfcTask.WorkMethod = taskmodel.WorkMethod;
                            theIfcTask.IsMilestone = taskmodel.IsMilestone;
                            if (int.TryParse(taskmodel.Priority, out int priorty))
                            {
                                theIfcTask.Priority = priorty;
                            }
                            else if (taskmodel.Priority == "")
                            {
                                theIfcTask.Priority = null;
                            }

                            if (theIfcTask.TaskTime != null)
                            {
                                theIfcTask.TaskTime.Name = taskmodel.TaskTime.Name;
                                if (taskmodel.TaskTime.DataOrigin != -1)
                                {
                                    theIfcTask.TaskTime.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                }
                                theIfcTask.TaskTime.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                if (taskmodel.TaskTime.DurationType != -1)
                                {
                                    theIfcTask.TaskTime.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                }
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
                                if (double.TryParse(taskmodel.TaskTime.Completion, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture , out double completion))
                                {
                                    theIfcTask.TaskTime.Completion = completion;
                                }
                                else if (taskmodel.TaskTime.Completion == "")
                                {
                                    theIfcTask.TaskTime.Completion = null;
                                }
                            }
                            else
                            {
                                IfcTaskTime newIfcTaskTime = ConvertTaskTimeModelToIfcTaskTime(model, taskmodel);
                                theIfcTask.TaskTime = newIfcTaskTime;
                            }

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
                                if (int.TryParse(taskmodel.Priority, out int priorty))
                                {
                                    t.Priority = priorty;
                                }
                                else if (taskmodel.Priority == "")
                                {
                                    t.Priority = null;
                                }
                                IfcTaskTime newIfcTaskTime = ConvertTaskTimeModelToIfcTaskTime(model, taskmodel);
                                t.TaskTime = newIfcTaskTime;
                                t.PredefinedType = (IfcTaskTypeEnum)taskmodel.PredefinedType;
                            });
                        }
                    }

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

                            List<IfcRelSequence> allIfcRelSequenceObjectsList = new List<IfcRelSequence>();
                            foreach(var element in allifcRelSequenceObjects)
                            {
                                allIfcRelSequenceObjectsList.Add(element);
                            }

                            List<IfcTask> allSuccessorsOfCurrentTask = new List<IfcTask>();

                            foreach (var relSequenceObject in allIfcRelSequenceObjectsList)
                            {
                                if(relSequenceObject.RelatedProcess != null)
                                {
                                    allSuccessorsOfCurrentTask.Add((IfcTask)relSequenceObject.RelatedProcess);
                                }
                                else
                                {
                                    model.Delete(relSequenceObject);
                                }                              
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
                            // delete redundant IfcRelSequence 
                            foreach (var successorIfc in allSuccessorsOfCurrentTask)
                            {
                                if (!taskmodel.IsPredecessorTo.Contains(successorIfc.GlobalId))
                                {
                                    var relSequences = model.Instances.Where<IfcRelSequence>(seq => seq.RelatingProcess == theIfcTask && seq.RelatedProcess == successorIfc);
                                    List<IfcRelSequence> relSeqToRemove = new List<IfcRelSequence>();
                                    foreach (var relSeq in relSequences)
                                    {
                                        relSeqToRemove.Add(relSeq);
                                    }
                                    foreach(var element in relSeqToRemove)
                                    {
                                        model.Delete(element);
                                    }
                                    
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
                model.SaveAs(filePathTemporary);
            }

            fileName = filePathWithoutExtension + wantedfileExtension;
            using (var model = IfcStore.Open(filePathTemporary))
            {
                model.SaveAs(fileName);
            }

            File.Delete(filePathTemporary);
        }

        private static IfcTaskTime ConvertTaskTimeModelToIfcTaskTime(IfcStore model, TaskModel taskmodel)
        {
            return model.Instances.New<IfcTaskTime>(tt =>
            {
                tt.Name = taskmodel.TaskTime.Name;
                {
                    tt.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                }
                tt.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                {
                    tt.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                }
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
                if (double.TryParse(taskmodel.TaskTime.Completion, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double completion))
                {
                    tt.Completion = completion;
                }
                else if (taskmodel.TaskTime.Completion == "")
                {
                    tt.Completion = null;
                }
            });
        }

        //create a new ifcproject and the ifctasks
        public static void NewIfcData(string fileName, List<TaskModel> allTaskModels)
        {
            string filePathWithoutExtension = System.IO.Path.ChangeExtension(fileName, null);
            string filePathTemporary = filePathWithoutExtension + "_temp321654987.ifc";
            using (var model = IfcStore.Create(XbimSchemaVersion.Ifc4, XbimStoreType.InMemoryModel))
            {
                model.SaveAs(filePathTemporary);
            }

            using (var model = IfcStore.Open(filePathTemporary))
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
                            if (int.TryParse(taskmodel.Priority, out int priorty))
                            {
                                theIfcTask.Priority = priorty;
                            }
                            else if (taskmodel.Priority == "")
                            {
                                theIfcTask.Priority = null;
                            }

                            if (theIfcTask.TaskTime != null)
                            {
                                theIfcTask.TaskTime.Name = taskmodel.TaskTime.Name;
                                if (taskmodel.TaskTime.DataOrigin != -1)
                                {
                                    theIfcTask.TaskTime.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                }
                                theIfcTask.TaskTime.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                if (taskmodel.TaskTime.DurationType != -1)
                                {
                                    theIfcTask.TaskTime.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                }
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
                                if (double.TryParse(taskmodel.TaskTime.Completion, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double completion))
                                {
                                    theIfcTask.TaskTime.Completion = completion;
                                }
                                else if (taskmodel.TaskTime.Completion == "")
                                {
                                    theIfcTask.TaskTime.Completion = null;
                                }
                            }
                            else
                            {
                                var newIfcTaskTime = model.Instances.New<IfcTaskTime>(tt =>
                                {
                                    tt.Name = taskmodel.TaskTime.Name;
                                    if (taskmodel.TaskTime.DataOrigin != -1)
                                    {
                                        tt.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                    }
                                    tt.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                    if (taskmodel.TaskTime.DurationType != -1)
                                    {
                                        tt.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                    }
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
                                    if (double.TryParse(taskmodel.TaskTime.Completion, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double completion))
                                    {
                                        tt.Completion = completion;
                                    }
                                    else if (taskmodel.TaskTime.Completion == "")
                                    {
                                        tt.Completion = null;
                                    }
                                });
                                theIfcTask.TaskTime = newIfcTaskTime;
                            }
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
                                if (int.TryParse(taskmodel.Priority, out int priorty))
                                {
                                    t.Priority = priorty;
                                }
                                else if (taskmodel.Priority == "")
                                {
                                    t.Priority = null;
                                }

                                var newIfcTaskTime = model.Instances.New<IfcTaskTime>(tt =>
                                {
                                    tt.Name = taskmodel.TaskTime.Name;
                                    {
                                        tt.DataOrigin = (IfcDataOriginEnum?)taskmodel.TaskTime.DataOrigin;
                                    }
                                    tt.UserDefinedDataOrigin = taskmodel.TaskTime.UserDefinedDataOrigin;
                                    if (taskmodel.TaskTime.DurationType != -1)
                                    {
                                        tt.DurationType = (IfcTaskDurationEnum?)taskmodel.TaskTime.DurationType;
                                    }
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
                                    if (double.TryParse(taskmodel.TaskTime.Completion, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double completion))
                                    {
                                        tt.Completion = completion;
                                    }
                                    else if (taskmodel.TaskTime.Completion == "")
                                    {
                                        tt.Completion = null;
                                    }
                                });
                               
                                t.TaskTime = newIfcTaskTime;
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
                model.SaveAs(filePathTemporary);
            }
            
            using (var model = IfcStore.Open(filePathTemporary))
            {
                model.SaveAs(fileName);
            }

            File.Delete(filePathTemporary);
        }

        

        // check if file type is allowed
        public static bool CheckFileExtension(string filename)
        {
            bool fileExtensionBool;
            switch (Path.GetExtension(filename))
            {
                case ".ifc":
                    fileExtensionBool = true;
                    break;
                case ".IFC":
                    fileExtensionBool = true;
                    break;
                case ".ifczip":
                    fileExtensionBool = true;
                    break;
                case ".ifcxml":
                    fileExtensionBool = true;
                    break;
                default:
                    fileExtensionBool = false;
                    break;
            }
            return fileExtensionBool;
        }

        // generating a new GUID which is accepted by the industry foundations classes
        public static string GenerateNewIfcGUID()
        {
            return Xbim.Ifc4.UtilityResource.IfcGloballyUniqueId.ConvertToBase64(Guid.NewGuid());
        }
    }
}
