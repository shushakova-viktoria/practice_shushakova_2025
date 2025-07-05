using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;

namespace task11
{
    public interface ICalculator
    {
        int Add(int a, int b);
        int Minus(int a, int b);
        int Mul(int a, int b);
        int Div(int a, int b);
    }

    public static class Calculate
    {
        public static async Task<ICalculator> CreatingCalculator()
        {
            var assembly = ScriptOptions.Default.AddReferences(Assembly.GetExecutingAssembly());

            string calc = @"
            using task11;

            public class Calculator : ICalculator
            {

            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;

            }

            new Calculator()";

            return await CSharpScript.EvaluateAsync<ICalculator>(calc, assembly);
        }
    }
}
