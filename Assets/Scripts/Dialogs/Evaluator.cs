using System;
using System.Collections.Generic;
using System.Text;

public class Evaluator
{
    private string _expr;
    private int _idx;
    private GlobalCtx _globalCtx;

    private static Dictionary<string, Func<GlobalCtx, List<string>, string>> Functions =
        new Dictionary<string, Func<GlobalCtx, List<string>, string>>
        {
            ["PlayerHas"] = (ctx, args) =>
            {
                var id = args[0];
                var quantity = int.Parse(args[1]);
                return ctx.PlayerInventory.HowMany(ctx.AllItems[id]) >= quantity
                    ? "true" : "false";
            },
            ["TakeFromPlayer"] = (ctx, args) =>
            {
                var id = args[0];
                var quantity = int.Parse(args[1]);
                return ctx.PlayerInventory.RemoveItems(ctx.AllItems[id], quantity).ToString();
            },
            ["GiveToPlayer"] = (ctx, args) =>
            {
                var id = args[0];
                var quantity = int.Parse(args[1]);
                ctx.PlayerInventory.AddItems(ctx.AllItems[id], quantity);
                return "true";
            }
        };

    public Evaluator(string expr, GlobalCtx globalCtx)
    {
        _expr = expr;
        _globalCtx = globalCtx;
    }

    public string Evaluate()
    {
        if (_expr == "") return "";

        return ParseStmt();
    }

    public string EvaluateExpression()
        => ParseExpr();

    private string ParseStmt()
    {
        while (true)
        {
            ConsumeSpace();
            if (Peek("set "))
            {
                Consume("set");
                ConsumeSpace();

                var left = ParseId();
                ConsumeSpace();
                Consume('=');

                var right = ParseExpr();
                _globalCtx.Set(left, right);
            }
            else
            {
                ParseExpr();
            }
            ConsumeSpace();

            if (Peek() == -1)
                return "";

            Consume(';');
            ConsumeSpace();

            if (Peek() == -1)
                return "";
        }

    }

    private string ParseExpr()
        => ParseComparison();

    private string ParseComparison()
    {
        var left = ParsePrimary();

        ConsumeSpace();
        if (!Peek("=="))
        {
            return left;
        }
        Consume("==");
        var right = ParseComparison();

        return left == right ? "true" : "false";
    }

    private string ParseFunction(string left)
    {
        Consume('(');
        ConsumeSpace();
        var args = new List<string>();
        while (true)
        {
            args.Add(ParseExpr());
            ConsumeSpace();

            if (Peek(')')) break;

            Consume(',');
            ConsumeSpace();
        }
        Consume(')');
        return Functions[left](_globalCtx, args);
    }

    private string ParsePrimary()
    {
        ConsumeSpace();
        if (Peek(Digit))
            return ParseNumber();
        if (Peek(Alpha))
        {
            var id = ParseId();

            ConsumeSpace();
            return Peek('(')
                ? ParseFunction(id)
                : _globalCtx.Get(id);
        }
        if (Peek('"'))
        {
            var res = new StringBuilder();
            Consume('"');
            while (!Peek('"'))
                res.Append(ConsumeChar());
            Consume('"');

            return res.ToString();
        }
        if (Peek('('))
        {
            Consume('(');
            ConsumeSpace();
            var left = ParseExpr();
            ConsumeSpace();
            Consume(')');
            return left;
        }
        throw UnexpectedPeek();
    }

    private string ParseId()
    {
        ConsumeSpace();
        var id = new StringBuilder();
        id.Append(Consume(Alpha));
        while (Peek(AlphaDigit))
        {
            id.Append(Consume(AlphaDigit));
        }
        return id.ToString();
    }

    private string ParseNumber()
    {
        ConsumeSpace();
        var id = new StringBuilder();
        while (Peek(Digit))
        {
            id.Append(Consume(Digit));
        }
        return id.ToString();
    }

    private char Consume(Func<char, bool> pred)
    {
        if (Peek(pred))
            return ConsumeChar();
        throw UnexpectedPeek();
    }

    private void Consume(char ch) => Consume(c => c == ch);

    private void Consume(string str)
    {
        foreach (var ch in str)
        {
            Consume(ch);
        }
    }

    private void ConsumeSpace()
    {
        while (Peek(it => char.IsWhiteSpace(it)))
            ConsumeChar();
    }

    private bool Peek(Func<char, bool> pred)
        => Peek() > -1 && pred((char)Peek());

    private bool Peek(char ch)
        => Peek() > -1 && (char)Peek() == ch;

    private bool Peek(string str)
    {
        for (var i = 0; i < str.Length; i++)
            if (Peek(i) < 0 || Peek(i) != str[i])
                return false;
        return true;
    }

    private int Peek(int depth = 0)
        => _idx >= _expr.Length ? -1 : _expr[_idx + depth];

    private Exception UnexpectedPeek()
    {
        if (Peek() < 0)
            return new System.Exception($"Unexpected EOF ({_expr})");
        return new System.Exception($"Unexpected char '{(char)Peek()}' ({_expr})");
    }

    private char ConsumeChar()
    {
        if (Peek() < 0)
            throw new System.Exception($"Unexpected EOF ({_expr})");
        return _expr[_idx++];
    }

    private static bool Alpha(char ch) => ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch == '_';
    private static bool Digit(char ch) => ch >= '0' && ch <= '9';
    private static bool AlphaDigit(char ch) => Alpha(ch) || Digit(ch);
}