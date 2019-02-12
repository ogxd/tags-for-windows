using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace LabelsForWindows {

    public static class Manager {

        private static string _CachePath = Environment.ExpandEnvironmentVariables(@"%tmp%\LabelsForWindows.cache");

        public static void AssignIcon(string file, string icon) {
            file = file.ToLower().Replace(@"/", @"\").Replace(@"\\", @"\");
            string[] lines = null;
            if (File.Exists(_CachePath)) {
                lines = File.ReadAllLines(_CachePath);
                for (int i = 0; i < lines.Length; i += 2) {
                    if (lines[i] == file) {
                        lines[i + 1] = icon;
                        File.WriteAllLines(_CachePath, lines.Where(x => !string.IsNullOrEmpty(x)));
                        return;
                    }
                }
                lines = lines.Add(file, icon);
            } else {
                lines = new string[] { file, icon };
            }
            File.WriteAllLines(_CachePath, lines.Where(x => !string.IsNullOrEmpty(x)));
        }

        public static void UnassignIcon(string file) {
            file = file.ToLower().Replace(@"/", @"\").Replace(@"\\", @"\");
            string[] lines = null;
            if (File.Exists(_CachePath)) {
                lines = File.ReadAllLines(_CachePath);
                for (int i = 0; i < lines.Length; i += 2) {
                    if (lines[i] == file) {
                        lines[i] = null;
                        lines[i + 1] = null;
                        break;
                    }
                }
                File.WriteAllLines(_CachePath, lines.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        private static string[] _CachedLines;

        private static DateTime _LastUpdate;

        public static string GetIcon(string file) {
            file = file.ToLower().Replace(@"/", @"\").Replace(@"\\", @"\");
            if (_CachedLines == null || DateTime.UtcNow > _LastUpdate.AddSeconds(2)) {
                _LastUpdate = DateTime.UtcNow;
                if (File.Exists(_CachePath)) {
                    _CachedLines = File.ReadAllLines(_CachePath);
                } else {
                    return null;
                }
            }
            for (int i = 0; i < _CachedLines.Length; i += 2) {
                if (_CachedLines[i] == file) {
                    return _CachedLines[i + 1];
                }
            }
            return null;
        }

        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        public static void Refresh() {
            SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
