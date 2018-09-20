using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace cshtmlCompress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.listView1.BeginUpdate();
                this.listView1.Items.Clear();
                this.listView1.EndUpdate();
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                DirectoryInfo di = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] dics = di.GetDirectories();//获取子文件夹列表
                FileInfo[] files = di.GetFiles();//获取文件列表
                if (dics != null && dics.Length > 0)
                {
                    foreach (DirectoryInfo d in dics)
                    {
                        System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(d.Name);
                        this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
                    }
                }
                if (files != null && files.Length > 0)
                {
                    foreach (FileInfo d in files)
                    {
                        System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(d.Name);
                        this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;
            if (!Directory.Exists(textBox1.Text))
            { return; }
            DirectoryInfo di = new DirectoryInfo(textBox1.Text);
            string path = di.FullName.Replace("\\" + di.Name,"") + "\\" + di.Name + "_bak";
            if (Directory.Exists(path))
            { Directory.Delete(path,true); }
            Directory.CreateDirectory(path);
            DirectoryInfo[] dics = di.GetDirectories();//获取子文件夹列表
            FileInfo[] files = di.GetFiles();//获取文件列表
            bool ok = true;
            if (files != null && files.Length > 0)
            {
                foreach (FileInfo f in files)
                {
                    if (!CompressHelper.compressor(f.FullName, path + "\\" + f.Name))
                    { ok = false; break; }
                }
            }
            if (dics != null && dics.Length > 0)
            {
                foreach (DirectoryInfo d in dics)
                {
                    if (!GetFiles(d, path)) { ok = false; break; }
                }
            }
            if (!ok) { MessageBox.Show("失败!"); return; } 
            MessageBox.Show("成功!"); 
        }
        private bool GetFiles(DirectoryInfo folder, string parentpath)
        {
            string path = parentpath + "\\" + folder.Name;
            if (Directory.Exists(path))
            { Directory.Delete(path); }
            Directory.CreateDirectory(path);
            FileInfo[] files = folder.GetFiles();
            if (files != null && files.Length > 0)
            {
                foreach (FileInfo f in files)
                {
                    if (!CompressHelper.compressor(f.FullName, path + "\\" + f.Name))
                    { return false; }
                }
            }
            DirectoryInfo[] dics = folder.GetDirectories();
            if (dics != null && dics.Length > 0)
            {
                foreach (DirectoryInfo d in dics)
                {
                    if (!GetFiles(d, path)) { return false; }
                }
            }
            return true;
        }
    }
}
