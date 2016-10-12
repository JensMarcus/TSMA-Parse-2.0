using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tsm.Accounting.Parse
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            this.txtInputFile.Text = TSMAParse.Properties.Settings.Default.TSMAccountingFile;
            this.txtOutputFile.Text = TSMAParse.Properties.Settings.Default.OutputFile;
        }

        private void cmdParse_Click(object sender, EventArgs e)
        {
            SavedVariables currentDB = new SavedVariables();
            string inputFileName = string.Empty;
            string outputFileName = string.Empty;

            inputFileName = this.txtInputFile.Text;
            outputFileName = this.txtOutputFile.Text;

            try
            {
                currentDB.Load(inputFileName);
                currentDB.WriteToFile(outputFileName);

                MessageBox.Show("TSM Accounting file successfully parsed");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "lua files (*.lua)|*.lua|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtInputFile.Text = openFileDialog1.FileName;
            }
        }

        private void cmdOutputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.AddExtension = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtOutputFile.Text = openFileDialog1.FileName;
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            TSMAParse.Properties.Settings.Default.TSMAccountingFile = this.txtInputFile.Text;
            TSMAParse.Properties.Settings.Default.OutputFile = this.txtOutputFile.Text;
            TSMAParse.Properties.Settings.Default.Save();
        }
    }
}
