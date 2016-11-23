using System;
using System.Threading.Tasks;
using E247.Fun.Exceptions;
using static E247.Fun.Fun;
using static E247.Fun.Unit;

#pragma warning disable 1591

// ReSharper disable RedundantArgumentName

namespace E247.Fun
{
    public struct Result<TSuccess, TFailure>
    {
        private TSuccess _successValue;
        private TFailure _failureValue;

        public TSuccess Success
        {
            get
            {
                if (!IsSuccessful)
                    throw new ResultAccessException(
                        "Can't access success on an unsuccessful result");

                return _successValue;
            }
            private set { _successValue = value; }
        }

        public TFailure Failure
        {
            get
            {
                if (IsSuccessful)
                    throw new ResultAccessException(
                        "Can't access failure on a successful result");

                return _failureValue;
            }
            private set { _failureValue = value; }
        }

        public bool IsSuccessful { get; }

        private Result(bool success) : this()
        {
            IsSuccessful = success;
        }

        public static Result<TSuccess, TFailure> Succeed(TSuccess success)
        {
            if (success == null)
                throw new ArgumentNullException(nameof(success));

            return new Result<TSuccess, TFailure>(true)
            {
                Success = success
            };
        }

        public static Result<TSuccess, TFailure> Fail(TFailure failure)
        {
            if (failure == null)
                throw new ArgumentNullException(nameof(failure));

            return new Result<TSuccess, TFailure>(false)
            {
                Failure = failure
            };
        }

        public static implicit operator Result<TSuccess, TFailure>(TSuccess value) => Succeed(value);

        public static implicit operator Result<TSuccess, TFailure>(TFailure error) => Fail(error);

        public static Result<TSuccess, TFailure> Try<TException, TS, TF>(
            Func<TS> f,
            Func<TException, TF> handler)
            where TException : Exception
            where TS : struct, TSuccess
            where TF : TFailure
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            try
            {
                return f();
            }
            catch (TException ex)
            {
                return handler(ex);
            }
        }

        public static Result<TSuccess, TFailure> Try<TException, TS, TF>(
            Func<TS> f,
            Func<TException, TF> handler,
            Func<TF> nullHandler)
            where TException : Exception
            where TS : class, TSuccess
            where TF : TFailure
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (nullHandler == null)
                throw new ArgumentNullException(nameof(nullHandler));

