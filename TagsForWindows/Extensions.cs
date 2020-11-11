using Ogx;
using System;
using System.Drawing;
using System.Linq;

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

        public static BinaryArray GetOrAddTagsArray(this DotUnderscore dotUnderscore)
        {
            const string TAG_KEYWORD = "com.apple.metadata:_kMDItemUserTags\0";

            Ogx.Attribute tagsAttribute = null;

            if (dotUnderscore.entries != null && dotUnderscore.entries.Length > 0)
            {
                tagsAttribute = (dotUnderscore.entries[0].data as AttributesHeader)?
                    .attributes?
                    .Where(x => x.name == TAG_KEYWORD)
                    .FirstOrDefault();
            }

            if (tagsAttribute == null)
            {
                var entry = new Entry(9u);
                var footerEntry = new Entry(2u);
                footerEntry.id = 2;
                footerEntry.size = 286;
                footerEntry.offset = 3810;
                dotUnderscore.entries = new Entry[] { entry, footerEntry };
                var attrHeader = new AttributesHeader();
                entry.data = attrHeader;
                tagsAttribute = new Ogx.Attribute();
                tagsAttribute.name = TAG_KEYWORD;
                attrHeader.attributes.Add(tagsAttribute);
            }

            BinaryArray tagsArray = (tagsAttribute.value as BinaryPropertyList)?.property as BinaryArray;

            if (tagsArray == null)
            {
                var bplist = new BinaryPropertyList();
                tagsAttribute.value = bplist;
                tagsArray = new BinaryArray();
                bplist.property = tagsArray;
            }

            return tagsArray;
        }
    }
}
