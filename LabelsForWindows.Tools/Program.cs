using SharpShell.ServerRegistration;
using System;
using System.IO;

namespace LabelsForWindows.Tools {

    internal class Program {

        static void Main(string[] args) {
            Console.WriteLine("--- LabelsForWindows.Tools ---");
            Process(args);
            Console.WriteLine("------------------------------");
            Console.ReadKey();
        }

        private static bool Process(string[] args) {

            bool install = false;
            bool register = false;
            bool uninstall = false;
            bool unregister = false;
            bool x32 = false;
            string assembly = null;

            foreach (string arg in args) {
                string argl = arg.ToLower();
                install = install || argl == "install";
                uninstall = uninstall || argl == "uninstall";
                register = register || argl == "register";
                unregister = unregister || argl == "unregister";
                x32 = x32 || argl == "x32";
                if (File.Exists(arg) && Path.GetExtension(argl) == ".dll") {
                    assembly = arg;
                }
            }

            if (string.IsNullOrEmpty(assembly)) {
                Console.WriteLine("No assembly specified");
                return false;
            }

            if (install) {
                Console.WriteLine($"Installing '{assembly}' ({(x32 ? 32 : 64)}bit)");
                ServerExtensions.Install(assembly, x32 ? RegistrationType.OS32Bit : RegistrationType.OS64Bit);
            }

            if (register) {
                Console.WriteLine($"Registering '{assembly}' ({(x32 ? 32 : 64)}bit)");
                ServerExtensions.Register(assembly, x32 ? RegistrationType.OS32Bit : RegistrationType.OS64Bit);
            }

            if (uninstall) {
                Console.WriteLine($"Uninstalling '{assembly}' ({(x32 ? 32 : 64)}bit)");
                ServerExtensions.uninstall(assembly, x32 ? RegistrationType.OS32Bit : RegistrationType.OS64Bit);
            }

            if (unregister) {
                Console.WriteLine($"Unregistering '{assembly}' ({(x32 ? 32 : 64)}bit)");
                ServerExtensions.Unregister(assembly, x32 ? RegistrationType.OS32Bit : RegistrationType.OS64Bit);
            }

            if (register || unregister) {
                WindowsExtensions.RestartExplorer();
            }

            return true;
        }
    }
}
