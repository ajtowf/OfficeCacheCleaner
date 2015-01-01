OfficeCacheCleaner
==================

Cleanes the office 365 offline cache of old files.

Update: Changed impl. to run as a windows service

Program that deletes all FSD and FSF files if they are older than X minutes (default 5). 
I had some trouble with backing 500GB of photos/videos from D: because the offline cache kept filling up my C: which runs on a SSD.
Onedrive / Office 365, what can I say?
