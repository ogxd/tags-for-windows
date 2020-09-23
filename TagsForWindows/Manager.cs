using Ogx;
using System;
using System.IO;
using System.Linq;

namespace TagsForWindows {

    public enum TagColor
    {
        Orange = 7,
        Red = 6,
        Yellow = 5,
        Blue = 4,
        Purple = 3,
        Green = 2,
        Gray = 1,
        None = 0,
    }

    public static class Manager {

        private static string GetDotUnderscorePath(string path)
        {
            string filename = Path.GetFileName(path);
            if (filename.StartsWith("._"))
                return null;
            return Path.Combine(Path.GetDirectoryName(path), "._" + filename);
        }

        public static void AssignTag(string path, TagColor tag) {

            var dotUnderscore = new DotUnderscore();
            var entry = new Entry();
            var footerEntry = new FooterEntry();
            footerEntry.id = 2;
            footerEntry.size = 286;
            footerEntry.offset = 3810;
            dotUnderscore.entries = new Entry[] { entry, footerEntry };
            var attrHeader = new AttributesHeader();
            entry.data = attrHeader;
            var tagAttribute = new Ogx.Attribute();
            tagAttribute.name = "com.apple.metadata:_kMDItemUserTags\0";
            attrHeader.attributes.Add(tagAttribute);
            var bplist = new BinaryPropertyList();
            tagAttribute.value = bplist;
            var barray = new BinaryArray();
            bplist.property = barray;
            barray.properties = new BinaryProperty[1];
            barray.properties[0] = new BinaryStringASCII { value = tag.ToString() + "\n" + ((int)tag).ToString() };

            var bytes = BinaryHelper.Write(dotUnderscore);

            string dotUnderscorePath = GetDotUnderscorePath(path);

            File.WriteAllBytes(dotUnderscorePath, bytes);

            File.SetAttributes(dotUnderscorePath, FileAttributes.Hidden);
        }

        public static void UnassignTag(string file) {
            //using (var db = GetDatabase(file)) {
            //    var collection = db.GetCollection<TaggedFile>();
            //    collection.Delete(x => x.file == file);
            //}

            // todo
        }

        public static TagAndLabel GetTag(string path) {

            string dotUnderscorePath = GetDotUnderscorePath(path);

            Debug.Log("Get tag : " + dotUnderscorePath);

            if (string.IsNullOrEmpty(dotUnderscorePath))
                return new TagAndLabel { color = TagColor.None, label = "None" };

            if (!File.Exists(dotUnderscorePath))
                return new TagAndLabel { color = TagColor.None, label = "None" };

            DotUnderscore dotUnderscore = BinaryHelper.Read<DotUnderscore>(dotUnderscorePath);
            Ogx.Attribute tagAttribute = (dotUnderscore.entries[0].data as AttributesHeader).attributes.Where(x => x.name == "com.apple.metadata:_kMDItemUserTags\0").FirstOrDefault();

            if (tagAttribute == null)
            {
                return new TagAndLabel { color = TagColor.None, label = "None" };
            }

            var bplist = tagAttribute.value as BinaryPropertyList;
            var tagsArray = bplist.property as BinaryArray;
            if (tagsArray == null)
            {
                return new TagAndLabel { color = TagColor.None, label = "None" };
            }

            foreach (BinaryStringASCII binaryString in tagsArray.properties)
            {
                var values = binaryString.value.Split('\n');
                string tagName = values[0];
                int tagColor = (values.Length > 1) ? int.Parse(values[1]) : 0;

                Debug.Log("Found tag : " + tagColor + ", " + tagName);

                return new TagAndLabel { color = (TagColor)tagColor, label = tagName };
            }

            return new TagAndLabel { color = TagColor.None, label = "None" };
        }
    }

    public struct TagAndLabel
    {
        public TagColor color;
        public string label;
    }
}
