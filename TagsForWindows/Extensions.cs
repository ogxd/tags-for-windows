using System;
using System.Drawing;

namespace TagsForWindows {

    public static class Extensions {

        public static T[] Add<T>(this T[] target, params T[] items) {
            // Validate the parameters
            if (target == null) {
                target = new T[] { };
            }
            if (items == null) {
                items = new T[] { };
            }

            // Join the arrays
            T[] result = new T[target.Length + items.Length];
            target.CopyTo(result, 0);
            items.CopyTo(result, target.Length);
            return result;
        }


        private static float _Dpi = -1;
        public static float Dpi {
            get {
                if (_Dpi == -1) {
                    using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero)) {
                        _Dpi = graphics.DpiY;
                    }
                }
                return _Dpi;
            }
        }

        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        public static void RefreshExplorer() {
            SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
