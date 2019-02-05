using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInspector {

    class Program {

        static void Main(string[] args) {
            foreach (string file in Directory.EnumerateFiles(@"..\..\", "*.dll", SearchOption.AllDirectories)) {
                Inspect(file);
            }
            Console.ReadKey();
        }

        static void Inspect(string filepath) {

            if (!File.Exists(filepath)) {
                Console.WriteLine($"Assembly '{filepath}' doesn't exists.");
                return;
            }

            Assembly assembly = null;

            try {
                assembly = Assembly.LoadFrom(filepath);
                if (assembly == null)
                    throw new Exception("Load failed.");
            } catch (Exception exception) {
                Console.WriteLine($"Couldn't load assembly '{filepath}' : {exception}");
                return;
            }

            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true).FirstOrDefault();
            if (attribute == null)
                return;

            Console.WriteLine($"GUID for '{filepath}' : {attribute.Value}");
        }
    }
}
