# ExpressionCalculator
A reverse Polish notation-based mathematical expression calculator

Supported operations: addition, subtraction, multiplication, division, exponentation, square root, floor/ceiling
Trigonometric: (arc)sine, (arc)cosine, (arc)tangent
Logarithmic: base 10 log, natural log
Other features: result history, memory

Calculation process:
- When the '=' button is pressed, the expression in the input box is converted into the reverse Polish notation using the shunting-yard algorithm.
- Converted expression is transferred to the RPNCalculator class which calculates its value or returns null if the expression was invalid.
- The result is then displayed via the output textbox and stored in the history.

Usage notes:
- In order to enter a negative number, first press the '+/-' button and then enter the number's value. Note that negation only affects the first following number and cannot be applied to brackets or functions (as of v1.0).
- In a valid expression, the number of opening parentheses must match the number of closing parentheses (e.g. "sin ( 0 )", *not* "sin ( 0").
- If there are no parentheses in an expression, the regular precedence of operators will be assumed (e.g. 2 + 2 * 2 will yield 6).
