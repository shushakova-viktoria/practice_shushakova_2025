using task14;
using static task14.DefiniteIntegral;

namespace task14tests
{
    public class task14tests
    {
        [Fact]
        public void Test1()
        {
            var X = (double x) => x;
            var SIN = (double x) => Math.Sin(x);

            Assert.Equal(0, SolveManyThreads(-1, 1, X, 1e-4, 2), 1e-4);

            Assert.Equal(0, SolveManyThreads(-1, 1, SIN, 1e-5, 8), 1e-4);
        }

        [Fact]
        public void Test2()
        {
            var result = SolveManyThreads(0, 5, x => x, 1e-6, 8);
            Assert.Equal(12.5, result);
        }

        [Fact]
        public void Test3()
        {
            var result = SolveManyThreads(0, 3, x => x * x, 1e-8, 8);
            Assert.Equal(9, result); 
        }
    }
}
