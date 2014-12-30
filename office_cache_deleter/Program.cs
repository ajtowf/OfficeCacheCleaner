using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace office_cache_deleter
{
    class Program
    {
        private const string OfficeCacheFolderKey = "OfficeCacheFolder";
        private const string LastAccessTimeMinutesBeforeDeleteKey = "LastAccessTimeMinutesBeforeDelete";

        static void Main(string[] args)
        {
            var officeCacheDir = ConfigurationManager.AppSettings[OfficeCacheFolderKey];
            var maxAgeMinutes = int.Parse(ConfigurationManager.AppSettings[LastAccessTimeMinutesBeforeDeleteKey]);
            
            while (true)
            {
                string[] files = Directory.GetFiles(officeCacheDir).Where(file => Regex.IsMatch(file, @"^.+\.(FSD|FSF)$")).ToArray();
                Console.WriteLine("Found {0} files", files.Length);
                var deletedCount = 0;
                foreach (string file in files)
                {
                    var fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddMinutes(maxAgeMinutes))
                    {
                        try
                        {
                            fi.Delete();
                            Console.WriteLine("Deleted {0}", fi.FullName);
                            deletedCount++;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Failed to delete {0}, probably being used.", fi.FullName);
                        }
                    }
                }

                Console.WriteLine("Deleted Totally {0} files this iteration", deletedCount);
                Console.WriteLine("Sleeping for 1 minute, safe to close application now.");
                Thread.Sleep(60 * 1000);
            }
        }
    }
}
