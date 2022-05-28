using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Mega_Mix_Mod_Manager.Objects;
using Mega_Mix_Mod_Manager.IO;
using YamlDotNet.Serialization;
using Mega_Mix_Mod_Manager.Lite_Merge;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Mega_Mix_Mod_Manager.GUI.Controls
{
    public partial class ModInstaller : UserControl
    {
        
        public ModInstaller()
        {
            InitializeComponent();
            InitialLoad();
        }
        public ModList installedmodList;

        private void InitialLoad()
        {
            if (File.Exists($"{Global.MainSettings.Mods_Folder}\\Modlist.yaml"))
                LoadModList();
            else
                installedmodList = new ModList();
        }

        #region UI Funtions
        private void B_RemoveMod_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                return;
            installedmodList.mods.Remove(installedmodList.mods.Where(x => x.hash == TV_ModList.SelectedNode.Name).First());
            PB_ModPreview.Image.Dispose();
            PB_ModPreview.Image = Properties.Resources.Logo;
            RTB_ModDetails.Clear();
            Directory.Delete($"{Global.MainSettings.Mods_Folder}\\{TV_ModList.SelectedNode.Name}", true);
            TV_ModList.SelectedNode.Remove();

            WriteModList();
            if (Global.MainSettings.Merge_Option <= (Enums.MergeOptions)1)
            {
                PB_InstallProgress.Visible = true;
                PB_InstallProgress.Value = 0;
                MergeMods();
                PB_InstallProgress.Visible = false;
                PB_InstallProgress.Value = 0;
            }

            TV_ModList.Focus();
        }

        private void B_ToggleMod_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                return;


            if (TV_ModList.SelectedNode.Checked)
            {

                TV_ModList.SelectedNode.Checked = false;
                MB_Disable.Text = "Enable";
            }
            else
            {

                TV_ModList.SelectedNode.Checked = true;
                MB_Disable.Text = "Disable";
            }
        }
        private void TV_ToggleMod_Click(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                installedmodList.mods.Where(x => x.hash == e.Node.Name).First().Enabled = true;
                e.Node.ForeColor = Color.Black;
            }
            else
            {
                installedmodList.mods.Where(x => x.hash == e.Node.Name).First().Enabled = false;
                e.Node.ForeColor = Color.Red;
            }
            WriteModList();
        }

        private void B_ModUp_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null || TV_ModList.SelectedNode.Index <= 0)
            {
                TV_ModList.Focus();
                return;
            }

            int currentIndex = TV_ModList.SelectedNode.Index;
            installedmodList.MoveUp(currentIndex);
            TreeViewAddon.MoveUp(TV_ModList.SelectedNode);
            WriteModList();
            TV_ModList.SelectedNode = TV_ModList.Nodes[currentIndex - 1];
            TV_ModList.Focus();
        }

        private void B_ModDown_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null || TV_ModList.SelectedNode.Index >= installedmodList.mods.Count - 1)
            {
                TV_ModList.Focus();
                return;
            }

            int currentIndex = TV_ModList.SelectedNode.Index;
            installedmodList.MoveDown(currentIndex);
            TreeViewAddon.MoveDown(TV_ModList.SelectedNode);
            WriteModList();
            TV_ModList.SelectedNode = TV_ModList.Nodes[currentIndex + 1];
            TV_ModList.Focus();
        }

        private void B_ClearMods_Click(object sender, EventArgs e)
        {
            installedmodList = new ModList();
            if (PB_ModPreview.Image != null)
            {
                PB_ModPreview.Image.Dispose();
                PB_ModPreview.Image = Properties.Resources.Logo;
            }
            RTB_ModDetails.Clear();
            TV_ModList.Nodes.Clear();
            Directory.Delete($"{Global.MainSettings.Mods_Folder}", true);
            Directory.CreateDirectory($"{Global.MainSettings.Mods_Folder}");
        }

        private void TV_ModList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string ModPath = $"{Global.MainSettings.Mods_Folder}\\{TV_ModList.SelectedNode.Name}";
            if (!TV_ModList.SelectedNode.Checked)
            {
                MB_Disable.Text = "Enable";
            }

            if (File.Exists($"{ModPath}\\thumbnail.jpg"))
            {
                Image img = new Bitmap($"{ModPath}\\thumbnail.jpg");
                PB_ModPreview.Image = img;
                //img.Dispose();
            }

            string yaml = File.ReadAllText($"{ModPath}\\modinfo.yaml");
            var deserializer = new DeserializerBuilder().Build();
            ModInfo modinfo = deserializer.Deserialize<ModInfo>(yaml);

            RTB_ModDetails.Text = $"Name: {modinfo.Name}\nAuthor: {modinfo.Author}\n{modinfo.Description}";
        }

        private void B_OpenMod_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                System.Diagnostics.Process.Start("explorer.exe", $"{Global.MainSettings.Mods_Folder}");
            else
                System.Diagnostics.Process.Start("explorer.exe", $"{Global.MainSettings.Mods_Folder}\\{TV_ModList.SelectedNode.Name}");
        }

        private void TV_ModList_MouseUp(object sender, MouseEventArgs e)
        {
            var clickedNode = TV_ModList.GetNodeAt(e.X, e.Y);
            if (clickedNode == null)
            {
                //clicked on background
                TV_ModList.SelectedNode = null;
                if (PB_ModPreview.Image != null)
                {
                    PB_ModPreview.Image.Dispose();
                    PB_ModPreview.Image = Properties.Resources.Logo;
                }
                RTB_ModDetails.Clear();
                MB_Disable.Text = "Disable";
            }
            else
            {
                //clicked on node
                return;
            }
        }

        private void B_OpenDump_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", $"{Global.MainSettings.Game_Dump}");
        }

        private void B_OpenExport_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", $"{Global.MainSettings.Export_Path}");
        }

        private void B_OpenStaging_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", $"{Global.MainSettings.Mods_Folder}");
        }
        #endregion

        private void MergeMods()
        {
            if ((Global.MainSettings.obj_Merge == "No Merge" ||
                Global.MainSettings.tex_Merge == "No Merge" ||
                Global.MainSettings.spr_Merge == "No Merge" ||
                Global.MainSettings.pv_Merge == "No Merge"))
            {
                MessageBox.Show("No valid game path found, merging will be skipped", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                if (Directory.Exists($"{Global.MainSettings.Mods_Folder}\\Merged"))
                    Directory.Delete($"{Global.MainSettings.Mods_Folder}\\Merged", true);
                Directory.CreateDirectory($"{Global.MainSettings.Mods_Folder}\\Merged");
                string[] Filters = new string[] { "obj_db.bin", "obj_db.yaml", "tex_db.bin", "tex_db.yaml", "spr_db.bin", "spr_db.yaml", "pv_db.txt" };

                List<string> Files = new List<string>();
                List<string> Farcs = new List<string>();

                foreach (TreeNode node in TV_ModList.Nodes)
                {
                    if (!node.Checked)
                        continue;
                    string[] files = Filters.SelectMany(x => Directory.GetFiles($"{Global.MainSettings.Mods_Folder}\\{node.Name}", x, SearchOption.AllDirectories)).ToArray();
                    Files.AddRange(files);

                    string[] farcs = Directory.GetFiles($"{Global.MainSettings.Mods_Folder}\\{node.Name}", "*.farc", SearchOption.AllDirectories);
                    Farcs.AddRange(farcs);
                }

                if (Global.MainSettings.pv_Merge == "Lite Merge")
                {
                    PB_InstallProgress.Value = 5;
                    string[] files = Files.Where(x => x.Contains("pv_db.txt")).ToArray();
                    if (files.Length > 0)
                        pv_db.MergeMods(files, $"{Global.MainSettings.Game_Dump}\\rom_steam_region\\rom\\pv_db.txt", $"{Global.MainSettings.Mods_Folder}\\Merged\\rom_steam\\rom\\pv_db.txt");
                    PB_InstallProgress.Value = 10;
                }
                else if (Global.MainSettings.pv_Merge == "Deep Merge")
                {
                    PB_InstallProgress.Value = 5;
                    string[] files = Files.Where(x => x.Contains("pv_db.txt")).ToArray();
                    if (files.Length > 0)
                        DeepMerge.pv_db.Read(Path.GetFullPath(files[0]));
                    PB_InstallProgress.Value = 10;
                }
                if (Global.MainSettings.obj_Merge != "No Merge")
                {
                    PB_InstallProgress.Value = 15;
                    string[] files = Files.Where(x => x.Contains("obj_db")).ToArray();
                    if (files.Length > 0)
                        obj_db.Merge($"{Global.MainSettings.Game_Dump}\\rom_steam\\rom\\objset\\obj_db.bin", files, $"{Global.MainSettings.Mods_Folder}\\Merged\\rom_steam\\rom\\objset\\obj_db.bin");
                    PB_InstallProgress.Value = 20;
                }
                if (Global.MainSettings.tex_Merge != "No Merge")
                {
                    PB_InstallProgress.Value = 25;
                    string[] files = Files.Where(x => x.Contains("tex_db")).ToArray();
                    if (files.Length > 0)
                        tex_db.Merge($"{Global.MainSettings.Game_Dump}\\rom_steam\\rom\\objset\\tex_db.bin", files, $"{Global.MainSettings.Mods_Folder}\\Merged\\rom_steam\\rom\\objset\\tex_db.bin");
                    PB_InstallProgress.Value = 30;
                }
                if (Global.MainSettings.spr_Merge != "No Merge")
                {
                    PB_InstallProgress.Value = 35;
                    string region = Enum.GetName(typeof(Enums.Region), installedmodList.region);
                    string[] files = Files.Where(x => x.Contains("spr_db")).ToArray();
                    if (files.Length > 0)
                        spr_db.Merge($"{Global.MainSettings.Game_Dump}\\{region}\\rom\\2d\\spr_db.bin", files, $"{Global.MainSettings.Mods_Folder}\\Merged\\{region}\\rom\\2d\\spr_db.bin");
                    PB_InstallProgress.Value = 40;
                }
                if (Global.MainSettings.farc_Merge != "No Merge")
                {
                    PB_InstallProgress.Value = 45;
                    farc.Merge(Global.MainSettings.Game_Dump, Farcs.ToArray(), $"{Global.MainSettings.Mods_Folder}\\Merged", installedmodList.region);
                    PB_InstallProgress.Value = 50;
                }
            }
        }

        private void TS_MergeMods_Click(object sender, EventArgs e)
        {
            PB_InstallProgress.Visible = true;
            PB_InstallProgress.Value = 0;
            MergeMods();
            PB_InstallProgress.Value = 100;
            PB_InstallProgress.Value = 0;
            PB_InstallProgress.Visible = false;
        }
        private void B_InstallMod_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                //ofd.Filter = "Archive Files|*.zip;*.rar;*.7z|Zip Files|*.zip|Rar File|*.rar|7zip File|*.7z";
                ofd.Filter = "Supported Files(*.zip, *.MikuMod)|*.zip;*.MikuMod|Zip Files(*.zip)|*.zip|Miku Mod(*.MikuMod)|*.MikuMod";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        byte[] data = File.ReadAllBytes(file);
                        string hash = Hash.HashFile(data);
                        if (Directory.Exists($"{Global.MainSettings.Mods_Folder}\\{hash}"))
                        {
                            //check to see if the mod is already installed
                            string message = "Mod with the same hash ID is already installed, would you like to reinstall it?";
                            if (MessageBox.Show(message, "Mod Already Installed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                continue;
                            else
                            {
                                PB_ModPreview.Image.Dispose();
                                PB_ModPreview.Image = Properties.Resources.Logo;
                                RTB_ModDetails.Clear();
                                Directory.Delete($"{Global.MainSettings.Mods_Folder}\\{hash}", true);
                            }
                        }
                        Directory.CreateDirectory($"{Global.MainSettings.Mods_Folder}\\{hash}");


                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            using (ZipArchive mod = new ZipArchive(ms, ZipArchiveMode.Read, false))
                            {
                                mod.ExtractToDirectory($"{Global.MainSettings.Mods_Folder}\\{hash}");
                                mod.Dispose();
                            }
                        }
                        ModInfo modinfo = new ModInfo();

                        if (!File.Exists($"{Global.MainSettings.Mods_Folder}\\{hash}\\modinfo.yaml"))
                        {
                            modinfo.Name = Path.GetFileNameWithoutExtension(file);
                            modinfo.Description = "No info found";
                            modinfo.Author = "Unknown";
                        }
                        else
                        {
                            string yaml = File.ReadAllText($"{Global.MainSettings.Mods_Folder}\\{hash}\\modinfo.yaml");
                            var deserializer = new DeserializerBuilder().Build();
                            modinfo = deserializer.Deserialize<ModInfo>(yaml);
                        }

                        ModList.ModList_Entry newmod = new ModList.ModList_Entry() { Name = modinfo.Name, hash = hash };

                        if (!installedmodList.mods.Contains(installedmodList.mods.Where(mod => mod.hash == newmod.hash).FirstOrDefault()))
                        {
                            installedmodList.mods.Add(newmod);
                            TV_ModList.Nodes.Add(hash, modinfo.Name);
                        }
                        WriteModList();
                    }
                    if ((int)Global.MainSettings.Merge_Option <= 1)
                    {
                        PB_InstallProgress.Visible = true;
                        PB_InstallProgress.Value = 0;
                        MergeMods();
                        PB_InstallProgress.Visible = false;
                        PB_InstallProgress.Value = 0;
                    }

                    TV_ModList.SelectedNode = TV_ModList.Nodes[TV_ModList.Nodes.Count - 1];
                    TV_ModList.SelectedNode.Checked = true;
                    TV_ModList.Focus();
                }
            }
        }

        private void B_ExportMods_Click(object sender, EventArgs e)
        {
            if (Global.MainSettings.Export_Path == null || Global.MainSettings.Export_Path.Length == 0)
                return;
            if (Directory.Exists($"{Global.MainSettings.Export_Path}"))
                Directory.Delete($"{Global.MainSettings.Export_Path}", true);
            Directory.CreateDirectory($"{Global.MainSettings.Export_Path}");
            PB_InstallProgress.Visible = true;
            PB_InstallProgress.Value = 0;
            if ((int)Global.MainSettings.Merge_Option == 0 || (int)Global.MainSettings.Merge_Option == 2)
                MergeMods();

            PB_InstallProgress.Value = 60;
            foreach (TreeNode node in TV_ModList.Nodes)
            {
                if (!node.Checked)
                    continue;
                string[] files = Directory.GetFiles($"{Global.MainSettings.Mods_Folder}\\{node.Name}", "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (Path.GetFileName(file) == "modinfo.yaml" || Path.GetFileName(file) == "thumbnail.jpg" || Path.GetDirectoryName(file).Contains("Log"))
                        continue;

                    string outfile = file.Replace($"{Global.MainSettings.Mods_Folder}\\{node.Name}", "");
                    outfile = Regex.Replace(outfile, "romfs", "", RegexOptions.IgnoreCase).Replace("\\\\", "\\");
                    if (!Directory.Exists(Path.GetDirectoryName($"{Global.MainSettings.Export_Path}\\{outfile}")))
                        Directory.CreateDirectory(Path.GetDirectoryName($"{Global.MainSettings.Export_Path}\\{outfile}"));
                    File.Copy(file, $"{Global.MainSettings.Export_Path}\\{outfile}", true);
                }
            }

            if (Directory.Exists($"{Global.MainSettings.Mods_Folder}\\Merged"))
            {
                PB_InstallProgress.Value = 70;
                string[] mergedFiles = Directory.GetFiles($"{Global.MainSettings.Mods_Folder}\\Merged", "*", SearchOption.AllDirectories);
                foreach (string file in mergedFiles)
                {
                    if (Path.GetDirectoryName(file).Contains(".farc"))
                        continue;

                    string outfile = file.Replace($"{Global.MainSettings.Mods_Folder}\\Merged", "");
                    if (!Directory.Exists(Path.GetDirectoryName($"{Global.MainSettings.Export_Path}\\{outfile}")))
                        Directory.CreateDirectory(Path.GetDirectoryName($"{Global.MainSettings.Export_Path}\\{outfile}"));
                    File.Copy(file, $"{Global.MainSettings.Export_Path}\\{outfile}", true);
                }
                PB_InstallProgress.Value = 75;

                PB_InstallProgress.Value = 80;
                mergedFiles = Directory.GetDirectories($"{Global.MainSettings.Mods_Folder}\\Merged", "*.farc", SearchOption.AllDirectories);
                foreach (string file in mergedFiles)
                {
                    farc.PackFarc(file, $"{Global.MainSettings.Mods_Folder}\\Merged", $"{Global.MainSettings.Export_Path}");
                }
                PB_InstallProgress.Value = 85;
            }

            PB_InstallProgress.Value = 100;
            MessageBox.Show("Mods Exported Successfully", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            PB_InstallProgress.Value = 0;
            PB_InstallProgress.Visible = false;

        }

        public void WriteModList()
        {
            if (File.Exists($"{Global.MainSettings.Mods_Folder}\\Modlist.yaml"))
                File.Delete($"{Global.MainSettings.Mods_Folder}\\Modlist.yaml");

            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(installedmodList);
            File.WriteAllText($"{Global.MainSettings.Mods_Folder}\\Modlist.yaml", yaml);
        }

        public void LoadModList()
        {
            if (File.Exists($"{Global.MainSettings.Mods_Folder}\\Modlist.yaml"))
            {
                string yaml = File.ReadAllText($"{Global.MainSettings.Mods_Folder}\\Modlist.yaml");
                var deserializer = new DeserializerBuilder().Build();
                ModList modlist = deserializer.Deserialize<ModList>(yaml);
                installedmodList = modlist;

                foreach (ModList.ModList_Entry mod in modlist.mods)
                {
                    TV_ModList.Nodes.Add(mod.hash, mod.Name);
                    if (!mod.Enabled)
                        TV_ModList.Nodes[mod.hash].ForeColor = Color.Red;
                    else
                        TV_ModList.Nodes[mod.hash].Checked = true;
                }
            }
        }
    }
}
