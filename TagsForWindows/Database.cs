#define DB_DIR_LEVEL

using LiteDB;
using System;
using System.IO;
using System.Linq;

namespace TagsForWindows {

    public static class Database {

        public static string GetDatabasePath(string file) {
#if DB_DIR_LEVEL
            return Path.Combine(Path.GetDirectoryName(file), "tags.db");
#else
            return Environment.ExpandEnvironmentVariables(@"%appdata%\TagsForWindows\tags.db");
#endif
        }

        public static bool CheckIfDatabaseExists(string file) {
            return File.Exists(GetDatabasePath(file));
        }

        public static LiteDatabase GetDatabase(string file) {
            bool dbExits = File.Exists(file);
            string dbPath = GetDatabasePath(file);
            if (!dbExits) {
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            }
            var db = new LiteDatabase(dbPath);
            if (!dbExits) {
                File.SetAttributes(dbPath, FileAttributes.Hidden);
            }
            return db;
        }

        public static void AssignTag(string file, string tag) {
            if (!CheckIfDatabaseExists(file)) {
#if !DB_DIR_LEVEL
                Directory.CreateDirectory(Path.GetDirectoryName(file));
#endif
                try {
                    // The database does not exists, so we create a blank one
                    var db = GetDatabase(file);
                    db.Dispose();
                    File.SetAttributes(GetDatabasePath(file), FileAttributes.Hidden);
                } catch (Exception exception) {
                    Debug.Log(exception.ToString());
                }
            }

            Debug.Log($"Assign tag '{tag}' to '{file}'");
            try {
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
            } catch(Exception exception) {
                Debug.Log(exception.ToString());
            }
        }

        public static void UnassignTag(string file) {
            Debug.Log($"Unassign tag of '{file}'");
            try {
                using (var db = GetDatabase(file)) {
                    var collection = db.GetCollection<TaggedFile>();
                    collection.Delete(x => x.file == file);
                }
            } catch (Exception exception) {
                Debug.Log(exception.ToString());
            }
        }

        public static string GetTag(string file) {
            // Avoids the creation of a database when no file is tagged in a given directory
            if (!CheckIfDatabaseExists(file)) {
                return null;
            }
            Debug.Log($"Get tag of '{file}'");
            try {
                using (var db = GetDatabase(file)) {
                    var result = db.GetCollection<TaggedFile>().Find(x => x.file == file);
                    if (result.Count() > 0) {
                        Debug.Log($"Returned tag : '{result.First().tag}'");
                        return result.First().tag;
                    } else {
                        return null;
                    }
                }
            } catch (Exception exception) {
                Debug.Log(exception.ToString());
                return null;
            }
        }

        public class TaggedFile {
            [BsonId]
            public string file { get; set; }
            [BsonField]
            public string tag { get; set; }
        }
    }
}
