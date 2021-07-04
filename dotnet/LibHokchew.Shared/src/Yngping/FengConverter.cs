
using System.Collections.Generic;

namespace LibHokchew.Shared.Yngping
{

    /// <summary>
    /// 将榕拼转换为冯爱珍版《福州方言词典》所用国际音标的 static helper.
    /// 
    /// 对照表： https://shimo.im/sheets/vXtJtdCtjHgDV83Y/2cwTj
    /// </summary>
    public static class FengConverter
    {

        private static readonly Dictionary<string, string> InitialMapping = new Dictionary<string, string> {
            {"b","p"},
            {"p", "p'"},
            {"m", "m"},
            {"d", "t"},
            {"t", "t'"},
            {"n", "n"},
            {"l", "l"},
            {"s", "s"},
            {"z", "ts"},
            {"c", "ts'"},
            {"g", "k"},
            {"k", "k'"},
            {"ng", "ŋ"},
            {"h", "h"},
            {"w", "β"},
            {"j", "ʒ"},
            {"nj", "ʒ"},
        };

        private static readonly Dictionary<string, string> FinalMapping = new Dictionary<string, string> {
            {"i", "i"},
            {"ei", "ɛi"},
            {"u", "u"},
            {"ou", "ou"},
            {"y", "y"},
            {"eoy", "øy"},
            {"a", "a"}, // TODO: 97-98 西松韵，需要变成ɑ
            {"ia", "ia"},
            {"ua", "ua"},
            {"e", "ɛ"},
            {"ie", "ie"},
            {"o", "o"}, // TODO: 143 初松韵，需要变成ɔ
            {"uo", "uo"},
            {"yo", "yo"},
            {"eo", "œ"},
            {"ooy", "ɔy"},
            {"ai", "ai"},
            {"uai", "uai"},
            {"au", "au"},
            {"iau", "iau"},
            {"eu", "ɛu"},
            {"ieu", "ieu"},
            {"ui", "uoi"},
            {"ing", "iŋ"},
            {"eing", "ɛiŋ"},
            {"ung", "uŋ"},
            {"oung", "ouŋ"},
            {"yng", "yŋ"},
            {"eoyng", "øyŋ"},
            {"ang", "aŋ"},
            {"iang", "iaŋ"},
            {"uang", "uaŋ"},
            {"ieng", "ieŋ"},
            {"uong", "uoŋ"},
            {"yong", "yoŋ"},
            {"ooyng", "ɔyŋ"},
            {"aing", "aiŋ"},
            {"ooung", "ɔuŋ"},
            {"ng", "ŋ̍"}, // TODO: 区分 ŋ̍ n̩ m̩̩
            {"ik", "iʔ"},
            {"ih", "iʔ"},
            {"eik", "ɛiʔ"},
            {"eih", "ɛiʔ"},
            {"uk", "uʔ"},
            {"uh", "uʔ"},
            {"ouk", "ouʔ"},
            {"ouh", "ouʔ"},
            {"yk", "yʔ"},
            {"yh", "yʔ"},
            {"eoyk", "øyʔ"},
            {"eoyh", "øyʔ"},
            {"ak", "aʔ"},
            {"ah", "aʔ"},
            {"iak", "iaʔ"},
            {"iah", "iaʔ"},
            {"uak", "uaʔ"},
            {"uah", "uaʔ"},
            {"ek", "ɛʔ"},
            {"eh", "ɛʔ"},
            {"iek", "ieʔ"},
            {"ieh", "ieʔ"},
            {"ok", "oʔ"},
            {"oh", "oʔ"},
            {"uok", "uoʔ"},
            {"uoh", "uoʔ"},
            {"yok", "yoʔ"},
            {"yoh", "yoʔ"},
            {"eok", "œʔ"},
            {"eoh", "œʔ"},
            {"aik", "aiʔ"},
            {"aih", "aiʔ"},
            {"oouk", "ɔuʔ"},
            {"oouh", "ɔuʔ"},
            {"ooyk", "ɔyʔ"},
            {"ooyh", "ɔyʔ"},
        };

        private static readonly Dictionary<string, string> ToneMapping = new Dictionary<string, string> {
            {"55", "˥˥"},
            {"33", "˧˧"},
            {"213", "˨˩˨"},
            {"24", "˨˦"},
            {"53", "˥˧"},
            {"242", "˨˦˨"},
            {"5", "˥"},
            {"21", "˨˩"},
        };


        /// <summary>
        /// 如果转换失败，返回 null.
        /// </summary>
        public static string ToFeng(string yngpingInitial, string yngpingFinal, string yngpingTone)
        {
            var i = yngpingInitial.ToLowerInvariant().Trim();
            var f = yngpingFinal.ToLowerInvariant().Trim();
            var t = yngpingTone.ToLowerInvariant().Trim();
            if (InitialMapping.ContainsKey(i) && FinalMapping.ContainsKey(f) && ToneMapping.ContainsKey(t))
            {
                return InitialMapping[i] + FinalMapping[f] + ToneMapping[t];
            }
            return null;
        }

        /// <summary>
        /// 如果转换失败，返回 null.
        /// </summary> 
        public static string ToFeng(string yngping)
        {
            var parsed = Yngping0_4_0Validator.ParseHukziuSyllable(yngping);
            if (parsed.HasValue)
            {
                return ToFeng(parsed.Value.Item1, parsed.Value.Item2, parsed.Value.Item3);
            }
            return null;
        }
    }
}