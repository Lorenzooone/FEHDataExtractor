using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FEHDataExtractor
{
    public partial class Form1 : Form
    {
        private ExtractionBase[] a;
        public static int offset = 0x20;
        private String Path;
        private String[] Pathes;
        private String MessagePath;

        public ExtractionBase[] A { get => a; set => a = value; }

        public Form1(params ExtractionBase[] a)
        {
            this.A = a;
            InitializeComponent();
            for (int i = 0; i < A.Length; i++)
            {
                this.comboBox1.Items.Add(A[i].Name);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Path = openFileDialog1.FileName;
                Pathes = openFileDialog1.FileNames;

                FileListBox.Items.Clear();

                foreach (String file in Pathes)
                {
                    FileListBox.Items.Add(file);
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Path = "";
            openFileDialog1.FileName = "";
            openFileDialog1.Reset();
            FileListBox.Items.Clear();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("https://github.com/Cass07/FEHDataExtractor", "About");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Path != null && Path != "" && !(comboBox1.SelectedItem == null || comboBox1.SelectedItem.ToString().Equals("")))
            {
                ExtractionBase tmp = null;
                for (int i = 0; i < A.Length; i++)
                    if (comboBox1.SelectedItem.ToString().Equals(A[i].Name))
                        tmp = A[i];
                foreach (String file in Pathes)
                {
                    string ext = System.IO.Path.GetExtension(file).ToLower();
                    byte[] data = Decompression.Open(file);
                    String output = "";

                    if (data != null && tmp != null && !(tmp.Name.Equals("") || tmp.Name.Equals("Decompress")))
                    {
                        HSDARC a = new HSDARC(0, data);
                        while (a.Ptr_list_length - a.NegateIndex > a.Index)
                        {
                            tmp.InsertIn(a, offset, data);
                            output += tmp.ToString();
                        }
                    }

                    String PathManip = file.Remove(file.Length - 3, 3);
                    if (ext.Equals(".lz"))
                        PathManip = file.Remove(file.Length - 6, 6);
                    PathManip += tmp.Name.Equals("Decompress") ? "bin" : "txt";
                    if (file.Equals(PathManip))
                        PathManip += tmp.Name.Equals("Decompress") ? ".bin" : ".txt";
                    if (tmp.Name.Equals("Decompress") && data != null)
                        File.WriteAllBytes(PathManip, data);
                    else
                        File.WriteAllText(PathManip, output);
                }
                MessageBox.Show(Pathes.Length > 1 ? "Files processed!" : "File processed!", "Success");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void messagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                MessagePath = folderBrowserDialog1.SelectedPath;
                LoadMessages.openFolder(MessagePath);
                MessageBox.Show("Loaded messages!", "Success");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Path != null && Path != "" && !(comboBox1.SelectedItem == null || comboBox1.SelectedItem.ToString().Equals("")))
            {
                ExtractionBase tmp = null;
                for (int i = 0; i < A.Length; i++)
                    if (comboBox1.SelectedItem.ToString().Equals(A[i].Name))
                        tmp = A[i];
                foreach (String file in Pathes)
                {
                    string ext = System.IO.Path.GetExtension(file).ToLower();
                    byte[] data = Decompression.Open(file);
                    String output = "";


                    if (data != null && tmp != null && !(tmp.Name.Equals("") || tmp.Name.Equals("Decompress")))
                    {
                        HSDARC a = new HSDARC(0, data);
                        while (a.Ptr_list_length - a.NegateIndex > a.Index)
                        {
                            if (!tmp.Name.Equals("Messages"))
                            {
                                tmp.InsertIn(a, 0, data);
                            }
                            else
                                tmp.InsertIn(a, offset, data);
                            output += tmp.ToString_json();
                        }
                    }
                    output = "[" + output.Substring(0, output.Length - 1) + "]";

                    String PathManip = file.Remove(file.Length - 3, 3);
                    if (ext.Equals(".lz"))
                        PathManip = file.Remove(file.Length - 6, 6);
                    PathManip += tmp.Name.Equals("Decompress") ? "bin" : "json";
                    if (file.Equals(PathManip))
                        PathManip += tmp.Name.Equals("Decompress") ? ".bin" : ".json";
                    if (tmp.Name.Equals("Decompress") && data != null)
                        File.WriteAllBytes(PathManip, data);
                    else if (tmp.Name.Equals("Messages") && data != null)
                        File.WriteAllBytes(PathManip, Encoding.UTF8.GetBytes(output));
                    else
                        File.WriteAllText(PathManip, output);
                }
                MessageBox.Show(Pathes.Length > 1 ? "JSON Files processed!" : "JSON File processed!", "Success");
            }
        }


        private void messagesJpnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                MessagePath = folderBrowserDialog1.SelectedPath;
                LoadMessages.openFolderJp(MessagePath);
                MessageBox.Show("Loaded JP messages!", "Success");
            }
        }

        private void File_DragDrop(object sender, DragEventArgs e)
        {
            string[] directoryName = (string[])e.Data.GetData(DataFormats.FileDrop);

            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            FileListBox.Items.Clear();
            Pathes = files;
            Path = Pathes[0];

            foreach (String file in Pathes)
            {
                FileListBox.Items.Add(file);
            }
        }

        private void File_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

    }
}
