using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionCalculator
{
    /// <summary>
    /// Shunting-yard algorithm implementation including precedence and associativity 
    /// </summary>
    class RPNConverter
    {
        static string[] arithOpList = { "+", "-", "*", "/", "^" };
        static string[] funcOpList = { "sin", "cos", "tan", "asin", "acos", "atan", "log", "ln", "√", "floor", "ceil" };
        public enum AssocType {Left, Right};

        public static int GetPrecedence(string op)
        {
            if (op == "+" || op == "-") { return 2; }
            if (op == "*" || op == "/") { return 3; }
            if (op == "^") { return 4; }
            return -1;
        }

        public static AssocType GetAssocType(string op)
        {
            if (op == "^") { return AssocType.Right; }
            else { return AssocType.Left; }
        }

        /// <summary>
        /// Converts the input expression into reverse Polish notation using the shunting-yard algorithm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> Convert(string[] input)
        {
            double conv;
            string stackTop;
            Stack<string> opStack = new Stack<string>();
            List<string> output = new List<string>();

            foreach (string token in input)
            {
                if (double.TryParse(token, out conv)) // if token is a real number
                {
                    output.Add(token);
                }
                else if (arithOpList.Contains(token)) // if token is an operator
                {
                    if (opStack.Count > 0) // if the operator stack is empty, the Peek method will throw an exception
                    {
                        stackTop = opStack.Peek(); // top of the stack
                        while (GetPrecedence(stackTop) >= GetPrecedence(token) && GetAssocType(stackTop) == AssocType.Left)
                        {
                            output.Add(opStack.Pop());
                            if (opStack.Count > 0) // if the operator stack is empty, the Peek method will throw an exception
                            {
                                stackTop = opStack.Peek();
                            }
                            else { break; }
                        }
                    }

                    opStack.Push(token);
                }
                else if (funcOpList.Contains(token) ||token == "(")
                {
                    opStack.Push(token);
                }
                else if (token == ")")
                {
                    // add all operators up to the left parenthesis into the output
                    while (opStack.Peek() != "(")
                    {
                        output.Add(opStack.Pop());
                    }
                    opStack.Pop(); // pops the "("

                    while (opStack.Count > 0 && funcOpList.Contains(opStack.Peek())) // pop all remainig functions
                    {
                        output.Add(opStack.Pop());
                    }
                }
            }

            while (opStack.Count > 0) // pop all remaining operators
            {
                stackTop = opStack.Peek();
                if (arithOpList.Contains(stackTop) || funcOpList.Contains(stackTop))
                {
                    output.Add(opStack.Pop());
                }
                else
                {
                    return null; // invalid expression
                }
            }

            return output;
        }
    }
}
