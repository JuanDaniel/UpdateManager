using BBI.JD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autodesk.RevitAddIns;
using CommandLine;
using CommandLine.Text;

namespace UpdateDaemon
{
    class Program
    {
        class Options
        {
            [Option('r', "revitversion", HelpText = "Versión de Revit.", Default = "2019")]
            public string RevitVersion { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        private static void RunOptions(Options options)
        {
            MessageConsole("WELCOME");

            UpdateAddins();

            MessageConsole("END");

            StartRevit(options.RevitVersion);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            MessageConsole("INVALIDARGS");

            Console.ReadKey();
        }

        private static void UpdateAddins()
        {
            UpdateDaemonCollection addinsUpdate = Config.GetAddinsUpdate();

            foreach (UpdateDaemonElement addin in addinsUpdate)
            {
                string[] args = { addin.Name, addin.Version, addin.File };

                MessageConsole("UPDATE", args);

                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Arguments = string.Format("/i {0}", addin.File);
                processInfo.FileName = "msiexec";

                Process process = new Process();
                process.StartInfo = processInfo;
                process.Start();
                process.WaitForExit();

                MessageConsole("OK");
            }

            Config.SetAddinsUpdate(new List<Addin>());
        }

        private static void StartRevit(string version)
        {
            foreach (var product in RevitProductUtility.GetAllInstalledRevitProducts())
            {
                if (product.Version == GetRevitVersion(version))
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.WorkingDirectory = product.InstallLocation;
                    processInfo.FileName = "Revit.exe";

                    Process process = new Process();
                    process.StartInfo = processInfo;
                    process.Start();

                    return;
                }
            }
        }

        private static RevitVersion GetRevitVersion(string version)
        {
            switch (version)
            {
                case "2011":
                    return RevitVersion.Revit2011;
                case "2012":
                    return RevitVersion.Revit2012;
                case "2013":
                    return RevitVersion.Revit2013;
                case "2014":
                    return RevitVersion.Revit2014;
                case "2015":
                    return RevitVersion.Revit2015;
                case "2016":
                    return RevitVersion.Revit2016;
                case "2017":
                    return RevitVersion.Revit2017;
                case "2018":
                    return RevitVersion.Revit2018;
                case "2019":
                    return RevitVersion.Revit2019;
                default:
                    break;
            }

            return RevitVersion.Unknown;
        }

        private static void MessageConsole(string option, object[] args = null)
        {
            switch (option)
            {
                case "INVALIDARGS":
                    Console.WriteLine("Error al iniciar BBI Update Manager, los parámetros provistos son incorrectos.");

                    break;

                case "WELCOME":
                    for (int i = 0; i < 32; i++)
                    {
                        Console.Write("=");
                    }
                    Console.WriteLine();
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 2)
                        {
                            Console.WriteLine("+      BBI Update Manager      +");
                        }
                        else
                        {
                            Console.WriteLine("+                              +");
                        }
                    }
                    for (int i = 0; i < 32; i++)
                    {
                        Console.Write("=");
                    }
                    Console.WriteLine();

                    break;

                case "UPDATE":
                    Console.WriteLine();
                    Console.WriteLine(string.Format("Actualizando: {0} Versión: {1}\nInstalando: {2}", args));

                    break;

                case "OK":
                    Console.WriteLine("================= OK =================");

                    break;

                case "END":
                    Console.WriteLine();
                    Console.WriteLine("Operación finalizada.");
                    Console.Write("Inicializando Revit ");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write(".");
                        System.Threading.Thread.Sleep(1000);
                    }
                    Console.WriteLine();

                    break;

                default:
                    break;
            }
        }
    }
}
