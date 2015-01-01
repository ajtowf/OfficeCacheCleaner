using System.ComponentModel;
using System.Configuration.Install;

namespace OfficeCacheCleanerService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
