using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;


public static class LinqUtil
{
    //this throws on lists with 0 elements
    //except for when TRes support division by zero (like float does)
    public static T AverageBy<T>(this IEnumerable<T> list, T zero, Func<T, T, T> add, Func<T, int, T> div)
    {
        int count = 0;
        var current = zero;
        foreach (var elem in list)
        {
            count++;
            current = add(current, elem);
        }

        return div(current, count);
    }
}

