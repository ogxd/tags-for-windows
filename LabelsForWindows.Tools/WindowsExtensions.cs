using System.Diagnostics;

namespace LabelsForWindows.Tools {

    public static class WindowsExtensions {

        public static void RestartExplorer() {

            foreach (Process p in Process.GetProcesses()) {
                // In case we get Access Denied
                try {
                    if (p.MainModule.FileName.ToLower().EndsWith(":\\windows\\explorer.exe")) {
                        p.Kill();
                        break;
                    }
                }
                catch { }
            }
            Process.Start("explorer.exe");
        }
    }
}
