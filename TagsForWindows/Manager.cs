using LiteDB;
using System;
using System.IO;
using System.Linq;

namespace TagsForWindows {

    public static class Manager {

        public static string GetDatabasePath(string file) {
            return Environment.ExpandEnvironmentVariables(@"%appdata%\TagsForWindows\.tags");
        }

        public static LiteDatabase GetDatabase(string file) {
            bool dbExits = File.Exists(file);
            string dbPath = GetDatabasePath(file);
            var db = new LiteDatabase(dbPath);
            if (!dbExits) {
                File.SetAttributes(dbPath, FileAttributes.Hidden);
            }
            return db;
        }

        public static void AssignTag(string file, string tag) {
            using (var db = GetDatabase(file)) {
                var collection = db.GetCollection<TaggedFile>();
                var result = collection.Find(x => x.file == file);
                if (result.Count() > 0) {
                    var taggedFile = result.First();
                    taggedFile.tag = tag;
                    collection.Update(taggedFile);
                } else {
                    var taggedFile = new TaggedFile {
                        file = file,
                        tag = tag,
                    };
                    collection.Insert(taggedFile);
                }
            }
        }

        public static void UnassignTag(string file) {
            using (var db = GetDatabase(file)) {
                var collection = db.GetCollection<TaggedFile>();
                collection.Delete(x => x.file == file);
            }
        }

        public static string GetTag(string file) {
            using (var db = GetDatabase(file)) {
                var result = db.GetCollection<TaggedFile>().Find(x => x.file == file);
                if (result.Count() > 0) {
                    return result.First().tag;
                } else {
                    return null;
                }
            }
        }

        public class TaggedFile {
            public string file { get; set; }
            public string tag { get; set; }
        }
    }
}
