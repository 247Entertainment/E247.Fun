using System;
using System.Linq;
using System.Threading.Tasks;

using E247.Fun.Exceptions;
using AutoFixture.Xunit2;
using Xunit;
using static E247.Fun.Unit;
using static E247.Fun.Fun;

namespace E247.Fun.UnitTest
{
    public class ResultTests
    {
        //[Theory(Skip = "Don't know if type constraint for nullability require special handling"), AutoData]
        //public void SutHasGuardClauses(GuardClauseAssertion assertion)
        //{
        //    assertion.Verify(typeof(Result<string, string>)
        //        .GetMethods()
        //        .Where(m => m.CustomAttributes.All(
        //            c => c.AttributeType != typeof(AsyncStateMachineAttribute))));
        //}

        [Theory, AutoData]
        public void FailSetsIsSuccessfulToFalse(string value)
        {
            var actual = Result<string, string>.Fail(value);

            Assert.False(actual.IsSuccessful);
        }

        [Theory, AutoData]
        public void SucceedSetsIsSuccessfulToTrue(string value)
        {
            var actual = Result<string, string>.Succeed(value);

            Assert.True(actual.IsSuccessful);
        }

        [Theory, AutoData]
        public void SucceedSetsSuccessValue(string value)
        {
            var actual = Result<string, string>.Succeed(value);

            Assert.NotNull(actual.Success);
            Assert.Equal(value, actual.Success);
        }

        [Theory, AutoData]
        public void FailSetsFailureValue(string value)
        {
            var actual = Result<string, string>.Fail(value);

            Assert.NotNull(actual.Failure);
            Assert.Equal(value, actual.Failure);
        }

        [Theory, AutoData]
        public void AccessingSuccessForFailedResultThrowsResultAccessException(
            string value)
        {
            var actual = Result<string, string>.Fail(value);

            Assert.Throws<ResultAccessException>(() => actual.Success);
        }

        [Theory, AutoData]
        public void AccessingFailureForSuccessfulResultThrowsResultAccessException(
            string value)
        {
            var actual = Result<string, string>.Succeed(value);

            Assert.Throws<ResultAccessException>(() => actual.Failure);
        }

        [Theory, AutoData]
        public void CannotWriteToIsSuccessful()
        {
            Assert.False(
                typeof(Result<string, string>)
                .GetProperties()
                .First(x => x.Name == "IsSuccessful")
                .CanWrite);
        }

        [Theory, AutoData]
        public void TryCatchesSpecifiedExceptionType(
            string message)
        {
            Func<int> willThrow = () =>
            {
                throw new ApplicationException(message);
            };
            Func<ApplicationException, string> handler = ex => ex.Message;

            //this should not throw
            Result<int, string>.Try(willThrow, handler);
        }

        [Theory, AutoData]
        public void TryDoesNotCatchOtherExceptions(
            string message)
        {
            Func<bool> willThrow = () =>
            {
                throw new ArgumentException(message);
            };
            Func<ApplicationException, string> handler = ex => ex.Message;

            Assert.Throws<ArgumentException>(() =>
            {
                var result =
                    Result<bool, string>.Try(willThrow, handler);
            });
        }

        [Theory, AutoData]
        public void TryReturnsFailureWhenExceptionIsCaught(
            string failMessage)
        {
            Func<double> throws = () => { throw new Exception(); };

            var result = Result<double, string>.Try(
                () => throws(),
                (Exception _) => failMessage);

            Assert.False(result.IsSuccessful);
            Assert.Equal(failMessage, result.Failure);
        }

        [Theory, AutoData]
        public void TryReturnsSuccessWhenNoExceptionIsThrown(
            int successMessage)
        {
            Func<int> doesNotThrow = () => successMessage;

            var result = Result<int, string>.Try(
                () => doesNotThrow(),
                (Exception _) => "failure");

            Assert.True(result.IsSuccessful);
            Assert.Equal(successMessage, result.Success);
        }

        [Theory, AutoData]
        public void TryInvokeNullHandlerOnNull(
            int nullint,
            int failint)
        {
            Func<string> returnNull = () => null;

            var result = Result.Try(
                returnNull,
                ifNull: () => nullint,
                failWith: (Exception _) => failint);
            Assert.False(result.IsSuccessful);
            Assert.Equal(nullint, result.Failure);
        }

        [Fact]
        public void UnitTryCatchesSpecifiedExceptionType()
        {
            Func<Unit> willThrow = () =>
            {
                throw new ApplicationException("message");
            };
            Func<ApplicationException, string> handler = _ => "caught";

            //this should not throw
            Result<Unit, string>.Try(willThrow, handler);
        }

