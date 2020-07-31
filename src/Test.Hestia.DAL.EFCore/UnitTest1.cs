using System;
using Hestia.DAL.EFCore;
using Xunit;

namespace Test.Hestia.DAL.EFCore
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var class1 = new Class1();
            Console.WriteLine(class1.ToString());
        }
    }
}
