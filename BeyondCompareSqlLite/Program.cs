﻿using System;
using System.IO;
using BeyondCompareSqlLite.Model;

namespace BeyondCompareSqlLite.CLI
{
    public class Program
    {
        private const int Success = 0x1;
        private const int ErrorBadArguments = -0x1;
        private const int ErrorFileDoesntExists = -0x2;
        private const int ErrorUnknown = -0x3;

        public static void Main(string[] args)
        {

            ConsoleHelper.PrintIntro();
            if (args == null || args.Length != 2)
            {
                ConsoleHelper.PrintHelp();
                Environment.ExitCode = ErrorBadArguments;
            }

            var source = args[0];
            var target = args[1];

            if (!File.Exists(source))
            {
                ConsoleHelper.PrintFileDoesntExists(source);
                Environment.ExitCode = ErrorFileDoesntExists;
            }

            try
            {
                var tableContentList = DbContext.GetTableContent(source);
                Report.CreateTextReport(tableContentList, target);
                Environment.ExitCode = Success;
            }
            catch (Exception e)
            {
                File.WriteAllText(target, e.ToString());
                Console.Out.WriteLine("Error ocurred: " + e);
                Environment.ExitCode = ErrorUnknown;
            }
        }
    }
}