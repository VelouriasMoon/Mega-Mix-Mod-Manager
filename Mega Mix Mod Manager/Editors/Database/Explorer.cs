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
            if (Path.GetFileName(infile) == "obj_db.bin")
                Database.Read(obj_db.Read(infile));
            else if (Path.GetFileName(infile) == "tex_db.bin")
                Database.Read(tex_db.Read(infile));
            else if (Path.GetFileName(infile) == "spr_db.bin")
                Database.Read(spr_db.Read(infile));

            foreach (var entry in Database.Entries)
            {
                form1.DB_List.Items.Add(entry.Name);
            }
        }

        public static void Select(Form1 form1, string selectedItem)
        {
            switch (Database.DatabaseType)
            {
                case CommonType.obj:
                    {
                        Property_Obj property_Obj = new Property_Obj();
                        CommonSet commonSet = Database.GetCommonSet(selectedItem);
                        property_Obj.Read(commonSet);
                        form1.DB_Data.SelectedObject = property_Obj;
                    }
                    break;
                case CommonType.tex:
                    {
                        Property_Tex property_Tex = new Property_Tex();
                        CommonSet commonSet = Database.GetCommonSet(selectedItem);
                        property_Tex.Read(commonSet);
                        form1.DB_Data.SelectedObject = property_Tex;
                    }
                    break;
                case CommonType.spr:
                    {
                        Property_Spr property_spr = new Property_Spr();
                        CommonSet commonSet = Database.GetCommonSet(selectedItem);
                        property_spr.Read(commonSet);
                        form1.DB_Data.SelectedObject = property_spr;
                    }
                    break;

            }
        }
    }
}
