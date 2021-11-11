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
using Microsoft.VisualBasic;
using Microsoft.WindowsAPICodePack.Dialogs;
using Mega_Mix_Mod_Manager.DeepMerge;
using Newtonsoft.Json;
using System.IO.Compression;
using Mega_Mix_Mod_Manager.IO;
using System.Text.RegularExpressions;

namespace Mega_Mix_Mod_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadSettings();
            if (TB_Default_Author.Text != null || TB_Default_Author.Text.Length != 0)
                TB_ModAuthor.Text = TB_Default_Author.Text;
            if (!Directory.Exists("Mods"))
                Directory.CreateDirectory("Mods");
            if (File.Exists($"Mods\\Modlist.json"))
                LoadModList();
            else
                installedmodList = new ModList();
        }
        public string pv_db_Path;
        public ModList installedmodList;

        #region Mod Install

        private void B_InstallMod_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Zip Files(*.zip)|*.zip";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        ZipArchive mod = ZipFile.Open(file, ZipArchiveMode.Read);

                        string hash = Hash.HashFile(File.ReadAllBytes(file));
                        if (Directory.Exists($"Mods\\{hash}"))
                            Directory.Delete($"Mods\\{hash}", true);

                        Directory.CreateDirectory($"Mods\\{hash}");
                        ZipFile.ExtractToDirectory(file, $"Mods\\{hash}");
                        ModInfo modinfo = new ModInfo();

                        if (mod.GetEntry("modinfo.json") == null)
                        {
                            modinfo.Name = Path.GetFileNameWithoutExtension(file);
                            modinfo.Description = "No info found";
                            modinfo.Author = "Unknown";
                        }
                        else
                        {
                            string json = File.ReadAllText($"Mods\\{hash}\\modinfo.json");
                            modinfo = JsonConvert.DeserializeObject<ModInfo>(json);
                        }

                        ModList.ModList_Entry newmod = new ModList.ModList_Entry();

                        if (!installedmodList.mods.Contains(newmod))
                        {
                            installedmodList.mods.Add(new ModList.ModList_Entry() { Name = modinfo.Name, hash = hash });
                            TV_ModList.Nodes.Add(hash, modinfo.Name);
                        }
                        WriteModList();
                    }
                }
            }
        }

        private void B_RemoveMod_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                return;
            installedmodList.mods.Remove(installedmodList.mods.Where(x => x.hash == TV_ModList.SelectedNode.Name).First());
            PB_ModPreview.Image = null;
            RTB_ModDetails.Clear();
            Directory.Delete($"Mods\\{TV_ModList.SelectedNode.Name}", true);
            TV_ModList.SelectedNode.Remove();

            WriteModList();
        }

        private void B_ExportMods_Click(object sender, EventArgs e)
        {
            if (TB_Export == null || TB_Export.Text.Length == 0)
                return;
            Directory.Delete(TB_Export.Text, true);
            Directory.CreateDirectory(TB_Export.Text);

            foreach (TreeNode node in TV_ModList.Nodes)
            {
                string[] files = Directory.GetFiles($"Mods\\{node.Name}", "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (Path.GetFileName(file) == "modinfo.json" || Path.GetFileName(file) == "thumbnail.jpg")
                        continue;

                    string outfile = file.Replace($"Mods\\{node.Name}", TB_Export.Text);
                    outfile = Regex.Replace(outfile, "romfs", "", RegexOptions.IgnoreCase).Replace("\\\\", "\\");
                    if (!Directory.Exists(Path.GetDirectoryName(outfile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(outfile));
                    File.Copy(file, outfile, true);
                }
            }
        }

        private void B_ModUp_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                return;
            TreeViewAddon.MoveUp(TV_ModList.SelectedNode);
        }

        private void B_ModDown_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                return;
            TreeViewAddon.MoveDown(TV_ModList.SelectedNode);
        }

        private void B_ClearMods_Click(object sender, EventArgs e)
        {
            installedmodList = new ModList();
            PB_ModPreview.Image = null;
            RTB_ModDetails.Clear();
            TV_ModList.Nodes.Clear();
            Directory.Delete("Mods", true);
            Directory.CreateDirectory("Mods");
        }

        private void TV_ModList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (File.Exists($"Mods\\{TV_ModList.SelectedNode.Name}\\thumbnail.jpg"))
                PB_ModPreview.Image = new Bitmap($"Mods\\{TV_ModList.SelectedNode.Name}\\thumbnail.jpg");

            string json = File.ReadAllText($"Mods\\{TV_ModList.SelectedNode.Name}\\modinfo.json");
            ModInfo modinfo = JsonConvert.DeserializeObject<ModInfo>(json);

            RTB_ModDetails.Text = $"Name: {modinfo.Name}\nAuthor: {modinfo.Author}\n{modinfo.Description}";
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
            if (!CB_PathVarify.Checked)
            {
                MessageBox.Show("No Game Dump Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (TB_ModPath.Text == null || TB_ModPath.Text.Length == 0)
            {
                MessageBox.Show("Mod Path Missing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (TB_ModName.Text == null || TB_ModName.Text.Length == 0)
            {
                MessageBox.Show("Mod Name Missing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] files = Directory.GetFiles(TB_ModPath.Text, "*", SearchOption.AllDirectories);
            Directory.CreateDirectory($"{TB_ModPath.Text}\\.temp");

            foreach (string file in files)
            {
                if (!Directory.Exists($"{TB_ModPath.Text}\\.temp\\{Path.GetDirectoryName(file.Replace(TB_ModPath.Text, ""))}"))
                {
                    Directory.CreateDirectory($"{TB_ModPath.Text}\\.temp\\{Path.GetDirectoryName(file.Replace(TB_ModPath.Text, ""))}");
                }
                File.Copy(file, $"{TB_ModPath.Text}\\.temp\\{file.Replace(TB_ModPath.Text, "")}", true);
            }
            
            if (PB_ModCreateImg.Image != null)
                PB_ModCreateImg.Image.Save($"{TB_ModPath.Text}\\.temp\\thumbnail.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            WriteModInfo();

            using (SaveFileDialog sdf = new SaveFileDialog())
            {
                sdf.Filter = "Zip Files(*.zip)|*.zip";
                sdf.RestoreDirectory = true;
                sdf.FileName = TB_ModName.Text + ".zip";

                if (sdf.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sdf.FileName))
                        File.Delete(sdf.FileName);
                    ZipFile.CreateFromDirectory($"{TB_ModPath.Text}\\.temp", sdf.FileName);
                }
            }
            Directory.Delete($"{TB_ModPath.Text}\\.temp", true);
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
                }
            }
        }

        private void TB_DumpPath_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists($"{TB_DumpPath.Text}\\rom_switch\\rom\\pv_db.txt"))
            {
                pv_db_Path = $"{TB_DumpPath.Text}\\rom_switch\\rom\\pv_db.txt";
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

            string json = JsonConvert.SerializeObject(modinfo, Formatting.Indented);
            File.WriteAllText($"{TB_ModPath.Text}\\.temp\\modinfo.json", json);
        }

        public void LoadModInfo()
        {
            if (File.Exists($"{TB_ModPath.Text}\\modinfo.json"))
            {
                string json = File.ReadAllText($"{TB_ModPath.Text}\\modinfo.json");
                ModInfo modinfo = JsonConvert.DeserializeObject<ModInfo>(json);

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
            settings.Default_Author = TB_Default_Author.Text;

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText($"settings.json", json);
        }

        public void LoadSettings()
        {
            if (File.Exists($"settings.json"))
            {
                string json = File.ReadAllText($"settings.json");
                Settings setting = JsonConvert.DeserializeObject<Settings>(json);

                TB_DumpPath.Text = setting.Game_Dump;
                TB_Export.Text = setting.Export_Path;
                TB_Default_Author.Text = setting.Default_Author;
            }
        }

        public void WriteModList()
        {
            if (File.Exists($"Mods\\Modlist.json"))
                File.Delete($"Mods\\Modlist.json");

            string json = JsonConvert.SerializeObject(installedmodList, Formatting.Indented);
            File.WriteAllText($"Mods\\Modlist.json", json);
        }

        public void LoadModList()
        {
            if (File.Exists($"Mods\\Modlist.json"))
            {
                string json = File.ReadAllText($"Mods\\Modlist.json");
                ModList modlist = JsonConvert.DeserializeObject<ModList>(json);
                installedmodList = modlist;

                foreach (ModList.ModList_Entry mod in modlist.mods)
                {
                    TV_ModList.Nodes.Add(mod.hash, mod.Name);
                }
            }
        }

        #endregion

        #region Classes

        public class ModInfo
        {
            public string Name { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
        }

        public class Settings
        {
            public string Game_Dump { get; set; }
            public string Export_Path { get; set; }
            public string Default_Author { get; set; }
        }

        public class ModList
        {
            public Region region { get; set; } = Region.EN;
            public List<ModList_Entry> mods { get; set; } = new List<ModList_Entry>();
            public class ModList_Entry
            {
                public string Name { get; set; }
                public string hash { get; set; }
            }

            public enum Region
            {
                JPN,
                EN
            }
        }

        #endregion

        
    }
}
