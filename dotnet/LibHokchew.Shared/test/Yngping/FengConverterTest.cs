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
        public void ToFeng(string yngping, string fengIpa)
        {
            Assert.Equal(fengIpa, FengConverter.ToFeng(yngping));
        }

    }
}
