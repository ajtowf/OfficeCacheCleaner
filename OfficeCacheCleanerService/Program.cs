using System.ServiceProcess;

namespace OfficeCacheCleanerService
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new CleanerService() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
