using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega_Mix_Mod_Manager.Lite_Merge;
using MikuMikuLibrary.Databases;
using Mega_Mix_Mod_Manager.Editors.Database;
using Mega_Mix_Mod_Manager.IO;

namespace Mega_Mix_Mod_Manager.Editors
{
    internal class Explorer
    {
        public static ObjectDatabase objDatabase { get; set; }
        public static TextureDatabase texDatabase { get; set; }
        public static SpriteDatabase spriteDatabase { get; set; }


        public static void Reset(Form1 form1)
        {
            form1.DB_List.Items.Clear();
            form1.DB_Data.SelectedObject = null;
            if (objDatabase != null)
                objDatabase = null;
            if (texDatabase != null)
                texDatabase = null;
            if (spriteDatabase != null)
                spriteDatabase = null;
        }

        public static void OpenObj(Form1 form1, string obj)
        {
            objDatabase = obj_db.Read(obj);
            form1.groupBox4.Text = "obj_db";
            foreach (var entry in objDatabase.ObjectSets)
            {
                form1.DB_List.Items.Add(entry.Name);
            }
        }

        public static void SelectObj(Form1 form1, string selectedItem)
        {
            Property_Obj property_Objset = new Property_Obj();
            ObjectSetInfo objectSet = objDatabase.GetObjectSetInfo(selectedItem);
            property_Objset.Read(objectSet);

            form1.DB_Data.SelectedObject = property_Objset;
        }

        public static void OpenTex(Form1 form1, string tex)
        {
            texDatabase = tex_db.Read(tex);
            form1.groupBox4.Text = "tex_db";
            foreach (var entry in texDatabase.Textures)
            {
                form1.DB_List.Items.Add(entry.Name);
            }
        }

        public static void SelectTex(Form1 form1, string selectedItem)
        {
            Property_Tex property_Tex = new Property_Tex();
            TextureInfo textureInfo = texDatabase.GetTextureInfo(selectedItem);
            property_Tex.Read(textureInfo);

            form1.DB_Data.SelectedObject = property_Tex;
        }

        public static void OpenSpr(Form1 form1, string spr)
        {
            spriteDatabase = spr_db.Read(spr);
            form1.groupBox4.Text = "spr_db";
            foreach (var entry in spriteDatabase.SpriteSets)
            {
                form1.DB_List.Items.Add(entry.Name);
            }
        }

        public static void SelectSpr(Form1 form1, string selectedItem)
        {
            Property_Spr property_spr = new Property_Spr();
            SpriteSetInfo spriteSetInfo = spriteDatabase.GetSpriteSetInfo(selectedItem);
            property_spr.Read(spriteSetInfo);

            form1.DB_Data.SelectedObject = property_spr;
        }
    }
}
