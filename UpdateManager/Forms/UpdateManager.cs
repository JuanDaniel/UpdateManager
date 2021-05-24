using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using Autodesk.Revit.UI;
using Autodesk.RevitAddIns;
using System.Reflection;
using System.IO;

namespace BBI.JD.Forms
{
    public partial class UpdateManager : Form
    {
        private UIApplication application;
        private Command command;

        public UpdateManager(UIApplication application, Command command)
        {
            InitializeComponent();

            this.application = application;
            this.command = command;
        }

        private void UpdateManager_Load(object sender, EventArgs e)
        {
            if (CheckPathVersionControl())
            {
                txt_VersionControl.Text = Config.Get("pathVersionControl");
                chk_Disable.Checked = Config.Get("disableOldAddin") == "True";

                LoadAddinsData();
            }
        }

        private void btn_VersionControl_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                txt_VersionControl.Text = openFileDialog1.FileName;

                Config.Set("pathVersionControl", openFileDialog1.FileName);

                LoadAddinsData();
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Para actualizar los plugins de BBI, el Revit será reiniciado.\n¿Desea proceder con la operación?", "Reinicio de Revit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                Config.Set("plannedUpdate", "True");

                Close();

                command.CloseRevit();
            }
        }

        private void chk_Disable_CheckedChanged(object sender, EventArgs e)
        {
            bool changed = false;

            if (((CheckBox)sender).Checked)
            {
                if (changed = Config.Get("disableOldAddin") == "False")
                {
                    Config.Set("disableOldAddin", "True");
                }
            }
            else
            {
                if (changed = Config.Get("disableOldAddin") == "True")
                {
                    Config.Set("disableOldAddin", "False");
                }
            }

            if (changed)
            {
                MessageBox.Show("Los cambios surtirán efecto en el próximo reinicio de Revit.", "Efecto de los cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckPathVersionControl()
        {
            if (!File.Exists(Config.Get("pathVersionControl")))
            {
                MessageBox.Show("El fichero de control de versiones de los plugins BBI no existe. Defina una dirección correcta para el mismo", "Control de versiones inexistente", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private string GetTiTleForm()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            return string.Format("{0} ({1}.{2}.{3}.{4})", "BBI Update Manager", version.Major, version.Minor, version.Build, version.Revision);
        }

        private void LoadAddinsData()
        {
            /* Only for test */
            //Updater.Write(Config.Get("pathVersionControl"));

            AddinsVersion addinsVersion = Updater.Read(Config.Get("pathVersionControl"));

            if (addinsVersion != null)
            {
                List<RevitAddInManifest> addInManifests = GetRevitAddInManifests();
                List<Addin> addinsUpdate = new List<Addin>();

                foreach (var addin in addinsVersion.Addins)
                {
                    Guid guid = new Guid(addin.Id);
                    RevitAddInManifest addInManifest = addInManifests.FirstOrDefault(x => x.AddInApplications.Any(y => y.AddInId == guid));

                    if (addInManifest != null)
                    {
                        RevitAddInApplication addInApplication = addInManifest.AddInApplications[0];

                        Assembly assembly = Assembly.LoadFrom(addInApplication.Assembly);
                        Version version = assembly.GetName().Version;

                        TreeNode node = new TreeNode(string.Format("Instalado: {0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision));
                        TreeNode node1 = new TreeNode(string.Format("Disponible: {0}", addin.Version));

                        TreeNode treeNode = new TreeNode(addin.Name, new TreeNode[] { node, node1 });
                        treeView1.Nodes.Add(treeNode);

                        if (new Version(addin.Version).CompareTo(version) > 0)
                        {
                            treeNode.ForeColor = Color.Red;

                            btn_Update.Enabled = true;

                            addinsUpdate.Add(addin);
                        }
                    }
                }

                treeView1.ExpandAll();

                Config.SetAddinsUpdate(addinsUpdate);
            }
        }

        private List<RevitAddInManifest> GetRevitAddInManifests()
        {
            List<RevitAddInManifest> addInManifests = new List<RevitAddInManifest>();

            foreach (var product in RevitProductUtility.GetAllInstalledRevitProducts())
            {
                addInManifests.AddRange(
                    AddInManifestUtility.GetRevitAddInManifests(
                        product.AllUsersAddInFolder, 
                        new Dictionary<string, string>()
                    )
                );
            }

            return addInManifests;
        }
    }
}
