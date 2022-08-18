using System;
using System.Collections.Generic;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{
    public static class D2ArrayHelper
    {
        private static IList<int> SpiralInMatrix(int[][] array) //锯齿数组
        {
            // 数组为空，返回null
            if (array.Length == 0 || array[0].Length == 0)
            {
                return null;
            }

            // 获取循环遍历的次数，取行、列中的最小值的1/2
            int cnt = (Math.Min(array.Length, array[0].Length) + 1) / 2;
            List<int> results = new List<int>();
            for (int pos = 0; pos < cnt; pos++)
            {
                // 需要遍历的总列
                int col = array[pos].Length - pos;
                // 需要遍历的总行
                int row = array.Length - pos;
                // 纵坐标，此时数据所在的列
                int x = pos;
                // 横坐标，此时数据所在的行
                int y = pos;
                // 从左往右遍历，此时纵坐标不变，横坐标依次递增，退出条件：y = col
                for (; y < col; y++)
                {
                    results.Add(array[x][y]);
                }

                // 从上往下遍历，此时横坐标不变，纵坐标依次递增。此时 y = col，数组的索引的起始值是0，故在使用y作为纵坐标的时候，y = y-1
                for (x += 1; x < row; x++)
                {
                    results.Add(array[x][y - 1]);
                }

                /**
                 * 从左往右遍历，此时纵坐标不变，横坐标依次递减
                 * 上面退出循环的条件时，x == row,而数组的索引的起始值是0，故在使用x作为纵坐标的时候，x = x-1
                 * 此时 y = col,纵坐标的索引的最大值为 col - 1，而在从上往下遍历元素的时候已经遍历了最右下角的元素，避免重复，故初始化 y = col - 1 -1;
                 */
                for (y = y - 2; y > pos; y--)
                {
                    results.Add(array[x - 1][y]);
                }

                /**
                 * 从下往上遍历，此时横坐标不变，纵坐标依次递减
                 * 上面退出循环的条件时，x == row,而数组的索引的起始值是0，故在使用x作为纵坐标的时候，初始化 x = x-1
                 * 在进行从左往右进行遍历的时候，已经对最左上角的元素进行了遍历，为了避免重复，所以纵坐标的结束条件为：遍历的次数+1
                 */
                for (x = x - 1; x >= pos + 1; x--)
                {
                    results.Add(array[x][y]);
                }
            }

            return results;
        }

        private static IList<int> SpiralInMatrix(int[,] array)
        {
            int arrayRow = array.Rank; //获取维数，这里指行数
            int arrayCol = array.GetLength(1); //获取指定维度中的元素个数，这里也就是列数了。（0是第一维，1表示的是第二维）
            //int arrayColT = array.GetUpperBound(0) + 1;//获取指定维度的索引上限，在加上一个1就是总数，这里表示二维数组的行数
            //int numb = array.Length;//获取整个二维数组的长度，即所有元的个数
            // 数组为空，返回null
            if (array.Length == 0 || arrayCol == 0 || arrayRow == 0)
            {
                return null;
            }

            // 获取循环遍历的次数，取行、列中的最小值的1/2
            int cnt = (Math.Min(arrayRow, arrayCol) + 1) / 2;
            List<int> results = new List<int>();
            for (int pos = 0; pos < cnt; pos++)
            {
                // 需要遍历的总列
                int col = arrayCol - pos;
                // 需要遍历的总行
                int row = array.Length - pos;
                // 纵坐标，此时数据所在的列
                int x = pos;
                // 横坐标，此时数据所在的行
                int y = pos;
                // 从左往右遍历，此时纵坐标不变，横坐标依次递增，退出条件：y = col
                for (; y < col; y++)
                {
                    results.Add(array[x, y]);
                }

                // 从上往下遍历，此时横坐标不变，纵坐标依次递增。此时 y = col，数组的索引的起始值是0，故在使用y作为纵坐标的时候，y = y-1
                for (x += 1; x < row; x++)
                {
                    results.Add(array[x, y - 1]);
                }

                /**
                 * 从左往右遍历，此时纵坐标不变，横坐标依次递减
                 * 上面退出循环的条件时，x == row,而数组的索引的起始值是0，故在使用x作为纵坐标的时候，x = x-1
                 * 此时 y = col,纵坐标的索引的最大值为 col - 1，而在从上往下遍历元素的时候已经遍历了最右下角的元素，避免重复，故初始化 y = col - 1 -1;
                 */
                for (y = y - 2; y > pos; y--)
                {
                    results.Add(array[x - 1, y]);
                }

                /**
                 * 从下往上遍历，此时横坐标不变，纵坐标依次递减
                 * 上面退出循环的条件时，x == row,而数组的索引的起始值是0，故在使用x作为纵坐标的时候，初始化 x = x-1
                 * 在进行从左往右进行遍历的时候，已经对最左上角的元素进行了遍历，为了避免重复，所以纵坐标的结束条件为：遍历的次数+1
                 */
                for (x = x - 1; x >= pos + 1; x--)
                {
                    results.Add(array[x, y]);
                }
            }

            return results;
        }

        public static bool SearchSortedMatrix(int[][] matrix, int target)
        {
            int m = matrix.Length;
            if (m == 0)
            {
                return false;
            }

            int n = matrix[0].Length;
            int left = 0, right = m * n - 1;
            int pivotIdx, pivotElement;
            while (left <= right)
            {

                pivotIdx = (left + right) / 2;
                pivotElement = matrix[pivotIdx / n][pivotIdx % n];
                if (target == pivotElement)
                {
                    return true;
                }
                else
                {
                    if (target < pivotElement)
                    {
                        right = pivotIdx - 1;
                    }
                    else
                    {
                        left = pivotIdx + 1;
                    }
                }
            }

            return false;
        }
    }
}