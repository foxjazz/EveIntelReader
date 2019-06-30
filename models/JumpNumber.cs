using System;
using System.Collections.Generic;

namespace IntelReader.models
{
    public class JumpData
    {
        public string targetSystem { get; set; }
        public List<JumpNumber>  jumpNumber { get; set; }
        public List<JumpNumber> special { get; set; }
    }
    
    public class JumpNumber
    {
        public string system;
        public string jumps;
    }
    public class LastRead
    {
        public string fileName;
        public int lineNumber;
    }
    public class LogFileInfo
    {
        public string fullName;
        public string name;
        public string prefix;
        public DateTime Created;
        public DateTime lastWrite;
    }
    public class loglist
    {
        public List<string> ChatFiles { get; set; }

    }
}