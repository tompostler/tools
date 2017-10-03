using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unlimitedinf.Tools.Parsing
{
    /// <summary>
    /// The class that will actually parse an expression and then evaluate the result.
    /// </summary>
    public static class ExpressionParser
    {
        /// <summary>
        /// Extension method to convert a string expression to a double. Empty strings equal zero.
        /// </summary>
        public static double EvaluateAsExpression(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            var postfix = ConvertInfixToPostfix(input);
            return EvaluatePostfix(postfix);
        }

        //TODO: do some performance testing to see how much faster it is just to use giant if-else statements instead of hash sets.
        private static class Ops
        {
            public static readonly HashSet<char> Exp = new HashSet<char>("^");
            public static readonly HashSet<char> Mul = new HashSet<char>("x*");
            public static readonly HashSet<char> Div = new HashSet<char>("/");
            public static readonly HashSet<char> Add = new HashSet<char>("+");
            public static readonly HashSet<char> Sub = new HashSet<char>("-");
            public static readonly HashSet<char> All = new HashSet<char>(Exp.Union(Mul).Union(Div).Union(Add).Union(Sub));
            public static readonly HashSet<char> Bkt = new HashSet<char>("({[]})");

            /// <summary>
            /// Rate the math operators. Higher precedence has a lower number.
            /// </summary>
            public static int GetTier(char op)
            {
                if (Exp.Contains(op))
                    return 0;
                else if (Mul.Contains(op) || Div.Contains(op))
                    return 1;
                else if (Add.Contains(op) || Sub.Contains(op))
                    return 2;
                else if (Bkt.Contains(op))
                    return 100;
                else
                    throw new KeyNotFoundException($"{op} is not a registered operator.");
            }
        }

        /// <summary>
        /// See <see href="https://en.wikipedia.org/wiki/Shunting-yard_algorithm"/> for more information.
        /// </summary>
        /// <remarks>
        /// while there are tokens to be read:
        /// 	read a token.
        /// 	if the token is a number, then push it to the output queue.
        /// 	if the token is an operator, then:
        /// 		while there is an operator at the top of the operator stack with
        /// 			greater than or equal to precedence and the operator is left associative:
        ///         pop operators from the operator stack, onto the output queue.
        /// 		push the read operator onto the operator stack.
        /// 	if the token is a left bracket (i.e. "("), then:
        /// 		push it onto the operator stack.
        /// 	if the token is a right bracket (i.e. ")"), then:
        /// 		while the operator at the top of the operator stack is not a left bracket:
        ///         pop operators from the operator stack onto the output queue.
        /// 		pop the left bracket from the stack.
        /// 		/* if the stack runs out without finding a left bracket, then there are
        /// 		mismatched parentheses. */
        /// if there are no more tokens to read:
        /// 	while there are still operator tokens on the stack:
        ///         /* if the operator token on the top of the stack is a bracket, then
        /// 		there are mismatched parentheses. */
        ///         pop the operator onto the output queue.
        ///         exit.
        /// </remarks>
        internal static List<string> ConvertInfixToPostfix(string infix)
        {
            infix = infix.Replace(" ", string.Empty);
            List<string> output = new List<string>(infix.Length);
            Stack<char> ops = new Stack<char>();

            // Because maths, if the string starts with a negation then insert a 0
            if (infix[0] == '-')
                output.Add("0");

            int i = 0;
            char c = '\0';
            StringBuilder sb = new StringBuilder(5);
            while (i < infix.Length)
            {
                c = infix[i];
                // If it's a number, then read the number
                if ((c >= '0' && c <= '9') || c == '.')
                {
                    sb.Clear();
                    sb.Append(c);
                    while (i + 1 < infix.Length && ((infix[i + 1] >= '0' && infix[i + 1] <= '9') || infix[i + 1] == '.'))
                        sb.Append(infix[++i]);
                    output.Add(sb.ToString());
                }
                // If it's an operator
                else if (Ops.All.Contains(c))
                {
                    while (ops.Count > 0 && Ops.GetTier(ops.Peek()) <= Ops.GetTier(c))
                        if (Ops.GetTier(ops.Peek()) == 0)
                            // Any ops that are 0 (for now at least) are right-associative and should break the
                            //  stack-popping
                            break;
                        else
                            output.Add(ops.Pop().ToString());
                    ops.Push(c);
                }
                // If it's an open bracket
                else if (c == '(' || c == '[' || c == '{')
                {
                    ops.Push(c);
                }
                // If it's a close bracket
                else if (c == ')' || c == ']' || c == '}')
                {
                    char p = ops.Peek();
                    while (!(p == '(' || p == '[' || p == '{'))
                    {
                        output.Add(ops.Pop().ToString());
                        p = ops.Peek();
                    }
                    ops.Pop();
                }
                // Else we messed something up
                else
                {
                    throw new FormatException("infix was not well formed.");
                }
                i++;
            }
            // Pop the rest of the ops
            while (ops.Count > 0)
                output.Add(ops.Pop().ToString());

            return output;
        }

        /// <summary>
        /// See <see href="http://scriptasylum.com/tutorials/infix_postfix/algorithms/postfix-evaluation/index.htm"/>
        /// for more information.
        /// </summary>
        /// <remarks>
        /// Initialise an empty stack.
        /// Scan the Postfix string from left to right.
        /// If the scannned character is an operand, add it to the stack. If the scanned character is an operator,
        ///     there will be at least two operands in the stack.
        /// If the scanned character is an operator, then we store the top most element of the stack(topStack) in a
        ///     variable temp. Pop the stack. Now evaluate topStack(Operator)temp. Let the result of this operation be
        ///     retVal. Pop the stack and Push retVal into the stack.
        /// Repeat this step till all the characters are scanned.
        /// After all characters are scanned, we will have only one element in the stack. Return topStack.
        /// </remarks>
        internal static double EvaluatePostfix(List<string> postfix)
        {
            Stack<double> stack = new Stack<double>(postfix.Count);

            double t = 0;
            char o = '\0';
            foreach (var token in postfix)
            {
                // If it's an operand
                if (double.TryParse(token, out double val))
                {
                    stack.Push(val);
                }
                // If it's an operator
                else if (token.Length == 1 && Ops.All.Contains(token[0]))
                {
                    o = token[0];
                    t = stack.Pop();
                    // Figure out what we got and do the thing
                    if (Ops.Add.Contains(o))
                        stack.Push(stack.Pop() + t);
                    else if (Ops.Sub.Contains(o))
                        stack.Push(stack.Pop() - t);
                    else if (Ops.Mul.Contains(o))
                        stack.Push(stack.Pop() * t);
                    else if (Ops.Div.Contains(o))
                        stack.Push(stack.Pop() / t);
                    else if (Ops.Exp.Contains(o))
                        stack.Push(Math.Pow(stack.Pop(), t));
                }
            }

            return stack.Pop();
        }
    }
}
