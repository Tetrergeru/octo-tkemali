using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ITopic
{
    [SerializeField]
    public string Id;

    [SerializeField]
    public Vector2 Position;
}

[Serializable]
public class Question : ITopic
{
    public string Text;
    public string AnswerId;
}

[Serializable]
public class ExactAnswer : ITopic
{
    public string Text;
    public List<string> NextTopicIds = new List<string>();
    public string Action;
}

[Serializable]
public class Condition : ITopic
{
    public List<Case> Cases = new List<Case>();
    public Case Default;

    public Case EvaluateCase(GlobalCtx globalCtx)
    {
        foreach (var @case in Cases)
        {
            if (globalCtx.EvaluateExpression(@case.Condition) == "true")
                return @case;
        }
        return Default;
    }

    [Serializable]
    public struct Case
    {
        public string Condition;
        public string NextId;
    }
}

[Serializable]
public class DialogAction : ITopic
{
    public string Script;

    public void Evaluate(GlobalCtx globalCtx)
    {
        globalCtx.Evaluate(Script);
    }
}
