using IFC_GUI.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tests
{
    public class Import
    {
        static void Main(string[] args)
        {
            int taskmodelcount = 4;

            string filename = @"C:\Users\AKnep\Desktop\Studienarbeit\Studienarbeit\IFC Dateien\4TaskTest.ifc";



            Console.WriteLine(filename);

            Console.WriteLine(Path.GetFileName(@"C:\Users\AKnep\Desktop\Studienarbeit\Studienarbeit\IFC Dateien\4TaskTest.ifc"));


            
            var taskmodellist = IfcDataHandling.OpenIfcData(filename);


            if (taskmodelcount.Equals(taskmodellist.Count()))
            {
                Console.WriteLine("number of tasks is equal");
            }
            else
            {
                Console.WriteLine("number of tasks is NOT equal");
            }

            //prints text but keeps cursor in same line
            Console.Write("Press  to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                //run loop until Enter is press
            }
        }
    }
}
