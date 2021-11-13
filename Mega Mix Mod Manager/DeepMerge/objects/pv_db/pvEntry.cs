using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega_Mix_Mod_Manager.IO;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry
    {
        public List<pvEntry_another_song> another_song {  get; set; }
        public List<pvEntry_auth_replace_by_module> auth_replace_by_module { get; set; }
        public int bpm { get; set; }
        public string chainslide_failure_name { get; set; }
        public string chainslide_first_name { get; set; }
        public string chainslide_sub_name { get; set; }
        public string chainslide_success_name { get; set; }
        public List<pvEntry_chrcam> chrcam { get; set; }
        public List<pvEntry_chreff> chreff { get; set; }
        public List<pvEntry_chrmot> chrmot {  get; set; }
        public int date { get; set; }
        public pvEntry_difficulty difficulty { get; set; }
        public pvEntry_disp2d disp2d { get; set; }
        public string effect_se_file_name { get; set; }
        public List<string> effect_se_name_list { get; set; }
        public List<pvEntry_ex_song> ex_song {  get; set; }
        public string eyes_base_adjust_type { get; set; }
        public int eyes_xrot_adjust { get; set; }
        public List<pvEntry_field> field { get; set; }
        public string frame_texture { get; set; }
        public string frame_texture_a { get; set; }
        public string frame_texture_b { get; set; }
        public string frame_texture_c { get; set; }
        public string frame_texture_d { get; set; }
        public string frame_texture_e { get; set; }
        public pvEntry_hand_a3d hand_a3d { get; set; }
        public List<string> hand_item { get; set; }
        public decimal hidden_timing { get; set; }
        public int high_speed_rate { get; set; }
        public bool is_extra { get; set; }
        public bool is_old_pv { get; set; }
        public List<string> lyric { get; set; }
        public List<string> lyric_en { get; set; }
        public int mdata_flag { get; set; }
        public List<string> motion { get; set; }
        public List<string> motion2P { get; set; }
        public List<string> motion3P { get; set; }
        public List<string> motion4P { get; set; }
        public List<string> motion5P { get; set; }
        public List<string> motion6P { get; set; }
        public string movie_file_name { get; set; }
        public List<string> movie_list { get; set; }
        public string movie_surface { get; set; }
        public List<pvEntry_osage_init> osage_init { get; set; }
        public int pack { get; set; }
        public List<pvEntry_performer> performer { get; set; }
        public string pv_expression_file_name { get; set; }
        public List<string> pv_item { get; set; }
        public string pvbranch_success_se_name { get; set; }
        public int rank_board_id { get; set; }
        public string reading { get; set; }
        public decimal resolution_scale { get; set; }
        public decimal resolution_scale_neo { get; set; }
        public decimal s3d_screen_dist { get; set; }
        public decimal sabi_play_time { get; set; }
        public decimal sabi_start_time { get; set; }
        public string se_name { get; set; }
        public string slide_name { get; set; }
        public string slidertouch_name { get; set; }
        public string song_file_name { get; set; }
        public string song_name { get; set; }
        public string song_name_en { get; set; }
        public string song_name_reading { get; set; }
        public string song_name_reading_en { get; set; }
        public pvEntry_song_play_param song_play_param { get; set; }
        public pvEntry_songinfo songinfo { get; set; }
        public pvEntry_songinfo songinfo_en { get; set; }
        public int sort_index { get; set; }
        public List<pvEntry_stage_param> stage_param { get; set; }
        public decimal sudden_timing { get; set; }
        public string title_image_aet_name { get; set; }
        public int title_image_time { get; set; }
        public int tutorial { get; set; }

        public void Read(StreamReader sr)
        {
            Dictionary<string, Action> op = new Dictionary<string, Action>();
            op["another_song"] = () =>              { another_song = new pvEntry_another_song().Read(sr); };
            op["auth_replace_by_module"] = () =>    { auth_replace_by_module = new pvEntry_auth_replace_by_module().Read(sr); };
            op["bmp"] = () =>                       { bpm = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["chainslide_failure_name"] = () =>   { chainslide_failure_name = sr.ReadLine().Split('=')[1]; };

            string line;
            while ((line = StreamReaderLookAhead.LookAheadLine(sr)) != null)
            {
                op[line.Split('.')[1]].Invoke();
            }
        }
    }
}
