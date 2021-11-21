using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.IO.Compression;
using System.Text.RegularExpressions;

using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;

using Mega_Mix_Mod_Manager.IO;
using Mega_Mix_Mod_Manager.Lite_Merge;
using Mega_Mix_Mod_Manager.Objects;
using MikuMikuLibrary.Archives;

namespace Mega_Mix_Mod_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists("Mods"))
                Directory.CreateDirectory("Mods");
            if (File.Exists($"Mods\\Modlist.json"))
                LoadModList();
            else
                installedmodList = new ModList();
            LoadSettings();
            if (TB_Default_Author.Text != null || TB_Default_Author.Text.Length != 0)
                TB_ModAuthor.Text = TB_Default_Author.Text;
        }
        public string pv_db_Path;
        public ModList installedmodList;

        #region Mod Install
        #region UI Funtions
        private void B_RemoveMod_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null)
                return;
            installedmodList.mods.Remove(installedmodList.mods.Where(x => x.hash == TV_ModList.SelectedNode.Name).First());
            PB_ModPreview.Image.Dispose();
            PB_ModPreview.Image = null;
            RTB_ModDetails.Clear();
            Directory.Delete($"Mods\\{TV_ModList.SelectedNode.Name}", true);
            TV_ModList.SelectedNode.Remove();

            WriteModList();
        }

        private void B_ModUp_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null || TV_ModList.SelectedNode.Index <= 0)
                return;
            TreeViewAddon.MoveUp(TV_ModList.SelectedNode);
            installedmodList.MoveUp(TV_ModList.SelectedNode.Index);
            WriteModList();
        }

        private void B_ModDown_Click(object sender, EventArgs e)
        {
            if (TV_ModList.SelectedNode == null || TV_ModList.SelectedNode.Index >= installedmodList.mods.Count - 1)
                return;
            TreeViewAddon.MoveDown(TV_ModList.SelectedNode);
            installedmodList.MoveDown(TV_ModList.SelectedNode.Index);
            WriteModList();
        }

        private void B_ClearMods_Click(object sender, EventArgs e)
        {
            installedmodList = new ModList();
            if (PB_ModPreview.Image != null)
            {
                PB_ModPreview.Image.Dispose();
                PB_ModPreview.Image = null;
            }
            RTB_ModDetails.Clear();
            TV_ModList.Nodes.Clear();
            Directory.Delete("Mods", true);
            Directory.CreateDirectory("Mods");
        }

        private void TV_ModList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (File.Exists($"Mods\\{TV_ModList.SelectedNode.Name}\\thumbnail.jpg"))
            {
                Image img = new Bitmap($"Mods\\{TV_ModList.SelectedNode.Name}\\thumbnail.jpg");
                PB_ModPreview.Image = img;
                //img.Dispose();
            }


            string json = File.ReadAllText($"Mods\\{TV_ModList.SelectedNode.Name}\\modinfo.json");
            ModInfo modinfo = JsonConvert.DeserializeObject<ModInfo>(json);

            RTB_ModDetails.Text = $"Name: {modinfo.Name}\nAuthor: {modinfo.Author}\n{modinfo.Description}";
        }
        #endregion

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
                        byte[] data = File.ReadAllBytes(file);
                        string hash = Hash.HashFile(data);
                        if (Directory.Exists($"Mods\\{hash}"))
                            Directory.Delete($"Mods\\{hash}", true);
                        Directory.CreateDirectory($"Mods\\{hash}");

                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            using (ZipArchive mod = new ZipArchive(ms, ZipArchiveMode.Read, false))
                            {
                                mod.ExtractToDirectory($"Mods\\{hash}");
                                mod.Dispose();
                            }
                        }
                        ModInfo modinfo = new ModInfo();

                        if (!File.Exists($"Mods\\{hash}\\modinfo.json"))
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

        private void B_ExportMods_Click(object sender, EventArgs e)
        {
            if (TB_Export == null || TB_Export.Text.Length == 0)
                return;
            Directory.Delete(TB_Export.Text, true);
            Directory.CreateDirectory(TB_Export.Text);
            PB_InstallProgress.Visible = true;
            PB_InstallProgress.Value = 0;

            #region Merging
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
                if (CB_pv_Merge.Text == "Lite Merge")
                {
                    PB_InstallProgress.Value = 5;
                    pv_db.MergeMods(Directory.GetFiles("Mods", "*", SearchOption.AllDirectories), pv_db_Path, $"{TB_Export.Text}\\rom_switch\\rom\\pv_db.txt");
                    PB_InstallProgress.Value = 10;
                }
                if (CB_obj_Merge.Text == "Lite Merge")
                {
                    PB_InstallProgress.Value = 15;
                    string[] files = Directory.GetFiles("Mods", "*obj_db.bin", SearchOption.AllDirectories);
                    obj_db.Merge($"{TB_DumpPath.Text}\\rom_switch\\rom\\objset\\obj_db.bin", files, $"{TB_Export.Text}\\rom_switch\\rom\\objset\\obj_db.bin");
                    PB_InstallProgress.Value = 20;
                }
                if (CB_tex_Merge.Text == "Lite Merge")
                {
                    PB_InstallProgress.Value = 25;
                    string[] files = Directory.GetFiles("Mods", "*tex_db.bin", SearchOption.AllDirectories);
                    tex_db.Merge($"{TB_DumpPath.Text}\\rom_switch\\rom\\objset\\tex_db.bin", files, $"{TB_Export.Text}\\rom_switch\\rom\\objset\\tex_db.bin");
                    PB_InstallProgress.Value = 30;
                }
                if (CB_spr_Merge.Text == "Lite Merge")
                {
                    PB_InstallProgress.Value = 35;
                    string[] files = Directory.GetFiles("Mods", "*spr_db.bin", SearchOption.AllDirectories);
                    string region = Enum.GetName(typeof(ModList.Region), installedmodList.region);

                    spr_db.Merge($"{TB_DumpPath.Text}\\{region}\\rom\\2d\\spr_db.bin", files, $"{TB_Export.Text}\\{region}\\rom\\2d\\spr_db.bin");
                    PB_InstallProgress.Value = 40;
                }
                /*
                if (CB_farc_Merge.SelectedIndex > 0)
                {
                    PB_InstallProgress.Value = 45;
                    farc.Merge(TB_DumpPath.Text, "Mods", TB_Export.Text);
                    PB_InstallProgress.Value = 50;
                }
                */
            }
            #endregion

            PB_InstallProgress.Value = 80;
            foreach (TreeNode node in TV_ModList.Nodes)
            {
                string[] files = Directory.GetFiles($"Mods\\{node.Name}", "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (Path.GetFileName(file) == "modinfo.json" || Path.GetFileName(file) == "thumbnail.jpg")
                        continue;

                    if (Path.GetFileName(file) == "pv_db.txt" && CB_PathVarify.Checked && CB_pv_Merge.SelectedIndex > 0)
                        continue;
                    if (Path.GetFileName(file) == "obj_db.bin" && CB_PathVarify.Checked && CB_obj_Merge.SelectedIndex > 0)
                        continue;
                    if (Path.GetFileName(file) == "tex_db.bin" && CB_PathVarify.Checked && CB_tex_Merge.SelectedIndex > 0)
                        continue;
                    if (Path.GetFileName(file) == "spr_db.bin" && CB_PathVarify.Checked && CB_spr_Merge.SelectedIndex > 0)
                        continue;
                    //if (Path.GetExtension(file) == ".farc" && !Path.GetDirectoryName(file).Contains("2d") && CB_PathVarify.Checked && CB_farc_Merge.SelectedIndex > 0)
                    //    continue;

                    string outfile = file.Replace($"Mods\\{node.Name}", "");
                    outfile = Regex.Replace(outfile, "romfs", "", RegexOptions.IgnoreCase).Replace("\\\\", "\\");
                    if (!Directory.Exists(Path.GetDirectoryName($"{TB_Export.Text}\\{outfile}")))
                        Directory.CreateDirectory(Path.GetDirectoryName($"{TB_Export.Text}\\{outfile}"));
                    File.Copy(file, $"{TB_Export.Text}\\{outfile}", true);
                }
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

            string[] files = Directory.GetFiles(TB_ModPath.Text, "*", SearchOption.AllDirectories);
            Directory.CreateDirectory($"{TB_ModPath.Text}\\.temp");

            foreach (string file in files)
            {
                if (CB_pv_Merge.Text == "Deep Merge" && CB_PathVarify.Checked)
                {
                    if (Path.GetFileName(file) == "pv_db.txt")
                    {

                    }
                }
                else
                {
                    if (!Directory.Exists($"{TB_ModPath.Text}\\.temp\\{Path.GetDirectoryName(file.Replace(TB_ModPath.Text, ""))}"))
                    {
                        Directory.CreateDirectory($"{TB_ModPath.Text}\\.temp\\{Path.GetDirectoryName(file.Replace(TB_ModPath.Text, ""))}");
                    }
                    File.Copy(file, $"{TB_ModPath.Text}\\.temp\\{file.Replace(TB_ModPath.Text, "")}", true);
                }
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
                }
            }
        }

        private void TB_DumpPath_TextChanged(object sender, EventArgs e)
        {
            string basepath = $"{TB_DumpPath.Text}\\rom_switch\\rom";
            if (File.Exists($"{basepath}\\pv_db.txt") && File.Exists($"{basepath}\\objset\\obj_db.bin") && File.Exists($"{basepath}\\objset\\tex_db.bin"))
            {
                if (File.Exists($"{ TB_DumpPath.Text}\\rom_switch_en\\rom\\2d\\spr_db.bin"))
                    installedmodList.region = ModList.Region.rom_switch_en;
                else
                    installedmodList.region = ModList.Region.rom_switch;

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
            settings.pv_Merge = CB_pv_Merge.Text;
            settings.obj_Merge = CB_obj_Merge.Text;
            settings.spr_Merge = CB_spr_Merge.Text;
            settings.tex_Merge = CB_tex_Merge.Text;
            settings.farc_Merge = CB_farc_Merge.Text;

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
                CB_pv_Merge.Text = setting.pv_Merge;
                CB_obj_Merge.Text = setting.obj_Merge;
                CB_spr_Merge.Text = setting.spr_Merge;
                CB_tex_Merge.Text = setting.tex_Merge;
                CB_farc_Merge.Text = setting.farc_Merge;
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
    }
}
