using Xunit;

namespace E247.Fun.UnitTest
{
    public class UnitTests
    {
        [Fact]
        public void SutIsNotNullable()
        {
            Assert.True(typeof(Unit).IsValueType);
        }

        [Fact]
        public void SutIsSingleton()
        {
            var unit1 = Unit.Value;
            var unit2 = Unit.Value;
            var unit3 = new Unit();

            Assert.Equal(unit1, unit2);
            Assert.Equal(unit1, unit3);
        }
    }
}
