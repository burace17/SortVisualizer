/*
 * This file only contains the implementations of the sorting algorithms.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortVisualizer
{
    public partial class UserInterface
    {
        /// <summary>
        /// Swaps the values at the given locations in the given IList.
        /// </summary>
        /// <param name="list">The list of values.</param>
        /// <param name="i">The first location.</param>
        /// <param name="j">The second location.</param>
        private void Swap(int[] list, int i, int j)
        {
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Performs a select sort
        /// </summary>
        /// <param name="l">The values to sort</param>
        private void SelectSort(int[] l)
        {
            for (int i = 0; i < l.Length; i++)
            {
                int smallest = int.MaxValue;
                int loc = 0;
                for (int j = i; j < l.Length; j++)
                {
                    if (l[j] < smallest)
                    {
                        smallest = l[j];
                        loc = j;
                    }
                }
                Swap(l, i, loc);
                MarkSorted(i);
                MarkSwapped(loc);

                // Try to update the textboxes. 
                try
                {
                    UpdateTextBoxes();
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Performs an insertion sort
        /// </summary>
        /// <param name="l">The values to sort</param>
        private void InsertSort(int[] l)
        {
            try
            {
                for (int i = 1; i < l.Length; i++)
                {
                    int temp = l[i];
                    int j = i;
                    while (j > 0 && l[j - 1] > temp)
                    {
                        l[j] = l[j - 1];
                        UpdateTextBoxes();
                        j--;
                    }
                    l[j] = temp;
                    UpdateTextBoxes();
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        /// <summary>
        /// Merges the two given sorted portions of the given list into a single sorted portion. The sorted
        /// portions are consecutive.
        /// </summary>
        /// <param name="list">The list to merge.</param>
        /// <param name="start">The first index of the first sorted portion.</param>
        /// <param name="len1">The length of the first sorted portion.</param>
        /// <param name="len2">The length of the second sorted portion.</param>
        private void Merge(int[] l, int start, int len1, int len2)
        {
            try
            {
                int n = len1 + len2;
                ClearMergedTextBoxes();
                _merged = new int[n];

                int next1 = start;
                int next2 = start + len1;
                int end1 = next2;
                int end2 = start + n;
                int nextMerged = 0;
                ColorRange(next1, end1, Color.LightGreen);
                ColorRange(next2, end2, Color.LightSalmon);
                UpdateMergeTextBoxes();
                while (next1 < end1 && next2 < end2)
                {
                    if (l[next1] <= l[next2])
                    {
                        _merged[nextMerged] = l[next1];
                        UpdateMergeTextBoxes();
                        next1++;
                    }
                    else
                    {
                        _merged[nextMerged] = l[next2];
                        UpdateMergeTextBoxes();
                        next2++;
                    }
                    nextMerged++;
                }
                while (next1 < end1)
                {
                    _merged[nextMerged] = l[next1];
                    UpdateMergeTextBoxes();
                    next1++;
                    nextMerged++;
                }
                while (next2 < end2)
                {
                    _merged[nextMerged] = l[next2];
                    UpdateMergeTextBoxes();
                    next2++;
                    nextMerged++;
                }
                for (int i = 0; i < _merged.Length; i++)
                {
                    l[start + i] = _merged[i];
                    UpdateTextBoxes();
                }

                ClearColors();
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        /// <summary>
        /// Merges the two given sorted portions of the given list into a single sorted portion. The sorted
        /// portions are consecutive.
        /// </summary>
        /// <param name="list">The list to merge.</param>
        /// <param name="start">The first index of the first sorted portion.</param>
        /// <param name="len1">The length of the first sorted portion.</param>
        /// <param name="len2">The length of the second sorted portion.</param>
        private void MergeSort(int[] l, int start, int len)
        {
            if (len > 1)
            {
                int half = len / 2;
                txtSortStatus.AppendLine("running merge sort: start = " + start + " len: " + half);
                MergeSort(l, start, half);
                int half2 = len - half;
                txtSortStatus.AppendLine("running merge sort: start = " + start + ", len: " + half2);
                MergeSort(l, start + half, half2);
                txtSortStatus.AppendLine("merging: start = " + start + ", len: " + half + ", len2: " + half2);
                Merge(l, start, half, half2);
            }
        }

        /// <summary>
        /// Sorts the given list.
        /// </summary>
        /// <param name="list">The values to sort.</param>
        private void MergeSort(int[] l)
        {
            MergeSort(l, 0, l.Length);


        }

        /// <summary>
        /// Sorts the given portion of the given list.
        /// </summary>
        /// <param name="l">The list to sort.</param>
        /// <param name="start">The index of the first element of the portion to sort.</param>
        /// <param name="len">The number of elements to sort.</param>
        private void QuickSort(int[] l, int start, int len)
        {
            try
            {
                if (len > 1)
                {
                    int pivot = l[start];
                    int afterLess = start;
                    int beforeEqual = start + len - 1;
                    int beforeGreater = beforeEqual;
                    while (afterLess <= beforeEqual)
                    {
                        if (l[beforeEqual] < pivot)
                        {
                            Swap(l, afterLess, beforeEqual);
                            afterLess++;


                            ClearColors();
                            ColorRange(start, len, Color.Gray);
                            ColorRange(start, afterLess, Color.Aqua);
                            ColorRange(beforeEqual, beforeGreater, Color.Green);
                            ColorRange(beforeGreater, len, Color.Salmon);
                            UpdateTextBoxes();
                        }
                        else if (l[beforeEqual] == pivot)
                        {
                            beforeEqual--;

                            ClearColors();
                            ColorRange(start, len, Color.Gray);
                            ColorRange(start, afterLess, Color.Aqua);
                            ColorRange(beforeEqual, beforeGreater, Color.Green);
                            ColorRange(beforeGreater, len, Color.Salmon);
                            UpdateTextBoxes();
                        }
                        else
                        {
                            Swap(l, beforeEqual, beforeGreater);
                            beforeEqual--;
                            beforeGreater--;

                            ClearColors();
                            ColorRange(start, len, Color.Gray);
                            ColorRange(start, afterLess, Color.Aqua);
                            ColorRange(beforeEqual, beforeGreater, Color.Green);
                            ColorRange(beforeGreater, len, Color.Salmon);
                            UpdateTextBoxes();
                        }
                    }
                    txtSortStatus.AppendLine("out of loop");
                    QuickSort(l, start, afterLess - start);
                    QuickSort(l, beforeGreater + 1, start + len - beforeGreater - 1);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        /// <summary>
        /// Sorts the given list using quick sort
        /// </summary>
        /// <param name="l">The values to sort</param>
        private void QuickSort(int[] l)
        {
            QuickSort(l, 0, l.Length);
        }
    }
}
