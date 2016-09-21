using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace SortVisualizer
{
    public partial class UserInterface : Form
    {
        /// <summary>
        /// References to the list content textboxes
        /// </summary>
        TextBox[] _textboxes = new TextBox[10];
        TextBox[] _mergeTextBoxes = new TextBox[10];

        /// <summary>
        /// This is the list that will be sorted
        /// </summary>
        int[] _list = new int[10];

        /// <summary>
        /// This is used by merge sort
        /// </summary>
        int[] _merged = new int[10];
        
        /// <summary>
        /// Used to advance the sorting algorithm. If false, the user hasn't allowed us to continue
        /// </summary>
        bool shouldContinue = false;

        /// <summary>
        /// Keep track of what sorting algorithm we are using.
        /// </summary>
        enum Sort { SelectSort, InsertionSort, MergeSort, QuickSort };
        Sort currentAlgo = Sort.SelectSort;

        /// <summary>
        /// Form constructor. 
        /// Initializes the contents of the two textbox arrays to hold references to the GUI controls.
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();
            _textboxes[0] = textBox1;
            _textboxes[1] = textBox2;
            _textboxes[2] = textBox3;
            _textboxes[3] = textBox4;
            _textboxes[4] = textBox5;
            _textboxes[5] = textBox6;
            _textboxes[6] = textBox7;
            _textboxes[7] = textBox8;
            _textboxes[8] = textBox9;
            _textboxes[9] = textBox10;

            _mergeTextBoxes[0] = textBox11;
            _mergeTextBoxes[1] = textBox12;
            _mergeTextBoxes[2] = textBox13;
            _mergeTextBoxes[3] = textBox14;
            _mergeTextBoxes[4] = textBox15;
            _mergeTextBoxes[5] = textBox16;
            _mergeTextBoxes[6] = textBox17;
            _mergeTextBoxes[7] = textBox18;
            _mergeTextBoxes[8] = textBox19;
            _mergeTextBoxes[9] = textBox20;
        }

        /// <summary>
        /// Randomizes the contents of the textboxes.
        /// </summary>
        private void RandomizeTextBoxes()
        {
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                int num = r.Next(0, 9);
                _textboxes[i].Text = num.ToString();
                _list[i] = num;
            }
        }

        /// <summary>
        /// Prepare the user interface for sorting.
        /// </summary>
        private void SetupGUIForSorting()
        {
            btnSelectSort.Enabled = false;
            btnInsertSort.Enabled = false;
            btnMergeSort.Enabled = false;
            btnQuickSort.Enabled = false;
            btnRandomize.Enabled = false;
            foreach (TextBox txt in _textboxes)
            {
                txt.ReadOnly = true;
            }

            btnContinue.Enabled = true;
        }

        #region Event Handlers
        /// <summary>
        /// Form load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInterface_Load(object sender, EventArgs e)
        {
            RandomizeTextBoxes();
        }

        /// <summary>
        /// Handles click event for randomize button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRandomize_Click(object sender, EventArgs e)
        {
            RandomizeTextBoxes();
        }

        private void btnSelectSort_Click(object sender, EventArgs e)
        {
            SetupGUIForSorting();
            currentAlgo = Sort.SelectSort;
            sorter.RunWorkerAsync();
        }

        /// <summary>
        /// Begin an insertion sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertSort_Click(object sender, EventArgs e)
        {
            SetupGUIForSorting();
            currentAlgo = Sort.InsertionSort;
            sorter.RunWorkerAsync();
        }

        /// <summary>
        /// Begin a merge sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMergeSort_Click(object sender, EventArgs e)
        {
            SetupGUIForSorting();
            currentAlgo = Sort.MergeSort;
            sorter.RunWorkerAsync();
        }

        /// <summary>
        /// Begin a quick sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuickSort_Click(object sender, EventArgs e)
        {
            SetupGUIForSorting();
            currentAlgo = Sort.QuickSort;
            sorter.RunWorkerAsync();
        }

        /// <summary>
        /// Handles a click event on the continue button.
        /// Advance the sorting algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinue_Click(object sender, EventArgs e)
        {
            shouldContinue = true;
        }


        /// <summary>
        /// Handles Click event for the Reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetVisualizer();
        }

        /// <summary>
        /// Begin sorting. The sorting will take place on a different thread so execution can be paused.
        /// This allows the user to visualize how the sorting is taking place.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sorter_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (currentAlgo)
            {
                case Sort.SelectSort:
                    SelectSort(_list);
                    break;
                case Sort.InsertionSort:
                    InsertSort(_list);
                    break;
                case Sort.MergeSort:
                    MergeSort(_list);
                    break;
                case Sort.QuickSort:
                    QuickSort(_list);
                    break;
            }
        }

        /// <summary>
        /// The sorting is over, so reset everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sorter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetVisualizer();
        }

        #endregion

        /// <summary>
        /// Updates the values of the TextBoxes which hold the current contents of the list.
        /// This function will not return until the user clicks the continue button.
        /// </summary>
        private void UpdateTextBoxes()
        {
            for (int i = 0; i < 10; i++)
            {
                SetText(_textboxes[i], _list[i].ToString());
            }

            while (!shouldContinue)
            {
                if (sorter.CancellationPending)
                {
                    // The user wants to cancel the sort
                    throw new OperationCanceledException();
                }
            }
            shouldContinue = false;
        }

        /// <summary>
        /// Updates the values of the TextBoxes which hold the current contents of the temporary merge sort array
        /// This function will not return until the user clicks the continue button
        /// </summary>
        private void UpdateMergeTextBoxes()
        {
            for (int i = 0; i < _merged.Length; i++)
            {
                SetText(_mergeTextBoxes[i], _merged[i].ToString());
            }

            while (!shouldContinue)
            {
                if (sorter.CancellationPending)
                {
                    // The user wishes to cancel the sort
                    throw new OperationCanceledException();
                }
            }
            shouldContinue = false;
        }

        /// <summary>
        /// A function to change the Text property of a given text box.
        /// This function uses Invoke to allow other threads to change elements on the user interface.
        /// </summary>
        /// <param name="txt">The target textbox</param>
        /// <param name="text">The replacement text</param>
        delegate void SetTextCallback(TextBox txt, string text);
        private void SetText(TextBox txt, string text)
        {
            if (txt.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { txt, text });
            }
            else
            {
                txt.Text = text;
            }
        }

        /// <summary>
        /// Used by the select sort algorithm to mark when two elements have been swapped
        /// </summary>
        /// <param name="index">The index which should be marked as swapped</param>
        delegate void MarkSwappedCallback(int index);
        private void MarkSwapped(int index)
        {
            if (_textboxes[index].InvokeRequired)
            {
                MarkSwappedCallback m = new MarkSwappedCallback(MarkSwapped);
                Invoke(m, new object[] { index });
            }
            else
            {
                _textboxes[index].BackColor = Color.LightSalmon;
            }
        }

        /// <summary>
        /// Used by the select sort algorithm to mark the sorted portion of the list
        /// </summary>
        /// <param name="index">Mark that the element at this index is in the correct position</param>
        delegate void MarkSortedCallBack(int index);
        private void MarkSorted(int index)
        {
            if (_textboxes[index].InvokeRequired)
            {
                MarkSortedCallBack m = new MarkSortedCallBack(MarkSorted);
                Invoke(m, new object[] { index });
            }
            else
            {
                foreach (TextBox t in _textboxes)
                {
                    if (t.BackColor == Color.LightSalmon)
                    {
                        t.BackColor = Color.White;
                    }
                }
                _textboxes[index].BackColor = Color.LightGreen;
            }
        }

        /// <summary>
        /// Change the background color of the TextBoxes in the given range
        /// </summary>
        /// <param name="start">Start position</param>
        /// <param name="end">End position</param>
        /// <param name="c">The color</param>
        delegate void ColorRangeCallback(int start, int end, Color c);
        private void ColorRange(int start, int end, Color c)
        {
            if (start >= 0)
            {
                for (int i = start; i < end; i++)
                {
                    if (_textboxes[i].InvokeRequired)
                    {
                        ColorRangeCallback m = new ColorRangeCallback(ColorRange);
                        Invoke(m, new object[] { start, end, c });
                    }
                    else
                    {
                        _textboxes[i].BackColor = c;
                    }
                }

            }

        }

        /// <summary>
        /// Reset the background color of all textboxes back to their default color
        /// </summary>
        delegate void ClearColorsCallback();
        private void ClearColors()
        {
            if (_textboxes[0].InvokeRequired)
            {
                ClearColorsCallback c = new ClearColorsCallback(ClearColors);
                Invoke(c, new object[] { });
            }
            else
            {
                foreach (TextBox t in _textboxes)
                {
                    t.BackColor = Color.White;
                }
            }
        }

       

        /// <summary>
        /// Clears all of the text in the merge sort textboxes
        /// </summary>
        delegate void ClearMergedTBCallback();
        private void ClearMergedTextBoxes()
        {
            if (_mergeTextBoxes[0].InvokeRequired)
            {
                ClearMergedTBCallback t = new ClearMergedTBCallback(ClearMergedTextBoxes);
                Invoke(t, new object[] { });
            }
            else
            {
                foreach (TextBox txt in _mergeTextBoxes)
                {
                    txt.Text = "";
                }
            }
            
        }

        /// <summary>
        /// Reset the visualizer
        /// </summary>
        private void ResetVisualizer()
        {
            sorter.CancelAsync();
            btnContinue.Enabled = false;
            btnRandomize.Enabled = true;
            btnSelectSort.Enabled = true;
            btnInsertSort.Enabled = true;
            btnMergeSort.Enabled = true;

            foreach (TextBox txt in _textboxes)
            {
                txt.ReadOnly = false;
                txt.BackColor = Color.White;
            }

            ClearMergedTextBoxes();
            txtSortStatus.Clear();

        }

       
    }
}
