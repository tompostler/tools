namespace Unlimitedinf.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A place to put extensions that I have not found a better home for.
    /// </summary>
    public static class Extensions
    {
        public static double Median<T>(this IEnumerable<T> source)
        {
            int count = source.Count();
            if (count == 0)
                throw new ArgumentException("No elements to median-ize", nameof(source));

            source = source.OrderBy(n => n);

            int midpoint = count / 2;
            if (count % 2 == 0)
                return (Convert.ToDouble(source.ElementAt(midpoint - 1)) + Convert.ToDouble(source.ElementAt(midpoint))) / 2.0;
            else
                return Convert.ToDouble(source.ElementAt(midpoint));
        }
    }
}
