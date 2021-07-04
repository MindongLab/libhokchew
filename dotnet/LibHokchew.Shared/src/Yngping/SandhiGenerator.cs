using System.Collections.Generic;

namespace LibHokchew.Shared.Yngping
{

    /// <summary>
    /// 连读计算器.
    /// </summary>
    public static class SandhiGenerator
    {

        private static readonly ISet<char> VowelLetters = new HashSet<char>() { 'a', 'o', 'e', 'y', 'u', 'i' };

        private static readonly IDictionary<(string, string), string> ToneSandhiMap = new Dictionary<(string, string), string>() {
            // 前字 a55
            {("55","55"), "55"},
            {("55","33"), "53"},
            {("55","213"), "53"},
            {("55","24"), "53"},
            {("55","53"), "55"},
            {("55","242"), "53"},
            {("55","5"), "55"},
            // 前字 a33
            {("33","55"), "21"},
            {("33","33"), "24"},
            {("33","213"), "55"},
            {("33","24"), "55"},
            {("33","53"), "21"},
            {("33","242"), "55"},
            {("33","5"), "21"},
            // 前字 a213
            {("213","55"), "55"},
            {("213","33"), "53"},
            {("213","213"), "53"},
            {("213","24"), "53"},
            {("213","53"), "55"},
            {("213","242"), "53"},
            {("213","5"), "55"},
            // 前字 ah24
            {("24","55"), "55"},
            {("24","33"), "53"},
            {("24","213"), "53"},
            {("24","24"), "53"},
            {("24","53"), "55"},
            {("24","242"), "53"},
            {("24","5"), "55"},
            // 前字 a53
            {("53","55"), "55"},
            {("53","33"), "33"},
            {("53","213"), "21"},
            {("53","24"), "21"},
            {("53","53"), "33"},
            {("53","242"), "21"},
            {("53","5"), "33"},
            // 前字 a242
            {("242","55"), "55"},
            {("242","33"), "53"},
            {("242","213"), "53"},
            {("242","24"), "53"},
            {("242","53"), "55"},
            {("242","242"), "53"},
            {("242","5"), "55"},
            // 前字 ak5/ah5
            {("5","55"), "55"},
            {("5","33"), "33"},
            {("5","213"), "21"},
            {("5","24"), "21"},
            {("5","53"), "33"},
            {("5","242"), "21"},
            {("5","5"), "33"}
        };

        private static readonly IDictionary<(string, string), string> ToneSandhiMapForAk = new Dictionary<(string, string), string>() {
            // 前字 ak24
            {("24","55"), "21"},
            {("24","33"), "24"},
            {("24","213"), "55"},
            {("24","24"), "55"},
            {("24","53"), "21"},
            {("24","242"), "55"},
            {("24","5"), "21"},
        };

        /// <summary>
        /// 松韵还原成紧韵.
        /// </summary>
        private static readonly IDictionary<string, string> VowelAlternation = new Dictionary<string, string>() {
            {"oo", "o"},
            {"ooy","eoy"},
            // TODO: {"oo","eo"},
            {"a","e"},
            // TODO: {"au","eu"},
            {"ei","i"},
            {"ou","u"},
            {"eoy", "y"},
            {"ooung","oung"},
            {"ooyng","eoyng"},
            {"aing","eing"},
            {"eing","ing"},
            {"eoyng","yng"},
            {"ooh","oh"},
            {"oouk","ouk"},
            {"ooyk","eoyk"},
            {"aik","eik"},
            {"eih","ih"},
            {"eik","ik"},
            {"ouh","uh"},
            {"ouk","uk"},
            {"eoyh","yh"},
            {"eoyk","yk"}
        };

        /// <summary>
        /// 松韵调
        /// <returns></returns>
        private static ISet<string> LooseTones = new HashSet<string>() {
            "213",
            "24",
            "242"
        };

        public static string[] GenerateSandhiSyllables(IList<string> original)
        {
            if (original.Count < 2)
            {
                return new List<string>(original).ToArray();
            }
            if (original.Count == 2)
            {
                var (first, second) = GenerateSandhiSyllables(original[0], original[1]);
                return new List<string> { first, second }.ToArray();
            }

            {
                var (first, second, third) = GenerateSandhiSyllables(
                    original[original.Count - 3],
                    original[original.Count - 2],
                    original[original.Count - 1]);
                if (original.Count == 3)
                {
                    return new List<string> { first, second, third }.ToArray();
                }
                var output = new string[original.Count];
                output[original.Count - 1] = third;
                output[original.Count - 2] = second;
                output[original.Count - 3] = first;
                for (var i = original.Count - 4; i >= 0; --i)
                {
                    var (initial, final, tone) = Yngping0_4_0Validator.TryParseHukziuSyllable(original[i]);
                    var shouldRestoreVowel = LooseTones.Contains(tone) && VowelAlternation.ContainsKey(final);
                    var newFinal = shouldRestoreVowel ? VowelAlternation[final] : final;
                    output[i] = initial + newFinal + "21";
                    var (initialNext, finalNext, toneNext) = Yngping0_4_0Validator.TryParseHukziuSyllable(output[i + 1]);
                    output[i + 1] = GetAssimilatedInitial(newFinal, initialNext) + finalNext + toneNext;
                }
                return output;
            }
        }

