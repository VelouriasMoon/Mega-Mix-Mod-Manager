using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega_Mix_Mod_Manager.Lite_Merge;
using MikuMikuLibrary.Databases;
using Mega_Mix_Mod_Manager.Editors.Database;
using Mega_Mix_Mod_Manager.Objects;
using YamlDotNet.Serialization;
using System.Windows.Forms;

namespace Mega_Mix_Mod_Manager.Editors
{
    internal class Explorer
    {
        public static CommonDatabase Database { get; set; }


        public static void Reset(Form1 form1)
        {
            form1.DB_List.Items.Clear();
            form1.DB_Data.SelectedObject = null;
            if (Database != null)
                Database = null;
        }
        public static void Open(Form1 form1, string infile)
        {
            Database = new CommonDatabase();
            if (Path.GetFileName(infile).Contains("obj_db"))
                Database.Read(obj_db.Read(infile));
            else if (Path.GetFileName(infile).Contains("tex_db"))
                Database.Read(tex_db.Read(infile));
            else if (Path.GetFileName(infile).Contains("spr_db"))
                Database.Read(spr_db.Read(infile));

            foreach (var entry in Database.Entries)
            {
                form1.DB_List.Items.Add(entry.Name);
            }
        }

        public static void SaveEntry(Form1 form1, string selectedindex)
        {
            CommonSet commonSet = new CommonSet();
            switch (Database.DatabaseType)
            {
                case CommonType.obj:
                    {
                        Property_Obj property_Obj = (Property_Obj)form1.DB_Data.SelectedObject;
                        commonSet = property_Obj.Write();
                    }
                    break;
                case CommonType.tex:
                    {
                        Property_Tex property_Tex = (Property_Tex)form1.DB_Data.SelectedObject;
                        commonSet = property_Tex.Write();
                    }
                    break;
                case CommonType.spr:
                    {
                        Property_Spr property_spr = (Property_Spr)form1.DB_Data.SelectedObject;
                        commonSet = property_spr.Write();
                    }
                    break;
            }
            var oldentry = Database.GetCommonSet(selectedindex);
            Database.Entries[Database.Entries.IndexOf(oldentry)] = commonSet;
            if (form1.DB_List.Items.Contains(commonSet.Name) && selectedindex != commonSet.Name)
            {
                MessageBox.Show("Set Name already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.DB_List.Items[form1.DB_List.Items.IndexOf(selectedindex)] = commonSet.Name;
        }

        public static void Select(Form1 form1, string selectedItem)
        {
            switch (Database.DatabaseType)
            {
                case CommonType.obj:
                    {
                        Property_Obj property_Obj = new Property_Obj();
                        CommonSet commonSet = Database.GetCommonSet(selectedItem);
                        if (commonSet == null) 
                            break;
                        property_Obj.Read(commonSet);
                        form1.DB_Data.SelectedObject = property_Obj;
                    }
                    break;
                case CommonType.tex:
                    {
                        Property_Tex property_Tex = new Property_Tex();
                        CommonSet commonSet = Database.GetCommonSet(selectedItem);
                        if (commonSet == null)
                            break;
                        property_Tex.Read(commonSet);
                        form1.DB_Data.SelectedObject = property_Tex;
                    }
                    break;
                case CommonType.spr:
                    {
                        Property_Spr property_spr = new Property_Spr();
                        CommonSet commonSet = Database.GetCommonSet(selectedItem);
                        if (commonSet == null)
                            break;
                        property_spr.Read(commonSet);
                        form1.DB_Data.SelectedObject = property_spr;
                    }
                    break;
            }
        }

        public static void Export(Form1 form1, string outpath)
        {
            string yaml = null;
            var serializer = new SerializerBuilder().Build();
            switch (Database.DatabaseType)
            {
                case CommonType.obj:
                    {
                        Property_Obj property_Obj = (Property_Obj)form1.DB_Data.SelectedObject;
                        ObjectSetInfo objectSetInfo = property_Obj.Export();
                        yaml = serializer.Serialize(property_Obj);
                    }
                    break;
                case CommonType.tex:
                    {
                        Property_Tex property_Tex = (Property_Tex)form1.DB_Data.SelectedObject;
                        TextureInfo textureInfo = property_Tex.Export();
                        yaml = serializer.Serialize(property_Tex);
                    }
                    break;
                case CommonType.spr:
                    {
                        Property_Spr property_Spr = (Property_Spr)form1.DB_Data.SelectedObject;
                        SpriteSetInfo spriteSetInfo = property_Spr.Export();
                        yaml = serializer.Serialize(property_Spr);
                        
                    }
                    break;
            }
            File.WriteAllText(outpath, yaml);
        }

        public static void Import(Form1 form1, string infile)
        {
            string yaml = File.ReadAllText(infile);
            var deserializer = new DeserializerBuilder().Build();
            CommonSet commonSet = new CommonSet();

            switch (Database.DatabaseType)
            {
                case CommonType.obj:
                    {
                        Property_Obj property_Obj = deserializer.Deserialize<Property_Obj>(yaml);
                        form1.DB_Data.SelectedObject = property_Obj;
                    }
                    break;
                case CommonType.tex:
                    {
                        Property_Tex property_Tex = deserializer.Deserialize<Property_Tex>(yaml);
                        form1.DB_Data.SelectedObject = property_Tex;
                    }
                    break;
                case CommonType.spr:
                    {
                        Property_Spr property_Spr = deserializer.Deserialize<Property_Spr>(yaml);
                        form1.DB_Data.SelectedObject = property_Spr;
                    }
                    break;
            }
        }
    }
}
