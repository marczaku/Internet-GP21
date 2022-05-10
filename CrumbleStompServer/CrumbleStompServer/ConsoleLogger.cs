using System;
using CrumbleStompShared.CrumbleStompShared.Interfaces;

namespace CrumbleStompServer
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}