using System;
using System.Collections.Generic;
using LibHokchew.Protos;

namespace LibHokchew.Shared
{
    public static class CikLingUtil
    {
        private static readonly IDictionary<char, Initial> CharToInitial = new Dictionary<char, Initial>() {
            {'柳', Initial.L},
            {'邊', Initial.B},
            {'边', Initial.B},
            {'求', Initial.G},
            {'氣', Initial.K},
            {'气', Initial.K},
            {'低', Initial.D},
            {'波', Initial.P},
            {'他', Initial.T},
            {'曾', Initial.Z},
            {'日', Initial.N},
            {'時', Initial.S},
            {'时', Initial.S},
            {'鶯', Initial.None},
            {'莺', Initial.None},
            {'蒙', Initial.M},
            {'語', Initial.Ng},
            {'语', Initial.Ng},
            {'出', Initial.C},
            {'非', Initial.H}
        };

        private static readonly IDictionary<char, Final> CharToFinal = new Dictionary<char, Final>() {
            {'春', Final.Ung},
            {'公', Final.Ung},
            {'花', Final.Ua},
            {'瓜', Final.Ua},
            {'香', Final.Yong},
            {'姜', Final.Yong},
            {'秋', Final.Iu},
            {'周', Final.Iu},
            {'山', Final.Ang},
            {'干', Final.Ang},
            {'開', Final.Ai},
            {'开', Final.Ai},
            {'哉', Final.Ai},
            {'嘉', Final.A},
            {'佳', Final.A},
            {'賓', Final.Ing},
            {'宾', Final.Ing},
            {'京', Final.Ing},
            {'歡', Final.Uang},
            {'欢', Final.Uang},
            {'官', Final.Uang},
            {'歌', Final.O},
            {'高', Final.O},
            {'須', Final.Y},
            {'须', Final.Y},
            {'車', Final.Y},
            {'车', Final.Y},
            {'杯', Final.Uoi},
            {'盃', Final.Uoi},
            {'孤', Final.U},
            {'姑', Final.U},
            {'燈', Final.Eing},
            {'灯', Final.Eing},
            {'庚', Final.Eing},
            {'光', Final.Uong},
            {'輝', Final.Ui},
            {'辉', Final.Ui},
            {'龜', Final.Ui},
            {'龟', Final.Ui},
            {'燒', Final.Ieu},
            {'烧', Final.Ieu},
            {'嬌', Final.Ieu},
            {'娇', Final.Ieu},
            {'銀', Final.Yng},
            {'银', Final.Yng},
            {'恭', Final.Yng},
            {'釭', Final.Ong},
            {'綱', Final.Ong},
            {'纲', Final.Ong},
            {'之', Final.I},
            {'箕', Final.I},
            {'東', Final.Oeng},
            {'东', Final.Oeng},
            {'江', Final.Oeng},
            {'郊', Final.Au},
            {'交', Final.Au},
            {'過', Final.Uo},
            {'过', Final.Uo},
            {'朱', Final.Uo},
            {'西', Final.E},
            {'街', Final.E},
            {'橋', Final.Io},
            {'桥', Final.Io},
            {'嬝', Final.Io},
            {'袅', Final.Io},
            {'雞', Final.Ie},
            {'鸡', Final.Ie},
            {'圭', Final.Ie},
            {'聲', Final.Iang},
            {'声', Final.Iang},
            {'正', Final.Iang},
            {'催', Final.Oey},
            {'初', Final.Oe},
            {'梳', Final.Oe},
            {'天', Final.Ieng},
            {'堅', Final.Ieng},
            {'坚', Final.Ieng},
            {'奇', Final.Ia},
            {'迦', Final.Ia},
            {'歪', Final.Uai},
            {'乖', Final.Uai},
            {'溝', Final.Eu},
            {'沟', Final.Eu},
            {'勾', Final.Eu},
        };   

        private static readonly IDictionary<string, Tone> StringToTone = new Dictionary<string, Tone>() {
            {"上平", Tone.UpLevel},
            {"上上", Tone.UpUp},
            {"上去", Tone.UpFalling},
            {"上入", Tone.UpAbrupt},
            {"下平", Tone.DownLevel},
            {"下上", Tone.UpUp}, // 下上调无字
            {"下去", Tone.DownFalling},
            {"下入", Tone.DownAbrupt}
        };

        private static readonly IDictionary<Initial, string> CikLingInitialToYngPing = new Dictionary<Initial, string>() {
            {Initial.L, "l"},
            {Initial.B, "b"},
            {Initial.G, "g"},
            {Initial.K, "k"},
            {Initial.D, "d"},
            {Initial.P, "p"},
            {Initial.T, "t"},
            {Initial.Z, "z"},
            {Initial.N, "n"},
            {Initial.S, "s"},
            {Initial.None, ""},
            {Initial.M, "m"},
            {Initial.Ng, "ng"},
            {Initial.C, "c"},
            {Initial.H, "h"},
        };

