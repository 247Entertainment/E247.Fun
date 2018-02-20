using System.Threading.Tasks;

namespace E247.Fun
{
    public static class TaskExtensions
    {
        public static Task<T> LiftAsync<T>(this T thing) => Task.FromResult(thing);
    }
}