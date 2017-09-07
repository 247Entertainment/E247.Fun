using System;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace E247.Fun
{
    public static class Tuples
    {
        // Constructors
        public static Tuple<T1, T2> Tuple<T1, T2>(T1 item1, T2 item2) =>
            System.Tuple.Create(item1, item2);

        public static Tuple<T1, T2, T3> Tuple<T1, T2, T3>(T1 item1, T2 item2, T3 item3) =>
            System.Tuple.Create(item1, item2, item3);

        public static Tuple<T1, T2, T3, T4> Tuple<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) =>
            System.Tuple.Create(item1, item2, item3, item4);

        public static Tuple<T1, T2, T3, T4, T5> Tuple<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) =>
            System.Tuple.Create(item1, item2, item3, item4, item5);

        public static Tuple<T1, T2, T3, T4, T5, T6> Tuple<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) =>
            System.Tuple.Create(item1, item2, item3, item4, item5, item6);

        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Tuple<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) =>
            System.Tuple.Create(item1, item2, item3, item4, item5, item6, item7);

        // Map
        // ReSharper disable InconsistentNaming
        public static R Map<T1, T2, R>(this Tuple<T1, T2> @this, Func<T1, T2, R> mapper) =>
            mapper(@this.Item1, @this.Item2);
        public static async Task<R> Map<T1, T2, R>(this Task<Tuple<T1, T2>> @this, Func<T1, T2, R> mapper)
        {
            var result = await @this;
            return mapper(result.Item1, result.Item2);
        }
        public static async Task<R> MapAsync<T1, T2, R>(this Tuple<T1, T2> @this, Func<T1, T2, Task<R>> mapper) =>
            await mapper(@this.Item1, @this.Item2);
        public static async Task<R> MapAsync<T1, T2, R>(this Task<Tuple<T1, T2>> @this, Func<T1, T2, Task<R>> mapper)
        {
            var result = await @this;
            return await mapper(result.Item1, result.Item2);
        }

        public static R Map<T1, T2, T3, R>(this Tuple<T1, T2, T3> @this, Func<T1, T2, T3, R> mapper) =>
            mapper(@this.Item1, @this.Item2, @this.Item3);
        public static async Task<R> Map<T1, T2, T3, R>(this Task<Tuple<T1, T2, T3>> @this, Func<T1, T2, T3, R> mapper)
        {
            var result = await @this;
            return mapper(result.Item1, result.Item2, result.Item3);
        }
        public static async Task<R> MapAsync<T1, T2, T3, R>(this Tuple<T1, T2, T3> @this, Func<T1, T2, T3, Task<R>> mapper) =>
            await mapper(@this.Item1, @this.Item2, @this.Item3);
        public static async Task<R> MapAsync<T1, T2, T3, R>(this Task<Tuple<T1, T2, T3>> @this, Func<T1, T2, T3, Task<R>> mapper)
        {
            var result = await @this;
            return await mapper(result.Item1, result.Item2, result.Item3);
        }

        // ReSharper restore InconsistentNaming
    }
}
