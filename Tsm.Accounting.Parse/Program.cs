using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Tsm.Accounting;

namespace Tsm.Accounting.Parse
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length != 1)
            {
                // Command line given, display console
                if (!AttachConsole(-1))  // Attach to a parent process console
                {
                    AllocConsole(); // Alloc a new console if none available
                }

                ConsoleMain(arguments);

                SendKeys.SendWait("{ENTER}");

                FreeConsole();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }

            return 0;
        }

        private static void ConsoleMain(string[] args)
        {
            string inputFileName = string.Empty;
            string outputFileName = string.Empty;

            if (args.Length == 3)
            {
                inputFileName = args[1];
                outputFileName = args[2];

                ParseFile(inputFileName, outputFileName);
            }
            else
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine("Parses a TSM_Accounting saved variables file and writes transaction details");
                Console.WriteLine("as a comma delimited file.");
                Console.WriteLine(string.Empty);
                Console.WriteLine("TSMAParse [inputfile] [outputfile]");
                Console.WriteLine(string.Empty);
                Console.WriteLine(" inputfile   Speficies the TSM_Accounting saved variables file to parse.");
                Console.WriteLine(" outputfile  Specifies the filename for the parsed file.");
                Console.WriteLine(string.Empty);
                Console.WriteLine("If no arguments are used, TSMAParse will start an interactive Windows App.");
                Console.WriteLine(string.Empty);
            }
        }

        private static void ParseFile(string inputFileName, string outputFileName)
        {
            SavedVariables currentDB = new SavedVariables();

            try
            {
                currentDB.Load(inputFileName);
                currentDB.WriteToFile(outputFileName);
                Console.WriteLine(string.Empty);
                Console.WriteLine("TSM Accounting file successfully parsed");
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
    }
}
