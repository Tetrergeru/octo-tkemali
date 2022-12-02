using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCtx
{
    public Dictionary<string, string> Data = new Dictionary<string, string>();

    public string Evaluate(string expr) 
    {
        return new Evaluator(expr, this).Evaluate();
    }

    public string EvaluateExpression(string expr) 
    {
        return new Evaluator(expr, this).EvaluateExpression();
    }

    public string Get(string name) => Data.ContainsKey(name) ? Data[name] : "";
    public string Set(string name, string value) => Data[name] = value;
}
