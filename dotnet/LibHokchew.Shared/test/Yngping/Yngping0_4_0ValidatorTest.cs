using Xunit;

namespace LibHokchew.Shared.Yngping
{

    public class Yngping0_4_0ValidatorTest
    {
        [Theory]
        [InlineData("nguai55")]
        [InlineData("neoyng53")]
        [InlineData("cinj24")]
        [InlineData("njing33")]
        [InlineData("meo33")]
        [InlineData("ng242")]
        public void ValidSyllables_ShouldPass(string syllable)
        {
            Assert.True(Yngping0_4_0Validator.CheckHukziuSyllable(syllable));
        }

        [Theory]
        [InlineData("nguai43")]
        [InlineData("a212")]
        [InlineData("ka234")]
        [InlineData("meo22")]
        [InlineData("o1")]
        public void InvalidTone_ShouldFail(string syllable)
        {
            Assert.False(Yngping0_4_0Validator.CheckHukziuSyllable(syllable));
        }

        [Theory]
        [InlineData("fuai55")]
        public void InvalidInitial_ShouldFail(string syllable)
        {
            Assert.False(Yngping0_4_0Validator.CheckHukziuSyllable(syllable));
        }

        [Theory]
        [InlineData("moe55")]
        [InlineData("loey55")]
        public void InvalidRime_ShouldFail(string syllable)
        {
            Assert.False(Yngping0_4_0Validator.CheckHukziuSyllable(syllable));
        }

        [Theory]
        [InlineData("kat5")]
        public void InvalidCoda_ShouldFail(string syllable)
        {
            Assert.False(Yngping0_4_0Validator.CheckHukziuSyllable(syllable));
        }

        [Theory]
        [InlineData("a33", "", "a", "33")]
        [InlineData("nguai55", "ng", "uai", "55")]
        [InlineData("neoyng53", "n", "eoyng", "53")]
        [InlineData("cinj24", "c", "inj", "24")]
        [InlineData("njing33", "nj", "ing", "33")]
        [InlineData("meo33", "m", "eo", "33")]
        [InlineData("ngiau55", "ng", "iau", "55")]
        [InlineData("zooung242", "z", "ooung", "242")]
        [InlineData("ng242", "ng", "", "242")]
        public void ParseHukziuSyllable(string syllable, string initial, string final, string tone)
        {
            Assert.Equal((initial, final, tone),
                Yngping0_4_0Validator.TryParseHukziuSyllable(syllable));
        }
    }
}
