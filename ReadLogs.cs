using IntelReader.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntelReader
{
    static class ReadLogs
    {
        public static string[] names;
        public static List<string> fileList;
        public static int fileNumber = 0;
        public static void readAndCheckNames()
        {
            //string cl1 = @"C:\Users\fox21\Documents\EVE\logs\Chatlogs";
            //var dir = new DirectoryInfo(cl1);



            // && file.Name.StartsWith("Int")
            // use this only if you want to clean up data.
            //foreach(var fi in datafiles)
            //{
            //    if(fi.LastWriteTimeUtc < threeDaysAgo)
            //    {
            //        File.Delete(fi.FullName);
            //    }
            //}

            string folder = config.baseEveFolder.Trim();
            if (!folder.Contains("Chatlogs"))
                folder = Path.Combine(folder, @"Chatlogs");
            var directory = new DirectoryInfo(folder);
            if (fileList == null || fileNumber != directory.GetFiles().Count())
            {
               
                DateTime from_date = DateTime.UtcNow.AddHours(-24);
                DateTime threeDaysAgo = DateTime.UtcNow.AddDays(-3);
                fileList = new List<string>();
                var fileInfos = new List<FileInfo>();
                var datafiles = directory.GetFiles();
                var logFileInfo = new List<LogFileInfo>();
                foreach (var fi in datafiles)
                {
                    if (fi.Name.ToLower().StartsWith("int") && fi.LastWriteTimeUtc >= from_date)
                    {

                        fileInfos.Add(fi);
                        var lfn = new LogFileInfo();
                        lfn.prefix = Utils.GetFilePrefix(fi.Name);
                        lfn.fullName = fi.FullName;
                        lfn.Created = fi.CreationTimeUtc;
                        lfn.lastWrite = fi.LastWriteTimeUtc;
                        lfn.name = fi.Name;
                        if (!Utils.CheckExists(logFileInfo, fi.Name))
                            logFileInfo.Add(lfn);
                    }
                }

                var logFilesDis = Utils.GetActiveFiles(logFileInfo);
                foreach (var lf in logFilesDis)
                {
                    fileList.Add(lf.fullName);
                }
            }
            //var files = directory.GetFiles()
            //  .Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);

            //Initial read, then next read.
            if (lastRead == null)
                {
                    lastRead = new List<LastRead>();
                    runTimes = 0;
                    Console.WriteLine($"Monitoring:");
                    foreach (string fn in fileList)
                    {
                        Console.WriteLine($"file: {fn} ");
                    }
                }
            int linenumber = 0;
            string filename;
            LastRead last;
            foreach (var file in fileList)
            {
                filename = Utils.GetFilePrefix(file);
                linenumber = 0;
                last = lastRead.FirstOrDefault(a => a.fileName == file);
                

                if (last == null)
                {
                    last = new LastRead();
                    last.fileName = file;
                    lastRead.Add(last);
                }
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    Skip(sr, last.lineNumber);
                    linenumber = last.lineNumber;
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        var data = line.Split(' ');
                        linenumber++;
                        last.lineNumber = linenumber;
                        if (runTimes > 0)
                        {
                            Console.WriteLine($"Log: {filename} comment: {line}");
                            bool clr = false;
                            foreach (string d in data)
                            {
                                if(d.ToLower() == "clr" || d.ToLower() == "clear")
                                {
                                    clr = true;
                                }
                            }
                            if (!clr)
                            {
                                IEnumerable<string> data6 = data.Where(a => a.Length == 6);
                                foreach (string d in data6)
                                {
                                        checkSystems(d, filename);
                                }
                            }

                        }

                    }
                    last.lineNumber = linenumber;
                }
            }
            runTimes++;


        }
        public static int runTimes;
        public static List<LastRead> lastRead;
        public static void checkSystems(string d, string fn)
        {
            string fn1 = fn;
            if(fn1.Contains("intel"))
            {
                fn1 = "";
            }

            var jd = setup.jumpData;
            string log = d.ToUpper();
            foreach(var jn in jd.jumpNumber)
            {
                if(jn.system == log)
                {
                    PlaySound.playAlert(jn.jumps.Trim());
                    Console.WriteLine($"CL: {fn} : {jn.system} which is {jn.jumps} away.");
                }
            }

            // special alert sounds if neut appears in special systems
            foreach (var jn in jd.special)
            {
                if (jn.system == log)
                {
                    PlaySound.playAlert(jn.jumps);
                    Console.WriteLine($"CL: {fn} : {jn.system} which is {jn.jumps} away.");
                }
            }
        }
        public static void Skip(StreamReader rd, int skip)
        {
            for(int i = 0; i < skip; i++)
            {
                var data  = rd.ReadLine();
            }
        }
    }
}