        public static (string, string, string) GenerateSandhiSyllables(string first, string second, string third)
        {
            var (initial1, final1, tone1) = Yngping0_4_0Validator.TryParseHukziuSyllable(first);
            var (initial2, final2, tone2) = Yngping0_4_0Validator.TryParseHukziuSyllable(second);
            var (initial3, final3, tone3) = Yngping0_4_0Validator.TryParseHukziuSyllable(third);
            var newTone2 = GetSandhiTone(tone2, final2.EndsWith("k"), tone3);
            var newTone1 =
                (tone2 == "5" || tone2 == "53")
                ? GetSandhiTone(tone1, final1.EndsWith("k"), newTone2)
                : "21";
            var shouldRestoreVowel1 = LooseTones.Contains(tone1) && VowelAlternation.ContainsKey(final1);
            var newFinal1 = shouldRestoreVowel1 ? VowelAlternation[final1] : final1;
            var shouldRestoreVowel2 = LooseTones.Contains(tone2) && VowelAlternation.ContainsKey(final2);
            var newFinal2 = shouldRestoreVowel2 ? VowelAlternation[final2] : final2;
            var newInitial2 = GetAssimilatedInitial(newFinal1, initial2);
            var newInitial3 = GetAssimilatedInitial(newFinal2, initial3);
            return (initial1 + newFinal1 + newTone1, newInitial2 + newFinal2 + newTone2, newInitial3 + final3 + tone3);
        }

        /// <summary>
        /// 生成两字连读.
        /// </summary>
        /// <param name="first">前字单字音榕拼</param>
        /// <param name="second">后字单字音榕拼</param>
        /// <returns>连读后两字榕拼</returns>
        public static (string, string) GenerateSandhiSyllables(string first, string second)
        {
            var (initial1, final1, tone1) = Yngping0_4_0Validator.TryParseHukziuSyllable(first);
            var (initial2, final2, tone2) = Yngping0_4_0Validator.TryParseHukziuSyllable(second);
            var newTone1 = GetSandhiTone(tone1, final1.EndsWith("k"), tone2);
            // 需要还原紧韵
            var shouldRestoreVowel = LooseTones.Contains(tone1) && VowelAlternation.ContainsKey(final1);
            var newFinal1 = shouldRestoreVowel ? VowelAlternation[final1] : final1;
            var newInitial2 = GetAssimilatedInitial(newFinal1, initial2);
            return (initial1 + newFinal1 + newTone1, newInitial2 + final2 + tone2);
        }

        public static string GetAssimilatedInitial(string firstFinal, string secondInitial)
        {
            if (firstFinal.EndsWith("h") || VowelLetters.Contains(firstFinal[firstFinal.Length - 1]))
            {
                if (secondInitial == "b" || secondInitial == "p")
                {
                    return "w";
                }
                if (secondInitial == "d" || secondInitial == "t" || secondInitial == "l" || secondInitial == "s")
                {
                    return "l";
                }
                if (secondInitial == "z" || secondInitial == "c")
                {
                    return "j";
                }
                if (secondInitial == "g" || secondInitial == "k" || secondInitial == "h" || secondInitial == string.Empty)
                {
                    return string.Empty;
                }
            }
            if (firstFinal.EndsWith("ng"))
            {
                if (secondInitial == "b" || secondInitial == "p")
                {
                    return "m";
                }
                if (secondInitial == "d" || secondInitial == "t" || secondInitial == "l" || secondInitial == "s")
                {
                    return "n";
                }
                if (secondInitial == "z" || secondInitial == "c")
                {
                    return "nj";
                }
                if (secondInitial == "g" || secondInitial == "k" || secondInitial == "h" || secondInitial == string.Empty)
                {
                    return "ng";
                }
            }
            return secondInitial;
            // TODO: 后字轻声的情况
        }

        /// <summary>
        /// 获得两字连读，连读后前字调声调。
        /// </summary>
        /// <param name="tone1">前字调</param>
        /// <param name="isFirstCodaK">前字是否为k尾</param>
        /// <param name="tone2">后字调</param>
        /// <returns>连读后前字声调</returns>
        public static string GetSandhiTone(string tone1, bool isFirstCodaK, string tone2)
        {
            if (tone2 == "21")
            {
                tone2 = "213";
            }
            if (isFirstCodaK && tone1 == "24")
            {
                return ToneSandhiMapForAk[(tone1, tone2)];
            }
            return ToneSandhiMap[(tone1, tone2)];
        }
    }
}