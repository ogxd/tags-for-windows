using SharpShell;
using SharpShell.ServerRegistration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace LabelsForWindows.Tools {

    public static class ServerExtensions {

        public static void Register(string assemblyPath, RegistrationType type) {
            foreach (ISharpShellServer server in GetServers(assemblyPath)) {
                ServerRegistrationManager.RegisterServer(server, type);
            }
        }

        public static void Unregister(string assemblyPath, RegistrationType type) {
            foreach (ISharpShellServer server in GetServers(assemblyPath)) {
                ServerRegistrationManager.UnregisterServer(server, type);
            }
        }

        public static void Install(string assemblyPath, RegistrationType type) {
            foreach (ISharpShellServer server in GetServers(assemblyPath)) {
                ServerRegistrationManager.InstallServer(server, type, true);
            }
        }

        public static void uninstall(string assemblyPath, RegistrationType type) {
            foreach (ISharpShellServer server in GetServers(assemblyPath)) {
                ServerRegistrationManager.UninstallServer(server, type);
            }
        }

        private static IEnumerable<ISharpShellServer> GetServers(string assemblyPath) {

            List<ISharpShellServer> servers = new List<ISharpShellServer>();

            try {
                //  Create an assembly catalog for the assembly and a container from it.
                var catalog = new AssemblyCatalog(Path.GetFullPath(assemblyPath));
                var container = new CompositionContainer(catalog);

                //  Get all exports of type ISharpShellServer.
                var serverTypes = container.GetExports<ISharpShellServer>();

                //  Go through each servertype (creating the instance from the lazy).
                foreach (var serverType in serverTypes) {
                    try {
                        servers.Add(serverType.Value);
                    } catch (Exception) {
                        
                    }
                }
            }
            catch (Exception) {
                //  It's almost certainly not a COM server.
            }

            return servers;
        }
    }
}
