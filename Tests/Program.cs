using LabelsForWindows;
using System;

namespace Tests {

    class Program {

        static void Main(string[] args) {

            //Manager.AssignIcon("test1", "red");
            //Manager.AssignIcon("test2", "yellow");
            //Manager.AssignIcon("test1", "blue");
            //Manager.AssignIcon("test3", "purple");
            //Manager.UnassignIcon("test1");

            Console.WriteLine(ContextMenu.GetBitmap("Green"));

            Console.ReadKey();
        }
    }
}
