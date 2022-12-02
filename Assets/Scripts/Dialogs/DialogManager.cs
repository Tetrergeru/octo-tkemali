using System.Collections.Generic;
using System.Linq;

public class DialogManager
{
    private Dialog _dialog;
    private GlobalCtx _globalCtx;

    private ExactAnswer _state;
    private Dictionary<string, ITopic> _topics;
    private List<Question> _startTopics = new List<Question>();


    public bool InConversation => _state != null;

    public DialogManager(Dialog dialog, GlobalCtx globalCtx)
    {
        _dialog = dialog;
        _globalCtx = globalCtx;
        _topics = _dialog.Topics.ToDictionary(it => it.PropId, it => it);
        EvaluateTopics();
    }

    private void EvaluateTopics()
    {
        var nonStartTopics = new HashSet<string>();
        foreach (var topic in _dialog.Topics)
        {
            switch (topic)
            {
                case ExactAnswer ea:
                    nonStartTopics.Add(topic.PropId);
                    foreach (var next in ea.NextTopicIds)
                    {
                        nonStartTopics.Add(next);
                    }
                    break;
                case Condition c:
                    nonStartTopics.Add(topic.PropId);
                    foreach (var next in c.Cases)
                    {
                        nonStartTopics.Add(next.NextId);
                    }
                    break;
            }
        }
        _startTopics = new List<Question>();
        _startTopics.AddRange(_topics.Where(it => !nonStartTopics.Contains(it.Key)).Select(it => it.Value as Question));
    }

    public IEnumerable<Question> StartTopics()
    {
        return _startTopics;
    }

    public IEnumerable<Question> WhatToSay()
    {
        if (!InConversation)
        {
            yield break;
        }

        foreach (var nextTopicId in _state.NextTopicIds)
        {
            yield return GetTopic(nextTopicId) as Question;
        }
    }

    public ExactAnswer PlayerSays(Question question)
    {
        var state = GetAnswer(question);
        _state = state;
        if (_state.NextTopicIds.Count == 0)
            _state = null;
        if (state.Action != null)
            _globalCtx.Evaluate((GetTopic(state.Action) as DialogAction)?.Script ?? "");
        return state;
    }

    private ExactAnswer GetAnswer(ITopic question)
    {
        if (question == null)
            return null;
        return question switch
        {
            Question q => GetAnswer(GetTopic(q.AnswerId)),
            ExactAnswer ea => ea,
            Condition c => GetAnswer(GetTopic(c.EvaluateCase(_globalCtx).NextId)),
            _ => throw new System.Exception(),
        };
    }

    private ITopic GetTopic(string id)
    {
        return _topics.ContainsKey(id) ? _topics[id] : null;
    }
}