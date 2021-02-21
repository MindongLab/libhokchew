using System;
using System.Collections.Generic;
using System.Linq;

namespace LibHokchew.Shared.Yngping
{

    /// <summary>
    /// Validator for Yngping 0.4.0 (Hukziu)
    /// https://yngping.zingzeu.org/spec/v0.4.0-preview2/hukziu.html
    /// </summary>
    public static class Yngping0_4_0Validator
    {

        /// <summary>
        /// 榕拼规范检查的例外。以下音节视为合法音节。
        /// </summary>
        private static readonly IDictionary<string, (string, string, string)> Exceptions = new Dictionary<string, (string, string, string)> {
            {"ng55",("ng","","55")},
            {"ng33",("ng","","33")},
            {"ng213",("ng","","213")},
            {"ng53",("ng","","53")},
            {"ng242",("ng","","242")},
            {"ng21",("ng","","21")},
            {"n242",("n","","242")},
            {"m242",("m","","242")},
        };

        private static readonly string[] AllowedInitials = new string[]
            {"b","p","m","d","t","n","l","s","z","c","g","k","ng","h",
            "w","l","j","nj",""};
        private static readonly string[] AllowedCodas = new string[] {
                "k","h","ng","nj",""
            };

        private static readonly string[] AllowedRimes = new string[] {
            "a","ia","ua","uai","ai","au","o","oo","yo","uo",
            "eoy","ooy","eo","oo","e","ie","eu","i","ei",
            "iu","u","ou","ui","y","eoy","oou",
            // 例外 https://github.com/ztl8702/yngping-rime/issues/68
            "iau"
        };

        private static readonly string[] AllowedTones = {
            "55", "33", "213", "24", "53", "242", "5", "0", "21"
        };

        private static readonly string[] AllowedInitialsSorted = AllowedInitials.OrderBy(x => -x.Length).ToArray();
        private static readonly string[] AllowedRimesSorted = AllowedRimes.OrderBy(x => -x.Length).ToArray();
        private static readonly string[] AllowedCodasSorted = AllowedCodas.OrderBy(x => -x.Length).ToArray();
        private static readonly string[] AllowedTonesSorted = AllowedTones.OrderBy(x => -x.Length).ToArray();

        /// <summary>
        /// Returns if the given syllable is a valid Hukziu syllable in Yngping 0.4.0.
        /// </summary>
        public static bool CheckHukziuSyllable(string syllable)
        {
            var tmp = ParseHukziuSyllable(syllable);
            return tmp != null;
        }

        public static (string, string, string) TryParseHukziuSyllable(string syllable)
        {
            var parsed = ParseHukziuSyllable(syllable);
            if (parsed == null)
            {
                throw new ArgumentException($"{syllable} is not a valid Yngping 0.4.0 syllable");
            }
            return parsed.Value;
        }

        public static (string, string, string)? ParseHukziuSyllable(string syllable)
        {
            if (Exceptions.ContainsKey(syllable))
            {
                return Exceptions[syllable];
            }
            var remaining = syllable.Trim();
            // Initial
            string initial;
            (initial, remaining) = TryConsume(remaining, AllowedInitialsSorted);
            if (initial == null)
            {
                return null;
            }
            // Rime
            string rime;
            (rime, remaining) = TryConsume(remaining, AllowedRimesSorted);
            if (rime == null)
            {
                return null;
            }
            // Coda
            string coda;
            (coda, remaining) = TryConsume(remaining, AllowedCodasSorted);
            if (coda == null)
            {
                return null;
            }
            // Tone
            string tone;
            (tone, remaining) = TryConsume(remaining, AllowedTonesSorted);
            if (tone == null)
            {
                return null;
            }
            if (remaining != string.Empty)
            {
                return null;
            }
            return (initial, rime + coda, tone);
        }

        private static (string, string) TryConsume(string s, string[] allowedTokens)
        {
            foreach (var i in allowedTokens)
            {
                if (s.StartsWith(i))
                {
                    return (i, s.Substring(i.Length));
                }
            }
            return (null, s);
        }
    }
}
