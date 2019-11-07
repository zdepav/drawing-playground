#nullable enable
using System.IO;
using System.Text;

namespace DrawingPlayground {

    internal static class FileUtils {
        
        public static void SaveFile(string path, string content) {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var tempPath = path + ".dpswp";
            File.WriteAllText(tempPath, content, Encoding.UTF8);
            File.Copy(tempPath, path, true);
            File.Delete(tempPath);
        }

        public static void SaveFile(FileInfo file, string content) =>
            SaveFile(file.FullName, content);
        
        public static void SaveFile(DirectoryInfo directory, string filename, string content) =>
            SaveFile(Path.Combine(directory.FullName, filename), content);
        
        public static void SaveFile(string path, byte[] content) {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var tempPath = path + ".dpswp";
            File.WriteAllBytes(tempPath, content);
            File.Copy(tempPath, path, true);
            File.Delete(tempPath);
        }

        public static void SaveFile(FileInfo file, byte[] content) =>
            SaveFile(file.FullName, content);

        public static void SaveFile(DirectoryInfo directory, string filename, byte[] content) =>
            SaveFile(Path.Combine(directory.FullName, filename), content);
        
        public static string LoadFile(string path) {
            return File.Exists(path) ? File.ReadAllText(path, Encoding.UTF8) : "";
        }

        public static string LoadFile(FileInfo file) =>
            LoadFile(file.FullName);

        public static string LoadFile(DirectoryInfo directory, string filename) =>
            LoadFile(Path.Combine(directory.FullName, filename));
        
        public static byte[] LoadBinaryFile(string path) {
            return File.Exists(path) ? File.ReadAllBytes(path) : new byte[0];
        }

        public static byte[] LoadBinaryFile(FileInfo file) =>
            LoadBinaryFile(file.FullName);

        public static byte[] LoadBinaryFile(DirectoryInfo directory, string filename) =>
            LoadBinaryFile(Path.Combine(directory.FullName, filename));

    }

}
