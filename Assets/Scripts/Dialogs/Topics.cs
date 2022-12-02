using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITopic
{
    public string PropId { get; set; }
    public Vector2 PropPosition { get; set; }
}

[Serializable]
public class Question : ITopic
{
    public string PropId { get => Id; set => Id = value; }
    public Vector2 PropPosition { get => Position; set => Position = value; }

    public string Id;
    public Vector2 Position;
    public string Text;
    public string AnswerId;
}

[Serializable]
public class ExactAnswer : ITopic
{
    public string PropId { get => Id; set => Id = value; }
    public Vector2 PropPosition { get => Position; set => Position = value; }

    public string Id;
    public Vector2 Position;
    public string Text;
    public List<string> NextTopicIds = new List<string>();
    public string Action;
}

[Serializable]
public class Condition : ITopic
{
    public string PropId { get => Id; set => Id = value; }
    public Vector2 PropPosition { get => Position; set => Position = value; }

    public string Id;
    public Vector2 Position;
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
    public string PropId { get => Id; set => Id = value; }
    public Vector2 PropPosition { get => Position; set => Position = value; }

    public string Id;
    public Vector2 Position;
    public string Script;

    public void Evaluate(GlobalCtx globalCtx)
    {
        globalCtx.Evaluate(Script);
    }
}
