using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1591

namespace E247.Fun
{
    public static class Enumerable
    {
        public static IEnumerable<T> WhereSome<T>(this IEnumerable<Maybe<T>> source)
          => source.Where(x => x.HasValue).Select(x => x.Value);

        public static IEnumerable<T> WhereSuccessful<T, TFailure>(
            this IEnumerable<Result<T, TFailure>> source)
            => source.Where(x => x.IsSuccessful).Select(x => x.Success);

        public static IEnumerable<TFailure> WhereFailed<T, TFailure>(
            this IEnumerable<Result<T, TFailure>> source)
            => source.Where(x => !x.IsSuccessful).Select(x => x.Failure);

        public static IEnumerable<T> Collect<T>(params T[] items) => items;
        public static IEnumerable<T> CollectSome<T>(params Maybe<T>[] items) => items.WhereSome();
        public static IEnumerable<T> CollectSuccess<T, TFailure>(params Result<T, TFailure>[] items) => items.WhereSuccessful();
        public static IEnumerable<T> CollectFailure<TSuccess, T>(params Result<TSuccess, T>[] items) => items.WhereFailed();

    }
}
