namespace EmptyCache
{
    public class Service
    {
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
                    Console.WriteLine($"{file} rimosso");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{file} NON rimosso - {ex.Message}");
                }
            }

            if (!Directory.GetFiles(folder).Any() && !Directory.GetDirectories(folder).Any())
                Directory.Delete(folder);
        }

        static IEnumerable<string> GetListfolder()
        {
            string localApplicationData = Environment.ExpandEnvironmentVariables("%localappdata%");
            string chromeFolder = @$"{localApplicationData}\Google\Chrome\User Data";
            string edgeFolder = @$"{localApplicationData}\Microsoft\Edge\User Data";
            
            return 
            findfolder(chromeFolder, @"Service Worker")
                .Union(findfolder(chromeFolder, @"Code Cache"))
                .Union(findfolder(chromeFolder, "Cache"))
                .Union(findfolder(edgeFolder, @"Service Worker"))
                .Union(findfolder(edgeFolder, @"Code Cache"))
                .Union(findfolder(edgeFolder, "Cache"));
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
        void LogFolder()
        {
            foreach (string f in folders)
            {
                Console.WriteLine(f);
            }
        }
    }

}