using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionCalculator
{
    class RPNCalculator
    {
        static string[] arithOpList = { "+", "-", "*", "/", "^" };
        static string[] funcOpList = { "sin", "cos", "tan", "asin", "acos", "atan", "log", "ln", "√", "floor", "ceil" };

        /// <summary>
        /// Calculates the value of an expression written in the reverse Polish notation.
        /// The return type is nullable to enable the function to return null if the calculation has failed.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double? Calculate(List<string> exp)
        {
            double res = 0;
            double conv;
            double op1;
            double op2;

            Stack<double> operandStack = new Stack<double>();

            try
            {
                foreach (string token in exp)
                {
                    if (double.TryParse(token, out conv)) // if token is a real number
                    {
                        operandStack.Push(conv);
                    }
                    else if (arithOpList.Contains(token)) // if token is an arithmetic operator
                    {
                        op1 = operandStack.Pop();
                        op2 = operandStack.Pop();

                        switch (token)
                        {
                            case "+":
                                operandStack.Push(op2 + op1);
                                break;
                            case "-":
                                operandStack.Push(op2 - op1);
                                break;
                            case "*":
                                operandStack.Push(op2 * op1);
                                break;
                            case "/":
                                if (op1 == 0) { return null; }
                                operandStack.Push(op2 / op1);
                                break;
                            case "^":
                                operandStack.Push(Math.Pow(op2, op1));
                                break;
                        }
                    }
                    else if (funcOpList.Contains(token)) // if token is a function operator
                    {
                        op1 = operandStack.Pop();

                        switch (token)
                        {
                            case "sin":
                                operandStack.Push(Math.Sin(op1));
                                break;
                            case "cos":
                                operandStack.Push(Math.Cos(op1));
                                break;
                            case "tan":
                                operandStack.Push(Math.Tan(op1));
                                break;
                            case "asin":
                                operandStack.Push(Math.Asin(op1));
                                break;
                            case "acos":
                                operandStack.Push(Math.Acos(op1));
                                break;
                            case "atan":
                                operandStack.Push(Math.Atan(op1));
                                break;
                            case "log":
                                operandStack.Push(Math.Log10(op1));
                                break;
                            case "ln":
                                operandStack.Push(Math.Log(op1));
                                break;
                            case "√":
                                operandStack.Push(Math.Sqrt(op1));
                                break;
                            case "floor":
                                operandStack.Push(Math.Floor(op1));
                                break;
                            case "ceil":
                                operandStack.Push(Math.Ceiling(op1));
                                break;
                        }
                    }
                }

                res = operandStack.Pop();
                if (operandStack.Count > 0)
                {
                    return null;
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
