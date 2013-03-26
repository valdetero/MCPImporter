using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCPImporter.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Async create of a System.Collections.Generic.List&lt;T&gt; from an
        /// System.Collections.Generic.IQueryable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="list">The System.Collections.Generic.IEnumerable&lt;T&gt;
        /// to create a System.Collections.Generic.IList&lt;T&gt; from.</param>
        /// <returns> A System.Collections.Generic.IList&lt;T&gt; that contains elements
        /// from the input sequence.</returns>
        public static Task<IList<T>> ToListAsync<T>(this IQueryable<T> list)
        {
            return Task.Run(() => (IList<T>)list.ToList());
        }

        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> list, Action<T> body, Action<T, Exception> error)
        {
            await Task.Run(() =>
                Parallel.ForEach(list, item =>
                {
                    try
                    {
                        if (body != null)
                            body(item);
                    }
                    catch (Exception ex)
                    {
                        if (error != null)
                            error(item, ex);
                    }
                }));
        }
    }
}
