using IntelReader.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IntelReader
{
    public static class setup
    {
        public static JumpData jumpData;
        public static void ImportData(string[] args)

        {
            using (StreamReader sr = new StreamReader(@"config\config.txt"))
            {
                for (int i = 0; i < 2; i++)
                {
                    string[] data = sr.ReadLine().Split(',');
                    setData(data);
                }

            }

            jumpData = new JumpData();
            using (StreamReader sr = new StreamReader("db/data.csv"))
            {

                string[] data = sr.ReadLine().Split(',');
                bool onTarget = false;
                if (data[0] == "target" && data[1].Replace("\"", "") == config.target)
                {
                    if (jumpData == null)
                    {
                        jumpData = new JumpData();

                    }
                    if (jumpData.jumpNumber == null)
                    {
                        jumpData.jumpNumber = new List<JumpNumber>();
                    }
                    while (!onTarget)
                    {
                        data = sr.ReadLine()?.Split(',');
                        if (data != null && data.Length > 0 && data[0] == "target" || data == null)
                            break;

                        var jn = new JumpNumber();
                        jn.system = data[0].Replace("\"", "");
                        if (data.Length > 1)
                        {
                            jn.jumps = data[1];
                            jumpData.jumpNumber.Add(jn);
                        }
                    }
                }
                foreach (var jn in jumpData.jumpNumber)
                {
                    Console.WriteLine($"{jn.system} : {jn.jumps}");
                }

            }

            using (StreamReader sr = new StreamReader("db/special.csv"))
            {

                string[] data = sr.ReadLine().Split(',');
                bool onTarget = false;


                if (jumpData.special == null)
                {
                    jumpData.special = new List<JumpNumber>();
                }
                while (!onTarget)
                {
                    data = sr.ReadLine()?.Split(',');
                    if (data != null && data.Length > 0 && data[0] == "target" || data == null)
                        break;

                    var jn = new JumpNumber();
                    jn.system = data[0].Replace("\"", "");
                    if (data.Length > 1)
                    {
                        jn.jumps = data[1];
                        jumpData.special.Add(jn);
                    }
                }

                foreach (var jn in jumpData.special)
                {
                    Console.WriteLine($"special: {jn.system} : {jn.jumps}");

                }

            }
            Console.WriteLine($"target: {config.target}");

        }
        public static void setData(string[] data)
        {
            if (data[0] == "chatFolder")
                config.baseEveFolder = data[1].Replace("\"", ""); ;
            if (data[0] == "targetSystem")
                config.target = data[1].Replace("\"", "");
        }

    }
}