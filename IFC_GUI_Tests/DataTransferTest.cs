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
        /// <summary>
        /// All import tests
        /// </summary>
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

        // import ifc file in .ifc format
        [TestMethod]
        public void Import_IfcXML_Format_With_4_Tasks()
        {
            // Expected values
            int taskmodelcount = 4;
            string filename = @"..\..\TestData\4TaskTest.ifcxml";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);

            Assert.AreEqual(taskmodelcount, taskmodellist.Count(), "difference between number of Taskmodels and number of Tasks in Ifcfile");
        }

        // import ifc file in .ifc format
        [TestMethod]
        public void Import_IfcZIP_Format_With_4_Tasks()
        {
            // Expected values
            int taskmodelcount = 4;
            string filename = @"..\..\TestData\4TaskTest.ifczip";

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

            IfcDataHandling.OpenIfcData(filename);
        }


        /// <summary>
        /// All export tests
        /// </summary>
        /// 

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

            string filename = @"..\..\TestData\newIfcFile1Task.ifcxml";

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

        // Datei Einlesen
        // Task ändern
        // Datei rausschreiben
        // Prüfen ob neue Datei NICHT die selbe ist
        [TestMethod]
        public void Import_Ifc_Format_ChangesToTaskModel_Export_Ifc_Format()
        {
            string filename = @"..\..\TestData\ModificationTaskTest.ifc";

            List<TaskModel> taskmodellist = IfcDataHandling.OpenIfcData(filename);

            int expectedTaskCount = 4;
            //add a new task
            TaskModel tmNew = new TaskModel
            {
                Name = "NewTask"
            };

            taskmodellist.Add(tmNew);

            //modify a task
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

            TaskTimeModel ttm = new TaskTimeModel();

            string expectedTimeName = "TestTime";
            int expectedDurationType = 1;
            string expectedScheduleDuration = "P2Y10M15DT10H30M20S";
            string expectedScheduleStart = "1111-11-11T11:11:11";
            string expectedScheduleFinish = "1111-11-11T11:11:11";

            ttm.Name = expectedTimeName;
            ttm.DurationType = expectedDurationType;
            ttm.ScheduleDuration = expectedScheduleDuration;
            ttm.ScheduleStart = expectedScheduleStart;
            ttm.ScheduleFinish = expectedScheduleFinish;

            tm.TaskTime = ttm;

            //delete a task
            taskmodellist.RemoveAt(1);


            IfcDataHandling.UpdateIfcData(filename, taskmodellist, ".ifcxml");
            List<TaskModel> taskmodellist2 = IfcDataHandling.OpenIfcData(@"..\..\TestData\ModificationTaskTest.ifcxml");

            Assert.AreEqual(expectedTaskCount, taskmodellist2.Count());

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

            Assert.AreEqual(expectedTimeName, taskmodellist2.First().TaskTime.Name);
            Assert.AreEqual(expectedDurationType, taskmodellist2.First().TaskTime.DurationType);
            Assert.AreEqual(expectedScheduleDuration, taskmodellist2.First().TaskTime.ScheduleDuration);
            Assert.AreEqual(expectedScheduleStart, taskmodellist2.First().TaskTime.ScheduleStart);
            Assert.AreEqual(expectedScheduleFinish, taskmodellist2.First().TaskTime.ScheduleFinish);

            File.Delete(@"..\..\TestData\ModificationTaskTest.ifcxml");
        }

        // Datei einlesen
        // Datei konvertieren rausschreiben
        // Prüfen ob neue Datei vorhanden
        [TestMethod]
        public void Convert_Between_All_File_Formats()
        {
            int expectedTaskCount = 1;
            //create new file with one task
            string filename = @"..\..\TestData\newConvertFile.ifc";

            List<TaskModel> taskmodellist = new List<TaskModel>();

            TaskModel tmNew = new TaskModel
            {
                Name = "NewTask"
            };

            taskmodellist.Add(tmNew);

            IfcDataHandling.NewIfcData(filename, taskmodellist);
            Assert.IsTrue(File.Exists(@"..\..\TestData\newConvertFile.ifc"));

            // convert .ifc to .ifcxml and .ifczip
            IfcDataHandling.UpdateIfcData(filename, taskmodellist, ".ifcxml");
            Assert.IsTrue(File.Exists(@"..\..\TestData\newConvertFile.ifcxml"));

            List<TaskModel> temporarytaskmodellist = IfcDataHandling.OpenIfcData(@"..\..\TestData\newConvertFile.ifcxml");
            Assert.AreEqual(expectedTaskCount, temporarytaskmodellist.Count());

            IfcDataHandling.UpdateIfcData(filename, taskmodellist, ".ifczip");
            Assert.IsTrue(File.Exists(@"..\..\TestData\newConvertFile.ifczip"));

            temporarytaskmodellist = IfcDataHandling.OpenIfcData(@"..\..\TestData\newConvertFile.ifczip");
            Assert.AreEqual(expectedTaskCount, temporarytaskmodellist.Count());

            File.Delete(@"..\..\TestData\newConvertFile.ifc");
            File.Delete(@"..\..\TestData\newConvertFile.ifczip");

            // convert .ifcxml to .ifczip and back
            IfcDataHandling.UpdateIfcData(@"..\..\TestData\newConvertFile.ifcxml", taskmodellist, ".ifczip");
            Assert.IsTrue(File.Exists(@"..\..\TestData\newConvertFile.ifczip"));

            temporarytaskmodellist = IfcDataHandling.OpenIfcData(@"..\..\TestData\newConvertFile.ifczip");
            Assert.AreEqual(expectedTaskCount, temporarytaskmodellist.Count());

            File.Delete(@"..\..\TestData\newConvertFile.ifcxml");

            IfcDataHandling.UpdateIfcData(@"..\..\TestData\newConvertFile.ifczip", taskmodellist, ".ifcxml");
            Assert.IsTrue(File.Exists(@"..\..\TestData\newConvertFile.ifcxml"));

            temporarytaskmodellist = IfcDataHandling.OpenIfcData(@"..\..\TestData\newConvertFile.ifcxml");
            Assert.AreEqual(expectedTaskCount, temporarytaskmodellist.Count());

            // convert .ifcxml and .ifczip back to .ifc
            IfcDataHandling.UpdateIfcData(@"..\..\TestData\newConvertFile.ifcxml", taskmodellist, ".ifc");
            Assert.IsTrue(File.Exists(@"..\..\TestData\newConvertFile.ifc"));

            temporarytaskmodellist = IfcDataHandling.OpenIfcData(@"..\..\TestData\newConvertFile.ifc");
            Assert.AreEqual(expectedTaskCount, temporarytaskmodellist.Count());

            File.Delete(@"..\..\TestData\newConvertFile.ifc");

            IfcDataHandling.UpdateIfcData(@"..\..\TestData\newConvertFile.ifczip", taskmodellist, ".ifc");
            Assert.IsTrue(File.Exists(@"..\..\TestData\newConvertFile.ifc"));

            temporarytaskmodellist = IfcDataHandling.OpenIfcData(@"..\..\TestData\newConvertFile.ifc");
            Assert.AreEqual(expectedTaskCount, temporarytaskmodellist.Count());

            // delete all files for the next test run
            File.Delete(@"..\..\TestData\newConvertFile.ifcxml");
            File.Delete(@"..\..\TestData\newConvertFile.ifczip");
            File.Delete(@"..\..\TestData\newConvertFile.ifc");
        }
    }
}