        private static readonly IDictionary<Tone, string> CikLingToneToYngPing = new Dictionary<Tone, string>() {
            {Tone.UpLevel, "55"},
            {Tone.UpUp, "33"},
            {Tone.UpFalling, "213"},
            {Tone.UpAbrupt, "24"},
            {Tone.DownLevel, "53"},
            {Tone.DownFalling, "242"},
            {Tone.DownAbrupt, "5"},
        };

        private static readonly ISet<Initial> YoUoInitials = new HashSet<Initial> {
            Initial.D,
            Initial.T,
            Initial.N,
            Initial.L,
            Initial.S,
            Initial.Z,
            Initial.C
        };

        public static string ToYngPingHokchew(string cikling) {
            if (cikling.Length != 4) {
                throw new ArgumentException($"{cikling} 不是戚林反切");
            }
            var initialChar = cikling[0];
            var finalChar = cikling[1];
            var toneStr = cikling.Substring(2);
            if (!CharToInitial.ContainsKey(initialChar)) {
                throw new ArgumentException($"{initialChar} 不是戚林声母");
            }
            if (!CharToFinal.ContainsKey(finalChar)) {
                throw new ArgumentException($"{finalChar} 不是戚林韵母");
            }
            if (!StringToTone.ContainsKey(toneStr)) {
                throw new ArgumentException($"{toneStr} 不是戚林声调");
            }
            var initial = CharToInitial[initialChar];
            var final = CharToFinal[finalChar];
            var tone = StringToTone[toneStr];
            return ToYngPingHokchew(initial, final, tone);
        }

        public static string ToYngPingHokchew(Initial initial, Final final, Tone tone) {
            return CikLingInitialToYngPing[initial] 
                + GetYngPingHokchewFinal(MaybeYoToUo(initial, final), tone) 
                + CikLingToneToYngPing[tone];
        }

        public static string ToYngPingHokchewToneless(string ciklingToneless) {
            if (ciklingToneless.Length != 2) {
                throw new ArgumentException($"{ciklingToneless} 不是戚林反切");
            }
            var initialChar = ciklingToneless[0];
            var finalChar = ciklingToneless[1];
            if (!CharToInitial.ContainsKey(initialChar)) {
                throw new ArgumentException($"{initialChar} 不是戚林声母");
            }
            if (!CharToFinal.ContainsKey(finalChar)) {
                throw new ArgumentException($"{finalChar} 不是戚林韵母");
            }
            var initial = CharToInitial[initialChar];
            var final = CharToFinal[finalChar];
            return ToYngPingHokchewToneless(initial, final);
        }

        public static string ToYngPingHokchewToneless(Initial initial, Final final) {
            return CikLingInitialToYngPing[initial] + GetYngPingHokchewFinal(
                MaybeYoToUo(initial, final), 
                Tone.UpLevel);
        }

        private static Final MaybeYoToUo(Initial initial, Final final) {
            if (final == Final.Yong && YoUoInitials.Contains(initial)) {
                return Final.Uong;
            }
            if (final == Final.Io && YoUoInitials.Contains(initial)) {
                return Final.Uo;
            }        
            return final;
        }

