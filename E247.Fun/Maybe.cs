using System;
using System.Threading.Tasks;
using E247.Fun.Exceptions;
using static E247.Fun.Fun;
using static E247.Fun.Unit;

// ReSharper disable InconsistentNaming

#pragma warning disable 1591

namespace E247.Fun
{
    /// <summary>
    /// Represents a value that may or may not be available
    /// </summary>
    /// <typeparam name="T">The type of the value that may be available</typeparam>
    public struct Maybe<T> : IEquatable<Maybe<T>>, IEquatable<T>
    {
        private readonly T _value;

        /// <summary>
        /// A boolean representation for if this Maybe does actually contain a value
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value (if it exists) or THROWS if this maybe is empty
        /// </summary>
        /// <exception cref="EmptyMaybeException">Thrown if this is accessed when the Maybe is empty</exception>
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new EmptyMaybeException();

                return _value;
            }
        }
         
        /// <summary>
        /// Create a new empty maybe of the given type param
        /// </summary>
        public static Maybe<T> Empty()
        {
            return new Maybe<T>();
        }

        /// <summary>
        /// Create a new maybe from the given type, if the value provided is null the maybe will be empty
        /// </summary>
        /// <param name="input"></param>
        public Maybe(T input)
        {
            if (input == null)
            {
                HasValue = false;
                _value = default(T);
            }
            else
            {
                _value = input;
                HasValue = true;
            }
        }

        /// <see cref="HasValue"/>
        public bool Any() => HasValue;
        public T Single() => Value;
        [Obsolete("Using SingleOrDefault can return null from a Maybe, when the whole point of this type is to avoid nulls. Using this makes you a bad person.")]
        public T SingleOrDefault() => HasValue ? Value : default (T);

        public static implicit operator Maybe<T>(T input) => new Maybe<T>(input);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Maybe<T> && Equals((Maybe<T>)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasValue && other.HasValue)
            {
                return Value.Equals(other.Value);
            }
            return !HasValue && !other.HasValue;
        }

        public bool Equals(T other)
        {
            return HasValue && _value.Equals(other);
        }

        public static bool operator ==(Maybe<T> lhs, Maybe<T> rhs) => lhs.Equals(rhs);
        public static bool operator !=(Maybe<T> lhs, Maybe<T> rhs) => !lhs.Equals(rhs);

        public static bool operator ==(Maybe<T> lhs, T rhs) => lhs.Equals(rhs);
        public static bool operator !=(Maybe<T> lhs, T rhs) => !lhs.Equals(rhs);

        public static bool operator ==(T lhs, Maybe<T> rhs) => rhs.Equals(lhs);
        public static bool operator !=(T lhs, Maybe<T> rhs) => !rhs.Equals(lhs);

    }

    public static class MaybeExtensions
    {
        /// <summary>
        /// Wraps the given value in a Maybe for that type
        /// </summary>
        public static Maybe<T> ToMaybe<T>(this T input) => input;

        /// <summary>
        /// Invokes one of the given handlers depending on if the Maybe contains a value
        /// </summary>
        /// <typeparam name="T">The type inside the Maybe</typeparam>
        /// <typeparam name="Y">The return type of the handlers</typeparam>
        /// <param name="input">Any maybe value</param>
        /// <param name="Some">Function to be invoked if the Maybe contains a value</param>
        /// <param name="None">Function to be invoked if the Maybe does NOT contain a value</param>
        public static Y Match<T, Y>(
            this Maybe<T> input,
            Func<T, Y> Some,
            Func<Y> None) =>
            input.HasValue
                ? Some(input.Value)
                : None();

        /// <summary>
        /// Invokes one of the given action handlers depending on if the Maybe contains a value
        /// </summary>
        /// <typeparam name="T">The type inside the Maybe</typeparam>
        /// <param name="input">Any maybe value</param>
        /// <param name="Some">Action to be invoked if the Maybe contains a value</param>
        /// <param name="None">Action to be invoked if the Maybe does NOT contain a value</param>
        public static Unit Match<T>(
            this Maybe<T> input,
            Action<T> Some,
            Action None) =>
            input.HasValue
                ? Func(Some)(input.Value)
                : Func(None)();

        /// <summary>
        /// Awaits the input and invokes one of the given handlers depending on if the Maybe contains a value
        /// </summary>
        /// <typeparam name="T">The type inside the Maybe</typeparam>
        /// <typeparam name="Y">The return type of the handlers</typeparam>
        /// <param name="this">Any maybe value</param>
        /// <param name="Some">Function to be invoked if the Maybe contains a value</param>
        /// <param name="None">Function to be invoked if the Maybe does NOT contain a value</param>
        public static async Task<Y> Match<T, Y>(
            this Task<Maybe<T>> @this,
            Func<T, Y> Some,
            Func<Y> None)
        {
            var maybeValue = await @this;
            return maybeValue.HasValue
                ? Some(maybeValue.Value)
                : None();
        }

        /// <summary>
        /// Awaits the input and invokes one of the given action handlers depending on if the Maybe contains a value
        /// </summary>
        /// <typeparam name="T">The type inside the Maybe</typeparam>
        /// <param name="this">Any maybe value</param>
        /// <param name="Some">Action to be invoked if the Maybe contains a value</param>
        /// <param name="None">Action to be invoked if the Maybe does NOT contain a value</param>
        public static async Task<Unit> Match<T>(
            this Task<Maybe<T>> @this,
            Action<T> Some,
            Action None)
        {
            var maybeValue = await @this;
            return maybeValue.HasValue
                ? Func(Some)(maybeValue.Value)
                : Func(None)();
        }

        /// <summary>
        /// Invokes one of the given asynchronous handlers depending on if the Maybe contains a value
        /// </summary>
        /// <remarks>MatchAsync is used where the handlers are async - Match can be used when the input is async but the handlers are not</remarks>
        /// <typeparam name="T">The type inside the Maybe</typeparam>
        /// <typeparam name="Y">The return type of the handlers</typeparam>
        /// <param name="input">Any maybe value</param>
        /// <param name="Some">Function to be invoked if the Maybe contains a value</param>
        /// <param name="None">Function to be invoked if the Maybe does NOT contain a value</param>
        public static async Task<Y> MatchAsync<T, Y>(
            this Maybe<T> input,
            Func<T, Task<Y>> Some,
            Func<Task<Y>> None) =>
            input.HasValue
                ? await Some(input.Value)
                : await None();

        /// <summary>
        /// Invokes one of the given asynchronous handlers actions depending on if the Maybe contains a value
        /// </summary>
        /// <remarks>MatchAsync is used where the handlers are async - Match can be used when the input is async but the handlers are not</remarks>
        /// <typeparam name="T">The type inside the Maybe</typeparam>
        /// <param name="this">Any maybe value</param>
        /// <param name="Some">Action to be invoked if the Maybe contains a value</param>
        /// <param name="None">Action to be invoked if the Maybe does NOT contain a value</param>
        public static async Task<Unit> MatchAsync<T>(
            this Maybe<T> @this,
            Func<T, Task> Some,
            Func<Task> None)
        {
            if (@this.HasValue)
            {
                await Some(@this.Value);
            }
            else
            {
                await None();
            }
            return unit;
        }

        /// <summary>
        /// Awaits the input and invokes  one of the given asynchronous handlers depending on if the Maybe contains a value
        /// </summary>
        /// <remarks>MatchAsync is used where the handlers are async - Match can be used when the input is async but the handlers are not</remarks>
        /// <typeparam name="T">The type inside the Maybe</typeparam>
        /// <typeparam name="Y">The return type of the handlers</typeparam>
        /// <param name="input">Any maybe value</param>
        /// <param name="Some">Function to be invoked if the Maybe contains a value</param>
        /// <param name="None">Function to be invoked if the Maybe does NOT contain a value</param>
        public static async Task<Y> MatchAsync<T, Y>(
            this Maybe<T> input,
            Func<T, Task<Y>> Some,
            Func<Y> None) =>
            input.HasValue
                ? await Some(input.Value)
                : None();

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<Unit> MatchAsync<T>(
            this Maybe<T> @this,
            Func<T, Task> Some,
            Action None)
        {
            if (@this.HasValue)
            {
                await Some(@this.Value);
            }
            else
            {
                None();
            }
            return unit;
        }

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<TR> MatchAsync<T, TR>(
            this Maybe<T> @this,
            Func<T, TR> Some,
            Func<Task<TR>> None) =>
            @this.HasValue
                ? Some(@this.Value)
                : await None();

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<Unit> MatchAsync<T>(
            this Maybe<T> @this,
            Action<T> Some,
            Func<Task> None)
        {
            if (@this.HasValue)
            {
                Some(@this.Value);
            }
            else
            {
                await None();
            }
            return unit;
        }

        /// <summary>
        /// Performs a Map operation - for a Maybe of type T and a Func that takes type T and returns type R, this allows the Func to work as though it takes Maybe{T} and returns Maybe{R}
        /// </summary>
        /// <typeparam name="T">The type of the Maybe</typeparam>
        /// <typeparam name="R">The return type of the function</typeparam>
        public static Maybe<R> Map<T, R>(this Maybe<T> @this, Func<T, R> mapper) =>
            @this.HasValue
                ? new Maybe<R>(mapper(@this.Value))
                : Maybe<R>.Empty();

        /// <summary>
        /// Performs a Map operation - for a Maybe of type T and a Func that takes type T and returns type R, this allows the Func to work as though it takes Maybe{T} and returns Maybe{R} where the function is asynchronous (and so returns a Task)
        /// </summary>
        /// <typeparam name="T">The type of the Maybe</typeparam>
        /// <typeparam name="R">The return type of the function</typeparam>
        public static async Task<Maybe<R>> MapAsync<T, R>(this Maybe<T> @this, Func<T, Task<R>> mapper) =>
            @this.HasValue
                ? new Maybe<R>(await mapper(@this.Value))
                : Maybe<R>.Empty();

        /// <summary>
        /// Performs a Bind operation - for a Maybe of type T and a Func that takes type T and returns a Maybe of type R, this invokes the func if the Maybe{T} has a value, and returns an empty Maybe{R} otherwise
        /// </summary>
        public static Maybe<R> Bind<T, R>(this Maybe<T> @this, Func<T, Maybe<R>> binder) =>
            @this.HasValue
                ? binder(@this.Value)
                : Maybe<R>.Empty();

        [Obsolete("This is not a real Bind")]
        public static Maybe<R> Bind<T, R>(this Maybe<T> @this, Func<T, Maybe<R>> Some, Func<Maybe<R>> None) =>
            @this.HasValue
                ? Some(@this.Value)
                : None();

        /// <summary>
        /// Performs a Bind operation - for a Maybe of type T and a Func that takes type T and returns a Maybe of type R, this invokes the func if the Maybe{T} has a value, and returns an empty Maybe{R} otherwise where the function is asynchronous
        /// </summary>
        public static async Task<Maybe<R>> BindAsync<T, R>(this Maybe<T> @this, Func<T, Task<Maybe<R>>> binder) =>
            @this.HasValue
                ? await binder(@this.Value)
                : Maybe<R>.Empty();

        [Obsolete("This is not a real Bind")]
        public static async Task<Maybe<R>> BindAsync<T, R>(this Maybe<T> @this, Func<T, Task<Maybe<R>>> Some, Func<Task<Maybe<R>>> None) =>
            @this.HasValue
                ? await Some(@this.Value)
                : await None();

        // Task (Promise) monad support 
        /// <summary>
        /// Converts a Task{T} into a Task{Maybe{T}}
        /// </summary>
        public static async Task<Maybe<T>> ToMaybe<T>(this Task<T> input) =>
            await input;

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<Y> MatchAsync<T, Y>(
            this Task<Maybe<T>> @this,
            Func<T, Task<Y>> Some,
            Func<Task<Y>> None)
        {
            var input = await @this;
            return input.HasValue
                ? await Some(input.Value)
                : await None();
        }

        public static async Task<Y> MatchAsync<T, Y>(
            this Task<Maybe<T>> @this,
            Func<T, Task<Y>> Some,
            Func<Y> None)
        {
            var input = await @this;
            return input.HasValue
                ? await Some(input.Value)
                : None();
        }

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<TR> MatchAsync<T, TR>(
            this Task<Maybe<T>> @this,
            Func<T, TR> Some,
            Func<Task<TR>> None)
        {
            var input = await @this;
            return input.HasValue
                ? Some(input.Value)
                : await None();
        }

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<Unit> MatchAsync<T>(
            this Task<Maybe<T>> @this,
            Func<T, Task> Some,
            Func<Task> None)
        {
            var value = await @this;
            if (value.HasValue)
            {
                await Some(value.Value);
            }
            else
            {
                await None();
            }
            return unit;
        }

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<Unit> MatchAsync<T>(
            this Task<Maybe<T>> @this,
            Func<T, Task> Some,
            Action None)
        {
            var value = await @this;
            if (value.HasValue)
            {
                await Some(value.Value);
            }
            else
            {
                None();
            }
            return unit;
        }

        /// <see cref="MatchAsync{T,Y}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{Y}},System.Func{System.Threading.Tasks.Task{Y}})"/>
        public static async Task<Unit> MatchAsync<T>(
            this Task<Maybe<T>> @this,
            Action<T> Some,
            Func<Task> None)
        {
            var value = await @this;
            if (value.HasValue)
            {
                Some(value.Value);
            }
            else
            {
                await None();
            }
            return unit;
        }

        ///<see cref="Map{T,R}(E247.Fun.Maybe{T},System.Func{T,R})"/>
        public static async Task<Maybe<R>> Map<T, R>(this Task<Maybe<T>> @this, Func<T, R> mapper)
        {
            var value = await @this;
            return value.HasValue
                ? new Maybe<R>(mapper(value.Value))
                : Maybe<R>.Empty();
        }

        ///<see cref="MapAsync{T,R}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{R}})"/>
        public static async Task<Maybe<R>> MapAsync<T, R>(this Task<Maybe<T>> @this, Func<T, Task<R>> mapper)
        {
            var value = await @this;
            return value.HasValue
                ? new Maybe<R>(await mapper(value.Value))
                : Maybe<R>.Empty();
        }

        ///<see cref="Bind{T,R}(E247.Fun.Maybe{T},System.Func{T,E247.Fun.Maybe{R}})"/>
        public static async Task<Maybe<R>> Bind<T, R>(this Task<Maybe<T>> @this, Func<T, Maybe<R>> binder)
        {
            var value = await @this;
            return value.HasValue
                ? binder(value.Value)
                : Maybe<R>.Empty();
        }

        ///<see cref="Bind{T,R}(E247.Fun.Maybe{T},System.Func{T,E247.Fun.Maybe{R}})"/>
        public static async Task<Maybe<R>> Bind<T, R>(this Task<Maybe<T>> @this, Func<T, Maybe<R>> Some, Func<Maybe<R>> None)
        {
            var value = await @this;
            return value.HasValue
                ? Some(value.Value)
                : None();
        }

        ///<see cref="BindAsync{T,R}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{E247.Fun.Maybe{R}}})"/>
        public static async Task<Maybe<R>> BindAsync<T, R>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<R>>> binder)
        {
            var value = await @this;
            return value.HasValue
                ? await binder(value.Value)
                : Maybe<R>.Empty();
        }

        ///<see cref="BindAsync{T,R}(E247.Fun.Maybe{T},System.Func{T,System.Threading.Tasks.Task{E247.Fun.Maybe{R}}})"/>
        public static async Task<Maybe<R>> BindAsync<T, R>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<R>>> Some,
            Func<Task<Maybe<R>>> None)
        {
            var value = await @this;
            return value.HasValue
                ? await Some(value.Value)
                : await None();
        }

        /// <summary>
        /// Execute an action only if Value of Maybe exists. Meant For side effect and not breaking the pipe.
        /// </summary>
        /// <typeparam name="TValue">The type of the Maybe</typeparam>
        public static Maybe<TValue> TeeMap<TValue>(this Maybe<TValue> @this, Action<TValue> act)
        {
            if (@this.HasValue)
                act(@this.Value);
            return @this;
        }

        ///<see cref="TeeMap{TValue}(Maybe{TValue}, Action{TValue})"/>
        public static Maybe<TValue> TeeMap<TValue>(this Maybe<TValue> @this, Action act)
        {
            if (@this.HasValue)
                act();
            return @this;
        }

        public static async Task<Maybe<TValue>> TeeMap<TValue>(this Task<Maybe<TValue>> @this, Action<TValue> act)
        {
            var maybe = await @this;
            if (maybe.HasValue)
                act(maybe.Value);
            return maybe;
        }

        ///<see cref="TeeMap{TValue}(Maybe{TValue}, Action{TValue})"/>
        public static async Task<Maybe<TValue>> TeeMap<TValue>(this Task<Maybe<TValue>> @this, Action act)
        {
            var maybe = await @this;
            if (maybe.HasValue)
                act();
            return maybe;
        }

        ///<see cref="TeeMap{TValue}(Maybe{TValue}, Action{TValue})"/>
        public static async Task<Maybe<TValue>> TeeMapAsync<TValue>(this Maybe<TValue> @this, Func<TValue, Task> act)
        {
            if (@this.HasValue)
                await act(@this.Value);
            return @this;
        }

        ///<see cref="TeeMap{TValue}(Maybe{TValue}, Action{TValue})"/>
        public static async Task<Maybe<TValue>> TeeMapAsync<TValue>(this Maybe<TValue> @this, Func<Task> act)
        {
            if (@this.HasValue)
                await act();
            return @this;
        }

        ///<see cref="TeeMap{TValue}(Maybe{TValue}, Action{TValue})"/>
        public static async Task<Maybe<TValue>> TeeMapAsync<TValue>(this Task<Maybe<TValue>> @this, Func<TValue, Task> act)
        {
            var maybe = await @this;
            if (maybe.HasValue)
                await act(maybe.Value);
            return maybe;
        }

        ///<see cref="TeeMap{TValue}(Maybe{TValue}, Action{TValue})"/>
        public static async Task<Maybe<TValue>> TeeMapAsync<TValue>(this Task<Maybe<TValue>> @this, Func<Task> act)
        {
            var maybe = await @this;
            if (maybe.HasValue)
                await act();
            return maybe;
        }

        public static Maybe<C> SelectMany<A, B, C>(this Maybe<A> a, Func<A, Maybe<B>> func, Func<A, B, C> select)
        {
            return a.Bind(x => func(x).Bind(y => select(x, y).ToMaybe()));
        }

        public static Maybe<B> Apply<A, B>(this Maybe<Func<A, B>> func, Maybe<A> input)
        {
            if (!func.HasValue || !input.HasValue)
                return Maybe<B>.Empty();

            return func.Value(input.Value).ToMaybe();
        }

        public static Maybe<B> Lift<A, B>(this Func<A, B> func, Maybe<A> input)
        {
            if (!input.HasValue)
                return Maybe<B>.Empty();

            return (func(input.Value)).ToMaybe();
        }

        public async static Task<Maybe<B>> Apply<A, B>(
            this Task<Maybe<Func<A, B>>> func, 
            Maybe<A> input)
        {
            var f = await func;

            return f.Apply(input);
        }

        public async static Task<Maybe<B>> Lift<A, B>(
            this Task<Func<A, B>> func, 
            Maybe<A> input)
        {
            if (!input.HasValue)
                return Maybe<B>.Empty();

            var f = await func;
            return (f(input.Value)).ToMaybe();
        }

        public async static Task<Maybe<B>> ApplyAsync<A, B>(
            this Maybe<Func<A, B>> func, 
            Task<Maybe<A>> input)
        {
            var i = await input;

            return func.Apply(i);
        }

        public async static Task<Maybe<B>> LiftAsync<A, B>(
            this Func<A, B> func, 
            Task<Maybe<A>> input)
        {
            var i = await input;

            return func.Lift(i);
        }

        public async static Task<Maybe<B>> ApplyAsync<A, B>(
            this Task<Maybe<Func<A, B>>> func,
            Task<Maybe<A>> input)
        {
            var f = await func;
            var i = await input;

            return f.Apply(i);
        }

        public async static Task<Maybe<B>> LiftAsync<A, B>(
            this Task<Func<A, B>> func,
            Task<Maybe<A>> input)
        {
            var f = await func;
            var i = await input;

            return f.Lift(i);
        }
    }
}
