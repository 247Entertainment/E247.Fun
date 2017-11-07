using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Xunit;
using static E247.Fun.Fun;

namespace E247.Fun.UnitTest
{
    public class FunTests
    {
        [Theory, AutoData]
        public void FuncDoesNotModifyInput(int returnValue)
        {
            Func<int> expected = () => returnValue;
            var actual = Func(() => returnValue);

            var expectedResult = expected();
            var actualResult = actual();

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory, AutoData]
        public void CurryDoesNotChangeEndFunctionResult(int a, int b)
        {
            var func = Func((int c, int d) => c + d);
            var curriedFunc = func.Curry();

            var result = func(a, b);
            var curriedResult = curriedFunc(a)(b);

            Assert.Equal(result, curriedResult);
        }

        [Fact]
        public void CurriedTwoParamFuncReturnsExpectedType()
        {
            var func = Func((int y, int z) => y + z);

            var curriedFunc = func.Curry();

            Assert.IsType<Func<int, int, int>>(func);
            Assert.IsType<Func<int, Func<int, int>>>(curriedFunc);
        }

        [Theory, AutoData]
        public void UncurriedFuncIsSameAsBeforeCurrying(int a, int b)
        {
            var func = Func((int y, int z) => y + z);
            var curriedFunc = func.Curry();

            var uncurriedFunc = curriedFunc.Uncurry();
            var originalResult = func(a, b);
            var uncurriedResult = uncurriedFunc(a, b);

            Assert.IsType(func.GetType(), uncurriedFunc);
            Assert.Equal(originalResult, uncurriedResult);
        }

