using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace FindReplace
{
    public partial class Form1 : Form
    {
        string filter = "*.*";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filter =  ConfigurationSettings.AppSettings["filter"].ToString();
            ClearControls();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txtDir.Text = FBD.SelectedPath;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            txtDir.Text = "";
            txtFind.Text = "";
            txtReplace.Text = "";
            txtMessage.Text = "";
            btnBrowse.Focus();
        }

        //private void txtDir_Validating(object sender, CancelEventArgs e)
        //{
        //    if (txtDir.Text == "")
        //        Err.SetError(txtDir, txtDir.AccessibleName);
        //    else
        //        Err.SetError(txtDir, "");
        //}

        private void txtFind_Validating(object sender, CancelEventArgs e)
        {
            if (txtFind.Text == "")
                Err.SetError(txtFind, txtFind.AccessibleName);
            else
                Err.SetError(txtFind, "");
        }

        private void txtReplace_Validating(object sender, CancelEventArgs e)
        {
            if (txtReplace.Text == "")
                Err.SetError(txtReplace, txtReplace.AccessibleName);
            else
                Err.SetError(txtReplace, "");
        }

        private bool TestValidating()
        {
            bool res = true;

            if (txtDir.Text == "")
            {
                Err.SetError(txtDir, txtDir.AccessibleName);
                res = false;
            }
            else
                Err.SetError(txtDir, "");

            if (txtFind.Text == "")
            {
                Err.SetError(txtFind, txtFind.AccessibleName);
                res = false;
            }
            else
                Err.SetError(txtFind, "");

            if (txtReplace.Text == "")
            {
                Err.SetError(txtReplace, txtReplace.AccessibleName);
                res = false;
            }
            else
                Err.SetError(txtReplace, "");

            return res;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (TestValidating())
            {
                string[] files = Directory.GetFiles(txtDir.Text, filter, SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    try
                    {                        
                        string contents = File.ReadAllText(file);  
                                             
                        contents = contents.Replace(txtFind.Text, txtReplace.Text);
                        // Make files writable
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.WriteAllText(file, contents, Encoding.Unicode);                        
                    }
                    catch (Exception ex)
                    {
                        txtMessage.Text += ex.Message;                      
                    }
                }
                txtMessage.Text = "Replaced text in " + files.Count() + " file(s).";
            }
        }

    }
}
