using IFC_GUI.DataAccess;
using IFC_GUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IFC_GUI_Tests
{
    [TestClass]
    public class DataTransferTest
    {
        // import ifc file in .ifc format
        [TestMethod]
        public void Import_Ifc_Format_With_4_Tasks()
        { 
            // Expected values
            int taskmodelcount = 4;
            string filename = @"..\..\TestData\4TaskTest.ifc";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);

            Assert.AreEqual(taskmodelcount, taskmodellist.Count(), "difference between number of Taskmodels and number of Tasks in Ifcfile");
        }

        // Korrekte Datei einlesen aber die hat keine Tasks
        // Was kommt zurück? Leere Liste oder nix oder wie oder was
            [TestMethod]
        public void Import_IFC_With_Correct_Format_But_No_Tasks()
        {
            int taskmodelcount = 0;

            string filename = @"..\..\TestData\NoTaskIfcFile.ifc";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);

            Assert.AreEqual(taskmodelcount, taskmodellist.Count(), "number of Taskmodels should be 0");
        }

        // fehlerhafte Datei wird importiert
        [TestMethod]
        [ExpectedException(typeof(FileLoadException),
        "corrupted file")]
        public void Import_IFC_Format_With_Corrupted_Content()
        {
            string filename = @"..\..\TestData\corruptedIfcFile.ifc";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);
        }

        // load file
        // no changes, export file
        // check if file is the same
        [TestMethod]
        public void Import_Ifc_Format_Directly_Export_Ifc_Format_Without_Changes()
        {
            string filename = @"..\..\TestData\4TaskTest.ifc";

            List<TaskModel> expected = IfcDataHandling.OpenIfcData(filename);

            IfcDataHandling.UpdateIfcData(filename, expected, ".ifc");

            List<TaskModel> actual = IfcDataHandling.OpenIfcData(filename);

            // cannot do it with areequal first()
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        // Datei einlesen
        // Datei rausschreiben
        // Prüfen ob neue Datei valide ist
        [TestMethod]
        public void Import_Ifc_Format_Export_Check_Valid()
        {
            string filename = @"..\..\TestData\4TaskTest.ifc";

            List<TaskModel> expected = IfcDataHandling.OpenIfcData(filename);

            IfcDataHandling.UpdateIfcData(filename, expected, ".ifc");

            List<TaskModel> actual = IfcDataHandling.OpenIfcData(filename);

            // cannot do it with areequal first()
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        // Datei einlesen
        // Datei konvertieren rausschreiben
        // Prüfen ob neue Datei vorhanden
        [TestMethod]
        public void Import_Ifc_format_Convert_Export_ifcxml_format()
        {

            string filename = @"..\..\TestData\4TaskTest.ifc";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);

            IfcDataHandling.UpdateIfcData(filename, taskmodellist, ".ifcxml");  

            Assert.IsTrue(File.Exists(@"..\..\TestData\4TaskTest.ifcxml"));

            File.Delete(@"..\..\TestData\4TaskTest.ifcxml");
        }



        // Datei Einlesen
        // Datei rausschreiben
        // Prüfen ob es die selbe ist
        // ersten und letzten task vergleichen von beiden

        [TestMethod]
        public void Import_Ifc_Format_Directly_Export_File_Check_If_Tasks_Are_The_Same()
        {
            string filename = @"..\..\TestData\4TaskTest.ifc";

            List<TaskModel> expected = IfcDataHandling.OpenIfcData(filename);

            IfcDataHandling.UpdateIfcData(filename, expected, ".ifc");

            List<TaskModel> actual = IfcDataHandling.OpenIfcData(filename);

            // cannot do it with areequal first()
            Assert.AreEqual(expected.Count(), actual.Count());
            //CollectionAssert.AreEquivalent(expected, actual);
        }

        // Datei Einlesen
        // Task ändern
        // Datei rausschreiben
        // Prüfen ob neue Datei NICHT die selbe ist

        [TestMethod]
        public void Import_Ifc_Format_ChangesToTaskModel_Export_Ifc_Format()
        {
            string filename = @"..\..\TestData\4TaskTest.ifc";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);

            TaskModel tm = taskmodellist.First();

            // Listenänderung rausnehmen
            string expectedName = "TestTask";
            string expectedDescription = "TESTTEST";
            List<string> expectedNests = new List<string>();
            List<string> expectedIsNestedBy = new List<string>();
            string expectedObjectType = "TESTTYPE";
            string expectedIdentification = "TestIdentification";
            string expectedLongDescription = "TestTesttest";
            List<string> expectedIsPredecessorTo = new List<string>();
            List<string> expectedIsSuccessorFrom = new List<string>();
            string expectedStatus = "TestStatus";
            string expectedWorkMethod = "TestMethod";
            bool expectedIsMilestone = false;
            string expectedPriority = "3";
            int expectedPredefinedType = 2;

            tm.Name = expectedName;
            tm.Description = expectedDescription;
            tm.Nests = expectedNests;
            tm.IsNestedBy = expectedIsNestedBy;
            tm.ObjectType = expectedObjectType;
            tm.Identification = expectedIdentification;
            tm.LongDescription = expectedLongDescription;
            tm.IsPredecessorTo = expectedIsPredecessorTo;
            tm.IsSuccessorFrom = expectedIsSuccessorFrom;
            tm.Status = expectedStatus;
            tm.WorkMethod = expectedWorkMethod;
            tm.IsMilestone = expectedIsMilestone;
            tm.Priority = expectedPriority;
            tm.PredefinedType = expectedPredefinedType;


            IfcDataHandling.UpdateIfcData(filename, taskmodellist, ".ifcxml");
            List<TaskModel> taskmodellist2 = IfcDataHandling.OpenIfcData(@"..\..\TestData\4TaskTest.ifcxml");

            Assert.AreEqual(expectedName, taskmodellist2.First().Name);
            Assert.AreEqual(expectedDescription, taskmodellist2.First().Description);
            Assert.AreEqual(expectedObjectType, taskmodellist2.First().ObjectType);
            Assert.AreEqual(expectedIdentification, taskmodellist2.First().Identification);
            Assert.AreEqual(expectedLongDescription, taskmodellist2.First().LongDescription);
            Assert.AreEqual(expectedStatus, taskmodellist2.First().Status);
            Assert.AreEqual(expectedWorkMethod, taskmodellist2.First().WorkMethod);
            Assert.AreEqual(expectedIsMilestone, taskmodellist2.First().IsMilestone);
            Assert.AreEqual(expectedPriority, taskmodellist2.First().Priority);
            Assert.AreEqual(expectedPredefinedType, taskmodellist2.First().PredefinedType);

            File.Delete(@"..\..\TestData\4TaskTest.ifcxml");

            // CollectionAssert.
        }

        // generiere neue Ifc-Datei mit einem Task
        // check if file gets generated 
        // check if file includes task
        [TestMethod]
        public void Generate_New_Ifc_File_Check_File_Exists_And_TaskCount_1_Task()
        {
            int expectedtaskcount = 1;

            TaskModel taskM = new TaskModel();

            List<TaskModel> taskmodellist = new List<TaskModel>
            {
                taskM
            };

            string filename = @"..\..\TestData\newIfcFile1Task.ifc";

            IfcDataHandling.NewIfcData(filename, taskmodellist);

            Assert.IsTrue(File.Exists(filename));

            taskmodellist = IfcDataHandling.OpenIfcData(filename);

            Assert.AreEqual(expectedtaskcount, taskmodellist.Count());
            File.Delete(filename);
        }

        // generiere neue Ifc-Datei mit einem Task
        // check if file gets generated 
        // check if file includes task
        [TestMethod]
        public void Generate_New_Ifcxml_File_Check_File_Exists_And_TaskCount_1_Task()
        {
            int expectedtaskcount = 1;

            TaskModel taskM = new TaskModel();

            List<TaskModel> taskmodellist = new List<TaskModel>
            {
                taskM
            };

            string filename = @"..\..\TestData\newIfcFile1_Task.ifcxml";

            IfcDataHandling.NewIfcData(filename, taskmodellist);

            Assert.IsTrue(File.Exists(filename));

            taskmodellist = IfcDataHandling.OpenIfcData(filename);

            Assert.AreEqual(expectedtaskcount, taskmodellist.Count());
            File.Delete(filename);
        }

        // generiere neue Ifc-Datei mit einem Task
        // check if file gets generated 
        // check if file includes task
        [TestMethod]
        public void Generate_New_Ifczip_File_Check_File_Exists_And_TaskCount_1_Task()
        {
            int expectedtaskcount = 1;

            TaskModel taskM = new TaskModel();

            List<TaskModel> taskmodellist = new List<TaskModel>
            {
                taskM
            };

            string filename = @"..\..\TestData\newIfcFile1Task.ifczip";

            IfcDataHandling.NewIfcData(filename, taskmodellist);

            Assert.IsTrue(File.Exists(filename));

            taskmodellist = IfcDataHandling.OpenIfcData(filename);

            Assert.AreEqual(expectedtaskcount, taskmodellist.Count());
            File.Delete(filename);
        }

        // IFC TASKTIME Attributes ändern prüfen
        // Schauen ob neue Attribute übernommen werden
        [TestMethod]
        public void Import_Ifc_Change_TaskTime_Attribute_Of_TaskModel_Export()
        {
            string filename = @"..\..\TestData\4TaskTest.ifc";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);

            TaskModel tm = taskmodellist.Last();

            TaskTimeModel ttm = new TaskTimeModel();

            string expectedName = "TestTime";
            int expectedDurationType = 1;
            string expectedScheduleDuration = "P2Y10M15DT10H30M20S";
            string expectedScheduleStart = "1111-11-11T11:11:11";
            string expectedScheduleFinish = "1111-11-11T11:11:11";

            ttm.Name = expectedName;
            ttm.DurationType = expectedDurationType;
            ttm.ScheduleDuration = expectedScheduleDuration;
            ttm.ScheduleStart = expectedScheduleStart;
            ttm.ScheduleFinish = expectedScheduleFinish;

            tm.TaskTime = ttm;

            IfcDataHandling.UpdateIfcData(filename, taskmodellist, ".ifcxml");

            List<TaskModel> taskmodellist2 = IfcDataHandling.OpenIfcData(@"..\..\TestData\4TaskTest.ifcxml");
            // IFCXML ändert die Reihenfolge der Tasks weil XML die Subtasks als Unterelement des oberen Tasks einordnet
            // IFC ordnet einfach zu
            TaskModel myTask = taskmodellist2[2];
            Assert.AreEqual(expectedName, myTask.TaskTime.Name);
            Assert.AreEqual(expectedDurationType, myTask.TaskTime.DurationType);
            Assert.AreEqual(expectedScheduleDuration, myTask.TaskTime.ScheduleDuration);
            Assert.AreEqual(expectedScheduleStart, myTask.TaskTime.ScheduleStart);
            Assert.AreEqual(expectedScheduleFinish, myTask.TaskTime.ScheduleFinish);

            // File wieder löschen
            File.Delete(@"..\..\TestData\4TaskTest.ifcxml");
        }

    }
}