        [Theory, AutoData]
        public void ComposedFuncIsEqualToCallingBothFuncs(int a)
        {
            var f1 = Func((int x) => x + 1);
            var f2 = Func((int y) => y - 1);

            var composedFunc = f1.Compose(f2);
            var expected = f1(f2(a));
            var actual = composedFunc(a);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void ComposeBackCallsFuncsInReverseOrderToCompose(int a)
        {
            var f1 = Func((int x) => x + 1);
            var f2 = Func((int y) => y - 1);

            var composedFunc = f1.ComposeBack(f2);
            var expected = f2(f1(a));
            var actual = composedFunc(a);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void CurriedPartialApplicationDoesNotModifyFunctionBehavior(int a, int b)
        {
            var func = Func((int y, int z) => y + z);
            var partiallyAppliedFunc = func.Curry()(a);

            var expected = func(a, b);
            var actual = partiallyAppliedFunc(b);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void PartialApplicationDoesNotModifyFunctionBehavior(int a, int b)
        {
            var func = Func((int y, int z) => y + z);
            var partiallyAppliedFunc = func.Apply(a);

            var expected = func(a, b);
            var actual = partiallyAppliedFunc(b);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void PartialApplicationDoesNotModifyFunctionBehavior2(int a, int b, int c)
        {
            var func = Func((int x, int y, int z) => x * (y + z));
            var partiallyAppliedFunc = func.Apply(a, b);

            var expected = func(a, b, c);
            var actual = partiallyAppliedFunc(c);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void CanFlip2Params(string first, string second)
        {
            var func = Func((string one, string two) => $"{one}{two}");
            var flippedFunc = func.Curry().Flip();

            var originalResult = func(first, second);
            var flippedResult = flippedFunc(first)(second);

            Assert.Equal($"{first}{second}", originalResult);
            Assert.Equal($"{second}{first}", flippedResult);
        }

        [Theory, AutoData]
        public void IdentityReturnsArgUnchanged(string arg)
        {
            Assert.Equal(arg, Identity(arg));
        }

        [Fact]
        public void FunkedActionsThrowAsTheOriginal()
        {
            Action original = () => { throw new Exception(); };
            var funked = Func(() => { throw new Exception(); });

            Assert.Throws<Exception>(() => original());
            Assert.Throws<Exception>(() => funked());
        }

        [Fact]
        public void RaiseThrowsFromTernaryIfSelected()
        {
            var ternary = Func((bool cond) => cond
                ? "OK"
                : Raise<string>(new Exception()));

            Assert.Throws<Exception>(() => ternary(false));
        }

        [Fact]
        public void RaiseDoesntThrowsFromTernaryIfNotSelected()
        {
            var ternary = Func((bool cond) => cond
                ? "OK"
                : Raise<string>(new Exception()));

            //this should not throw
            ternary(true);
        }

        [Theory, AutoData]
        public void MemoizedFuncRepeatedlyReturnsAsTheOriginal(int seed)
        {
            var veryLongComputation = Func((int arg) => 2 * arg);
            var originalClosure = Func(() => veryLongComputation(seed));
            var memoized = originalClosure.Memoize();

            var originalResult = originalClosure();
            for (var i = 0; i < 10; i++)
            {
                Assert.Equal(originalResult, memoized());
            }
        }

        [Theory, AutoData]
        public void MemoizedFuncIsEvaluetedOnlyOnce(int seed)
        {
            // an impure function should NEVER be memoized.
            // But I cannot imagine any way to test that the memoized func is evalueted only once without violating the referential transparency
            // https://en.wikipedia.org/wiki/Referential_transparency
            var counter = 0;
            var veryLongImpureComputation = Func((int arg) =>
            {
                // NEVER do this in real code in memoized functions
                counter++;
                return 2 * arg;
            });
            var originalClosure = Func(() => veryLongImpureComputation(seed));
            var memoized = originalClosure.Memoize();

            for (var i = 0; i < 10; i++)
            {
                var result = memoized();
            }
            Assert.Equal(1, counter);
        }

        [Theory, AutoData]
        public void MemoizedFuncWithArgRepeatedlyReturnsAsTheOriginal(int seed1, int seed2)
        {
            var veryLongComputation = Func((int arg) => seed1 * arg);
            var memoized = veryLongComputation.Memoize();

            var offset = seed2;
            const int numTestValues = 20;
            var originalResults = new int[numTestValues];
            for (var i = 0; i < numTestValues; i++)
            {
                originalResults[i] = veryLongComputation(i + offset);
            }
            for (var i = 0; i < numTestValues; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    Assert.Equal(originalResults[i], memoized(i + offset));
                }
            }
        }

        [Theory, AutoData]
        public void MemoizedFuncWithArgIsEvaluetedOnlyOnce(int seed1, int seed2)
        {
            var counter = 0;
            var veryLongImpureComputation = Func((int arg) =>
            {
                // NEVER do this in real code in memoized functions
                counter++;
                return seed1 * arg;
            });
            var memoized = veryLongImpureComputation.Memoize();

            var offset = seed2;
            const int numTestValues = 20;
            for (var i = 0; i < numTestValues; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    memoized(i + offset);
                }
            }

            Assert.Equal(numTestValues, counter);
        }

        [Theory, AutoData]
        public void MapMapsFunctions(
            int value,
            int const1,
            int const2)
        {
            var func1 = Func((int v) => const1 + v);
            var func2 = Func((int v) => const2 * v);

            var temp1 = func1(value);
            var expected = func2(temp1);
            var actual = value
                .Map(func1)
                .Map(func2);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void IfBehavesLikeIf(
            int value,
            int const1,
            int const2)
        {
            var temp1 = value > 10
                ? const1 / value
                : const1 * value;
            var expected = temp1 < 30
                ? const2 * temp1
                : const2 / temp1;

            var actual = value
                .If(v => v > 10,
                    Then: v => const1 / v,
                    Else: v => const1 * v)
                .If(v => v < 30,
                    Then: v => const2 * v,
                    Else: v => const2 / v);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void TeeExecuteAction(
            int value,
            string seed)
        {
            var sideEffected = seed;
            var act = Act((int v) => sideEffected = sideEffected + v);

            var result = value
                .Tee(act);

            Assert.Equal(sideEffected, seed + value);
        }

        [Theory, AutoData]
        public void TeeReturnValue(
            int value,
            string seed)
        {
            var sideEffected = seed;
            var act = Act((int v) => sideEffected = sideEffected + v);

            var result = value
                .Tee(act);

            Assert.Equal(result, value);
        }

        [Theory, AutoData]
        public async Task TeeAsyncAwaitsInCorrectOrder(
            int firstValue,
            int secondValue)
        {
            var queue = new ConcurrentQueue<int>();

            var _ = await 
                AsyncOperation1(queue, firstValue)
                .TeeAsync(() => AsyncOperation2(queue, secondValue));

            Assert.Equal(new[] { firstValue, secondValue }, queue.ToArray());
        }

        static async Task<int> AsyncOperation1(ConcurrentQueue<int> queue, int value)
        {
            await Task.Delay(100);
            queue.Enqueue(value);
            return value;
        }

        static Task AsyncOperation2(ConcurrentQueue<int> queue, int value)
        {
            queue.Enqueue(value);
            return Task.FromResult(value);
        }
    }
}
