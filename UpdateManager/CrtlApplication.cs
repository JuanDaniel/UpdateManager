using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Autodesk.Revit.DB.Events;

namespace BBI.JD
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        private UIApplication application;

        static uint WM_CLOSE = 0x10;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        static IntPtr hWndRevit = IntPtr.Zero;
        private static IntPtr RevitWndHandle
        {
            get
            {
                if (hWndRevit == IntPtr.Zero)
                {
                    Process process = Process.GetCurrentProcess();

                    hWndRevit = process.MainWindowHandle;
                }

                return hWndRevit;
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            application = commandData.Application;

            try
            {
                ShowForm();
            }
            catch (Exception ex)
            {
                message = ex.Message;

                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private void ShowForm()
        {
            using (var form = new Forms.UpdateManager(application, this))
            {
                form.ShowDialog();
            }
        }

        public void CloseRevit()
        {
            SendMessage(RevitWndHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
    }

    public class CrtlApplication : IExternalApplication
    {
        private UIControlledApplication application;
        private string tabName = "BBI";

        public Result OnStartup(UIControlledApplication application)
        {
            this.application = application;

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string folder = new FileInfo(assemblyPath).Directory.FullName;

            // Create a customm ribbon tab
            Autodesk.Windows.RibbonTab tab = CreateRibbonTab(application, tabName);

            // Add new ribbon panel
            string panelName = "Tools";
            RibbonPanel ribbonPanel = CreateRibbonPanel(application, tab, panelName);

            // Create a push button in the ribbon panel
            PushButton pushButton = ribbonPanel.AddItem(new PushButtonData(
                "UpdateManager",
                "BBI Update Manager",
                assemblyPath, "BBI.JD.Command")) as PushButton;

            // Set the large image shown on button
            Uri uriImage = new Uri(string.Concat(folder, "/icon_32x32.png"));
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;

            application.ControlledApplication.ApplicationInitialized += ManageBBIAddins;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            if (Config.Get("plannedUpdate") == "True")
            {
                if (Config.GetAddinsUpdate().Count > 0)
                {
                    string assemblyPath = Assembly.GetExecutingAssembly().Location;
                    string folder = new FileInfo(assemblyPath).Directory.FullName;
                    string updateDaemon = string.Concat(folder, "/UpdateDaemon.exe");

                    // Only in debug mode
                    //updateDaemon = @"C:\Users\jd.santana\Documents\Visual Studio 2017\Projects\UpdateManager\UpdateDaemon\bin\Debug\UpdateDaemon.exe";

                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.Arguments = string.Format("--revitversion {0}", application.ControlledApplication.VersionNumber);
                    processInfo.FileName = updateDaemon;

                    Process process = new Process();
                    process.StartInfo = processInfo;
                    process.Start();
                }

                Config.Set("plannedUpdate", "False");
            }

            application.ControlledApplication.ApplicationInitialized -= ManageBBIAddins;

            return Result.Succeeded;
        }

        private void ManageBBIAddins(object sender, ApplicationInitializedEventArgs args)
        {
            if (Config.Get("disableOldAddin") == "True")
            {
                if (Config.GetAddinsUpdate().Count > 0)
                {
                    List<RibbonPanel> panels = application.GetRibbonPanels(tabName);

                    foreach (RibbonPanel panel in panels)
                    {
                        IList<RibbonItem> ribbons = panel.GetItems();

                        foreach (RibbonItem ribbon in ribbons)
                        {
                            if (Config.AddinHasUpdate("Name", ribbon.Name))
                            {
                                ribbon.Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        private Autodesk.Windows.RibbonTab CreateRibbonTab(UIControlledApplication application, string tabName)
        {
            Autodesk.Windows.RibbonTab tab = Autodesk.Windows.ComponentManager.Ribbon.Tabs.FirstOrDefault(x => x.Id == tabName);

            if (tab == null)
            {
                application.CreateRibbonTab(tabName);

                tab = Autodesk.Windows.ComponentManager.Ribbon.Tabs.FirstOrDefault(x => x.Id == tabName);
            }

            return tab;
        }

        private RibbonPanel CreateRibbonPanel(UIControlledApplication application, Autodesk.Windows.RibbonTab tab, string panelName)
        {
            RibbonPanel panel = application.GetRibbonPanels(tab.Name).FirstOrDefault(x => x.Name == panelName);

            if (panel == null)
            {
                panel = application.CreateRibbonPanel(tab.Name, panelName);
            }

            return panel;
        }
    }
}