            try
            {
                var result = f();
                if (ReferenceEquals(result, null))
                {
                    return nullHandler();
                }
                return result;
            }
            catch (TException ex)
            {
                return handler(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TException, TS, TF>(
           Func<Task<TS>> f,
           Func<TException, TF> handler)
            where TException : Exception
            where TS : struct, TSuccess
            where TF : TFailure
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            try
            {
                return await f();
            }
            catch (TException ex)
            {
                return handler(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TException, TS, TF>(
           Func<Task<TS>> f,
           Func<TException, TF> handler,
           Func<TF> nullHandler)
            where TException : Exception
            where TS : class, TSuccess
            where TF : TFailure
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (nullHandler == null)
                throw new ArgumentNullException(nameof(nullHandler));

            try
            {
                var result = await f();
                if (ReferenceEquals(result, null))
                {
                    return nullHandler();
                }
                return result;
            }
            catch (TException ex)
            {
                return handler(ex);
            }
        }

        public static async Task<Result<Unit, TFailure>> TryAsync<TException>(
           Func<Task> f,
           Func<TException, TFailure> handler)
           where TException : Exception
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            try
            {
                await f();
                return unit;
            }
            catch (TException ex)
            {
                return handler(ex);
            }
        }
    }

    public static class Result
    {
        public static Unit Success =>
            unit;

        public static Result<Unit, TFailure> Try<TException, TFailure>(
            Action tryDo,
            Func<TException, TFailure> failWith)
            where TException : Exception =>
                Result<Unit, TFailure>.Try(
                    () =>
                    {
                        tryDo();
                        return unit;
                    },
                    failWith);

        public static Result<TSuccess, TFailure> Try<TException, TSuccess, TFailure>(
            Func<TSuccess> tryGet,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : struct =>
                Result<TSuccess, TFailure>.Try(tryGet, failWith);

        public static Task<Result<Unit, TFailure>> TryAsync<TException, TFailure>(
            Func<Task> tryDo,
            Func<TException, TFailure> failWith)
            where TException : Exception =>
                Result<Unit, TFailure>.TryAsync(tryDo, failWith);

        public static Task<Result<TSuccess, TFailure>> TryAsync<TException, TSuccess, TFailure>(
           Func<Task<TSuccess>> tryGet,
           Func<TException, TFailure> failWith)
           where TException : Exception
           where TSuccess : struct =>
               Result<TSuccess, TFailure>.TryAsync(tryGet, failWith);

        public static Result<TSuccess, TFailure> Try<TException, TSuccess, TFailure>(
            Func<TSuccess> tryGet,
            Func<TException, TFailure> failWith,
            Func<TFailure> ifNull)
            where TException : Exception
            where TSuccess : class =>
                Result<TSuccess, TFailure>.Try(tryGet, failWith, ifNull);

        public static Task<Result<TSuccess, TFailure>> TryAsync<TException, TSuccess, TFailure>(
            Func<Task<TSuccess>> tryGet,
            Func<TException, TFailure> failWith,
            Func<TFailure> ifNull)
            where TException : Exception
            where TSuccess : class =>
                Result<TSuccess, TFailure>.TryAsync(tryGet, failWith, ifNull);
    }

    public static class ResultExtensions
    {
        public static Result<TSuccess, TFailure> ToResult<TSuccess, TFailure>(
            this Maybe<TSuccess> @this,
            Func<TFailure> ifEmpty) =>
                @this.Match(
                    Some: Result<TSuccess, TFailure>.Succeed,
                    None: () => ifEmpty());

        public static async Task<Result<TSuccess, TFailure>> ToResult<TSuccess, TFailure>(
            this Task<Maybe<TSuccess>> @this,
            Func<TFailure> ifEmpty) =>
                await @this.Match(
                    Some: Result<TSuccess, TFailure>.Succeed,
                    None: () => ifEmpty());

        public static Result<TNewSuccess, TFailure> Map<TSuccess, TFailure, TNewSuccess>(
            this Result<TSuccess, TFailure> @this, Func<TSuccess, TNewSuccess> mapper) =>
                @this.MapSuccess(mapper);

        public static Task<Result<TNewSuccess, TFailure>> Map<TSuccess, TFailure, TNewSuccess>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, TNewSuccess> mapper) =>
                @this.MapSuccess(mapper);

        public static Task<Result<TNewSuccess, TFailure>> MapAsync<TSuccess, TFailure, TNewSuccess>(
            this Result<TSuccess, TFailure> @this, Func<TSuccess, Task<TNewSuccess>> mapper) =>
                @this.MapSuccessAsync(mapper);

        public static Task<Result<TNewSuccess, TFailure>> MapAsync<TSuccess, TFailure, TNewSuccess>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, Task<TNewSuccess>> mapper) =>
                @this.MapSuccessAsync(mapper);

        public static Result<TNewSuccess, TFailure> MapSuccess<TSuccess, TFailure, TNewSuccess>(
            this Result<TSuccess, TFailure> result, Func<TSuccess, TNewSuccess> mapper)
        {
            return result.IsSuccessful
                ? Result<TNewSuccess, TFailure>.Succeed(mapper(result.Success))
                : Result<TNewSuccess, TFailure>.Fail(result.Failure);
        }

        public static async Task<Result<TNewSuccess, TFailure>> MapSuccess<TSuccess, TFailure, TNewSuccess>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, TNewSuccess> mapper)
        {
            var result = await @this;
            return result.IsSuccessful
                ? Result<TNewSuccess, TFailure>.Succeed(mapper(result.Success))
                : Result<TNewSuccess, TFailure>.Fail(result.Failure);
        }

