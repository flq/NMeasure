using System.Collections;
using System.Linq;
using Xunit;

namespace NMeasure.Tests
{
    public static class TestExtensions
    {
        public static void IsEqualTo<T>(this T value, T expected)
        {
            Assert.Equal(expected, value);
        }

        public static void IsNotNull<T>(this T value) where T : class
        {
            Assert.NotNull(value);
        }

        public static void HasCount(this IEnumerable value, int expected)
        {
            value.Cast<object>().Count().IsEqualTo(expected);
        }

        public static void IsFalse(this bool value)
        {
            Assert.False(value);
        }

        public static void IsTrue(this bool value)
        {
            Assert.True(value);
        }

        public static void IsOfType<T>(this object o)
        {
            Assert.IsAssignableFrom<T>(o);
        }
    }
}