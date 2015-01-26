OfficeCacheCleaner
==================

Cleanes the office 365 offline cache of old files.

Program that deletes all FSD and FSF files if they are older than X minutes (default 5). 
I had some trouble with backing 500GB of photos/videos from D: because the offline cache kept filling up my C: which runs on a SSD. Onedrive / Office 365, what can I say?

Update: Now running as a windows service, you'll find the installer under dist/

If you run into any problems report them here, otherwise this application is completed.

Blog post : http://www.towfeek.se/2014/12/office-365-onedrive-offline-cache-size-problem/

<h1>Install Instructions</h1>
After installing the MSI, you need to configure the service and start it.
<h2>Configure the Service</h2>
<ol>
<li>Navigate to where the service is installed with Explorer, i.e. <i>C:\Program Files (x86)\Towfeek Solutions AB\Office Cache Cleaner\</i></li>
<li>Open <i>OfficeCacheCleanerService.exe.config</i> with notepad</li>
<li>Change the path <i>C:\Users\Ajden\AppData\Local\Microsoft\Office\15.0\OfficeFileCache</i> to your path. You'll probably just need to change the username <i>Ajden</i></li>
<li>Optional: Change how old files can get in minutes before automatically deleting them, default is 5 minutes.</li>
<li>Optional: Configure how often the cleaner runs, default is every minute.</li>
</ol>
<h2>Start the service</h2>
<p>The service will start automatically next time you reboot, if you wan't to do it manually follow these steps.
<ol>
<li>Press <strong>Windows+R</strong></li>
<li>Type <strong>services.msc</strong>, press <strong>Enter</strong></li>
<li>Find the service named <i>Office Cache Cleaner</i></li>
<li><strong>Right-click</strong> and select <strong>Start</strong></li>
</ol>


