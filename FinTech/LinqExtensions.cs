namespace FinTech
{
    using System.Collections.Generic;

    public static class LinqExtensions
    {
        public static T[,] To2DArray<T>(this IEnumerable<T> source, int rows, int cols)
        {
            var result = new T[rows, cols];

            var pos = 0;
            
            foreach (var item in source)
            {
                var r = pos / cols;
                var c = pos % cols;

                if (r >= rows) break;

                result[r, c] = item;
                pos += 1;
            }

            return result;
        }
    }
}