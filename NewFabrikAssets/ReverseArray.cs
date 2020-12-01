using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NewFabrikAssets
{
    public static class Reverse
    {
        public static void ReverseArray<T>(T[] arr)
        {
            T temp;
            int x = 0;
            int y = arr.Length - 1;
            while (x < y)
            {
                temp = arr[y];
                arr[y] = arr[x];
                arr[x] = temp;
                x++;
                y--;
            }
        }
    }
}
