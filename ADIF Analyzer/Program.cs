using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Security.Permissions;
using System.IO;

namespace ADIF_Analyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /*----------------------------------------------------------------------------------------
         * Handler to catch unhandled exceptions.
         * Append an entry to Unhandled Exceptions.log
         */
        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);

            string strUnhandledException = Globals.TimestampEx() + 
               sender.ToString() + ": " + "\n" + e.Message.Trim() + "\n" + e.StackTrace +
               "\n" + e.TargetSite.ToString() + "\n";

            File.AppendAllText(Globals.strLogsDirectory +
               "Unhandled Exceptions.log", strUnhandledException);
            return;
        }

    }
}
