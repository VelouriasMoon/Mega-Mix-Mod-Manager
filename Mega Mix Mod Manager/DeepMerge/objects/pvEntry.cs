using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects
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
    }

    public class pvEntry_another_song
    {
        public string name { get; set; }
        public string name_en { get; set; }
        public string song_file_name { get; set; }
        public string vocal_chara_num { get; set; }
        public string vocal_disp_name { get; set; }
        public string vocal_disp_name_en { get; set; }
    }

    public class pvEntry_auth_replace_by_module
    {
        public int id { get; set; }
        public int module_id { get; set; }
        public string name { get; set; }
        public string org_name { get; set; }

    }

    public class pvEntry_chrcam
    {
        public string chara { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string org_name { get; set; }

    }

    public class pvEntry_chreff
    {
        public List<chreff_data> data {  get; set; }
        public int id { get; set; }
        public string name { get; set; }

        public class chreff_data
        {
            public string name { get; set;}
            public string type { get; set; }
        }
    }

    public class pvEntry_chrmot
    {
        public string chara { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string org_name { get; set; }
    }

    public class pvEntry_difficulty
    {
        public int original { get; set; }
        public int slide { get; set; }
        public List<difficulty> easy { get; set; }
        public List<difficulty> encore { get; set; }
        public List<difficulty> extreme { get; set; }
        public List<difficulty> hard { get; set; }
        public List<difficulty> normal { get; set; }

        public class difficulty
        {
            public int edition { get; set; }
            public string level { get; set; }
            public int level_sort_index { get; set; }
            public string script_file_name { get; set; }
            public string script_format { get; set; }
            public string version { get; set; }
        }
    }

    public class pvEntry_disp2d
    {
        public int set_name { get; set; }
        public int target_shadow_type { get; set; }
        public string title_2d_layer { get; set; }
        public int title_end_2d_field { get; set; }
        public int title_end_2d_low_field { get; set; }
        public int title_end_3d_field { get; set; }
        public int title_start_2d_field { get; set; }
        public int title_start_2d_low_field { get; set; }
        public int title_start_3d_field { get; set; }
    }

    public class pvEntry_ex_song
    {
        public string chara { get; set; }
        public List<ex_song_ex_auth> ex_auth { get; set; }
        public string file { get; set; }

        public class ex_song_ex_auth
        {
            public string name { get; set; }
            public string org_name { get; set; }
        }
    }

    public class pvEntry_field
    {
        public string ex_stage { get; set; }
        public string stage { get; set; }
    }

    public class pvEntry_hand_a3d
    {
        public List<string> _1P { get; set; }
    }

    public class pvEntry_osage_init
    {
        public int frame { get; set; }
        public string motion { get; set; }
        public string stage { get; set; }
    }

    public class pvEntry_performer
    {
        public string chara { get; set; }
        public bool Fixed { get; set; }
        public int pseudo_same_id { get; set; }
        public int pv_costume { get; set; }
        public string size { get; set; }
        public string type { get; set; }
    }

    public class pvEntry_song_play_param
    {
        public List<beat> bpm { get; set; }
        public int max_frame { get; set; }
        public List<beat> rhythm { get; set; }

        public class beat
        {
            public int bar { get; set; }
            public int data { get; set; }

        }
    }

    public class pvEntry_songinfo
    {
        public string arranger { get; set; }
        public string ex_info_key { get; set; }
        public string ex_info_val { get; set; }
        public string guitar_player { get; set; }
        public string illustrator { get; set; }
        public string lyrics { get; set; }
        public string manipulator { get; set; }
        public string music { get; set; }
        public string pv_editor { get; set; }
    }

    public class pvEntry_stage_param
    {
        public string collision_file { get; set; }
        public int mhd_id { get; set; }
        public string stage { get; set; }
        public string wind_file { get; set; }
    }
}
