using Xunit;
using System.Linq;

namespace LibHokchew.Shared.Yngping
{

    public class SandhiGeneratorTest
    {
        [Theory]
        // 腹老
        [InlineData("bouk24", "lo33", "buk24", "lo33")]
        // 鉸刀
        [InlineData("ga55", "do55", "ga55", "lo55")]
        // 做風
        [InlineData("zoo213", "hung55", "zo55", "ung55")]
        // 階座
        [InlineData("gie55", "zoo242", "gie53", "joo242")]
        // 明旦
        [InlineData("mieng53", "dang213", "mieng21", "nang213")]
        // 粽箬
        [InlineData("zooyng213", "nuoh5", "zeoyng55", "nuoh5")]
        public void GenerateSandhiSyllables_TwoSyllables(string first, string second, string sandhiFirst, string sandhiSecond)
        {
            Assert.Equal(
                (sandhiFirst, sandhiSecond),
                SandhiGenerator.GenerateSandhiSyllables(first, second));
        }

        [Theory]
        // 福州話
        [InlineData("houk24 ziu55 ua242", "huk21 ziu53 ua242")]
        // 白馬河
        [InlineData("bah5 ma33 o53", "bah21 ma21 o53")]
        // 美國客
        [InlineData("mi33 guok24 kah24", "mi21 uok55 kah24")]
        // 野有味
        [InlineData("ia33 ou242 ei242", "ia21 u53 ei242")]
        // 晡時雨
        [InlineData("buo55 si53 y33", "buo53 li33 y33")]
        public void GenerateSandhiSyllables_ThreeSyllables(string original, string sandhi)
        {
            var o = original.Split();
            var s = sandhi.Split();
            Assert.Equal(
                (s[0], s[1], s[2]),
                SandhiGenerator.GenerateSandhiSyllables(o[0], o[1], o[2]));
        }

        [Theory]
        // 三十暝晡
        [InlineData("sang55 seik5 mang53 buo55", "sang21 neik55 mang55 muo55")]
        // 趁頭趁腦
        [InlineData("teing213 tau53 teing213 no33", "ting21 nau21 ling53 no33")]
        public void GenerateSandhiSyllables_FourAndMoreSyllables(string original, string sandhi)
        {
            var o = original.Split();
            var expected = sandhi.Split();
            Assert.Equal(
                expected,
                SandhiGenerator.GenerateSandhiSyllables(o));
        }
    }
}
