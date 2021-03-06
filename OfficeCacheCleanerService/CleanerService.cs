﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace OfficeCacheCleanerService
{
    public partial class CleanerService : ServiceBase
    {
        private const string OfficeCacheFolderKey = "OfficeCacheFolder";
        private const string LastAccessTimeMinutesBeforeDeleteKey = "LastAccessTimeMinutesBeforeDelete";
        private const string CleanIntervalMinutes = "CleanIntervalMinutes";
        
        private readonly LogWriter _logger;
        private readonly string _cacheDir;
        private readonly int _maxAgeMinutes;
        private readonly int _cleanIntervalMinutes;
        
        private Timer _timer;

        public CleanerService()
        {
            InitializeComponent();
            _logger = new LogWriter(BuildProgrammaticConfig());

            _cacheDir = ConfigurationManager.AppSettings[OfficeCacheFolderKey];
            _maxAgeMinutes = int.Parse(ConfigurationManager.AppSettings[LastAccessTimeMinutesBeforeDeleteKey]);
            _cleanIntervalMinutes = int.Parse(ConfigurationManager.AppSettings[CleanIntervalMinutes]);
        }

        protected override void OnStart(string[] args)
        {
            if (Directory.Exists(_cacheDir))
            {
                _timer = new Timer(_cleanIntervalMinutes * 60 * 1000);
                _timer.Elapsed += HandleTick;
                _timer.Enabled = true;

                Log("Office Cache Cleaner Started");
            }
            else
            {
                Log(string.Format("Couldn't find the directory: {0}", _cacheDir));
            }
        }

        private void HandleTick(object sender, ElapsedEventArgs e)
        {
            var files = GetCacheFiles();
            Log(string.Format("Found {0} files", files.Count));

            var deletedCount = 0;
            foreach (string file in files)
            {
                var fi = new FileInfo(file);
                if (fi.LastWriteTime < DateTime.Now.AddMinutes(-_maxAgeMinutes))
                {
                    try
                    {
                        fi.Delete();
                        Log(string.Format("Deleted {0}", fi.FullName));
                        deletedCount++;
                    }
                    catch (Exception)
                    {
                        Log(string.Format("Failed to delete {0}, probably being used.", fi.FullName));
                    }
                }
            }

            Log(string.Format("Deleted Totally {0} files this iteration", deletedCount));
        }

        private List<string> GetCacheFiles()
        {
            var result = new List<string>();

            try
            {
                result.AddRange(Directory.GetFiles(_cacheDir).Where(file => Regex.IsMatch(file, @"^.+\.(FSD|FSF)$")));
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }

            return result;
        }

        protected override void OnStop()
        {
            Log("Office Cache Cleaner Stopped");
        }

        private static LoggingConfiguration BuildProgrammaticConfig()
        {
            var formatter = new TextFormatter();
            var flatFileTraceListener = new FlatFileTraceListener(
                "log.txt",
                "----------------------------------------",
                "----------------------------------------", 
                formatter);
            
            var config = new LoggingConfiguration();
            config.AddLogSource("General", SourceLevels.All, true).AddTraceListener(flatFileTraceListener);
            config.IsTracingEnabled = true;
            return config;
        }

        private void Log(string message)
        {
            _logger.Write(new LogEntry { Message = message });
        }
    }
}
