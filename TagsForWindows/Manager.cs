using Ogx;
using System;
using System.Collections.Generic;
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

        public static void AssignTag(string path, TagColor tagColor, string tagName) {

            try
            {
                if (string.IsNullOrEmpty(tagName))
                    tagName = tagColor.ToString();

                DotUnderscore dotUnderscore;

                string dotUnderscorePath = GetDotUnderscorePath(path);

                if (File.Exists(dotUnderscorePath))
                    dotUnderscore = BinaryHelper.Read<DotUnderscore>(dotUnderscorePath);
                else
                    dotUnderscore = new DotUnderscore();

                Debug.Log("Assign tag : " + tagName);

                BinaryArray tagsArray = dotUnderscore.GetOrAddTagsArray();

                string tagString = $"{tagName}\n{(int)tagColor}";

                foreach (BinaryStringASCII binaryString in tagsArray.properties)
                {
                    var values = binaryString.value.Split('\n');
                    if (values.Length < 1)
                        continue;

                    TagColor presentTagColor = (TagColor)int.Parse(values[1]);

                    if (presentTagColor == tagColor)
                    {
                        binaryString.value = tagString;
                        goto tagSet;
                    }
                }

                BinaryStringASCII newBinaryString = new BinaryStringASCII();
                newBinaryString.value = tagString;
                tagsArray.properties = tagsArray.properties.Add(newBinaryString);

                tagSet:;

                var bytes = BinaryHelper.Write(dotUnderscore);

                FileInfo myFile = new FileInfo(dotUnderscorePath);

                using (FileStream fs = new FileStream(dotUnderscorePath, FileMode.OpenOrCreate))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.SetLength(fs.Position);
                }

                File.SetAttributes(dotUnderscorePath, FileAttributes.Hidden);

            }
            catch (Exception ex)
            {
                Debug.Log("ERROR : " + ex);
            }
        }

        public static void UnassignAllTags(string path)
        {
            string dotUnderscorePath = GetDotUnderscorePath(path);

            if (File.Exists(dotUnderscorePath))
                File.Delete(dotUnderscorePath);
        }

        public static bool UnassignTag(string path, TagColor tagColor) {

            DotUnderscore dotUnderscore;

            string dotUnderscorePath = GetDotUnderscorePath(path);

            if (File.Exists(dotUnderscorePath))
                dotUnderscore = BinaryHelper.Read<DotUnderscore>(dotUnderscorePath);
            else
                return false;

            BinaryArray tagsArray = dotUnderscore.GetOrAddTagsArray();

            bool tagsModified = false;

            foreach (BinaryStringASCII binaryString in tagsArray.properties)
            {
                var values = binaryString.value.Split('\n');
                if (values.Length < 1)
                    continue;

                TagColor presentTagColor = (TagColor)int.Parse(values[1]);

                if (presentTagColor == tagColor)
                {
                    binaryString.value = null;
                    tagsModified = true;
                }
            }

            if (tagsModified)
            {
                tagsArray.properties = tagsArray.properties.Where(x => (x as BinaryStringASCII)?.value != null).ToArray();

                var bytes = BinaryHelper.Write(dotUnderscore);

                File.WriteAllBytes(dotUnderscorePath, bytes);
                File.SetAttributes(dotUnderscorePath, FileAttributes.Hidden);

                return true;
            }

            return false;
        }

        public static IEnumerable<TagAndLabel> GetTags(string path)
        {
            string dotUnderscorePath = GetDotUnderscorePath(path);

            Debug.Log("Get tag : " + dotUnderscorePath);

            if (string.IsNullOrEmpty(dotUnderscorePath))
                yield break;

            if (!File.Exists(dotUnderscorePath))
                yield break;

            DotUnderscore dotUnderscore = BinaryHelper.Read<DotUnderscore>(dotUnderscorePath);

            Ogx.Attribute tagsAttribute = null;

            if (dotUnderscore.entries != null && dotUnderscore.entries.Length > 0)
            {
                tagsAttribute = (dotUnderscore.entries[0].data as AttributesHeader)?
                    .attributes?
                    .Where(x => x.name == "com.apple.metadata:_kMDItemUserTags\0")
                    .FirstOrDefault();
            }

            if (tagsAttribute == null)
                yield break;

            var bplist = tagsAttribute.value as BinaryPropertyList;
            var tagsArray = bplist.property as BinaryArray;

            if (tagsArray == null)
                yield break;

            Debug.Log("ASSIGNED TAGS = " + tagsArray.properties.Length);

            foreach (BinaryStringASCII binaryString in tagsArray.properties)
            {
                var values = binaryString.value.Split('\n');
                if (values.Length < 1)
                    continue;

                string tagName = values[0];
                int tagColor = int.Parse(values[1]);

                Debug.Log("Found tag : " + tagColor + ", " + tagName);

                yield return new TagAndLabel { color = (TagColor)tagColor, label = tagName };
            }

            yield break;
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
