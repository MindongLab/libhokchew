using System;
using Xunit;
using FluentAssertions;

namespace LibHokchew.Shared.Tests
{
    public class CikLingUtilTest
    {
        [Theory]
        [InlineData("莺催上入", "ooyh24")]
        [InlineData("求须上入", "geoyh24")]
        [InlineData("求须上平", "gy55")]
        // TODO: write more test cases
        public void ToYngPingHokchew(string input, string output)
        {
            CikLingUtil.ToYngPingHokchew(input).Should().Be(output);
        }

        [Theory]
        [InlineData("是初下平")]
        public void ToYngPingHokchew_InvalidInitial_Throws(string input)
        {
            Action act = () => CikLingUtil.ToYngPingHokchew(input);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("柳香下平", "luong53")]
        [InlineData("求桥下平", "gyo53")]
        [InlineData("出桥上平", "cuo55")]
        public void ToYngPingHokchew_HonorsUoYoConversion(string input, string output)
        {
            CikLingUtil.ToYngPingHokchew(input).Should().Be(output);
        }
    }
}