        private static string GetYngPingHokchewFinal(Final final, Tone tone) {
            switch (tone) {
                // 紧
                case Tone.UpLevel:
                case Tone.UpUp:
                case Tone.DownLevel:
                    switch (final) {
                        case Final.Ung:
                            return "ung";
                        case Final.Ua:
                            return "ua";
                        case Final.Yong:
                            return "yong";
                        case Final.Iu:
                            return "iu";
                        case Final.Ang:
                            return "ang";
                        case Final.Ai:
                            return "ai";
                        case Final.A:
                            return "a";
                        case Final.Ing:
                            return "ing";
                        case Final.Uang:
                            return "uang";
                        case Final.O:
                            return "o";
                        case Final.Y:
                            return "y";
                        case Final.Uoi:
                            return "ui";
                        case Final.U:
                            return "u";
                        case Final.Eing:
                            return "eing";
                        case Final.Uong:
                            return "uong";
                        case Final.Ui:
                            return "ui";
                        case Final.Ieu:
                            return "iu";
                        case Final.Yng:
                            return "yng";
                        case Final.Ong:
                            return "oung";
                        case Final.I:
                            return "i";
                        case Final.Oeng:
                            return "eoyng";
                        case Final.Au:
                            return "au";
                        case Final.Uo:
                            return "uo";
                        case Final.E:
                            return "e";
                        case Final.Io:
                            return "yo";
                        case Final.Ie:
                            return "ie";
                        case Final.Iang:
                            return "iang";
                        case Final.Oey:
                            return "eoy";
                        case Final.Oe:
                            return "eo";
                        case Final.Ieng:
                            return "ieng";
                        case Final.Ia:
                            return "ia";
                        case Final.Uai:
                            return "uai";
                        case Final.Eu:
                            return "eu";
                    }
                   throw new Exception("The impossible happened");
                // 紧入
                case Tone.DownAbrupt:
                    switch (final) {
                        case Final.Ung:
                            return "uk";
                        case Final.Ua:
                            return "uah";
                        case Final.Yong:
                            return "yok";
                        case Final.Iu:
                            return "iuh";
                        case Final.Ang:
                            return "ak";
                        case Final.Ai:
                            return "aih";
                        case Final.A:
                            return "ah";
                        case Final.Ing:
                            return "ik";
                        case Final.Uang:
                            return "uak";
                        case Final.O:
                            return "oh";
                        case Final.Y:
                            return "yh";
                        case Final.Uoi:
                            return "uih";
                        case Final.U:
                            return "uh";
                        case Final.Eing:
                            return "eik";
                        case Final.Uong:
                            return "uok";
                        case Final.Ui:
                            return "uih";
                        case Final.Ieu:
                            return "iuh";
                        case Final.Yng:
                            return "yk";
                        case Final.Ong:
                            return "ouk";
                        case Final.I:
                            return "ih";
                        case Final.Oeng:
                            return "eoyk";
                        case Final.Au:
                            return "auh";
                        case Final.Uo:
                            return "uoh";
                        case Final.E:
                            return "eh";
                        case Final.Io:
                            return "yoh";
                        case Final.Ie:
                            return "ieh";
                        case Final.Iang:
                            return "iak";
                        case Final.Oey:
                            return "eoyh";
                        case Final.Oe:
                            return "eoh";
                        case Final.Ieng:
                            return "iek";
                        case Final.Ia:
                            return "ieh";
                        case Final.Uai:
                            return "uaih";
                        case Final.Eu:
                            return "euh";
                    }
                   throw new Exception("The impossible happened");
                // 松
                case Tone.UpFalling:
                case Tone.DownFalling:
                    switch (final) {
                        case Final.Ung:
                            return "oung";
                        case Final.Ua:
                            return "ua";
                        case Final.Yong:
                            return "yong";
                        case Final.Iu:
                            return "iu";
                        case Final.Ang:
                            return "ang";
                        case Final.Ai:
                            return "ai";
                        case Final.A:
                            return "a";
                        case Final.Ing:
                            return "eing";
                        case Final.Uang:
                            return "uang";
                        case Final.O:
                            return "oo";
                        case Final.Y:
                            return "eoy";
                        case Final.Uoi:
                            return "ui";
                        case Final.U:
                            return "ou";
                        case Final.Eing:
                            return "aing";
                        case Final.Uong:
                            return "uong";
                        case Final.Ui:
                            return "ui";
                        case Final.Ieu:
                            return "iu";
                        case Final.Yng:
                            return "eoyng";
                        case Final.Ong:
                            return "ooung";
                        case Final.I:
                            return "ei";
                        case Final.Oeng:
                            return "ooyng";
                        case Final.Au:
                            return "au";
                        case Final.Uo:
                            return "uo";
                        case Final.E:
                            return "a";
                        case Final.Io:
                            return "yo";
                        case Final.Ie:
                            return "ie";
                        case Final.Iang:
                            return "iang";
                        case Final.Oey:
                            return "ooy";
                        case Final.Oe:
                            return "oo";
                        case Final.Ieng:
                            return "ieng";
                        case Final.Ia:
                            return "ia";
                        case Final.Uai:
                            return "uai";
                        case Final.Eu:
                            return "au";
                    }
                   throw new Exception("The impossible happened");
                // 松入
                case Tone.UpAbrupt:
                    switch (final) {
                        case Final.Ung:
                            return "ouk";
                        case Final.Ua:
                            return "uah";
                        case Final.Yong:
                            return "yok";
                        case Final.Iu:
                            return "iuh";
                        case Final.Ang:
                            return "ak";
                        case Final.Ai:
                            return "aih";
                        case Final.A:
                            return "ah";
                        case Final.Ing:
                            return "eik";
                        case Final.Uang:
                            return "uak";
                        case Final.O:
                            return "ooh";
                        case Final.Y:
                            return "eoyh";
                        case Final.Uoi:
                            return "uih";
                        case Final.U:
                            return "ouh";
                        case Final.Eing:
                            return "aik";
                        case Final.Uong:
                            return "uok";
                        case Final.Ui:
                            return "uih";
                        case Final.Ieu:
                            return "iuh";
                        case Final.Yng:
                            return "eoyk";
                        case Final.Ong:
                            return "oouk";
                        case Final.I:
                            return "eih";
                        case Final.Oeng:
                            return "ooyk";
                        case Final.Au:
                            return "auh";
                        case Final.Uo:
                            return "uoh";
                        case Final.E:
                            return "ah";
                        case Final.Io:
                            return "yoh";
                        case Final.Ie:
                            return "ieh";
                        case Final.Iang:
                            return "iak";
                        case Final.Oey:
                            return "ooyh";
                        case Final.Oe:
                            return "ooh";
                        case Final.Ieng:
                            return "iek";
                        case Final.Ia:
                            return "ieh";
                        case Final.Uai:
                            return "uaih";
                        case Final.Eu:
                            return "auh";
                    }
                   throw new Exception("The impossible happened");
            }
            throw new Exception("The impossible happened");
        }

    }
}
