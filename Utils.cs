
using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntelReader
{
    public static class Utils
    {
        public static string GetFilePrefix(string fq)
        {
            int first, len;
            first = fq.LastIndexOf("\\");
            first++;
            len = fq.Substring(first).IndexOf("_");
            return fq.Substring(first, len);
        }

        public static IEnumerable<LogFileInfo> GetActiveFiles(List<LogFileInfo> lfn)
        {
            var rlfn = new List<LogFileInfo>();
            var data = new List<string>();
            foreach(var d in lfn)
            {
                if(!data.Contains(d.prefix))
                    data.Add(d.prefix);
            }

            foreach(var d in data)
            {
                LogFileInfo latest = null;
                foreach(var lf in lfn)
                {
                    if(d == lf.prefix)
                    {
                        if(latest == null)
                            latest = lf;
                        if (latest.lastWrite < lf.lastWrite)
                            latest = lf;
                    }
                }
                rlfn.Add(latest);
            }
            return rlfn;

        }
        public static bool CheckExists(List<LogFileInfo> lfn, string name)
        {
            
            
            foreach (var d in lfn)
            {
                if (d.name == name)
                    return true;
            }
            return false;

        }
    }
}
