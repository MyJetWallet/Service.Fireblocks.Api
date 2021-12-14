using System;
using NUnit.Framework;

namespace Service.Fireblocks.Api.Tests
{
    public class TestExample
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine("Debug output");
            Assert.Pass();
        }
    }
}
