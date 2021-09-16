using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyLab.ExpressionTools;
using Xunit;

namespace UnitTests
{
    public class ExpressionExtensionsBehavior
    {
        [Theory]
        [MemberData(nameof(GetGetValueTestCases))]
        public void ShouldSupportExpressionValueProviding(Expression<Func<TestClass>> expr)
        {
            //Arrange
            
            //Act
            var val = expr.GetValue<TestClass>();

            //Assert
            Assert.NotNull(val);
            Assert.Equal("foo", val.Value);
        }

        [Fact]
        public void ShouldProvideDictionaryInitialization()
        {
            //Arrange
            Expression<Func<Dictionary<string, object>>> expr = () => new Dictionary<string, object> { { "Message", "f" }, { "Message2", "oo" } };

            //Act
            var val = expr.GetValue<Dictionary<string, object>>();

            //Assert
            Assert.NotNull(val);
            Assert.Equal("f", val["Message"]);
            Assert.Equal("oo", val["Message2"]);
        }

        [Fact]
        public void ShouldSupportNullablePropertySet()
        {
            //Arrange
            Expression<Func<TestClass>> expr = () => new TestClass
            {
                IntValue = 123
            };

            //Act
            var val = expr.GetValue<TestClass>();

            //Assert
            Assert.NotNull(val);
            Assert.Equal(123, val.IntValue);
        }

        public static IEnumerable<object[]> GetGetValueTestCases()
        {
            return new []
            {
                new object[] { (Expression<Func<TestClass>>)( () => new TestClass("foo")) },
                new object[] { (Expression<Func<TestClass>>)( () => new TestClass{ Value = "foo" }) },
                new object[] { (Expression<Func<TestClass>>)( () => new TestClass(GetFoo())) },
                new object[] { (Expression<Func<TestClass>>)( () => new TestClass("f" + "oo")) },
                new object[] { (Expression<Func<TestClass>>)( () => new TestClass(() => "foo")) },
                new object[] { (Expression<Func<TestClass>>)( () => new TestClass(() =>
                    string.Join("", new Dictionary<string, object> { {"bar", "f" }, { "baz", "oo" } }.Select(kv => kv.Value))
                    )) },
            };
        }

        static string GetFoo()
        {
            return "foo";
        }
    }

    public class TestClass
    {
        public string Value { get; set; }
        public int? IntValue { get; set; }

        public TestClass()
        {
            
        }

        public TestClass(string value)
        {
            Value = value;
        }

        public TestClass(Func<string> valProvider)
        {
            Value = valProvider();
        }
    }
}
