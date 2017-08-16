using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace E247.Fun.UnitTest
{
    public class ChoiceTests
    {
        [Theory, AutoData]
        public void Choice2PreservesInputValue(string input)
        {
            var sut = new Choice<string, int>(input);

            var equal =
                sut.Match(
                    (string a) => input == a,
                    (int b) => false);
            Assert.True(equal);
        }

        [Theory, AutoData]
        public void Choice3PreservesInputValue(string input)
        {
            var sut = new Choice<string, int, bool>(input);

            var equal =
                sut.Match(
                    (string a) => input == a,
                    () => false);
            Assert.True(equal);
        }

        [Theory, AutoData]
        public void Choice4PreservesInputValue(string input)
        {
            var sut = new Choice<string, int, bool, byte>(input);

            var equal =
                sut.Match(
                    (string a) => input == a,
                    () => false);
            Assert.True(equal);
        }

        [Theory, AutoData]
        public void Choice5PreservesInputValue(string input)
        {
            var sut = new Choice<string, int, bool, byte, decimal>(input);

            var equal =
                sut.Match(
                    (string a) => input == a,
                    () => false);

            Assert.True(equal);
        }

        [Theory, AutoData]
        public void Choice6PreservesInputValue(string input)
        {
            var sut = new Choice<string, int, bool, byte, decimal, float>(input);

            var equal =
                sut.Match(
                    (string a) => input == a,
                    () => false);

            Assert.True(equal);
        }

        [Theory, AutoData]
        public void CaseElseMatchesAllOtherTypes(int input)
        {
            var sut = new Choice<string, int, bool, byte, decimal>(input);

            var equal =
               sut.Match(
                   (string a) => false,
                   () => true);

            Assert.True(equal);
        }

        [Theory, AutoData]
        public void SecondDistinctMatchWorks(bool input)
        {
            var sut = new Choice<string, int, bool, byte, decimal>(input);

            var equal =
               sut.Match(
                   (string a) => false,
                   (bool b) => true,
                   () => false);

            Assert.True(equal);
        }

        [Theory, AutoData]
        public void ThirdDistinctMatchWorks(bool input)
        {
            var sut = new Choice<string, int, bool, byte, decimal>(input);

            var equal =
               sut.Match(
                   (string a) => false,
                   (int c) => false,
                   (bool b) => true,
                   () => false);

            Assert.True(equal);
        }

        [Theory, AutoData]
        public void ChoicesAreEquatable(string a, int b)
        {
            var choiceA = new Choice<string, int>(a);
            var choiceB = new Choice<string, int>(b);
            var choiceA2 = new Choice<string, int>(a);

            Assert.Equal(choiceA, choiceA2);
            Assert.NotEqual(choiceA, choiceB);
            Assert.NotEqual(choiceA2, choiceB);
        }

        [Theory, AutoData]
        public void GetHashCodeDoesNotThrow(string a)
        {
            var sut1 = new Choice<string, int>(a);
            var sut2 = new Choice<int, string>(a);

            sut1.GetHashCode();
            sut2.GetHashCode();
        }

    }
}