        [Fact]
        public void UnitTryDoesNotCatchOtherExceptions()
        {
            Func<Unit> willThrow = () =>
            {
                throw new ArgumentException("message");
            };
            Func<ApplicationException, string> handler = _ => "caught";

            Assert.Throws<ArgumentException>(() =>
            {
                var result =
                    Result<Unit, string>.Try(willThrow, handler);
            });
        }

        [Theory, AutoData]
        public void UnitTryReturnsFailureWhenExceptionIsCaught(
            string failMessage)
        {
            Func<Unit> throws = () => { throw new Exception(); };

            var result = Result<Unit, string>.Try(
                throws,
                (Exception _) => failMessage);

            Assert.False(result.IsSuccessful);
            Assert.Equal(failMessage, result.Failure);
        }

        [Fact]
        public void UnitTryReturnsSuccessWhenNoExceptionIsThrown()
        {
            Func<Unit> doesNotThrow = () => unit;

            var result = Result<Unit, string>.Try(
                doesNotThrow,
                (Exception _) => "failure");

            Assert.True(result.IsSuccessful);
            Assert.Equal(unit, result.Success);
        }

        [Theory, AutoData]
        public async Task TryCatchesSpecifiedExceptionTypeAsync()
        {
            Func<Task<bool>> willThrow = () =>
            {
                throw new ApplicationException("message");
            };
            Func<ApplicationException, string> handler = _ => "caught";

            //this should not throw
            await Result<bool, string>.TryAsync(willThrow, handler);
        }

        [Theory, AutoData]
        public async Task TryDoesNotCatchOtherExceptionsAsync()
        {
            Func<Task<int>> willThrow = () =>
            {
                throw new ArgumentException("message");
            };
            Func<ApplicationException, string> handler = _ => "caught";

            await Assert.ThrowsAsync<ArgumentException>(() =>
                Result<int, string>.TryAsync(willThrow, handler));
        }

        [Theory, AutoData]
        public async Task TryReturnsFailureWhenExceptionIsCaughtAsync(
           string failMessage)
        {
            Func<Task<string>> throws = () => { throw new Exception(); };

            var result = await Result<string, string>.TryAsync<Exception>(
                throws,
                _ => failMessage);

            Assert.False(result.IsSuccessful);
            Assert.Equal(failMessage, result.Failure);
        }

        [Theory, AutoData]
        public async Task TryInvokeNullHandlerOnNullAsync(
            int nullint,
            int failint)
        {
            Func<Task<string>> returnNull = () => Task.FromResult<string>(null);

            var result = await Result.TryAsync(
                returnNull,
                ifNull: () => nullint,
                failWith: (Exception _) => failint);
            Assert.False(result.IsSuccessful);
            Assert.Equal(nullint, result.Failure);
        }

        [Theory, AutoData]
        public async Task TryReturnsSuccessWhenNoExceptionIsThrownAsync(
            double successMessage)
        {
            Func<Task<double>> doesNotThrow =
                () => Task.FromResult(successMessage);

            var result = await Result<double, string>.TryAsync(
                doesNotThrow,
                (Exception _) => "failure");

            Assert.True(result.IsSuccessful);
            Assert.Equal(successMessage, result.Success);
        }

        [Fact]
        public async Task UnitTryCatchesSpecifiedExceptionTypeAsync()
        {
            Func<Task> willThrow = () =>
            {
                throw new ApplicationException("message");
            };
            Func<ApplicationException, string> handler = _ => "caught";

            //this should not throw
            await Result<Unit, string>.TryAsync(willThrow, handler);
        }

        [Fact]
        public async Task UnitTryDoesNotCatchOtherExceptionsAsync()
        {
            Func<Task> willThrow = () =>
            {
                throw new ArgumentException("message");
            };
            Func<ApplicationException, string> handler = _ => "caught";

            await Assert.ThrowsAsync<ArgumentException>(() =>
                Result<Unit, string>.TryAsync(willThrow, handler));
        }

        [Theory, AutoData]
        public async Task UnitTryReturnsFailureWhenExceptionIsCaughtAsync(
           string failMessage)
        {
            Func<Task> throws = () => { throw new Exception(); };

            var result = await Result<Unit, string>.TryAsync<Exception>(
                throws,
                _ => failMessage);

            Assert.False(result.IsSuccessful);
            Assert.Equal(failMessage, result.Failure);
        }

