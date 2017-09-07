using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using static E247.Fun.Unit;

#pragma warning disable 1591

// ReSharper disable InconsistentNaming

namespace E247.Fun
{
    public static class Fun
    {
        // enables type inference on lambdas
        public static Func<TR> Func<TR>(Func<TR> f) => f;
        public static Func<T1, TR> Func<T1, TR>(Func<T1, TR> f) => f;
        public static Func<T1, T2, TR> Func<T1, T2, TR>(Func<T1, T2, TR> f) => f;
        public static Func<T1, T2, T3, TR> Func<T1, T2, T3, TR>(Func<T1, T2, T3, TR> f) => f;
        public static Func<T1, T2, T3, T4, TR> Func<T1, T2, T3, T4, TR>(Func<T1, T2, T3, T4, TR> f) => f;
        public static Func<T1, T2, T3, T4, T5, TR> Func<T1, T2, T3, T4, T5, TR>(Func<T1, T2, T3, T4, T5, TR> f) => f;
        public static Func<T1, T2, T3, T4, T5, T6, TR> Func<T1, T2, T3, T4, T5, T6, TR>(Func<T1, T2, T3, T4, T5, T6, TR> f) => f;
        public static Func<T1, T2, T3, T4, T5, T6, T7, TR> Func<T1, T2, T3, T4, T5, T6, T7, TR>(Func<T1, T2, T3, T4, T5, T6, T7, TR> f) => f;

