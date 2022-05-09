namespace FinTech
{
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        public static object[,] ToExcelArray<T>(this IEnumerable<T> source, bool vertical)
        {
            var sourceArray = source.ToArray();
            var rows = 1;
            var cols = 1;
            
            if (vertical)
            {
                rows = sourceArray.Length;
            }
            else
            {
                cols = sourceArray.Length;
            }

            var result = new object[rows, cols];

            var pos = 0;
            
            foreach (var item in sourceArray)
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