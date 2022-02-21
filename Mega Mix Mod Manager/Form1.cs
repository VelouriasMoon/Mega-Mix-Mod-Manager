using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.IO.Compression;
using System.Text.RegularExpressions;

using Microsoft.VisualBasic;
using Microsoft.WindowsAPICodePack.Dialogs;
using YamlDotNet.Serialization;

using Mega_Mix_Mod_Manager.IO;
using Mega_Mix_Mod_Manager.Lite_Merge;
using Mega_Mix_Mod_Manager.Objects;
using Mega_Mix_Mod_Manager.Editors;
using Mega_Mix_Mod_Manager.Editors.ModCreator;
using MikuMikuLibrary.Textures;
using MikuMikuLibrary.Textures.Processing;
using MikuMikuLibrary.IO;

namespace Mega_Mix_Mod_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MainLoad();
        }
        public string pv_db_Path;
        public ModList installedmodList;
        public PatchList installedPatchList;
        public VersionMap versionMap;

        public void MainLoad()
        {
            CB_farc_Merge.SelectedIndex = 0;
            CB_obj_Merge.SelectedIndex = 0;
            CB_pv_Merge.SelectedIndex = 0;
            CB_spr_Merge.SelectedIndex = 0;
            CB_tex_Merge.SelectedIndex = 0;
            CB_MergeWhen.SelectedIndex = 0;
            CB_Region.SelectedIndex = 1;
            if (!Directory.Exists($"{TB_ModStagePath.Text}"))
                Directory.CreateDirectory($"{TB_ModStagePath.Text}");
            LoadSettings();
            if (File.Exists($"{TB_ModStagePath.Text}\\Modlist.yaml"))
                LoadModList();
            else
                installedmodList = new ModList();
            if (File.Exists($"{TB_ModStagePath.Text}\\Patchlist.yaml"))
                LoadPatchList();
            else
                installedPatchList = new PatchList();
            if (TB_Default_Author.Text != null || TB_Default_Author.Text.Length != 0)
                TB_ModAuthor.Text = TB_Default_Author.Text;
        }

        #region Mod Install
        #region UI Funtions
        private void B_RemoveMod_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                return;
            installedmodList.mods.Remove(installedmodList.mods.Where(x => x.hash == TV_ModList.SelectedNode.Name).First());
            PB_ModPreview.Image.Dispose();
            PB_ModPreview.Image = Properties.Resources.Logo;
            RTB_ModDetails.Clear();
            Directory.Delete($"{TB_ModStagePath.Text}\\{TV_ModList.SelectedNode.Name}", true);
            TV_ModList.SelectedNode.Remove();

            WriteModList();
            if (CB_MergeWhen.SelectedIndex <= 1)
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
            Directory.Delete($"{TB_ModStagePath.Text}", true);
            Directory.CreateDirectory($"{TB_ModStagePath.Text}");
        }

        private void TV_ModList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string ModPath = $"{TB_ModStagePath.Text}\\{TV_ModList.SelectedNode.Name}";
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
                System.Diagnostics.Process.Start("explorer.exe", $"{TB_ModStagePath.Text}");
            else
                System.Diagnostics.Process.Start("explorer.exe", $"{TB_ModStagePath.Text}\\{TV_ModList.SelectedNode.Name}");
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
            System.Diagnostics.Process.Start("explorer.exe", $"{TB_DumpPath.Text}");
        }

        private void B_OpenExport_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", $"{TB_Export.Text}");
        }

        private void B_OpenStaging_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", $"{TB_ModStagePath.Text}");
        }
        #endregion

        private void MergeMods()
        {
            if (!CB_PathVarify.Checked &&
                (CB_obj_Merge.SelectedIndex > 1 ||
                CB_tex_Merge.SelectedIndex > 1 ||
                CB_spr_Merge.SelectedIndex > 1 ||
                CB_pv_Merge.SelectedIndex > 1))
            {
                MessageBox.Show("No valid game path found, merging will be skipped", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                if (Directory.Exists($"{TB_ModStagePath.Text}\\Merged"))
                    Directory.Delete($"{TB_ModStagePath.Text}\\Merged", true);
                Directory.CreateDirectory($"{TB_ModStagePath.Text}\\Merged");
                string[] Filters = new string[] { "obj_db.bin", "obj_db.yaml", "tex_db.bin", "tex_db.yaml", "spr_db.bin", "spr_db.yaml", "pv_db.txt" };

                List<string> Files = new List<string>();
                List<string> Farcs = new List<string>();

                foreach (TreeNode node in TV_ModList.Nodes)
                {
                    if (!node.Checked)
                        continue;
                    string[] files = Filters.SelectMany(x => Directory.GetFiles($"{TB_ModStagePath.Text}\\{node.Name}", x, SearchOption.AllDirectories)).ToArray();
                    Files.AddRange(files);

                    string[] farcs = Directory.GetFiles($"{TB_ModStagePath.Text}\\{node.Name}", "*.farc", SearchOption.AllDirectories);
                    Farcs.AddRange(farcs);
                }

                if (CB_pv_Merge.Text == "Lite Merge")
                {
                    PB_InstallProgress.Value = 5;
                    string[] files = Files.Where(x => x.Contains("pv_db.txt")).ToArray();
                    if (files.Length > 0)
                        pv_db.MergeMods(files, pv_db_Path, $"{TB_ModStagePath.Text}\\Merged\\rom_switch\\rom\\pv_db.txt");
                    PB_InstallProgress.Value = 10;
                }
                if (CB_obj_Merge.SelectedIndex > 0)
                {
                    PB_InstallProgress.Value = 15;
                    string[] files = Files.Where(x => x.Contains("obj_db")).ToArray();
                    if (files.Length > 0)
                        obj_db.Merge($"{TB_DumpPath.Text}\\rom_switch\\rom\\objset\\obj_db.bin", files, $"{TB_ModStagePath.Text}\\Merged\\rom_switch\\rom\\objset\\obj_db.bin");
                    PB_InstallProgress.Value = 20;
                }
                if (CB_tex_Merge.SelectedIndex > 0)
                {
                    PB_InstallProgress.Value = 25;
                    string[] files = Files.Where(x => x.Contains("tex_db")).ToArray();
                    if (files.Length > 0)
                        tex_db.Merge($"{TB_DumpPath.Text}\\rom_switch\\rom\\objset\\tex_db.bin", files, $"{TB_ModStagePath.Text}\\Merged\\rom_switch\\rom\\objset\\tex_db.bin");
                    PB_InstallProgress.Value = 30;
                }
                if (CB_spr_Merge.SelectedIndex > 0)
                {
                    PB_InstallProgress.Value = 35;
                    string region = Enum.GetName(typeof(Enums.Region), installedmodList.region);
                    string[] files = Files.Where(x => x.Contains("spr_db")).ToArray();
                    if (files.Length > 0)
                        spr_db.Merge($"{TB_DumpPath.Text}\\{region}\\rom\\2d\\spr_db.bin", files, $"{TB_ModStagePath.Text}\\Merged\\{region}\\rom\\2d\\spr_db.bin");
                    PB_InstallProgress.Value = 40;
                }
                if (CB_farc_Merge.SelectedIndex > 0)
                {
                    PB_InstallProgress.Value = 45;
                    farc.Merge(TB_DumpPath.Text, Farcs.ToArray(), $"{TB_ModStagePath.Text}\\Merged", installedmodList.region);
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
                        if (Directory.Exists($"{TB_ModStagePath.Text}\\{hash}"))
                            Directory.Delete($"{TB_ModStagePath.Text}\\{hash}", true);
                        Directory.CreateDirectory($"{TB_ModStagePath.Text}\\{hash}");
                        

                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            using (ZipArchive mod = new ZipArchive(ms, ZipArchiveMode.Read, false))
                            {
                                mod.ExtractToDirectory($"{TB_ModStagePath.Text}\\{hash}");
                                mod.Dispose();
                            }
                        }
                        ModInfo modinfo = new ModInfo();

                        if (!File.Exists($"{TB_ModStagePath.Text}\\{hash}\\modinfo.yaml"))
                        {
                            modinfo.Name = Path.GetFileNameWithoutExtension(file);
                            modinfo.Description = "No info found";
                            modinfo.Author = "Unknown";
                        }
                        else
                        {
                            string yaml = File.ReadAllText($"{TB_ModStagePath.Text}\\{hash}\\modinfo.yaml");
                            var deserializer = new DeserializerBuilder().Build();
                            modinfo = deserializer.Deserialize<ModInfo>(yaml);
                        }

                        ModList.ModList_Entry newmod = new ModList.ModList_Entry();

                        if (!installedmodList.mods.Contains(newmod))
                        {
                            installedmodList.mods.Add(new ModList.ModList_Entry() { Name = modinfo.Name, hash = hash });
                            TV_ModList.Nodes.Add(hash, modinfo.Name);
                        }
                        WriteModList();
                    }
                    if (CB_MergeWhen.SelectedIndex <= 1)
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
            if (TB_Export == null || TB_Export.Text.Length == 0)
                return;
            if (Directory.Exists($"{TB_Export.Text}\\romfs"))
                Directory.Delete($"{TB_Export.Text}\\romfs", true);
            Directory.CreateDirectory($"{TB_Export.Text}\\romfs");
            PB_InstallProgress.Visible = true;
            PB_InstallProgress.Value = 0;
            if (CB_MergeWhen.SelectedIndex == 0 || CB_MergeWhen.SelectedIndex == 2)
                MergeMods();

            PB_InstallProgress.Value = 60;
            foreach (TreeNode node in TV_ModList.Nodes)
            {
                if (!node.Checked)
                    continue;
                string[] files = Directory.GetFiles($"{TB_ModStagePath.Text}\\{node.Name}", "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (Path.GetFileName(file) == "modinfo.yaml" || Path.GetFileName(file) == "thumbnail.jpg" || Path.GetDirectoryName(file).Contains("Log"))
                        continue;

                    string outfile = file.Replace($"{TB_ModStagePath.Text}\\{node.Name}", "");
                    outfile = Regex.Replace(outfile, "romfs", "", RegexOptions.IgnoreCase).Replace("\\\\", "\\");
                    if (!Directory.Exists(Path.GetDirectoryName($"{TB_Export.Text}\\romfs\\{outfile}")))
                        Directory.CreateDirectory(Path.GetDirectoryName($"{TB_Export.Text}\\romfs\\{outfile}"));
                    File.Copy(file, $"{TB_Export.Text}\\romfs\\{outfile}", true);
                }
            }
            
            if (Directory.Exists($"{TB_ModStagePath.Text}\\Merged"))
            {
                PB_InstallProgress.Value = 70;
                string[] mergedFiles = Directory.GetFiles($"{TB_ModStagePath.Text}\\Merged", "*", SearchOption.AllDirectories);
                foreach (string file in mergedFiles)
                {
                    if (Path.GetDirectoryName(file).Contains(".farc"))
                        continue;

                    string outfile = file.Replace($"{TB_ModStagePath.Text}\\Merged", "");
                    if (!Directory.Exists(Path.GetDirectoryName($"{TB_Export.Text}\\romfs\\{outfile}")))
                        Directory.CreateDirectory(Path.GetDirectoryName($"{TB_Export.Text}\\romfs\\{outfile}"));
                    File.Copy(file, $"{TB_Export.Text}\\romfs\\{outfile}", true);
                }
                PB_InstallProgress.Value = 75;

                PB_InstallProgress.Value = 80;
                mergedFiles = Directory.GetDirectories($"{TB_ModStagePath.Text}\\Merged", "*.farc", SearchOption.AllDirectories);
                foreach (string file in mergedFiles)
                {
                    farc.PackFarc(file, $"{TB_ModStagePath.Text}\\Merged", $"{TB_Export.Text}\\romfs");
                }
                PB_InstallProgress.Value = 85;
            }

            if (installedPatchList.Patches.Count > 0)
            {
                PB_InstallProgress.Value = 90;
                ExportPacthes();
                PB_InstallProgress.Value = 95;
            }

            PB_InstallProgress.Value = 100;
            MessageBox.Show("Mods Exported Successfully", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            PB_InstallProgress.Value = 0;
            PB_InstallProgress.Visible = false;
            
        }
        

        #endregion

        #region Mod Creator
        private void B_ModPath_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog cofd = new CommonOpenFileDialog())
            {
                cofd.IsFolderPicker = true;
                cofd.RestoreDirectory = true;

                if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    TB_ModPath.Text = cofd.FileName;
                    LoadModInfo();
                }
            }
        }

        private void B_ImgPreview_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Bitmap bmp = new Bitmap(ofd.FileName);
                    }
                    catch
                    {
                        return;
                    }
                    PB_ModCreateImg.Image = new Bitmap(ofd.FileName);
                    TB_ImgPreview.Text = ofd.FileName;
                }
            }
        }

        private void B_CreateMod_Click(object sender, EventArgs e)
        {
            //verify check
            if (TB_ModPath.Text == null || TB_ModPath.Text.Length == 0)
            {
                MessageBox.Show("Mod Path Missing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (TB_ModName.Text == null || TB_ModName.Text.Length == 0)
            {
                MessageBox.Show("Mod Name Missing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ModInfoCreator creator = new ModInfoCreator()
            {
                Name = TB_ModName.Text,
                Author = TB_ModAuthor.Text,
                Description = RTB_ModDescription.Text,
                Path = TB_ModPath.Text,
                Img = PB_ModCreateImg.Image == null ? null : PB_ModCreateImg.Image,
                DumpPath = TB_DumpPath.Text,
                Region = Enum.GetName(typeof(Enums.Region), installedmodList.region),
            };

            Creator.MakeMod(creator);
        }

        private void B_ModCreatorClear_Click(object sender, EventArgs e)
        {
            TB_ModPath.Text = null;
            TB_ImgPreview.Text = null;
            TB_ModName.Text = null;
            RTB_ModDescription.Text = null;
            PB_ModCreateImg.Image.Dispose();
            PB_ModCreateImg.Image = null;
        }

        #endregion

        #region Settings
        private void B_GamePath_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog cofd = new CommonOpenFileDialog())
            {
                cofd.IsFolderPicker = true;
                cofd.RestoreDirectory = true;

                if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    TB_DumpPath.Text = cofd.FileName;
                }
            }
        }

        private void B_ExportPath_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog cofd = new CommonOpenFileDialog())
            {
                cofd.IsFolderPicker = true;
                cofd.RestoreDirectory = true;

                if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    TB_Export.Text = cofd.FileName;
                    if (TB_Export.Text.EndsWith("romfs"))
                        TB_Export.Text = TB_Export.Text.Remove(TB_Export.Text.Length - 6, 6);
                }
            }
        }

        private void B_ModsPath_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog cofd = new CommonOpenFileDialog())
            {
                cofd.IsFolderPicker = true;
                cofd.RestoreDirectory = true;

                if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (cofd.FileName == TB_ModStagePath.Text || cofd.FileName.Replace(Directory.GetCurrentDirectory(), ".") == TB_ModStagePath.Text)
                    {
                        return;
                    }

                    string OldPath = TB_ModStagePath.Text;
                    if (cofd.FileName.Contains(Directory.GetCurrentDirectory()))
                        TB_ModStagePath.Text = cofd.FileName.Replace(Directory.GetCurrentDirectory(), ".");
                    else
                        TB_ModStagePath.Text = cofd.FileName;
                    ChangeModStagingPath(OldPath, TB_ModStagePath.Text);
                }
            }
        }

        private void ChangeModStagingPath(string OldPath, string NewPath)
        {
            string[] Files = Directory.GetFiles(OldPath, "*", SearchOption.AllDirectories);
            TV_ModList.SelectedNode = null;
            PB_ModPreview.Image.Dispose();
            PB_ModPreview.Image = Properties.Resources.Logo;
            RTB_ModDetails.Clear();
            foreach (string file in Files)
            {
                if (!Directory.Exists(Path.GetDirectoryName(file.Replace(OldPath, NewPath))))
                    Directory.CreateDirectory(Path.GetDirectoryName(file.Replace(OldPath, NewPath)));
                File.Copy(file, file.Replace(OldPath, NewPath), true);
                File.Delete(file);
            }
            processDirectory(OldPath);
            WriteSettings();
        }
        private void processDirectory(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                processDirectory(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private void TB_DumpPath_TextChanged(object sender, EventArgs e)
        {
            string basepath = $"{TB_DumpPath.Text}\\rom_switch\\rom";
            if (File.Exists($"{basepath}\\pv_db.txt") && File.Exists($"{basepath}\\objset\\obj_db.bin") && File.Exists($"{basepath}\\objset\\tex_db.bin"))
            {
                if (File.Exists($"{ TB_DumpPath.Text}\\rom_switch_en\\rom\\2d\\spr_db.bin"))
                    CB_Region.SelectedIndex = (int)Enums.Region.rom_switch_en;
                else
                    CB_Region.SelectedIndex = (int)Enums.Region.rom_switch;

                pv_db_Path = $"{basepath}\\pv_db.txt";
                CB_PathVarify.Checked = true;
            }
            else
                return;
        }

        private void B_SaveSettings_Click(object sender, EventArgs e)
        {
            WriteSettings();
        }

        public void WriteModInfo()
        {
            ModInfo modinfo = new ModInfo();
            modinfo.Name = TB_ModName.Text;
            modinfo.Description = RTB_ModDescription.Text;
            modinfo.Author = TB_ModAuthor.Text;

            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(modinfo);
            File.WriteAllText($"{TB_ModPath.Text}\\.temp\\modinfo.yaml", yaml);
        }

        public void LoadModInfo()
        {
            if (File.Exists($"{TB_ModPath.Text}\\modinfo.yaml"))
            {
                string yaml = File.ReadAllText($"{TB_ModPath.Text}\\modinfo.yaml");
                var deserializer = new DeserializerBuilder().Build();
                ModInfo modinfo = deserializer.Deserialize<ModInfo>(yaml);

                TB_ModName.Text = modinfo.Name;
                TB_ModAuthor.Text = modinfo.Author;
                RTB_ModDescription.Text = modinfo.Description;
            }
            if (File.Exists($"{TB_ModPath.Text}\\thumbnail.jpg"))
            {
                TB_ImgPreview.Text = $"{TB_ModPath.Text}\\thumbnail.jpg";
                PB_ModCreateImg.Image = new Bitmap($"{TB_ModPath.Text}\\thumbnail.jpg");
            }
        }

        public void WriteSettings()
        {
            Settings settings = new Settings();
            settings.Game_Dump = TB_DumpPath.Text;
            settings.Export_Path = TB_Export.Text;
            if (TB_ModStagePath.Text.Length == 0)
                settings.Mods_Folder = ".\\Mods";
            else
                settings.Mods_Folder = TB_ModStagePath.Text;
            settings.Default_Author = TB_Default_Author.Text;
            settings.pv_Merge = CB_pv_Merge.Text;
            settings.obj_Merge = CB_obj_Merge.Text;
            settings.spr_Merge = CB_spr_Merge.Text;
            settings.tex_Merge = CB_tex_Merge.Text;
            settings.farc_Merge = CB_farc_Merge.Text;
            settings.Merge_Option = (Enums.MergeOptions)CB_MergeWhen.SelectedIndex;
            settings.region = (Enums.Region)CB_Region.SelectedIndex;
            settings.Version = CB_Version.SelectedIndex;

            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(settings);
            File.WriteAllText($"settings.yaml", yaml);
        }

        public void LoadSettings()
        {
            if (File.Exists($"settings.yaml"))
            {
                string yaml = File.ReadAllText($"settings.yaml");
                var deserializer = new DeserializerBuilder().Build();
                Settings setting = deserializer.Deserialize<Settings>(yaml);

                TB_DumpPath.Text = setting.Game_Dump;
                TB_Export.Text = setting.Export_Path;
                TB_ModStagePath.Text = setting.Mods_Folder;
                TB_Default_Author.Text = setting.Default_Author;
                CB_pv_Merge.Text = setting.pv_Merge;
                CB_obj_Merge.Text = setting.obj_Merge;
                CB_spr_Merge.Text = setting.spr_Merge;
                CB_tex_Merge.Text = setting.tex_Merge;
                CB_farc_Merge.Text = setting.farc_Merge;
                CB_MergeWhen.SelectedIndex = (int)setting.Merge_Option;
                CB_Region.SelectedIndex = (int)setting.region;
                LoadVersions(setting.region, setting.Version);
                if (TB_Export.Text.EndsWith("romfs"))
                    TB_Export.Text = TB_Export.Text.Remove(TB_Export.Text.Length - 6, 6);
            }
        }

        public void WriteModList()
        {
            if (File.Exists($"{TB_ModStagePath.Text}\\Modlist.yaml"))
                File.Delete($"{TB_ModStagePath.Text}\\Modlist.yaml");

            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(installedmodList);
            File.WriteAllText($"{TB_ModStagePath.Text}\\Modlist.yaml", yaml);
        }

        public void LoadModList()
        {
            if (File.Exists($"{TB_ModStagePath.Text}\\Modlist.yaml"))
            {
                string yaml = File.ReadAllText($"{TB_ModStagePath.Text}\\Modlist.yaml");
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

        public void LoadVersions(Enums.Region region, int SelectedVersion = 0)
        {
            versionMap = new VersionMap(VersionMap.GetRegion(CB_Region.Text));
            foreach (var version in versionMap.Version)
            {
                CB_Version.Items.Add(version.Key);
            }
            if (SelectedVersion > CB_Version.Items.Count)
                CB_Version.SelectedIndex = CB_Version.Items.Count - 1;
            else
                CB_Version.SelectedIndex = SelectedVersion;
        }

        #endregion

        #region Database Explorer
        private void B_DBOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "db Files(*.bin)|*.bin";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Explorer.Reset(this);
                    Explorer.Open(this, ofd.FileName);
                }
            }
        }

        private void DB_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB_Data.SelectedObject = null;
            Explorer.Select(this, DB_List.Text);
        }

        private void DB_Save_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "db Files(*.bin)|*.bin";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Explorer.Database.Save(sfd.FileName);
                }
            }
        }

        private void B_DBPlus_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Name of Set to add", "Set Name").Replace(" ", "_");
            DB_List.Items.Add(input);
            Explorer.Database.Add(input);
        }

        private void B_DBMinus_Click(object sender, EventArgs e)
        {
            DB_Data.SelectedObject = null;
            Explorer.Database.Entries.Remove(Explorer.Database.GetCommonSet(DB_List.Text));
            DB_List.Items.Remove(DB_List.Text);
        }

        private void BD_EntrySave_Click(object sender, EventArgs e)
        {
            Explorer.SaveEntry(this, DB_List.Text);
        }

        private void DB_EntryImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "yaml Files(*.yaml)|*.yaml";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Explorer.Import(this, ofd.FileName);
                }
            }
        }

        private void DB_EntryExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "yaml Files(*.yaml)|*.yaml";
                sfd.RestoreDirectory = true;
                sfd.FileName = DB_List.Text;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Explorer.Export(this, sfd.FileName);
                }
            }
        }

        #endregion

        #region Patches
        public void ExportPacthes()
        {
            if (installedPatchList.Patches.Count == 0)
                return;

            List<string> pchtxt = new List<string>()
            {
                $"@nsobid-{versionMap.Version[CB_Version.Text]}",
                $"# Hatsune Miku: Project DIVA Mega Mix v{CB_Version.Text}",
                $"@flag offset_shift 0x100\r\n"
            };

            foreach (var patch in installedPatchList.Patches)
            {
                pchtxt.Add($"// {patch.Name}");
                if (patch.Enabled)
                    pchtxt.Add($"@enabled");
                else
                    pchtxt.Add($"@disabled");
                pchtxt.Add($"{patch.Code}\r\n");
            }

            if (Directory.Exists($"{TB_Export.Text}\\exefs"))
                Directory.Delete($"{TB_Export.Text}\\exefs", true);
            Directory.CreateDirectory($"{TB_Export.Text}\\exefs");
            File.WriteAllLines($"{TB_Export.Text}\\exefs\\MegaMixModManager.pchtxt", pchtxt.ToArray());
            if (installedPatchList.args != null && installedPatchList.args.Length > 0)
                File.WriteAllText($"{TB_Export.Text}\\romfs\\args.txt", installedPatchList.args);
        }

        public void WritePatchList()
        {
            if (File.Exists($"{TB_ModStagePath.Text}\\PatchList.yaml"))
                File.Delete($"{TB_ModStagePath.Text}\\PatchList.yaml");

            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(installedPatchList);
            File.WriteAllText($"{TB_ModStagePath.Text}\\PatchList.yaml", yaml);
        }

        public void LoadPatchList()
        {
            if (File.Exists($"{TB_ModStagePath.Text}\\PatchList.yaml"))
            {
                string yaml = File.ReadAllText($"{TB_ModStagePath.Text}\\PatchList.yaml");
                var deserializer = new DeserializerBuilder().Build();
                PatchList patchlist = deserializer.Deserialize<PatchList>(yaml);
                installedPatchList = patchlist;

                foreach (PatchList.Patch patch in patchlist.Patches)
                {
                    if (TV_PatchList.Nodes.ContainsKey(patch.Name))
                        continue;
                    TV_PatchList.Nodes.Add(patch.hash, patch.Name);
                    if (!patch.Enabled)
                        TV_PatchList.Nodes[patch.hash].ForeColor = Color.Red;
                    else
                        TV_PatchList.Nodes[patch.hash].Checked = true;
                }
                TB_args.Text = patchlist.args == null? string.Empty : patchlist.args;
            }
        }

        private void B_PatchAdd_Click(object sender, EventArgs e)
        {
            if (TB_PatchName.Text.Length <= 0 && RTB_PatchCode.Text.Length <= 0)
            {
                MessageBox.Show("Patch is missing Name and Code", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            PatchList.Patch patch = new PatchList.Patch()
            {
                Name = TB_PatchName.Text,
                hash = Hash.HashString(RTB_PatchCode.Text),
                Enabled = true,
                Code = RTB_PatchCode.Text
            };

            TV_PatchList.Nodes.Add(patch.hash, patch.Name);
            installedPatchList.Patches.Add(patch);
            TV_PatchList.Nodes[patch.hash].Checked = true;
            WritePatchList();
            TB_PatchName.Clear();
            RTB_PatchCode.Clear();
        }

        private void B_PatchRemove_Click(object sender, EventArgs e)
        {
            if (TV_PatchList.SelectedNode == null)
                return;

            installedPatchList.Patches.Remove(installedPatchList.GetPatchByHash(TV_PatchList.SelectedNode.Name));
            TV_PatchList.SelectedNode.Remove();
            WritePatchList();
        }

        private void B_PatchSave_Click(object sender, EventArgs e)
        {
            if (TV_PatchList.SelectedNode == null)
                return;

            var patch = installedPatchList.GetPatchByHash(TV_PatchList.SelectedNode.Name);
            patch.Name = TB_PatchName.Text;
            patch.hash = Hash.HashString(RTB_PatchCode.Text);
            patch.Code = RTB_PatchCode.Text;
            TV_PatchList.SelectedNode.Name = patch.hash;
            WritePatchList();
            TB_PatchName.Clear();
            RTB_PatchCode.Clear();
            TV_PatchList.SelectedNode = null;
        }

        private void B_PatchClear_Click(object sender, EventArgs e)
        {
            TB_PatchName.Clear();
            RTB_PatchCode.Clear();
        }

        private void TV_PatchList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TB_PatchName.Text = installedPatchList.GetPatchByHash(TV_PatchList.SelectedNode.Name).Name;
            RTB_PatchCode.Text = installedPatchList.GetPatchByHash(TV_PatchList.SelectedNode.Name).Code;
        }

        private void TV_PatchList_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                installedPatchList.GetPatchByHash(e.Node.Name).Enabled = true;
                e.Node.ForeColor = Color.Black;
            }
            else
            {
                installedPatchList.GetPatchByHash(e.Node.Name).Enabled = false;
                e.Node.ForeColor = Color.Red;
            }
            WritePatchList();
        }

        private void TV_PatchList_MouseUp(object sender, MouseEventArgs e)
        {
            var clickedNode = TV_ModList.GetNodeAt(e.X, e.Y);
            if (clickedNode == null)
            {
                //clicked on background
                TV_PatchList.SelectedNode = null;
            }
            else
            {
                //clicked on node
                return;
            }
        }

        private void TB_args_TextChanged(object sender, EventArgs e)
        {
            installedPatchList.args = TB_args.Text;
        }

        private void B_SaveArgs_Click(object sender, EventArgs e)
        {
            WritePatchList();
        }

        #endregion
    }
}