        public static Func<Unit> Func(Action f) =>
            () => { f(); return unit; };
        public static Func<T1, Unit> Func<T1>(Action<T1> f) =>
            a1 => { f(a1); return unit; };
        public static Func<T1, T2, Unit> Func<T1, T2>(Action<T1, T2> f) =>
            (a1, a2) => { f(a1, a2); return unit; };
        public static Func<T1, T2, T3, Unit> Func<T1, T2, T3>(Action<T1, T2, T3> f) =>
            (a1, a2, a3) => { f(a1, a2, a3); return unit; };
        public static Func<T1, T2, T3, T4, Unit> Func<T1, T2, T3, T4>(Action<T1, T2, T3, T4> f) =>
            (a1, a2, a3, a4) => { f(a1, a2, a3, a4); return unit; };
        public static Func<T1, T2, T3, T4, T5, Unit> Func<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> f) =>
            (a1, a2, a3, a4, a5) => { f(a1, a2, a3, a4, a5); return unit; };
        public static Func<T1, T2, T3, T4, T5, T6, Unit> Func<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> f) =>
            (a1, a2, a3, a4, a5, a6) => { f(a1, a2, a3, a4, a5, a6); return unit; };
        public static Func<T1, T2, T3, T4, T5, T6, T7, Unit> Func<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> f) =>
            (a1, a2, a3, a4, a5, a6, a7) => { f(a1, a2, a3, a4, a5, a6, a7); return unit; };


        public static Action Act(Action f) => f;
        public static Action<T1> Act<T1>(Action<T1> f) => f;
        public static Action<T1, T2> Act<T1, T2>(Action<T1, T2> f) => f;
        public static Action<T1, T2, T3> Act<T1, T2, T3>(Action<T1, T2, T3> f) => f;
        public static Action<T1, T2, T3, T4> Act<T1, T2, T3, T4>(Action<T1, T2, T3, T4> f) => f;
        public static Action<T1, T2, T3, T4, T5> Act<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> f) => f;
        public static Action<T1, T2, T3, T4, T5, T6> Act<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> f) => f;
        public static Action<T1, T2, T3, T4, T5, T6, T7> Act<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> f) => f;

        public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> f) =>
            a => b => f(a, b);
        public static Func<T1, Func<T2, Func<T3, TR>>> Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> f) =>
            a => b => c => f(a, b, c);
        public static Func<T1, Func<T2, Func<T3, Func<T4, TR>>>> Curry<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> f) =>
            a => b => c => d => f(a, b, c, d);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TR>>>>> Curry<T1, T2, T3, T4, T5, TR>(this Func<T1, T2, T3, T4, T5, TR> f) =>
            a => b => c => d => e => f(a, b, c, d, e);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TR>>>>>> Curry<T1, T2, T3, T4, T5, T6, TR>(this Func<T1, T2, T3, T4, T5, T6, TR> func) =>
            a => b => c => d => e => f => func(a, b, c, d, e, f);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TR>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, TR>(this Func<T1, T2, T3, T4, T5, T6, T7, TR> func) =>
            a => b => c => d => e => f => g => func(a, b, c, d, e, f, g);

        public static Func<T1, T2, TR> Uncurry<T1, T2, TR>(this Func<T1, Func<T2, TR>> f) =>
            (a, b) => f(a)(b);
        public static Func<T1, T2, T3, TR> Uncurry<T1, T2, T3, TR>(this Func<T1, Func<T2, Func<T3, TR>>> f) =>
            (a, b, c) => f(a)(b)(c);
        public static Func<T1, T2, T3, T4, TR> Uncurry<T1, T2, T3, T4, TR>(this Func<T1, Func<T2, Func<T3, Func<T4, TR>>>> f) =>
            (a, b, c, d) => f(a)(b)(c)(d);
        public static Func<T1, T2, T3, T4, T5, TR> Uncurry<T1, T2, T3, T4, T5, TR>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TR>>>>> f) =>
            (a, b, c, d, e) => f(a)(b)(c)(d)(e);
        public static Func<T1, T2, T3, T4, T5, T6, TR> Uncurry<T1, T2, T3, T4, T5, T6, TR>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TR>>>>>> func) =>
            (a, b, c, d, e, f) => func(a)(b)(c)(d)(e)(f);
        public static Func<T1, T2, T3, T4, T5, T6, T7, TR> Uncurry<T1, T2, T3, T4, T5, T6, T7, TR>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TR>>>>>>> func) =>
            (a, b, c, d, e, f, g) => func(a)(b)(c)(d)(e)(f)(g);

        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T2, T3> b, Func<T1, T2> a) =>
            v => b(a(v));
        public static Func<T1, T3> ComposeBack<T1, T2, T3>(this Func<T1, T2> a, Func<T2, T3> b) =>
            v => b(a(v));

        public static Func<T2, Func<T1, TR>> Flip<T1, T2, TR>(this Func<T1, Func<T2, TR>> f) =>
            a => b => f(b)(a);

        // partial application
        public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, T2, TR> func, T1 a) =>
            b => func(a, b);
        public static Func<T3, TR> Apply<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func, T1 a, T2 b) =>
            c => func(a, b, c);
        public static Func<T2, T3, TR> Apply<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func, T1 a) =>
            (b, c) => func(a, b, c);
        public static Func<T4, TR> Apply<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func, T1 a, T2 b, T3 c) =>
            d => func(a, b, c, d);
        public static Func<T3, T4, TR> Apply<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func, T1 a, T2 b) =>
            (c, d) => func(a, b, c, d);
        public static Func<T2, T3, T4, TR> Apply<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func, T1 a) =>
            (b, c, d) => func(a, b, c, d);
        public static Func<T5, TR> Apply<T1, T2, T3, T4, T5, TR>(this Func<T1, T2, T3, T4, T5, TR> func, T1 a, T2 b, T3 c, T4 d) =>
            e => func(a, b, c, d, e);
        public static Func<T4, T5, TR> Apply<T1, T2, T3, T4, T5, TR>(this Func<T1, T2, T3, T4, T5, TR> func, T1 a, T2 b, T3 c) =>
            (d, e) => func(a, b, c, d, e);
        public static Func<T3, T4, T5, R> Apply<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> func, T1 a, T2 b) =>
            (c, d, e) => func(a, b, c, d, e);
        public static Func<T2, T3, T4, T5, R> Apply<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> func, T1 a) =>
            (b, c, d, e) => func(a, b, c, d, e);


        public static Func<T1, TR> ApplyLeft<T1, T2, TR>(this Func<T1, T2, TR> func, T2 b) =>
            a => func(a, b);

        public static Action<T2> Apply<T1, T2>(this Action<T1, T2> func, T1 a) =>
            b => func(a, b);
        public static Action<T3> Apply<T1, T2, T3>(this Action<T1, T2, T3> func, T1 a, T2 b) =>
            c => func(a, b, c);
        public static Action<T2, T3> Apply<T1, T2, T3>(this Action<T1, T2, T3> func, T1 a) =>
            (b, c) => func(a, b, c);
        public static Action<T4> Apply<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> func, T1 a, T2 b, T3 c) =>
            d => func(a, b, c, d);
        public static Action<T3, T4> Apply<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> func, T1 a, T2 b) =>
            (c, d) => func(a, b, c, d);
        public static Action<T2, T3, T4> Apply<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> func, T1 a) =>
            (b, c, d) => func(a, b, c, d);
        public static Action<T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> func, T1 a, T2 b, T3 c, T4 d) =>
            e => func(a, b, c, d, e);
        public static Action<T4, T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> func, T1 a, T2 b, T3 c) =>
            (d, e) => func(a, b, c, d, e);
        public static Action<T3, T4, T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> func, T1 a, T2 b) =>
            (c, d, e) => func(a, b, c, d, e);
        public static Action<T2, T3, T4, T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> func, T1 a) =>
            (b, c, d, e) => func(a, b, c, d, e);

        // Pipe support
        public static TResult Map<TValue, TResult>(this TValue @this, Func<TValue, TResult> func) =>
            func(@this);
        public static async Task<TResult> Map<TValue, TResult>(this Task<TValue> @this, Func<TValue, TResult> func) =>
            func(await @this);

        public static async Task<TResult> MapAsync<TValue, TResult>(this TValue @this, Func<TValue, Task<TResult>> func) =>
            await func(@this);
        public static async Task<TResult> MapAsync<TValue, TResult>(this Task<TValue> @this, Func<TValue, Task<TResult>> func) =>
            await func(await @this);

        // Chainable if
        public static TResult If<TValue, TResult>(this TValue @this, Func<TValue, bool> If, Func<TValue, TResult> Then, Func<TValue, TResult> Else) =>
            If(@this)
                ? @this.Map(Then)
                : @this.Map(Else);
        public static async Task<TResult> If<TValue, TResult>(this Task<TValue> @this, Func<TValue, bool> If, Func<TValue, TResult> Then, Func<TValue, TResult> Else)
        {
            var value = await @this;
            return If(value)
                ? value.Map(Then)
                : value.Map(Else);
        }

        public static async Task<TResult> IfAsync<TValue, TResult>(this TValue @this, Func<TValue, Task<bool>> If, Func<TValue, Task<TResult>> Then, Func<TValue, Task<TResult>> Else) =>
            (await If(@this))
                ? await @this.Map(Then)
                : await @this.Map(Else);
        public static async Task<TResult> IfAsync<TValue, TResult>(this TValue @this, Func<TValue, bool> If, Func<TValue, Task<TResult>> Then, Func<TValue, Task<TResult>> Else) =>
            If(@this)
                ? await @this.Map(Then)
                : await @this.Map(Else);
        public static async Task<TResult> IfAsync<TValue, TResult>(this TValue @this, Func<TValue, Task<bool>> If, Func<TValue, TResult> Then, Func<TValue, TResult> Else) =>
            (await If(@this))
                ? @this.Map(Then)
                : @this.Map(Else);
        public static async Task<TResult> IfAsync<TValue, TResult>(this Task<TValue> @this, Func<TValue, Task<bool>> If, Func<TValue, Task<TResult>> Then, Func<TValue, Task<TResult>> Else)
        {
            var value = await @this;
            return (await If(value))
                ? await value.Map(Then)
                : await value.Map(Else);
        }
        public static async Task<TResult> IfAsync<TValue, TResult>(this Task<TValue> @this, Func<TValue, bool> If, Func<TValue, Task<TResult>> Then, Func<TValue, Task<TResult>> Else)
        {
            var value = await @this;
            return If(value)
                ? await value.Map(Then)
                : await value.Map(Else);
        }
        public static async Task<TResult> IfAsync<TValue, TResult>(this Task<TValue> @this, Func<TValue, Task<bool>> If, Func<TValue, TResult> Then, Func<TValue, TResult> Else)
        {
            var value = await @this;
            return (await If(value))
                ? value.Map(Then)
                : value.Map(Else);
        }

        // Execute an action for side effect without breaking the pipe
        public static TValue Tee<TValue>(this TValue @this, Action<TValue> act)
        {
            act(@this);
            return @this;
        }
        public static TValue Tee<TValue>(this TValue @this, Action act)
        {
            act();
            return @this;
        }
        public static async Task<TValue> Tee<TValue>(this Task<TValue> @this, Action<TValue> act)
        {
            var value = await @this;
            act(value);
            return value;
        }
        public static async Task<TValue> Tee<TValue>(this Task<TValue> @this, Action act)
        {
            act();
            return await @this;
        }

        public static async Task<TValue> TeeAsync<TValue>(this TValue @this, Func<TValue, Task> act)
        {
            await act(@this);
            return @this;
        }
        public static async Task<TValue> TeeAsync<TValue>(this TValue @this, Func<Task> act)
        {
            await act();
            return @this;
        }
        public static async Task<TValue> TeeAsync<TValue>(this Task<TValue> @this, Func<TValue, Task> act)
        {
            var value = await @this;
            await act(value);
            return value;
        }
        public static async Task<TValue> TeeAsync<TValue>(this Task<TValue> @this, Func<Task> act)
        {
            var val = await @this;
            await act();
            return val;
        }

        // For the bad OO cases when a function is executed for side effect, but also returns a value that we want to ignore
        public static TValue TeeIgnore<TValue, TResult>(this TValue @this, Func<TValue, TResult> func)
        {
            func(@this);
            return @this;
        }
        public static async Task<TValue> TeeIgnore<TValue, TResult>(this Task<TValue> @this, Func<TValue, TResult> func)
        {
            var value = await @this;
            func(value);
            return value;
        }

        // Special functions
        public static T Identity<T>(T x) =>
            x;

        // The return type of this function is "used". This allows exceptions to be thrown in ternary operators, LINQ expressions etc.
        public static TR Raise<TR>(Exception ex)
        {
            throw ex;
        }

        public static System.Collections.Generic.List<T> EmptyList<T>() =>
            new System.Collections.Generic.List<T>();

        // Returns a Func<T> that wraps func.  The first call to the resulting Func<T> will cache the result.
        // Subsequent calls return the cached item.
        public static Func<T> Memoize<T>(this Func<T> func)
        {
            var value = new Lazy<T>(func, true);
            return () => value.Value;
        }

        // Adapted from https://github.com/louthy/language-ext/blob/master/LanguageExt.Core/Prelude_Memoize.cs
        // Returns a Func<T,R> that wraps func.  Each time the resulting
        // Func<T,R> is called with a new value, its result is memoized (cached).
        // Subsequent calls use the memoized value.  
        //     Thread-safe and memory-leak safe.  
        public static Func<T, R> Memoize<T, R>(this Func<T, R> func)
        {
            var cache = new WeakDict<T, R>();
            var syncMap = new ConcurrentDictionary<T, object>();

            return inp => cache.TryGetValue(inp)
                .MatchUnsafe(
                    Some: x => x,
                    None: () =>
                    {
                        R res;
                        var sync = syncMap.GetOrAdd(inp, new object());
                        lock (sync)
                        {
                            res = cache.GetOrAdd(inp, func);
                        }
                        syncMap.TryRemove(inp, out sync);
                        return res;
                    });
        }

        private struct OptionUnsafe<T>
        {
            private readonly T value;

            private OptionUnsafe(T value, bool isSome)
            {
                this.IsSome = isSome;
                this.value = value;
            }

            private OptionUnsafe(T value)
            {
                this.IsSome = true;
                this.value = value;
            }

            public static OptionUnsafe<T> Some(T value) =>
                new OptionUnsafe<T>(value, true);

            public static readonly OptionUnsafe<T> None = new OptionUnsafe<T>();

            public bool IsSome { get; }

            private T Value =>
                IsSome
                    ? value
                    : Raise<T>(new Exception("Option isn't set."));

            // ReSharper disable once ParameterHidesMember
            public R MatchUnsafe<R>(Func<T, R> Some, Func<R> None) =>
                IsSome
                    ? Some(Value)
                    : None();

            public static OptionUnsafe<T1> SomeUnsafe<T1>(T1 value) =>
                OptionUnsafe<T1>.Some(value);
        }

        // Used internally by the memo function.  It wraps a concurrent dictionary that has 
        // its value objects wrapped in a WeakReference<OnFinalise<...>>
        // The OnFinalise type is a private class within WeakDict and does nothing but hold
        // the value and an Action to call when its finalised.  So when the WeakReference is
        // collected by the GC, it forces the finaliser to be called on the OnFinalise object,
        // which in turn executes the action which renmoves it from the ConcurrentDictionary.  
        // That means that both the key and value are collected when the GC fires rather than 
        // just the value.  That should mitigate a memory leak of keys.
        private class WeakDict<T, R>
        {
            private class OnFinalise<V>
            {
                public readonly V Value;
                private readonly Action onFinalise;

                public OnFinalise(Action onFinalise, V value)
                {
                    this.Value = value;
                    this.onFinalise = onFinalise;
                }

                ~OnFinalise()
                {
                    onFinalise();
                }
            }

            private readonly ConcurrentDictionary<T, WeakReference<OnFinalise<R>>> dict = new ConcurrentDictionary<T, WeakReference<OnFinalise<R>>>();

            private WeakReference<OnFinalise<R>> NewRef(T key, Func<T, R> addFunc) =>
                new WeakReference<OnFinalise<R>>(
                    new OnFinalise<R>(() =>
                    {
                        WeakReference<OnFinalise<R>> ignore = null;
                        dict.TryRemove(key, out ignore);
                    },
                        addFunc(key)));

            public OptionUnsafe<R> TryGetValue(T key)
            {
                WeakReference<OnFinalise<R>> res = null;
                OnFinalise<R> target = null;
                return dict.TryGetValue(key, out res)
                    ? res.TryGetTarget(out target)
                        ? OptionUnsafe<R>.SomeUnsafe(target.Value)
                        : OptionUnsafe<R>.None
                    : OptionUnsafe<R>.None;
            }

            public R GetOrAdd(T key, Func<T, R> addFunc)
            {
                var res = dict.GetOrAdd(key, _ => NewRef(key, addFunc));

                OnFinalise<R> target = null;
                if (res.TryGetTarget(out target))
                {
                    return target.Value;
                }
                else
                {
                    var upd = NewRef(key, addFunc);
                    res = dict.AddOrUpdate(key, upd, (_, __) => upd);
                    if (res.TryGetTarget(out target))
                    {
                        return target.Value;
                    }
                    else
                    {
                        // This is a best guess of why the target can't be got.
                        // It might not be the best approach, perhaps a retry, or a 
                        // better/more-descriptive exception.
                        throw new OutOfMemoryException();
                    }
                }
            }

            public bool TryRemove(T key)
            {
                WeakReference<OnFinalise<R>> ignore = null;
                return dict.TryRemove(key, out ignore);
            }
        }

    }
}
