using System;

namespace IntelReader
{
    public class Intel
    {
        public void start(string[] args)
        {
            var alertSystem = new AlertSystem();
            alertSystem.start(args);

        }

    }
}
