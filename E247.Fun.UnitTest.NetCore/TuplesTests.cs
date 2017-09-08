using System.Threading.Tasks;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using static E247.Fun.Tuples;
using static E247.Fun.Fun;

namespace E247.Fun.UnitTest
{
    public class TuplesTests
    {
        [Theory, AutoData]
        public void TwoPartConstructorCreatesTupleWithCorrectData(
            string item1,
            int item2)
        {
            var actual = Tuple(item1, item2);

            Assert.IsType<System.Tuple<string, int>>(actual);
            Assert.Equal(item1, actual.Item1);
            Assert.Equal(item2, actual.Item2);
        }

        [Theory, AutoData]
        public void ThreePartConstructorCreatesTupleWithCorrectData(
            string item1,
            int item2,
            bool item3)
        {
            var actual = Tuple(item1, item2, item3);

            Assert.IsType<System.Tuple<string, int, bool>>(actual);
            Assert.Equal(item1, actual.Item1);
            Assert.Equal(item2, actual.Item2);
            Assert.Equal(item3, actual.Item3);
        }

        [Theory, AutoData]
        public void FourPartConstructorCreatesTupleWithCorrectData(
            string item1,
            int item2,
            bool item3,
            string item4)
        {
            var actual = Tuple(item1, item2, item3, item4);

            Assert.IsType<System.Tuple<string, int, bool, string>>(actual);
            Assert.Equal(item1, actual.Item1);
            Assert.Equal(item2, actual.Item2);
            Assert.Equal(item3, actual.Item3);
            Assert.Equal(item4, actual.Item4);
        }

        [Theory, AutoData]
        public void FivePartConstructorCreatesTupleWithCorrectData(
            string item1,
            int item2,
            bool item3,
            string item4,
            int item5)
        {
            var actual = Tuple(item1, item2, item3, item4, item5);

            Assert.IsType<System.Tuple<string, int, bool, string, int>>(actual);
            Assert.Equal(item1, actual.Item1);
            Assert.Equal(item2, actual.Item2);
            Assert.Equal(item3, actual.Item3);
            Assert.Equal(item4, actual.Item4);
            Assert.Equal(item5, actual.Item5);
        }

        [Theory, AutoData]
        public void SixPartConstructorCreatesTupleWithCorrectData(
           string item1,
           int item2,
           bool item3,
           string item4,
           int item5,
           bool item6)
        {
            var actual = Tuple(item1, item2, item3, item4, item5, item6);

            Assert.IsType<System.Tuple<string, int, bool, string, int, bool>>(
                actual);
            Assert.Equal(item1, actual.Item1);
            Assert.Equal(item2, actual.Item2);
            Assert.Equal(item3, actual.Item3);
            Assert.Equal(item4, actual.Item4);
            Assert.Equal(item5, actual.Item5);
            Assert.Equal(item6, actual.Item6);
        }

        [Theory, AutoData]
        public void SevenPartConstructorCreatesTupleWithCorrectData(
          string item1,
          int item2,
          bool item3,
          string item4,
          int item5,
          bool item6,
          string item7)
        {
            var actual = Tuple(item1, item2, item3, item4, item5, item6, item7);

            Assert.IsType<System.Tuple<string, int, bool, string, int, bool, string>>(
                actual);
            Assert.Equal(item1, actual.Item1);
            Assert.Equal(item2, actual.Item2);
            Assert.Equal(item3, actual.Item3);
            Assert.Equal(item4, actual.Item4);
            Assert.Equal(item5, actual.Item5);
            Assert.Equal(item6, actual.Item6);
            Assert.Equal(item7, actual.Item7);
        }

        [Theory, AutoData]
        public void MapPassesExpectedValuesToMapFunction(
            string item1,
            int item2)
        {
            var mapper = Func((string a, int b) =>
            {
                Assert.Equal(item1, a);
                Assert.Equal(item2, b);
                return true;
            });
            var sut = Tuple(item1, item2);

            var actual = sut.Map(mapper);

            Assert.True(actual);
        }

        [Theory, AutoData]
        public async Task MapAsyncPassesExpectedValuesToMapFunction(
           string item1,
           int item2)
        {
            var mapper = Func((string a, int b) =>
            {
                Assert.Equal(item1, a);
                Assert.Equal(item2, b);
                return Task.FromResult(true);
            });
            var sut = Tuple(item1, item2);

            var actual = await sut.MapAsync(mapper);

            Assert.True(actual);
        }
    }
}
