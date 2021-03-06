﻿using System.Collections;
using System.Collections.Generic;


public class SentenceGenerator
{
    private Dictionary<string, string[]> hiragana_roma = new Dictionary<string, string[]>
    {
        {"ぁ", new string[2] {"la", "xa" } },
        {"あ", new string[1] {"a"} },
        {"ぃ", new string[4] {"li","lyi", "xi","xyi"} },
        {"い", new string[2] {"i", "yi"} },
        {"いぇ", new string[1] {"ye"} },
        {"ぅ", new string[2] {"lu","xu"} },
        {"う", new string[3] {"u", "whu", "wu"} },
        {"うぃ", new string[2] {"whi", "wi"} },
        {"うぇ", new string[2] {"we", "whe" } },
        {"うぉ", new string[1] {"who"} },
        {"ぇ", new string[4] {"le", "lye", "xe", "xye"} },
        {"え", new string[1] {"e"} },
        {"ぉ", new string[2] {"lo", "xo"} },
        {"お", new string[1] {"o"} },
        {"か", new string[2] {"ka", "ca"} },
        {"が", new string[1] {"ga"} },
        {"き", new string[1] {"ki"} },
        {"きぃ", new string[1] {"kyi"} },
        {"きぇ", new string[1] {"kye"} },
        {"きゃ", new string[1] {"kya"} },
        {"きゅ", new string[1] {"kyu"} },
        {"きょ", new string[1] {"kyo"} },
        {"ぎ", new string[1] {"gi"} },
        {"ぎぃ", new string[1] {"gyi"} },
        {"ぎぇ", new string[1] {"gye"} },
        {"ぎゃ", new string[1] {"gya"} },
        {"ぎゅ", new string[1] {"gyu"} },
        {"ぎょ", new string[1] {"gyo"} },
        {"く", new string[3] {"ku", "cu", "qu"} },
        {"くぁ", new string[3] {"kwa", "qa", "qwa"} },
        {"くぃ", new string[3] {"kwi", "qi", "qwi"} },
        {"くぅ", new string[1] {"qu"} },
        {"くぇ", new string[3] {"qwe", "qe", "qye"} },
        {"くぉ", new string[2] {"qo", "qwo"} },
        {"くゃ", new string[1] {"qya"} },
        {"くゅ", new string[1] {"qyu"} },
        {"くょ", new string[1] {"qyo"} },
        {"ぐ", new string[1] {"gu"} },
        {"ぐぁ", new string[1] {"gwa"} },
        {"ぐぃ", new string[1] {"gwi"} },
        {"ぐぅ", new string[1] {"gwu"} },
        {"ぐぇ", new string[1] {"gwe"} },
        {"ぐぉ", new string[1] {"gwo"} },
        {"け", new string[1] {"ke"} },
        {"げ", new string[1] {"ge"} },
        {"こ", new string[2] {"ko", "co"} },
        {"ご", new string[1] {"go"} },
        {"さ", new string[1] {"sa"} },
        {"ざ", new string[1] {"za"} },
        {"し", new string[3] {"ci", "si", "shi"} },
        {"しぃ", new string[1] {"syi"} },
        {"しぇ", new string[2] {"she", "sye"} },
        {"しゃ", new string[2] {"sha", "sya"} },
        {"しゅ", new string[2] {"shu", "syu"} },
        {"しょ", new string[2] {"sho", "syo"} },
        {"じ", new string[2] {"ji", "zi"} },
        {"じぃ", new string[2] {"jyi", "zyi"} },
        {"じぇ", new string[3] {"je", "jye", "zye"} },
        {"じゃ", new string[3] {"ja" ,"jya", "zya"} },
        {"じゅ", new string[3] {"ju" ,"jyu", "zyu"} },
        {"じょ", new string[3] {"jo", "jyo", "zyo"} },
        {"す", new string[1] {"su"} },
        {"すぁ", new string[1] {"swa"} },
        {"すぃ", new string[1] {"swi"} },
        {"すぅ", new string[1] {"swu"} },
        {"すぇ", new string[1] {"swe"} },
        {"すぉ", new string[1] {"swo"} },
        {"ず", new string[1] {"zu"} },
        {"せ", new string[2] {"ce", "se"} },
        {"ぜ", new string[1] {"ze"} },
        {"そ", new string[1] {"so"} },
        {"ぞ", new string[1] {"zo"} },
        {"た", new string[1] {"ta"} },
        {"だ", new string[1] {"da"} },
        {"ち", new string[2] {"chi", "ti"} },
        {"ちぃ", new string[2] {"tyi", "cyi"} },
        {"ちぇ", new string[3] {"che", "cye", "tye"} },
        {"ちゃ", new string[3] {"cha", "cya", "tya"} },
        {"ちゅ", new string[3] {"chu", "cyu", "tyu"} },
        {"ちょ", new string[3] {"cho", "cyo", "tyo"} },
        {"ぢ", new string[1] {"di"} },
        {"ぢぃ", new string[1] {"dyi"} },
        {"ぢぇ", new string[1] {"dye"} },
        {"ぢゃ", new string[1] {"dya"} },
        {"ぢゅ", new string[1] {"dyu"} },
        {"ぢょ", new string[1] {"dyo"} },
        {"っ", new string[4] {"ltsu", "ltu", "xtsu", "xtu"} },
        {"つ", new string[2] {"tu", "tsu"} },
        {"つぁ", new string[1] {"tsa"} },
        {"つぃ", new string[1] {"tsi"} },
        {"つぇ", new string[1] {"tse"} },
        {"つぉ", new string[1] {"tso"} },
        {"づ", new string[1] {"du"} },
        {"て", new string[1] {"te"} },
        {"てぃ", new string[1] {"thi"} },
        {"てぇ", new string[1] {"the"} },
        {"てゃ", new string[1] {"tha"} },
        {"てゅ", new string[1] {"thu"} },
        {"てょ", new string[1] {"tho"} },
        {"で", new string[1] {"de"} },
        {"でぃ", new string[1] {"dhi"} },
        {"でぇ", new string[1] {"dhe"} },
        {"でゃ", new string[1] {"dha"} },
        {"でゅ", new string[1] {"dhu"} },
        {"でょ", new string[1] {"dho"} },
        {"と", new string[1] {"to"} },
        {"とぁ", new string[1] {"twa"} },
        {"とぃ", new string[1] {"twi"} },
        {"とぅ", new string[1] {"twu"} },
        {"とぇ", new string[1] {"twe"} },
        {"とぉ", new string[1] {"two"} },
        {"ど", new string[1] {"do"} },
        {"どぁ", new string[1] {"dwa"} },
        {"どぃ", new string[1] {"dwi"} },
        {"どぅ", new string[1] {"dwu"} },
        {"どぇ", new string[1] {"dwe"} },
        {"どぉ", new string[1] {"dwo"} },
        {"な", new string[1] {"na"} },
        {"に", new string[1] {"ni"} },
        {"にぃ", new string[1] {"nyi"} },
        {"にぇ", new string[1] {"nye"} },
        {"にゃ", new string[1] {"nya"} },
        {"にゅ", new string[1] {"nyu"} },
        {"にょ", new string[1] {"nyo"} },
        {"ぬ", new string[1] {"nu"} },
        {"ね", new string[1] {"ne"} },
        {"の", new string[1] {"no"} },
        {"は", new string[1] {"ha"} },
        {"ば", new string[1] {"ba"} },
        {"ぱ", new string[1] {"pa"} },
        {"ひ", new string[1] {"hi"} },
        {"ひぃ", new string[1] {"hyi"} },
        {"ひぇ", new string[1] {"hye"} },
        {"ひゃ", new string[1] {"hya"} },
        {"ひゅ", new string[1] {"hyu"} },
        {"ひょ", new string[1] {"hyo"} },
        {"び", new string[1] {"bi"} },
        {"びぃ", new string[1] {"byi"} },
        {"びぇ", new string[1] {"bye"} },
        {"びゃ", new string[1] {"bya"} },
        {"びゅ", new string[1] {"byu"} },
        {"びょ", new string[1] {"byo"} },
        {"ぴ", new string[1] {"pi"} },
        {"ぴぃ", new string[1] {"pyi"} },
        {"ぴぇ", new string[1] {"pye"} },
        {"ぴゃ", new string[1] {"pya"} },
        {"ぴゅ", new string[1] {"pyu"} },
        {"ぴょ", new string[1] {"pyo"} },
        {"ふ", new string[2] {"fu", "hu"} },
        {"ふぁ", new string[2] {"fa", "fwa"} },
        {"ふぃ", new string[3] {"fi", "fwi", "fyi"} },
        {"ふぅ", new string[1] {"fwu"} },
        {"ふぇ", new string[3] {"fe", "fwe", "fye"} },
        {"ふぉ", new string[2] {"fo", "fwo"} },
        {"ふゃ", new string[1] {"fya"} },
        {"ふゅ", new string[1] {"fyu"} },
        {"ふょ", new string[1] {"fyo"} },
        {"ぶ", new string[1] {"bu"} },
        {"ぷ", new string[1] {"pu"} },
        {"へ", new string[1] {"he"} },
        {"べ", new string[1] {"be"} },
        {"ぺ", new string[1] {"pe"} },
        {"ほ", new string[1] {"ho"} },
        {"ぼ", new string[1] {"bo"} },
        {"ぽ", new string[1] {"po"} },
        {"ま", new string[1] {"ma"} },
        {"み", new string[1] {"mi"} },
        {"みぃ", new string[1] {"myi"} },
        {"みぇ", new string[1] {"mye"} },
        {"みゃ", new string[1] {"mya"} },
        {"みゅ", new string[1] {"myu"} },
        {"みょ", new string[1] {"myo"} },
        {"む", new string[1] {"mu"} },
        {"め", new string[1] {"me"} },
        {"も", new string[1] {"mo"} },
        {"ゃ", new string[2] {"lya", "xya"} },
        {"や", new string[1] {"ya"} },
        {"ゅ", new string[2] {"lyu", "xyu"} },
        {"ゆ", new string[1] {"yu"} },
        {"ょ", new string[2] {"lyo", "xyo"} },
        {"よ", new string[1] {"yo"} },
        {"ら", new string[1] {"ra"} },
        {"り", new string[1] {"ri"} },
        {"りぃ", new string[1] {"ryi"} },
        {"りぇ", new string[1] {"rye"} },
        {"りゃ", new string[1] {"rya"} },
        {"りゅ", new string[1] {"ryu"} },
        {"りょ", new string[1] {"ryo"} },
        {"る", new string[1] {"ru"} },
        {"れ", new string[1] {"re"} },
        {"ろ", new string[1] {"ro"} },
        {"ゎ", new string[2] {"lwa", "xwa"} },
        {"わ", new string[1] {"wa"} },
        {"ゐ", new string[1] {"wyi"} },
        {"ゑ", new string[1] {"wye"} },
        {"を", new string[1] {"wo"} },
        {"ん", new string[3] {"nn", "xn", "n"} },
        {"ヴ", new string[1] {"vu"} },
        {"ヴぁ", new string[1] {"va"} },
        {"ヴぃ", new string[2] {"vi", "vyi"} },
        {"ヴぇ", new string[2] {"ve", "vye"} },
        {"ヴぉ", new string[1] {"vo"} },
        {"ヴゃ", new string[1] {"vya"} },
        {"ヴゅ", new string[1] {"vyu"} },
        {"ヴょ", new string[1] {"vyo"} },        
        {"んあ", new string[1] {"nna"} },        
        {"んい", new string[2] {"nni", "nnyi"} },        
        {"んう", new string[3] {"nnu", "nwhu", "nwu"} },                        
        {"んえ", new string[1] {"nne"} },        
        {"んお", new string[1] {"nno"} },
        {"んな", new string[1] {"nnna"} },
        {"んに", new string[1] {"nnni"} },
        {"んぬ", new string[1] {"nnnu"} },
        {"んえ", new string[1] {"nnne"} },
        {"んの", new string[1] {"nnno"} },
        {"んや", new string[1] {"nnya"} },
        {"んゆ", new string[1] {"nnyu"} }

    };
}
