using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Lexer;

public record Lexer(string Source)
{
    private int Index = 0;

    [MemberNotNullWhen(false, nameof(Current))]
    public bool EOF => Index >= Source.Length;

    public char? Current => Source[Index];

    public int Advance(int count = 1)
    {
        Index += count;
        return Index;
    }

    public char Consume()
    {
        var aux = Current!.Value!;
        Advance();
        return aux;
    }

    public char Peek(int offset = 0)
    {
        Debug.Assert(Source.Length > offset + Index);
        return Source[offset];
    }

    public SyntaxToken NextToken()
    {
        while (!EOF)
        {
            var current = Current.Value;
            switch (current)
            {
                case ' ':
                    return new SyntaxToken(SyntaxKind.WhiteSpaceToken, current, new Position(Advance(), 1));
                case '+':
                {
                    if (Peek(1) == '+')
                        return new SyntaxToken(SyntaxKind.IncrementOperator, current, new Position(Advance(2), 2));
                    if (Peek(1) == '=')
                        return new SyntaxToken(SyntaxKind.PlusAssignmentOperator, current, new Position(Advance(2), 2));
                    if (char.IsNumber(Peek(1))) // this is a positive number
                    {
                        Advance();
                        goto case '0';
                    }

                    return new SyntaxToken(SyntaxKind.PlusToken, current, new Position(Advance(), 1));
                }
                case '-':
                    if (Peek(1) == '-')
                        return new SyntaxToken(SyntaxKind.DecrementOperator, current, new Position(Advance(2), 2));
                    if (Peek(1) == '=')
                        return new SyntaxToken(SyntaxKind.MinusAssignmentOperator, current,
                            new Position(Advance(2), 2));
                    if (char.IsNumber(Peek(1))) // this is a negative number 
                    {
                        Advance();
                        goto case '0';
                    }

                    return new SyntaxToken(SyntaxKind.MinusToken, current, new Position(Advance(), 1));

                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, current, new Position(Advance(), 1));
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, current, new Position(Advance(), 1));
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesis, current, new Position(Advance(), 1));
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesis, current, new Position(Advance(), 1));
                case '{':
                    return new SyntaxToken(SyntaxKind.OpenBrace, current, new Position(Advance(), 1));
                case '}':
                    return new SyntaxToken(SyntaxKind.CloseBrace, current, new Position(Advance(), 1));
                case '=':
                    if (Peek(1) == '=')
                        return new SyntaxToken(SyntaxKind.EqualsEqualsOperator, current, new Position(Advance(), 1));
                    return new SyntaxToken(SyntaxKind.EqualsOperator, current, new Position(Advance(), 1));
                case '"':
                {
                    Advance();
                    var (value, startIndex, lenght) =
                        ReadToken((ch, index) => ch != '"');
                    Advance();
                    return new SyntaxToken(SyntaxKind.StringLiteral, value, new Position(startIndex, lenght + 1));
                }

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                {
                    var (value, startIndex, lenght) = ReadToken(char.IsNumber);
                    var position = new Position(startIndex, lenght);
                    return new SyntaxToken(SyntaxKind.NumberLiteral, int.Parse(value), position);
                }

                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                {
                    var (value, startIndex, lenght) =
                        ReadToken((ch, index) => char.IsLetter(ch) || (index > 0 && char.IsDigit(ch)));
                    var keyword = KeywordExtensions.FromString(value);
                    var keywordKind = KeywordExtensions.ToTokenKind(keyword);
                    var position = new Position(startIndex, lenght);
                    return new SyntaxToken(
                        keywordKind != SyntaxKind.BadToken ? keywordKind : SyntaxKind.Identifier,
                        value,
                        position
                    );
                }
            }

            return new SyntaxToken(SyntaxKind.BadToken, current, new Position(Advance(), 1));
        }

        return new SyntaxToken(SyntaxKind.EndOfFileToken, '\0', new Position(Advance(), 1));
    }

    /// <summary>
    /// Read interval of values based on filter function
    /// </summary>
    /// <param name="predicate">Predicate to filter. (CurrentChar, Index) => true | false</param>
    /// <returns>(ValueString, StartIndex, EndIndex)</returns>
    public (string, int, int) ReadToken(Func<char, int, bool> predicate)
    {
        var startIndex = Index;
        var buffer = new StringBuilder();
        while (!EOF && predicate(Current.Value, buffer.Length))
            buffer.Append(Consume());

        return (buffer.ToString(), startIndex, buffer.Length);
    }

    public (string, int, int) ReadToken(Func<char, bool> predicate)
    {
        return ReadToken((ch, _) => predicate(ch));
    }
}