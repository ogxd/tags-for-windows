using LabelsForWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {

    class Program {

        static void Main(string[] args) {

            Manager.AssignIcon("test1", "red");
            Manager.AssignIcon("test2", "yellow");
            Manager.AssignIcon("test1", "blue");
            Manager.AssignIcon("test3", "purple");
            Manager.UnassignIcon("test1");
            Console.ReadKey();
        }
    }
}