        [Fact]
        public async Task UnitTryReturnsSuccessWhenNoExceptionIsThrownAsync()
        {
            Func<Task> doesNotThrow =
                async () => { await Task.FromResult(true); };

            var result = await Result<Unit, string>.TryAsync<Exception>(
                doesNotThrow,
                _ => "failure");

            Assert.True(result.IsSuccessful);
            Assert.Equal(unit, result.Success);
        }

        [Theory, AutoData]
        public void MapPreservesSuccess(string value)
        {
            var result = Result<string, int>.Succeed(value);
            Func<string, int> mapper = (x) => x?.Length ?? 0;
            var mapped = result.Map(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.True(mapped.IsSuccessful);
            Assert.Equal(mapped.Success, mapper(result.Success));
        }

        [Theory, AutoData]
        public void MapPreservesFailure(int error)
        {
            var result = Result<string, int>.Fail(error);
            Func<string, int> mapper = (x) => x?.Length ?? 0;
            var mapped = result.Map(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.False(mapped.IsSuccessful);
            Assert.Equal(mapped.Failure, error);
        }
        [Theory, AutoData]
        public async Task MapAsyncPreservesSuccess(string value)
        {
            var result = Result<string, int>.Succeed(value);
            Func<string, Task<int>> mapper = (x) => Task.FromResult(x?.Length ?? 0);
            var mapped = await result.MapAsync(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.True(mapped.IsSuccessful);
            Assert.Equal(mapped.Success, await mapper(result.Success));
        }

        [Theory, AutoData]
        public async Task MapAsyncPreservesFailure(int error)
        {
            var result = Result<string, int>.Fail(error);
            Func<string, Task<int>> mapper = (x) => Task.FromResult(x?.Length ?? 0);
            var mapped = await result.MapAsync(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.False(mapped.IsSuccessful);
            Assert.Equal(mapped.Failure, error);
        }

        [Theory, AutoData]
        public void MapSuccessPreservesSuccess(string value)
        {
            var result = Result<string, int>.Succeed(value);
            Func<string, int> mapper = (x) => x?.Length ?? 0;
            var mapped = result.MapSuccess(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.True(mapped.IsSuccessful);
            Assert.Equal(mapped.Success, mapper(result.Success));
        }

        [Theory, AutoData]
        public void MapSuccessPreservesFailure(int error)
        {
            var result = Result<string, int>.Fail(error);
            Func<string, int> mapper = (x) => x?.Length ?? 0;
            var mapped = result.MapSuccess(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.False(mapped.IsSuccessful);
            Assert.Equal(mapped.Failure, error);
        }

        [Theory, AutoData]
        public void MapFailurePreservesSuccess(
            string value,
            int error)
        {
            var result = Result<string, int>.Succeed(value);
            Func<int, string> mapper = (x) => x.ToString();
            var mapped = result.MapFailure(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.True(mapped.IsSuccessful);
            Assert.Equal(mapped.Success, result.Success);
        }

        [Theory, AutoData]
        public void MapFailurePreservesFailure(int error)
        {
            var result = Result<string, int>.Fail(error);
            Func<int, string> mapper = (x) => x.ToString();
            var mapped = result.MapFailure(mapper);

            Assert.Equal(result.IsSuccessful, mapped.IsSuccessful);
            Assert.False(mapped.IsSuccessful);
            Assert.Equal(mapped.Failure, mapper(result.Failure));
        }

        [Theory, AutoData]
        public void ToMaybeASuccessReturnsTheValue(string value)
        {
            var actual = Result<string, int>.Succeed(value)
                    .ToMaybe();

            Assert.True(actual.HasValue);
            Assert.Equal(value, actual.Value);
        }

        [Theory, AutoData]
        public void ToMaybeAFailureReturnsNone(int error)
        {
            var actual = Result<string, int>.Fail(error)
                    .ToMaybe();

            Assert.False(actual.HasValue);
        }

        [Theory, AutoData]
        public void BindingAFailureToASuccessReturnsAFailure(int error)
        {
            var result = Result<string, int>.Fail(error);
            Func<string, Result<int, int>> binder = (x) => Result<int, int>.Succeed(x?.Length ?? 0);

            Result<int, int> actual = result.Bind(binder);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, error);
        }

        [Theory, AutoData]
        public void BindingAFailureToAFailureReturnsTheFirstFailure(
            int firstint,
            int secondint)
        {
            var result = Result<string, int>.Fail(firstint);
            Func<string, Result<int, int>> binder = (x) => Result<int, int>.Fail(secondint);

            Result<int, int> actual = result.Bind(binder);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, firstint);
        }

        [Theory, AutoData]
        public void BindingAFailureToAThrowerDoesNotThrow(
            int firstint)
        {
            var result = Result<string, int>.Fail(firstint);
            Func<string, Result<int, int>> binder = (x) => { throw new Exception(); };

            //this shouldn't throw
            Result<int, int> actual = result.Bind(binder);
        }

        [Theory, AutoData]
        public void BindingASuccessToASuccessReturnsASuccess(string value)
        {
            var result = Result<string, int>.Succeed(value);
            Func<string, int> func = (string x) => x?.Length ?? 0;
            Func<string, Result<int, int>> binder = (x) => Result<int, int>.Succeed(func(x));

            Result<int, int> actual = result.Bind(binder);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.True(actual.IsSuccessful);
            Assert.Equal(actual.Success, func(value));
        }

        [Theory, AutoData]
        public void BindingASuccessToAFailureReturnsTheFailure(
            string value,
            int error)
        {
            var result = Result<string, int>.Succeed(value);
            Func<string, Result<int, int>> binder = (x) => Result<int, int>.Fail(error);

            Result<int, int> actual = result.Bind(binder);

            Assert.NotEqual(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, error);
        }

        [Theory, AutoData]
        public void BindingWithMappingAFailureMapsTheFailure(
            Exception firstint,
            int secondint)
        {
            var result = Result<string, Exception>.Fail(firstint);
            Func<string, Result<int, int>> binder = (x) => Result<int, int>.Succeed(x?.Length ?? 0);
            Func<Exception, int> failureMapper = (x) => secondint;

            Result<int, int> actual = result.Bind(
                binder,
                mapFailure: failureMapper);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, secondint);
        }

        [Theory, AutoData]
        public async Task BindingAsyncAFailureToASuccessReturnsAFailure(int error)
        {
            var result = Result<string, int>.Fail(error);
            Func<string, Task<Result<int, int>>> binder = (x) => Task.FromResult(Result<int, int>.Succeed(x?.Length ?? 0));

            Result<int, int> actual = await result.BindAsync(binder);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, error);
        }

        [Theory, AutoData]
        public async Task BindingAsyncAFailureToAFailureReturnsTheFirstFailure(
            int firstint,
            int secondint)
        {
            var result = Result<string, int>.Fail(firstint);
            Func<string, Task<Result<int, int>>> binder = (x) => Task.FromResult(Result<int, int>.Fail(secondint));

            Result<int, int> actual = await result.BindAsync(binder);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, firstint);
        }

        [Theory, AutoData]
        public async Task BindingAsyncAFailureToAThrowerDoesNotThrow(
            int firstint)
        {
            var result = Result<string, int>.Fail(firstint);
            Func<string, Task<Result<int, int>>> binder = (x) => { throw new Exception(); };

            //this should not throw
            Result<int, int> actual = await result.BindAsync(binder);
        }

        [Theory, AutoData]
        public async Task BindingAsyncASuccessToASuccessReturnsASuccess(string value)
        {
            var result = Result<string, int>.Succeed(value);
            Func<string, int> func = (string x) => x?.Length ?? 0;
            Func<string, Task<Result<int, int>>> binder = (x) => Task.FromResult(Result<int, int>.Succeed(func(x)));

            Result<int, int> actual = await result.BindAsync(binder);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.True(actual.IsSuccessful);
            Assert.Equal(actual.Success, func(value));
        }

        [Theory, AutoData]
        public async Task BindingAsyncASuccessToAFailureReturnsTheFailure(
            string value,
            int error)
        {
            var result = Result<string, int>.Succeed(value);
            Func<string, Task<Result<int, int>>> binder = (x) => Task.FromResult(Result<int, int>.Fail(error));

            Result<int, int> actual = await result.BindAsync(binder);

            Assert.NotEqual(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, error);
        }

        [Theory, AutoData]
        public async Task BindingAsyncWithMappingAFailureMapsTheFailure(
            Exception firstint,
            int secondint)
        {
            var result = Result<string, Exception>.Fail(firstint);
            Func<string, Task<Result<int, int>>> binder = (x) => Task.FromResult(Result<int, int>.Succeed(x?.Length ?? 0));
            Func<Exception, Task<int>> failureMapper = (x) => Task.FromResult(secondint);

            Result<int, int> actual = await result.BindAsync(
                binder,
                mapFailure: failureMapper);

            Assert.Equal(result.IsSuccessful, actual.IsSuccessful);
            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, secondint);
        }

        // Match tests
        [Theory, AutoData]
        public void MatchCallsSuccessForResultWithSuccess(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Succeed(successInput);

            var actual = resultInput.Match(
                Success: s => s + successMatched,
                Failure: f => f + failureMatched);

            Assert.Equal(successInput + successMatched, actual);
        }

        [Theory, AutoData]
        public void MatchCallsFailureForResultWithFailure(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Fail(failureInput);

            var actual = resultInput.Match(
                Success: s => s + successMatched,
                Failure: f => f + failureMatched);

            Assert.Equal(failureInput + failureMatched, actual);
        }

        [Theory, AutoData]
        public void MatchCallsSuccessActionForResultWithSuccess(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Succeed(successInput);
            var target = "42";

            resultInput.Match(
                Success: s => { target = s + successMatched; },
                Failure: f => { target = f + failureMatched; });

            Assert.Equal(successInput + successMatched, target);
        }

        [Theory, AutoData]
        public void MatchCallsFailureActionForResultWithFailure(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Fail(failureInput);
            var target = "42";

            resultInput.Match(
                Success: s => { target = s + successMatched; },
                Failure: f => { target = f + failureMatched; });

            Assert.Equal(failureInput + failureMatched, target);
        }

        [Theory, AutoData]
        public async Task MatchAsyncCallsSuccessForResultWithSuccess(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Succeed(successInput);

            var actual = await resultInput.MatchAsync(
                Success: s => Task.FromResult(s + successMatched),
                Failure: f => Task.FromResult(f + failureMatched));

            Assert.Equal(successInput + successMatched, actual);
        }

        [Theory, AutoData]
        public async Task MatchAsyncCallsFailureForResultWithFailure(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Fail(failureInput);

            var actual = await resultInput.MatchAsync(
                Success: s => Task.FromResult(s + successMatched),
                Failure: f => Task.FromResult(f + failureMatched));

            Assert.Equal(failureInput + failureMatched, actual);
        }

        [Theory, AutoData]
        public async Task MatchAsyncCallsSuccessActionForResultWithSuccess(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Succeed(successInput);
            var target = "42";

            await resultInput.MatchAsync(
                Success: async s => { target = await Task.FromResult(s + successMatched); },
                Failure: async f => { target = await Task.FromResult(f + failureMatched); });

            Assert.Equal(successInput + successMatched, target);
        }

        [Theory, AutoData]
        public async Task MatchAsyncCallsFailureActionForResultWithFailure(
            int successInput,
            string failureInput,
            string successMatched,
            string failureMatched)
        {
            var resultInput = Result<int, string>.Fail(failureInput);
            var target = "42";

            await resultInput.MatchAsync(
                Success: async s => { target = await Task.FromResult(s + successMatched); },
                Failure: async f => { target = await Task.FromResult(f + failureMatched); });

            Assert.Equal(failureInput + failureMatched, target);
        }

        [Theory, AutoData]
        public void BindTryBindSuccessToNewNullableSuccess(
            string successInput,
            string successResult)
        {
            var resultInput = Result<string, string>.Succeed(successInput);
            var tryFunc = Func((string value) => value + successResult);

            var actual = resultInput
                .BindTry(
                    tryFunc,
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.True(actual.IsSuccessful);
            Assert.Equal(tryFunc(successInput), actual.Success);
        }

        [Theory, AutoData]
        public void BindTryBindFailureToInputFailure(
            string failureInput,
            string successResult)
        {
            var resultInput = Result<string, string>.Fail(failureInput);
            var tryFunc = Func((string value) => value + successResult);

            var actual = resultInput
                .BindTry(
                    tryFunc,
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.False(actual.IsSuccessful);
            Assert.Equal(failureInput, actual.Failure);
        }

        [Theory, AutoData]
        public void BindTryBindSuccessToIfNull(
            string successInput)
        {
            var resultInput = Result<string, string>.Succeed(successInput);

            var actual = resultInput
                .BindTry(
                    _ => (string)null,
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.False(actual.IsSuccessful);
            Assert.Equal("is null", actual.Failure);
        }

        #pragma warning disable 162  // unreachable code
        [Theory, AutoData]
        public void BindTryBindSuccessToFailWith(
            string successInput,
            Exception exceptionToThrow)
        {
            var resultInput = Result<string, string>.Succeed(successInput);

            var actual = resultInput
                .BindTry(
                    _ =>
                    {
                        throw exceptionToThrow;
                        return "";
                    },
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.False(actual.IsSuccessful);
            Assert.Equal(exceptionToThrow.Message, actual.Failure);
        }
        #pragma warning restore 162

        [Theory, AutoData]
        public async Task BindTryAsyncBindSuccessToNewNullableSuccess(
            string successInput,
            string successResult)
        {
            var resultInput = Task.FromResult(Result<string, string>.Succeed(successInput));
            var tryFunc = Func((string value) => Task.FromResult(value + successResult));

            var actual = await resultInput
                .BindTryAsync(
                    tryFunc,
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.True(actual.IsSuccessful);
            Assert.Equal(await tryFunc(successInput), actual.Success);
        }

        [Theory, AutoData]
        public async Task BindTryAsyncBindFailureToInputFailure(
            string failureInput,
            string successResult)
        {
            var resultInput = Task.FromResult(Result<string, string>.Fail(failureInput));
            var tryFunc = Func((string value) => Task.FromResult(value + successResult));

            var actual = await resultInput
                .BindTryAsync(
                    tryFunc,
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.False(actual.IsSuccessful);
            Assert.Equal(failureInput, actual.Failure);
        }

        [Theory, AutoData]
        public async Task BindTryAsyncBindSuccessToIfNull(
            string successInput)
        {
            var resultInput = Task.FromResult(Result<string, string>.Succeed(successInput));

            var actual = await resultInput
                .BindTryAsync(
                    _ => Task.FromResult((string)null),
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.False(actual.IsSuccessful);
            Assert.Equal("is null", actual.Failure);
        }

        #pragma warning disable 162  // unreachable code
        [Theory, AutoData]
        public async void BindTryAsyncBindSuccessToFailWith(
            string successInput,
            Exception exceptionToThrow)
        {
            var resultInput = Task.FromResult(Result<string, string>.Succeed(successInput));

            var actual = await resultInput
                .BindTryAsync(
                    _ => 
                    {
                        throw exceptionToThrow;
                        return Task.FromResult("");
                    },
                    ifNull: () => "is null",
                    failWith: (Exception ex) => ex.Message
                );

            Assert.False(actual.IsSuccessful);
            Assert.Equal(exceptionToThrow.Message, actual.Failure);
        }
        #pragma warning restore 162

        [Theory, AutoData]
        public void MaybeToResultMapsSomeToSuccess(
            int someValue,
            string failureText)
        {
            var maybeValue = someValue.ToMaybe();
            var failureForEmpty = Func(() => failureText);

            var actual = maybeValue.ToResult(
                ifEmpty: failureForEmpty);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(someValue, actual.Success);
        }

        [Theory, AutoData]
        public void MaybeToResultMapsNoneToFailure(
            string failureText)
        {
            var maybeValue = Maybe<int>.Empty();
            var failureForEmpty = Func(() => failureText);

            var actual = maybeValue.ToResult(
                ifEmpty: failureForEmpty);

            Assert.False(actual.IsSuccessful);
            Assert.Equal(failureText, actual.Failure);
        }

        [Theory, AutoData]
        public async void TaskMaybeToResultMapsSomeToSuccess(
            int someValue,
            string failureText)
        {
            var maybeValueAsync = Func(() => Task.FromResult(someValue.ToMaybe()));
            var failureForEmpty = Func(() => failureText);

            var actual = await maybeValueAsync()
                .ToResult(
                    ifEmpty: failureForEmpty);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(someValue, actual.Success);
        }

        [Theory, AutoData]
        public async void TaskMaybeToResultMapsNoneToFailure(
            string failureText)
        {
            var maybeValueAsync = Func(() => Task.FromResult(Maybe<int>.Empty()));
            var failureForEmpty = Func(() => failureText);

            var actual = await maybeValueAsync()
                .ToResult(
                    ifEmpty: failureForEmpty);


            Assert.False(actual.IsSuccessful);
            Assert.Equal(failureText, actual.Failure);
        }

        [Theory, AutoData]
        public void SelectManyLetsUsUseWeirdSyntax(string success1, string success2)
        {
            var result1 = Result<string, bool>.Succeed(success1);
            var result2 = Result<string, bool>.Succeed(success2);

            var actual = from a in result1
                         from b in result2
                         select $"Success: {a} {b}";

            Assert.True(actual.IsSuccessful);
            Assert.Contains(success1, actual.Success);
            Assert.Contains(success2, actual.Success);
        }

        [Theory, AutoData]
        public void SelectManyLetsUsUseWeirdSyntaxWithFailures(
            string success1, 
            string success2,
            int failure)
        {
            var result1 = Result<string, int>.Succeed(success1);
            var result2 = Result<string, int>.Succeed(success2);
            var result3 = Result<string, int>.Fail(failure);

            var actual = from a in result1
                         from b in result2
                         from c in result3
                         select $"Success: {a} {b} {c}";

            Assert.False(actual.IsSuccessful);
            Assert.Equal(failure, actual.Failure);
        }

        [Theory, AutoData]
        public void SelectAndSelectManyLetsUsUseLetKeywordInLinqExpressions(
            string success1,
            string success2)
        {
            var result1 = Result<string, bool>.Succeed(success1);
            var result2 = Result<string, bool>.Succeed(success2);

            var actual =
                from a in result1
                from b in result2
                let r = $"Success: {a} {b}"
                select r;

            Assert.True(actual.IsSuccessful);
            Assert.Contains(success1, actual.Success);
            Assert.Contains(success2, actual.Success);
        }

        [Theory, AutoData]
        public async Task SelectManyLetsUsUseAsyncResultLinqExpressions(
            string success1,
            string success2)
        {
            var asyncResult1 = Task.FromResult(Result<string, bool>.Succeed(success1));
            var asyncResult2 = Task.FromResult(Result<string, bool>.Succeed(success2));

            var actual = await (
                from a in asyncResult1
                from b in asyncResult2
                select $"Success: {a} {b}" );

            Assert.True(actual.IsSuccessful);
            Assert.Contains(success1, actual.Success);
            Assert.Contains(success2, actual.Success);
        }

        [Theory, AutoData]
        public async Task SelectManyLetsUsUseAsyncResultLinqExpressionsWithFailures(
            string success1, 
            string success2,
            int failure)
        {
            var asyncResult1 = Task.FromResult(Result<string, int>.Succeed(success1));
            var asyncResult2 = Task.FromResult(Result<string, int>.Succeed(success2));
            var asyncResult3 = Task.FromResult(Result<string, int>.Fail(failure));

            var actual = await (
                from a in asyncResult1
                from b in asyncResult2
                from c in asyncResult3
                select $"Success: {a} {b} {c}" );

            Assert.False(actual.IsSuccessful);
            Assert.Equal(failure, actual.Failure);
        }

        [Theory, AutoData]
        public async Task SelectManyLetsUsUseResultInAsyncResultLinqExpressions(
            string success1,
            string success2,
            int failure)
        {
            var asyncResult = Task.FromResult(Result<string, int>.Succeed(success1));
            var result = Result<string, int>.Succeed(success2);
            var asyncFailureResult = Task.FromResult(Result<string, int>.Fail(failure));

            var actual = await (
                from a in asyncResult
                from b in result
                from c in asyncFailureResult
                select $"Success: {a} {b}" );

            Assert.False(actual.IsSuccessful);
            Assert.Equal(failure, actual.Failure);
        }

        [Theory, AutoData]
        public async Task SelectManyLetsUsUseResultInAsyncResultLinqExpressionsWithFailure(
            string success1,
            string success2)
        {
            var asyncResult = Task.FromResult(Result<string, bool>.Succeed(success1));
            var result = Result<string, bool>.Succeed(success2);

            var actual = await (
                from a in asyncResult
                from b in result
                select $"Success: {a} {b}" );

            Assert.True(actual.IsSuccessful);
            Assert.Contains(success1, actual.Success);
            Assert.Contains(success2, actual.Success);
        }

        [Theory, AutoData]
        public async Task SelectAndSelectManyLetsUsUseLetKeywordInAsyncResultLinqExpressions(
            string success1,
            string success2)
        {
            var asyncResult1 = Task.FromResult(Result<string, bool>.Succeed(success1));
            var asyncResult2 = Task.FromResult(Result<string, bool>.Succeed(success2));

            var actual = await (
                from a in asyncResult1
                from b in asyncResult2
                let r = $"Success: {a} {b}"
                select r );

            Assert.True(actual.IsSuccessful);
            Assert.Contains(success1, actual.Success);
            Assert.Contains(success2, actual.Success);
        }

        [Theory, AutoData]
        public void TeeBindWorksForSuccess(string success1, string success2)
        {
            var result1 = Result<string, bool>.Succeed(success1);
            var func =
                Func((string x) =>
                {
                    Assert.Equal(success1, x);
                    return Result<string, bool>.Succeed(success2);
                });

            var actual = result1.TeeBind(func);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(actual.Success, success1);
        }

        [Theory, AutoData]
        public async Task TeeBindWorksForSuccessTask(string success1, string success2)
        {
            var result1 = Task.FromResult(Result<string, bool>.Succeed(success1));
            var func =
                Func((string x) =>
                {
                    Assert.Equal(success1, x);
                    return Result<string, bool>.Succeed(success2);
                });

            var actual = await result1.TeeBind(func);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(actual.Success, success1);
        }

        [Theory, AutoData]
        public void TeeBindWorksForSuccessWithFailingFunc(string success, bool failure)
        {
            var result1 = Result<string, bool>.Succeed(success);
            var func =
                Func((string x) =>
                {
                    Assert.Equal(success, x);
                    return Result<string, bool>.Fail(failure);
                });

            var actual = result1.TeeBind(func);

            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, failure);
        }

        [Theory, AutoData]
        public void TeeBindWorksForFailure(string failure1, string success)
        {
            var result1 = Result<bool, string>.Fail(failure1);
            var func = Func((bool _) =>
            {
                throw new Exception("Should not be called");
#pragma warning disable CS0162 // Unreachable code detected
                return Result<string, string>.Succeed(success);
#pragma warning restore CS0162 // Unreachable code detected
            });

            var actual = result1.TeeBind(func);

            Assert.False(actual.IsSuccessful);
            Assert.Equal(actual.Failure, failure1);
        }

        [Theory, AutoData]
        public async Task TeeBindAsyncWorksForSuccess(string success1, string success2)
        {
            var result = Result<string, bool>.Succeed(success1);
            var func =
                Func((string x) =>
                {
                    Assert.Equal(success1, x);
                    return Task.FromResult(Result<string, bool>.Succeed(success2));
                });

            var actual = await result.TeeBindAsync(func);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(success1, actual.Success);
        }

        [Theory, AutoData]
        public async Task TeeBindAsyncWorksForFailure(string failure1, string success)
        {
            var result = Result<string, string>.Fail(failure1);
            var func = Func((string _) =>
            {
                throw new Exception("Should not be called");
#pragma warning disable CS0162 // Unreachable code detected
                return Task.FromResult(Result<string, string>.Succeed(success));
#pragma warning restore CS0162 // Unreachable code detected
            });

            var actual = await result.TeeBindAsync(func);

            Assert.False(actual.IsSuccessful);
            Assert.Equal(failure1, actual.Failure);
        }

        [Theory, AutoData]
        public void ApplyIsTheSameAsJustCallingFunctionsWithValues(int input)
        {
            var func = Func((int i) => i + 1);
            var expected = func(input);
            var funcResult = Result<Func<int, int>, string>.Succeed(func);
            var inputResult = Result<int, string>.Succeed(input);

            var actual = funcResult.Apply(inputResult);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(expected, actual.Success);
        }

        [Theory, AutoData]
        public void LiftAndApplyAreTheSameAsJustCallingFunctionsWithValues(
            int input1,
            int input2)
        {
            var func = Func((int i, int j) => i + j + 1);
            var expected = func(input1, input2);
            var inputResult1 = Result<int, string>.Succeed(input1);
            var inputResult2 = Result<int, string>.Succeed(input2);

            var actual = func
                .Curry()
                .Lift(inputResult1)
                .Apply(inputResult2);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(expected, actual.Success);
        }

        [Theory, AutoData]
        public void LiftReturnsFailureForFailedResult(string failure)
        {
            var func = Func((int i) => i + 1);
            var funcResult = Result<Func<int, int>, string>.Succeed(func);
            var inputResult = Result<int, string>.Fail(failure);

            var actual = funcResult.Apply(inputResult);

            Assert.False(actual.IsSuccessful);
        }

        [Theory, AutoData]
        public void ApplyReturnsFailureForFailedResult(
            int success, 
            string failure)
        {
            var func = Func((int i, int j) => i + j + 1);
            var input1Result = Result<int, string>.Succeed(success);
            var input2Result = Result<int, string>.Fail(failure);

            var actual = func
                .Curry()
                .Lift(input1Result)
                .Apply(input2Result);

            Assert.False(actual.IsSuccessful);
        }

        [Theory, AutoData]
        public async Task LiftAndApplyAsyncAreCoolAndGood(int i1, int i2)
        {
            var func = Func((int i, int j) => i + j + 1);
            var expected = func(i1, i2);
            var input1 = Task.FromResult(Result<int, string>.Succeed(i1));
            var input2 = Task.FromResult(Result<int, string>.Succeed(i2));

            var actual =
                await func.Curry()
                    .LiftAsync(input1)
                    .ApplyAsync(input2);

            Assert.True(actual.IsSuccessful);
            Assert.Equal(expected, actual.Success);
        }
    }
}
