using System;
using System.IO;
using System.Linq;

namespace LabelsForWindows {

    public static class Manager {

        private static string _CachePath;
        public static string CachePath {
            get {
                if (string.IsNullOrEmpty(_CachePath)) {
                    string dir = Environment.ExpandEnvironmentVariables(@"%appdata%\LabelsForWindows");
                    Directory.CreateDirectory(dir);
                    _CachePath = dir + @"\cache.db";
                }
                return _CachePath;
            }
        }

        public static void AssignIcon(string file, string icon) {
            file = file.ToLower().Replace(@"/", @"\").Replace(@"\\", @"\");
            string[] lines = null;
            if (File.Exists(CachePath)) {
                lines = File.ReadAllLines(CachePath);
                for (int i = 0; i < lines.Length; i += 2) {
                    if (lines[i] == file) {
                        lines[i + 1] = icon;
                        File.WriteAllLines(CachePath, lines.Where(x => !string.IsNullOrEmpty(x)));
                        return;
                    }
                }
                lines = lines.Add(file, icon);
            } else {
                lines = new string[] { file, icon };
            }
            File.WriteAllLines(CachePath, lines.Where(x => !string.IsNullOrEmpty(x)));
        }

        public static void UnassignIcon(string file) {
            file = file.ToLower().Replace(@"/", @"\").Replace(@"\\", @"\");
            string[] lines = null;
            if (File.Exists(CachePath)) {
                lines = File.ReadAllLines(CachePath);
                for (int i = 0; i < lines.Length; i += 2) {
                    if (lines[i] == file) {
                        lines[i] = null;
                        lines[i + 1] = null;
                        break;
                    }
                }
                File.WriteAllLines(CachePath, lines.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        private static string[] _CachedLines;

        private static DateTime _LastUpdate;

        public static string GetIcon(string file) {
            file = file.ToLower().Replace(@"/", @"\").Replace(@"\\", @"\");
            if (_CachedLines == null || DateTime.UtcNow > _LastUpdate.AddSeconds(2)) {
                _LastUpdate = DateTime.UtcNow;
                if (File.Exists(CachePath)) {
                    _CachedLines = File.ReadAllLines(CachePath);
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
    }
}
