namespace BBI.JD.Forms
{
    partial class UpdateManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateManager));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.txt_VersionControl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_VersionControl = new System.Windows.Forms.Button();
            this.btn_Update = new System.Windows.Forms.Button();
            this.chk_Disable = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 117);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(588, 257);
            this.treeView1.TabIndex = 4;
            // 
            // txt_VersionControl
            // 
            this.txt_VersionControl.Location = new System.Drawing.Point(12, 42);
            this.txt_VersionControl.Name = "txt_VersionControl";
            this.txt_VersionControl.ReadOnly = true;
            this.txt_VersionControl.Size = new System.Drawing.Size(549, 20);
            this.txt_VersionControl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Control de versiones";
            // 
            // btn_VersionControl
            // 
            this.btn_VersionControl.Location = new System.Drawing.Point(567, 39);
            this.btn_VersionControl.Name = "btn_VersionControl";
            this.btn_VersionControl.Size = new System.Drawing.Size(29, 23);
            this.btn_VersionControl.TabIndex = 2;
            this.btn_VersionControl.Text = "...";
            this.btn_VersionControl.UseVisualStyleBackColor = true;
            this.btn_VersionControl.Click += new System.EventHandler(this.btn_VersionControl_Click);
            // 
            // btn_Update
            // 
            this.btn_Update.BackgroundImage = global::UpdateManager.Properties.Resources.icon_32x321;
            this.btn_Update.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_Update.Enabled = false;
            this.btn_Update.Location = new System.Drawing.Point(265, 68);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(75, 43);
            this.btn_Update.TabIndex = 3;
            this.btn_Update.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Update.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // chk_Disable
            // 
            this.chk_Disable.AutoSize = true;
            this.chk_Disable.Location = new System.Drawing.Point(12, 384);
            this.chk_Disable.Name = "chk_Disable";
            this.chk_Disable.Size = new System.Drawing.Size(222, 17);
            this.chk_Disable.TabIndex = 5;
            this.chk_Disable.Text = "Inhabilitar los plugins BBI desactualizados";
            this.chk_Disable.UseVisualStyleBackColor = true;
            this.chk_Disable.CheckedChanged += new System.EventHandler(this.chk_Disable_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "BBI Control Versiones|*.ver";
            this.openFileDialog1.Title = "Seleccionar el Control de Versiones";
            // 
            // UpdateManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 420);
            this.Controls.Add(this.chk_Disable);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.btn_VersionControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_VersionControl);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(628, 459);
            this.MinimumSize = new System.Drawing.Size(628, 459);
            this.Name = "UpdateManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = GetTiTleForm(); //"BBI Update Manager (version)";
            this.Load += new System.EventHandler(this.UpdateManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox txt_VersionControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_VersionControl;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.CheckBox chk_Disable;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}