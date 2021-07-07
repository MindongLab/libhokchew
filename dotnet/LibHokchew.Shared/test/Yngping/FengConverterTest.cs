using Xunit;

namespace LibHokchew.Shared.Yngping
{

    public class FengConverterTest
    {
        [Theory]
        // 腹
        [InlineData("bouk24", "pouʔ˨˦")]
        // 赘
        [InlineData("tui53", "t'uoi˥˧")]
        // 闷
        [InlineData("moung242", "mouŋ˨˦˨")]
        [InlineData("oung242", "ouŋ˨˦˨")]
        public void ToFeng(string yngping, string fengIpa)
        {
            Assert.Equal(fengIpa, FengConverter.ToFeng(yngping));
        }

        public void ToFeng_ReturnsNullOnError(string yngping, string fengIpa)
        {
            Assert.Null(FengConverter.ToFeng("abc"));
        }

    }
}
