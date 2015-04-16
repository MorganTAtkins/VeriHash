using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Security;
using System.Security.AccessControl;



namespace VeriHash
{
    public partial class VeriHash : Form
    {
        string copiedHash01;
        string hashValueString;

        public VeriHash()
        {
            InitializeComponent();

            RegistrySecurity rs = new RegistrySecurity();

            // Allow the current user to read and delete the key. 
            //
            rs.AddAccessRule(new RegistryAccessRule(Environment.UserName,
                RegistryRights.WriteKey | RegistryRights.Delete,
                InheritanceFlags.None,
                PropagationFlags.None,
                AccessControlType.Allow));




            const string MenuName = "Folder\\shell\\NewMenuOption";
            const string Command = "Folder\\shell\\NewMenuOption\\command";

            //File.SetAccessControl(MenuName, rs);
            //File.SetAccessControl(Command, rs);

            RegistryKey root = null;
            RegistryKey rk = null;
            try
            {
                //RegistryKey root;
                //RegistryKey rk;

                root = Registry.LocalMachine;
                rk = root.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ShellExecuteHooks", true);
                rk.SetValue("CLSID", ".Net ISO 8601 Date Parser Shell Extension");
                rk.Close();
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.ToString());
            }
        } 
                
        public string GetHash(string file)
        {
            
            try
            {
                byte[] hashValue;
                MD5 md5HashObj = MD5.Create();

                Console.WriteLine("Its a file");
                // Create a fileStream for the file.
                FileStream fileStream = new FileStream(file,FileMode.Open);
                // select the position of the filestream.
                fileStream.Position = 0;
                // Compute the hash of the fileStream.
                hashValue = md5HashObj.ComputeHash(fileStream);
                //instantiate a string builder 
                StringBuilder sBuilder = new StringBuilder();
                // Loop through each hash value bytes formatting each one as a hexadecimal string. 
                for (int i = 0; i < hashValue.Length; i++)
                {
                    sBuilder.Append(hashValue[i].ToString("x2"));
                }
                hashValueString = Convert.ToString(sBuilder);
                // Write the hash value to the Console.
                Debug.WriteLine("MD5 hash: " + hashValueString);
                //Close the file.
                fileStream.Close();   

            }
            catch (IOException e)
            {
                Debug.WriteLine(e);
                MessageBox.Show(e.ToString());
            }

            return hashValueString;
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == Copied.Text)
            {
                label3.Text = "Match";
                label3.ForeColor = System.Drawing.Color.Green;
            }
            else 
            {
                label3.Text = "No match";
                label3.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void Copied_TextChanged(object sender, EventArgs e)
        {
            copiedHash01 = Copied.Text;
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path;
            textBox1.Enabled = true;
            Path.Enabled = true;

            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                Path.Text = file.FileName;
                path = file.FileName.ToString();
                textBox1.Text = GetHash(path);
            }
            


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
