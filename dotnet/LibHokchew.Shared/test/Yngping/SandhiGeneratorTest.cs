using Xunit;

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
        [InlineData("mieng53","dang213", "mieng21","nang213")]
        // 粽箬
        [InlineData("zooyng213","nuoh5", "zeoyng55","nuoh5")]
        public void GenerateSandhiSyllables(string first,  string second, string sandhiFirst, string sandhiSecond)
        {
            Assert.Equal(
                (sandhiFirst,sandhiSecond),
                SandhiGenerator.GenerateSandhiSyllables(first,second));
        }
    }
}