        public static async Task<Result<TNewSuccess, TFailure>> MapSuccessAsync<TSuccess, TFailure, TNewSuccess>(
            this Result<TSuccess, TFailure> result, Func<TSuccess, Task<TNewSuccess>> mapper)
        {
            return result.IsSuccessful
                ? Result<TNewSuccess, TFailure>.Succeed(await mapper(result.Success))
                : Result<TNewSuccess, TFailure>.Fail(result.Failure);
        }

        public static async Task<Result<TNewSuccess, TFailure>> MapSuccessAsync<TSuccess, TFailure, TNewSuccess>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, Task<TNewSuccess>> mapper)
        {
            var result = await @this;
            return result.IsSuccessful
                ? Result<TNewSuccess, TFailure>.Succeed(await mapper(result.Success))
                : Result<TNewSuccess, TFailure>.Fail(result.Failure);
        }

        public static Result<TSuccess, TNewFailure> MapFailure<TSuccess, TFailure, TNewFailure>(
            this Result<TSuccess, TFailure> result, Func<TFailure, TNewFailure> mapper)
        {
            return result.IsSuccessful
                ? Result<TSuccess, TNewFailure>.Succeed(result.Success)
                : Result<TSuccess, TNewFailure>.Fail(mapper(result.Failure));
        }

        public static async Task<Result<TSuccess, TNewFailure>> MapFailure<TSuccess, TFailure, TNewFailure>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TFailure, TNewFailure> mapper)
        {
            var result = await @this;
            return result.IsSuccessful
                ? Result<TSuccess, TNewFailure>.Succeed(result.Success)
                : Result<TSuccess, TNewFailure>.Fail(mapper(result.Failure));
        }

        public static async Task<Result<TSuccess, TNewFailure>> MapFailureAsync<TSuccess, TFailure, TNewFailure>(
            this Result<TSuccess, TFailure> result, Func<TFailure, Task<TNewFailure>> mapper)
        {
            return result.IsSuccessful
                ? Result<TSuccess, TNewFailure>.Succeed(result.Success)
                : Result<TSuccess, TNewFailure>.Fail(await mapper(result.Failure));
        }

        public static async Task<Result<TSuccess, TNewFailure>> MapFailureAsync<TSuccess, TFailure, TNewFailure>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TFailure, Task<TNewFailure>> mapper)
        {
            var result = await @this;
            return result.IsSuccessful
                ? Result<TSuccess, TNewFailure>.Succeed(result.Success)
                : Result<TSuccess, TNewFailure>.Fail(await mapper(result.Failure));
        }

        // ReSharper disable InconsistentNaming

        // Bind
        public static Result<TNewSuccess, TFailure> Bind<TSuccess, TFailure, TNewSuccess>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Result<TNewSuccess, TFailure>> binder)
        {
            return @this.IsSuccessful
                ? binder(@this.Success)
                : Result<TNewSuccess, TFailure>.Fail(@this.Failure);
        }

        public static async Task<Result<TNewSuccess, TFailure>> Bind<TSuccess, TFailure, TNewSuccess>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Result<TNewSuccess, TFailure>> binder)
        {
            var value = await @this;
            return value.IsSuccessful
                ? binder(value.Success)
                : Result<TNewSuccess, TFailure>.Fail(value.Failure);
        }

        public static async Task<Result<TNewSuccess, TFailure>> BindAsync<TSuccess, TFailure, TNewSuccess>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> binder)
        {
            return @this.IsSuccessful
                ? await binder(@this.Success)
                : Result<TNewSuccess, TFailure>.Fail(@this.Failure);
        }

        public static async Task<Result<TNewSuccess, TFailure>> BindAsync<TSuccess, TFailure, TNewSuccess>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> binder)
        {
            var value = await @this;
            return value.IsSuccessful
                ? await binder(value.Success)
                : Result<TNewSuccess, TFailure>.Fail(value.Failure);
        }

        public static Result<TNewSuccess, TNewFailure> Bind<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Result<TNewSuccess, TNewFailure>> binder,
            Func<TFailure, TNewFailure> mapFailure)
        {
            return @this.IsSuccessful
                ? binder(@this.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(mapFailure(@this.Failure));
        }

        public static async Task<Result<TNewSuccess, TNewFailure>> Bind<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Result<TNewSuccess, TNewFailure>> binder,
            Func<TFailure, TNewFailure> mapFailure)
        {
            var value = await @this;
            return value.IsSuccessful
                ? binder(value.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(mapFailure(value.Failure));
        }

        public static async Task<Result<TNewSuccess, TNewFailure>> BindAsync<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TNewFailure>>> binder,
            Func<TFailure, Task<TNewFailure>> mapFailure)
        {
            return @this.IsSuccessful
                ? await binder(@this.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(await mapFailure(@this.Failure));
        }

        public static async Task<Result<TNewSuccess, TNewFailure>> BindAsync<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TNewFailure>>> binder,
            Func<TFailure, TNewFailure> mapFailure)
        {
            return @this.IsSuccessful
                ? await binder(@this.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(mapFailure(@this.Failure));
        }

        public static async Task<Result<TNewSuccess, TNewFailure>> BindAsync<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Result<TNewSuccess, TNewFailure>> binder,
            Func<TFailure, Task<TNewFailure>> mapFailure)
        {
            return @this.IsSuccessful
                ? binder(@this.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(await mapFailure(@this.Failure));
        }

        public static async Task<Result<TNewSuccess, TNewFailure>> BindAsync<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TNewFailure>>> binder,
            Func<TFailure, Task<TNewFailure>> mapFailure)
        {
            var value = await @this;
            return value.IsSuccessful
                ? await binder(value.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(await mapFailure(value.Failure));
        }

        public static async Task<Result<TNewSuccess, TNewFailure>> BindAsync<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TNewFailure>>> binder,
            Func<TFailure, TNewFailure> mapFailure)
        {
            var value = await @this;
            return value.IsSuccessful
                ? await binder(value.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(mapFailure(value.Failure));
        }

        public static async Task<Result<TNewSuccess, TNewFailure>> BindAsync<TSuccess, TFailure, TNewSuccess, TNewFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Result<TNewSuccess, TNewFailure>> binder,
            Func<TFailure, Task<TNewFailure>> mapFailure)
        {
            var value = await @this;
            return value.IsSuccessful
                ? binder(value.Success)
                : Result<TNewSuccess, TNewFailure>.Fail(await mapFailure(value.Failure));
        }


        // Match
        public static TResult Match<TResult, TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this, Func<TSuccess, TResult> Success, Func<TFailure, TResult> Failure) =>
            @this.IsSuccessful
                ? Success(@this.Success)
                : Failure(@this.Failure);
        public static async Task<TResult> Match<TResult, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, TResult> Success, Func<TFailure, TResult> Failure)
        {
            var result = await @this;
            return result.IsSuccessful
                ? Success(result.Success)
                : Failure(result.Failure);
        }

        public static async Task<TResult> MatchAsync<TResult, TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this, Func<TSuccess, Task<TResult>> Success, Func<TFailure, Task<TResult>> Failure) =>
            @this.IsSuccessful
                ? await Success(@this.Success)
                : await Failure(@this.Failure);
        public static async Task<TResult> MatchAsync<TResult, TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this, Func<TSuccess, Task<TResult>> Success, Func<TFailure, TResult> Failure) =>
            @this.IsSuccessful
                ? await Success(@this.Success)
                : Failure(@this.Failure);
        public static async Task<TResult> MatchAsync<TResult, TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this, Func<TSuccess, TResult> Success, Func<TFailure, Task<TResult>> Failure) =>
            @this.IsSuccessful
                ? Success(@this.Success)
                : await Failure(@this.Failure);
        public static async Task<TResult> MatchAsync<TResult, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, Task<TResult>> Success, Func<TFailure, Task<TResult>> Failure)
        {
            var result = await @this;
            return result.IsSuccessful
                ? await Success(result.Success)
                : await Failure(result.Failure);
        }
        public static async Task<TResult> MatchAsync<TResult, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, Task<TResult>> Success, Func<TFailure, TResult> Failure)
        {
            var result = await @this;
            return result.IsSuccessful
                ? await Success(result.Success)
                : Failure(result.Failure);
        }
        public static async Task<TResult> MatchAsync<TResult, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this, Func<TSuccess, TResult> Success, Func<TFailure, Task<TResult>> Failure)
        {
            var result = await @this;
            return result.IsSuccessful
                ? Success(result.Success)
                : await Failure(result.Failure);
        }

        public static Unit Match<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this,
            Action<TSuccess> Success,
            Action<TFailure> Failure) =>
            @this.IsSuccessful
                ? Func(Success)(@this.Success)
                : Func(Failure)(@this.Failure);

        public static async Task<Unit> Match<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Action<TSuccess> Success,
            Action<TFailure> Failure)
        {
            var result = await @this;
            return result.IsSuccessful
                ? Func(Success)(result.Success)
                : Func(Failure)(result.Failure);
        }

        public static async Task<Unit> MatchAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task> Success,
            Func<TFailure, Task> Failure)
        {
            if (@this.IsSuccessful)
            {
                await Success(@this.Success);
            }
            else
            {
                await Failure(@this.Failure);
            }
            return unit;
        }

        public static async Task<Unit> MatchAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task> Success,
            Action<TFailure> Failure)
        {
            if (@this.IsSuccessful)
            {
                await Success(@this.Success);
            }
            else
            {
                Failure(@this.Failure);
            }
            return unit;
        }

        public static async Task<Unit> MatchAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this,
            Action<TSuccess> Success,
            Func<TFailure, Task> Failure)
        {
            if (@this.IsSuccessful)
            {
                Success(@this.Success);
            }
            else
            {
                await Failure(@this.Failure);
            }
            return unit;
        }


        public static async Task<Unit> MatchAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task> Success,
            Func<TFailure, Task> Failure)
        {
            var value = await @this;
            if (value.IsSuccessful)
            {
                await Success(value.Success);
            }
            else
            {
                await Failure(value.Failure);
            }
            return unit;
        }

        public static async Task<Unit> MatchAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task> Success,
            Action<TFailure> Failure)
        {
            var value = await @this;
            if (value.IsSuccessful)
            {
                await Success(value.Success);
            }
            else
            {
                Failure(value.Failure);
            }
            return unit;
        }

        public static async Task<Unit> MatchAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Action<TSuccess> Success,
            Func<TFailure, Task> Failure)
        {
            var value = await @this;
            if (value.IsSuccessful)
            {
                Success(value.Success);
            }
            else
            {
                await Failure(value.Failure);
            }
            return unit;
        }

        public static Maybe<TSuccess> ToMaybe<TSuccess, TFailure>(this Result<TSuccess, TFailure> @this) =>
            @this.IsSuccessful
                ? @this.Success
                : Maybe<TSuccess>.Empty();

        public static async Task<Maybe<TSuccess>> ToMaybe<TSuccess, TFailure>(this Task<Result<TSuccess, TFailure>> @this)
        {
            var result = await @this;
            return result.IsSuccessful
                ? result.Success
                : Maybe<TSuccess>.Empty();
        }

        public static Result<TSuccess, TFailure> Try<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, TSuccess> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : class
        {
            try
            {
                var result = func(@this);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> Try<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, TSuccess> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : class
        {
            var value = await @this;
            try
            {
                var result = func(value);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : class
        {
            try
            {
                var result = await func(@this);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, Task<TSuccess>> func,
            Func<Task<TFailure>> ifNull,
            Func<TException, Task<TFailure>> failWith)
            where TException : Exception
            where TSuccess : class
        {
            try
            {
                var result = await func(@this);
                if (ReferenceEquals(result, null))
                {
                    return await ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return await failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TFailure> ifNull,
            Func<TException, Task<TFailure>> failWith)
            where TException : Exception
            where TSuccess : class
        {
            try
            {
                var result = await func(@this);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return await failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, TSuccess> func,
            Func<TFailure> ifNull,
            Func<TException, Task<TFailure>> failWith)
            where TException : Exception
            where TSuccess : class
        {
            try
            {
                var result = func(@this);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return await failWith(ex);
            }
        }


        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : class
        {
            var value = await @this;
            try
            {
                var result = await func(value);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, Task<TSuccess>> func,
            Func<Task<TFailure>> ifNull,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : class
        {
            var value = await @this;
            try
            {
                var result = await func(value);
                if (ReferenceEquals(result, null))
                {
                    return await ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TFailure> ifNull,
            Func<TException, Task<TFailure>> failWith)
            where TException : Exception
            where TSuccess : class
        {
            var value = await @this;
            try
            {
                var result = await func(value);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return await failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, TSuccess> func,
            Func<TFailure> ifNull,
            Func<TException, Task<TFailure>> failWith)
            where TException : Exception
            where TSuccess : class
        {
            var value = await @this;
            try
            {
                var result = func(value);
                if (ReferenceEquals(result, null))
                {
                    return ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return await failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, TSuccess> func,
            Func<Task<TFailure>> ifNull,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : class
        {
            var value = await @this;
            try
            {
                var result = func(value);
                if (ReferenceEquals(result, null))
                {
                    return await ifNull();
                }
                return result;
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static Result<TSuccess, TFailure> Try<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, TSuccess> func,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : struct
        {
            try
            {
                return func(@this);
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> Try<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, TSuccess> func,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : struct
        {
            var value = await @this;
            try
            {
                return func(value);
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : struct
        {
            try
            {
                return await func(@this);
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this TValue @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TException, Task<TFailure>> failWith)
            where TException : Exception
            where TSuccess : struct
        {
            try
            {
                return await func(@this);
            }
            catch (TException ex)
            {
                return await failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TException, TFailure> failWith)
            where TException : Exception
            where TSuccess : struct
        {
            var value = await @this;
            try
            {
                return await func(value);
            }
            catch (TException ex)
            {
                return failWith(ex);
            }
        }

        public static async Task<Result<TSuccess, TFailure>> TryAsync<TValue, TSuccess, TFailure, TException>(
            this Task<TValue> @this,
            Func<TValue, Task<TSuccess>> func,
            Func<TException, Task<TFailure>> failWith)
            where TException : Exception
            where TSuccess : struct
        {
            var value = await @this;
            try
            {
                return await func(value);
            }
            catch (TException ex)
            {
                return await failWith(ex);
            }
        }

        // BindTry
        // non nullable
        public static Result<TNewSuccess, TFailure> BindTry<TSuccess, TFailure, TNewSuccess, TException>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, TNewSuccess> func,
            Func<TException, TFailure> failWith)
            where TNewSuccess : struct
            where TException : Exception =>
                @this
                    .Bind(v => v
                        .Try(func,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTry<TSuccess, TFailure, TNewSuccess, TException>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, TNewSuccess> func,
            Func<TException, TFailure> failWith)
            where TNewSuccess : struct
            where TException : Exception =>
                @this
                    .Bind(v => v
                        .Try(func,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTryAsync<TSuccess, TFailure, TNewSuccess, TException>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task<TNewSuccess>> func,
            Func<TException, TFailure> failWith)
            where TNewSuccess : struct
            where TException : Exception =>
                @this
                    .BindAsync(v => v
                        .TryAsync(func,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTryAsync<TSuccess, TFailure, TNewSuccess, TException>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<TNewSuccess>> func,
            Func<TException, TFailure> failWith)
            where TNewSuccess : struct
            where TException : Exception =>
                @this
                    .BindAsync(v => v
                        .TryAsync(func,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTryAsync<TSuccess, TFailure, TNewSuccess, TException>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<TNewSuccess>> func,
            Func<TException, Task<TFailure>> failWith)
            where TNewSuccess : struct
            where TException : Exception =>
                @this
                    .BindAsync(v => v
                        .TryAsync(func,
                            failWith: failWith));


        // nullable
        public static Result<TNewSuccess, TFailure> BindTry<TSuccess, TFailure, TNewSuccess, TException>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, TNewSuccess> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TNewSuccess : class
            where TException : Exception =>
                @this
                    .Bind(v => v
                        .Try(func,
                            ifNull: ifNull,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTry<TSuccess, TFailure, TNewSuccess, TException>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, TNewSuccess> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TNewSuccess : class
            where TException : Exception =>
                @this
                    .Bind(v => v
                        .Try(func,
                            ifNull: ifNull,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTryAsync<TSuccess, TFailure, TNewSuccess, TException>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task<TNewSuccess>> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TNewSuccess : class
            where TException : Exception =>
                @this
                    .BindAsync(v => v
                        .TryAsync(func,
                            ifNull: ifNull,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTryAsync<TSuccess, TFailure, TNewSuccess, TException>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<TNewSuccess>> func,
            Func<TFailure> ifNull,
            Func<TException, TFailure> failWith)
            where TNewSuccess : class
            where TException : Exception =>
                @this
                    .BindAsync(v => v
                        .TryAsync(func,
                            ifNull: ifNull,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTryAsync<TSuccess, TFailure, TNewSuccess, TException>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<TNewSuccess>> func,
            Func<Task<TFailure>> ifNull,
            Func<TException, Task<TFailure>> failWith)
            where TNewSuccess : class
            where TException : Exception =>
                @this
                    .BindAsync(v => v
                        .TryAsync(func,
                            ifNull: ifNull,
                            failWith: failWith));

        public static Task<Result<TNewSuccess, TFailure>> BindTryAsync<TSuccess, TFailure, TNewSuccess, TException>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<TNewSuccess>> func,
            Func<TFailure> ifNull,
            Func<TException, Task<TFailure>> failWith)
            where TNewSuccess : class
            where TException : Exception =>
                @this
                    .BindAsync(v => v
                        .TryAsync(func,
                            ifNull: ifNull,
                            failWith: failWith));

        public static Result<C, Fail> SelectMany<A, B, C, Fail>(
            this Result<A, Fail> a, 
            Func<A, Result<B, Fail>> func, 
            Func<A, B, C> select)
        {
            return a.Bind(x => func(x)
                    .Bind<B, Fail, C>(y => select(x, y)));
        }

        public static Result<TSuccess, TFailure> TeeBind<TSuccess, TNewSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Result<TNewSuccess, TFailure>> func) =>
            @this.Bind(func).Map((TNewSuccess _) => @this.Success);

        public static Task<Result<TSuccess, TFailure>> TeeBind<TSuccess, TNewSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Result<TNewSuccess, TFailure>> func) =>
            @this.Bind(func).MapAsync(async (TNewSuccess _) =>(await  @this).Success);

        public static Task<Result<TSuccess, TFailure>> TeeBindAsync<TSuccess, TNewSuccess, TFailure>(
            this Result<TSuccess, TFailure> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> func) =>
            @this.BindAsync(func).Map((TNewSuccess _) => @this.Success);

        public static Task<Result<TSuccess, TFailure>> TeeBindAsync<TSuccess, TNewSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> @this,
            Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> func) =>
            @this.BindAsync(func).MapAsync(async (TNewSuccess _) => (await @this).Success);

        public static Result<TNewSuccess, TFailure> Apply<TSuccess,TNewSuccess,TFailure>(
            this Result<Func<TSuccess, TNewSuccess>, TFailure> func,
            Result<TSuccess, TFailure> input)
        {
            if (!func.IsSuccessful)
                return func.Failure;

            if(!input.IsSuccessful)
                return input.Failure;

            return func.Success(input.Success);
        }

        public static Result<TNewSuccess, TFailure> Lift<TSuccess, TNewSuccess, TFailure>(
            this Func<TSuccess, TNewSuccess> func,
            Result<TSuccess, TFailure> input)
        {
            if (!input.IsSuccessful)
                return input.Failure;

            return func(input.Success);
        }
    }
}
