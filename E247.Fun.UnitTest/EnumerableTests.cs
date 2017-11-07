using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Xunit;

namespace E247.Fun.UnitTest
{
    public sealed class EnumerableTests
    {
        [Theory, AutoData]
        public void WhereSomeReturnsShorterListWhenInputContainedEmptyMaybes(
            List<string> input)
        {
            var inputMaybes = input.Select(x => x.ToMaybe()).ToList();
            inputMaybes.Add(Maybe<string>.Empty());

            var actual = inputMaybes.WhereSome();

            Assert.True(inputMaybes.Count > actual.Count());
        }

        [Theory, AutoData]
        public void WhereSomeReturnsCorrectListForNoEmptyMaybes(
            List<string> input)
        {
            var inputMaybes = input.Select(x => x.ToMaybe());

            List<string> actual = inputMaybes.WhereSome().ToList();

            Assert.Equal(input.Count, actual.Count);
            Assert.Equal(input, actual);
        }

        [Theory, AutoData]
        public void WhereSuccessfulReturnsCorrectNumberOfResults(
            List<string> successValues,
            List<int> failureValues)
        {
            var successes = successValues.Select(Result<string, int>.Succeed).ToList();
            var failures = failureValues.Select(Result<string, int>.Fail);
            var combined = successes.Concat(failures);

            var actual = combined.WhereSuccessful();

            Assert.Equal(successes.Count(), actual.Count());
        }

        [Theory, AutoData]
        public void WhereFailedReturnsCorrectNumberOfResults(
            List<string> successValues,
            List<int> failureValues)
        {
            var successes = successValues.Select(Result<string, int>.Succeed);
            var failures = failureValues.Select(Result<string, int>.Fail).ToList();
            var combined = successes.Concat(failures);

            var actual = combined.WhereFailed();

            Assert.Equal(failures.Count(), actual.Count());
        }

        [Theory, AutoData]
        public void CollectReturnsCorrectEnumerable(
            string value1,
            string value2,
            string value3)
        {
            var actual = Enumerable.Collect(value1, value2, value3).ToList();

            Assert.Equal(3, actual.Count);
            Assert.Equal(value1, actual[0]);
            Assert.Equal(value2, actual[1]);
            Assert.Equal(value3, actual[2]);
        }
    }
}
