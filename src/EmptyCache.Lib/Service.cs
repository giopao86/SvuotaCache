using EmptyCache.Lib.Models;
namespace EmptyCache.Lib
{
    public class Service
    {
        public event EventHandler<LogEventArgs>? LogEvent;
        IEnumerable<string> folders = new List<string>();
        public void Execute()
        {
            folders = GetListfolder().OrderBy(x => x);
            DeleteFiles();
        }

        private void DeleteFiles()
        {
            foreach (string folder in folders)
            {
                RemoveFileFromFolder(folder);
            }
        }

        private void RemoveFileFromFolder(string folder)
        {
            IEnumerable<string> files = Directory.GetFiles(folder);
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Log(file, true, null);
                }
                catch (Exception ex)
                {
                    Log(file, false, ex.Message);
                }
            }

            if (!Directory.GetFiles(folder).Any() && !Directory.GetDirectories(folder).Any())
                Directory.Delete(folder);
        }
        private void Log(string file, bool success, string? ErrorMessage)
        {

            if (LogEvent != null)
            {
                LogEventArgs e = new LogEventArgs()
                {
                    FilePath = file,
                    Success = success,
                    ErrorMessage = ErrorMessage
                };
                LogEvent(this, e);
            }
        }
        static IEnumerable<string> GetListfolder()
        {
            string localApplicationData = Environment.ExpandEnvironmentVariables("%localappdata%");
            string chromeFolder = @$"{localApplicationData}\Google\Chrome\User Data";
            string edgeFolder = @$"{localApplicationData}\Microsoft\Edge\User Data";

            return
                ChromeBrowser(chromeFolder)
                .Union(ChromeBrowser(edgeFolder));
        }

        static IEnumerable<string> ChromeBrowser(string prefixPath)
        {
            return findfolder(prefixPath, @"Service Worker")
                .Union(findfolder(prefixPath, @"Code Cache"))
                .Union(findfolder(prefixPath, "Cache"))
                .Union(findfolder(prefixPath, "Storage"))
                .Union(findfolder(prefixPath, "blob_storage"))
                .Union(findfolder(prefixPath, "Session Storage"))
                .Union(findfolder(prefixPath, "IndexedDB"));
        }
        static IEnumerable<string> LoadSubDirs(string dir)
        {

            string[] subdirectoryEntries = Directory.GetDirectories(dir);

            foreach (string subdirectory in subdirectoryEntries)
            {
                yield return subdirectory;
                IEnumerable<string> result = LoadSubDirs(subdirectory);
                foreach (var item in result)
                {
                    yield return item;
                }
            }
        }
        static IEnumerable<string> findfolder(string root, string search)
        {
            IEnumerable<string> collection = Directory.GetDirectories(root, search, SearchOption.AllDirectories);
            foreach (var item in collection)
            {
                IEnumerable<string> collectionsub = LoadSubDirs(item);
                foreach (var itemsub in collectionsub)
                {
                    yield return itemsub;
                }
                yield return item;
            }
        }
    }

}