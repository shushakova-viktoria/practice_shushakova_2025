using System.Threading.Tasks;
using task11;
using Xunit;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;

namespace task11tests
{
    public class CalculatorTests
    {
        [Fact]
        public async Task Adding()
        {
            var calc = await Calculate.CreatingCalculator();
            Assert.Equal(52, calc.Add(50, 2));
        }

        [Fact]
        public async Task Minusing()
        {
            var calc = await Calculate.CreatingCalculator();
            Assert.Equal(133, calc.Minus(178, 45));
        }

        [Fact]
        public async Task Mulltuplying()
        {
            var calc = await Calculate.CreatingCalculator();
            Assert.Equal(42, calc.Mul(21, 2));
        }

        [Fact]
        public async Task Divving()
        {
            var calc = await Calculate.CreatingCalculator();
            Assert.Equal(87, calc.Div(783, 9));
        }
    }
}
