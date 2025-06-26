using System.Reflection;
using task05;
using Xunit;

namespace task05tests
{
    public class TestClass
    {
        public int PublicField;
        private string _privateField;
        public int Property { get; set; }
        public void Method() { }
        public void MethodForParams(string par1, string par2) { }
    }

    [Serializable]
    public class AttributedClass { }

    public class ClassAnalyzerTests
    {
        [Fact]
        public void GetPublicMethods_ReturnsCorrectMethods()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var methods = analyzer.GetPublicMethods();

            Assert.Contains("Method", methods);
        }

        [Fact]
        public void GetAllFields_IncludesPrivateFields()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var fields = analyzer.GetAllFields().ToList();

            Assert.Contains("_privateField", fields);
        }

        [Fact]
        public void GetAllFields_IncludesPublicFields()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var fields = analyzer.GetAllFields().ToList();

            Assert.Contains("PublicField", fields);
        }

        [Fact]
        public void GetMethodParams()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var par = analyzer.GetMethodParams("MethodForParams").ToList();

            Assert.Contains("par1", par);
            Assert.Contains("par2", par);
        }

        [Fact]
        public void GetProperties()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var prop = analyzer.GetProperties().ToList();

            Assert.Contains("Property", prop);
        }

        [Fact]
        public void HasAttribute()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var prop = analyzer.GetProperties().ToList();

            Assert.False(analyzer.HasAttribute<ObsoleteAttribute>());
        }


    }
}