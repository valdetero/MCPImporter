using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCPImporter.Business;
using MCPImporter.Business.Application;
using MCPImporter.Common.Entities;
using MCPImporter.Import;


namespace MCPImporter
{
    public partial class Form1 : Form
    {
        private IImporter _importer;

        delegate void InvokeDelegate();

        public Form1()
        {
            InitializeComponent();

            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx";
            openFileDialog1.FileName = "*.xlsx";
            openFileDialog1.DefaultExt = ".xlsx";
            openFileDialog1.Multiselect = false;
        }

        private async Task<bool> extractInformation(string filename)
        {
            var success = await _importer.ExtractInformation(filename);

            return success;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            _importer = new ExcelImporter();

            _importer.NumberOfPeopleImported = s => lblTotalImported.BeginInvoke(new InvokeDelegate(() => lblTotalImported.Text = s));

            lblTotalImported.BeginInvoke(new InvokeDelegate(() => lblTotalImported.Text = ""));
            progressBar1.BeginInvoke(new InvokeDelegate(() => progressBar1.Style = ProgressBarStyle.Marquee));

            openFileDialog1.ShowDialog();
        }

        private async void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var filename = openFileDialog1.FileName;

            _importer.GetConnectionString(filename);

            await extractInformation(filename);

            progressBar1.BeginInvoke(new InvokeDelegate(() => progressBar1.Style = ProgressBarStyle.Blocks));
        }
    }
}
