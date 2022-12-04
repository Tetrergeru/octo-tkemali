using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Topic
{
    [SerializeField]
    public string Id;

    [SerializeField]
    public Vector2 Position;
}

[Serializable]
public class Question : Topic
{
    public string Text;
    public string AnswerId;
}

[Serializable]
public class Answer : Topic
{
    public string Text;
    public List<string> NextTopicIds = new List<string>();
    public string Action;
}

[Serializable]
public class AnswerSwitch : Topic
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
public class DialogAction : Topic
{
    public string Script;

    public void Evaluate(GlobalCtx globalCtx)
    {
        globalCtx.Evaluate(Script);
    }
}
