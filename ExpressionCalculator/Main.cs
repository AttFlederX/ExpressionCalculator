/* ExpressionCalculator
 * v1.0
 * 2017, AttFlederX */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpressionCalculator
{
    public partial class Main : Form
    {
        bool isOperatorLastReceived = true; // prevents operators from being typed in a row 
        bool isDecimalPointReceived = false; // prevents more than one decimal point from appearing in a single number
        bool initialState = true;
        bool resultState = false;

        List<double?> resultHistory = new List<double?>();

        double memValue = 0; // memory storage

        public Main()
        {
            InitializeComponent();
            ResetState();
            ClearInput();
        }

        /// <summary>
        /// Moves the cursor to make the last typed text visible
        /// </summary>
        private void MoveCursorToEnd()
        {
            inputTextBox.SelectionStart = inputTextBox.Text.Length - 1;
            inputTextBox.SelectionLength = 0;
        }

        /// <summary>
        /// Resets the textboxes if the program is in its initial state or result display state
        /// </summary>
        private void ResetState()
        {
            if (initialState || resultState) // if the textbox contains the default value
            {
                inputTextBox.Clear();
                outputTextBox.Clear();
                initialState = false;
                resultState = false;
            }
        }

        /// <summary>
        /// Resets the program to its initial state
        /// </summary>
        private void ClearInput()
        {
            inputTextBox.Text = "0.";
            outputTextBox.Text = string.Empty;
            isOperatorLastReceived = true;
            isDecimalPointReceived = false;
            initialState = true;
            resultState = false;
        }


        /// <summary>
        /// Handles the 1 through 9 digit buttons 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void digitButton_Click(object sender, EventArgs e)
        {
            Button b = (sender as Button);

            // if the first entered digit was not zero
            if (!inputTextBox.Text.EndsWith(" 0") && !(inputTextBox.Text.EndsWith("0") && inputTextBox.Text.Length == 1))
            {
                ResetState();
                inputTextBox.Text += b.Text;
                //exp.Append(b.Text);

                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Handles the +, -, * and / operators
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void operatorButton_Click(object sender, EventArgs e)
        {
            Button b = (sender as Button);

            if (!isOperatorLastReceived && !resultState && !inputTextBox.Text.EndsWith("( ") &&
                inputTextBox.Text[inputTextBox.Text.Length - 1] != '.') // can't have two operators in a row
            {

                inputTextBox.Text += string.Format(" {0} ", b.Text); // frame the operator into spaces for Split function
                //exp.Append(b.Text);
                isDecimalPointReceived = false;

                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Handles all function expressions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void funcButton_Click(object sender, EventArgs e)
        {
            Button b = (sender as Button);

            if (isOperatorLastReceived)
            {
                ResetState();

                inputTextBox.Text += string.Format("{0} ( ", b.Text);
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Changes the sign of the number(has to be put before entering the number)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plusMinusButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived || inputTextBox.Text.EndsWith("-"))
            {
                ResetState();

                if (inputTextBox.Text.EndsWith("-")) // changes back to positive
                {
                    inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.Text.Length - 1);
                    if (inputTextBox.Text.Length == 0) { ClearInput(); }
                }
                else
                {
                    inputTextBox.Text += "-";
                }
            }
        }

        /// <summary>
        /// Puts the pi constant into the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void piButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived)
            {
                ResetState();

                inputTextBox.Text += Math.PI;
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Puts the e constant into the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived)
            {
                ResetState();

                inputTextBox.Text += Math.E;
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Equals button handling(result calculation & display)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void equalsButton_Click(object sender, EventArgs e)
        {
            // converts the expression into reverse Polish notation and immidiately calculates the result
            double? result = RPNCalculator.Calculate(RPNConverter.Convert(inputTextBox.Text.Split(' ')));
            // MessageBox.Show(string.Join(string.Empty, RPNConverter.Convert(inputTextBox.Text.Split(' ')).ToArray()));

            if (result == null)
            {
                outputTextBox.Text = "Error";
            }
            else
            {
                outputTextBox.Text = result.ToString();
                // add to history
                historyListBox.Items.Add(string.Format("{0} = {1}", inputTextBox.Text, result.ToString()));
                resultHistory.Add(result);
                clearHistoryButton.Enabled = true;
                historyListBox.SelectedIndex = historyListBox.Items.Count - 1; // select the last item
            }
            resultState = true;
        }

        /// <summary>
        /// Left parenthesis handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leftParButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived || inputTextBox.Text.EndsWith("( "))
            {
                ResetState();

                inputTextBox.Text += "( ";
                //isOperatorLastReceived = false;
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Right parenthesis handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rightParButton_Click(object sender, EventArgs e)
        {
            if (!isOperatorLastReceived && inputTextBox.Text.Contains("(") && inputTextBox.Text[inputTextBox.Text.Length - 1] != '.')
            {
                inputTextBox.Text += " )";
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Decimal point handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decimalButton_Click(object sender, EventArgs e)
        {
            // can't have multiple decimal points in a single number or in the beginning of a number 
            if (!isOperatorLastReceived && !inputTextBox.Text.EndsWith("( ") && !isDecimalPointReceived)
            {
                inputTextBox.Text += ".";
                isDecimalPointReceived = true;
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// '0' handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zeroButton_Click(object sender, EventArgs e)
        {
            //if the first digit of a number was not 0
            if (!inputTextBox.Text.EndsWith(" 0") && !(inputTextBox.Text.EndsWith("0") && inputTextBox.Text.Length == 1))
            {
                ResetState();

                inputTextBox.Text += "0";
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Input & output clearing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        /// <summary>
        /// Backspace button handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backspaceButton_Click(object sender, EventArgs e)
        {
            if (!initialState && inputTextBox.Text.Length > 0)
            {
                if (inputTextBox.Text.EndsWith("( ") || inputTextBox.Text.EndsWith(" )"))
                {
                    inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.Text.Length - 2);
                }
                else if (inputTextBox.Text.EndsWith(".") || inputTextBox.Text.EndsWith("-"))
                {
                    inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.Text.Length - 1);
                    isDecimalPointReceived = false;
                }
                else if (isOperatorLastReceived)
                {
                    while (!char.IsDigit(inputTextBox.Text[inputTextBox.Text.Length - 1]) && !inputTextBox.Text.EndsWith(" )") &&
                        !inputTextBox.Text.EndsWith("( ")) // remove the operator and the spaces
                    {
                        inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.Text.Length - 1);
                    }
                }
                else
                {
                    inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.Text.Length - 1);
                }

                if (inputTextBox.Text.Length == 0) // if the input field is empty afteer the backspace
                {
                    ClearInput();
                }

                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Updates the isOperatorLastReceived condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            // anything other than a digit, a right parethesis or a decimal point is an operator 
            if (inputTextBox.Text.Length > 0)
            {
                // isOperatorLastReceived = (!char.IsDigit(inputTextBox.Text[inputTextBox.Text.Length - 1]) &&
                //    !inputTextBox.Text.EndsWith(".") && !inputTextBox.Text.EndsWith(" )"));
                isOperatorLastReceived = inputTextBox.Text.EndsWith(" "); // an operator is always followed by a space
            }
            else
            {
                isOperatorLastReceived = true;
            }
        }

        /// <summary>
        /// Copying context button handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // get the calling control
                TextBox t = (((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl as TextBox);
                Clipboard.SetText(t.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to copy the textbox contents: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Moves the result into the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveToInputButton_Click(object sender, EventArgs e)
        {
            if (outputTextBox.Text.Length > 0 && outputTextBox.Text != "Error")
            {
                double res = Convert.ToDouble(outputTextBox.Text);
                ClearInput();
                initialState = false;
                inputTextBox.Text = res.ToString();
            }
        }

        #region Memory buttons handling
        /// <summary>
        /// Memory reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memClearButton_Click(object sender, EventArgs e)
        {
            memValue = 0;
        }

        /// <summary>
        /// Memory read button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memRecallButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived)
            {
                ResetState();

                inputTextBox.Text += memValue;
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Memory addition button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memPlusButton_Click(object sender, EventArgs e)
        {
            if (resultState)
            {
                memValue += Convert.ToDouble(outputTextBox.Text);
            }
        }

        /// <summary>
        /// Memory subtraction button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memMinusButton_Click(object sender, EventArgs e)
        {
            if (resultState)
            {
                memValue -= Convert.ToDouble(outputTextBox.Text);
            }
        }

        /// <summary>
        /// Memory overwrite button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memStoreButton_Click(object sender, EventArgs e)
        {
            if (resultState)
            {
                memValue = Convert.ToDouble(outputTextBox.Text);
            }
        }
        #endregion

        /// <summary>
        /// History recall by double-click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyListBox_DoubleClick(object sender, EventArgs e)
        {
            int idx = historyListBox.SelectedIndex;

            if (isOperatorLastReceived)
            {
                ResetState();

                inputTextBox.Text += resultHistory[idx];
                MoveCursorToEnd();
            }
        }

        /// <summary>
        /// Clear history button handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearHistoryButton_Click(object sender, EventArgs e)
        {
            historyListBox.Items.Clear();
            resultHistory.Clear();
            clearHistoryButton.Enabled = false;
        }
    }
}
