using System;
using System.IO;

namespace TagsForWindows {

    public static class Debug {

        public static void Log(object message) {
#if DEBUG
            string logPath = Environment.ExpandEnvironmentVariables(@"%appdata%\TagsForWindows\debug.log");
            if (!File.Exists(logPath)) {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            }
            File.AppendAllText(logPath, message.ToString() + "\n");
#endif
        }
    }
}