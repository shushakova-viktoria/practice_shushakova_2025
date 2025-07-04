using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection;

namespace task10tests
{
    public class UnitTest1
    {
        [PluginLoad("TestPlugin", "")]
        public class TestPlugin : IPlugin
        {
            public void Execute()
            {
                Console.WriteLine("TestPlugin");
            }
        }

        [Fact]
        public void CreatingTestPlugin()
        {
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var plug = new TestPlugin();
            plug.Execute();

            Assert.Equal("TestPlugin\r\n", consoleOutput.ToString());
        }

        [Fact]
        public void FindingAttribute()
        {
            var typeOfPlugin = typeof(TestPlugin);

            var attr = (PluginLoad)Attribute.GetCustomAttribute(typeOfPlugin, typeof(PluginLoad));

            Assert.Equal("TestPlugin", attr.Name);
            Assert.Equal("", attr.Dependence);
        }

    }
}