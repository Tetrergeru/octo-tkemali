using System;
using System.Collections.Generic;
using System.Linq;

public class DialogManager
{
    public Action OnStartTopicsUpdate = () => { };

    private Dialog _dialog;
    private GlobalCtx _globalCtx;
    private Answer _state;
    private Dictionary<string, Topic> _topics;
    private Dictionary<string, QuestionCondition> _questionConditions;
    private List<Question> _startTopics = new List<Question>();

    public bool InConversation => _state != null;

    public DialogManager(Dialog dialog, GlobalCtx globalCtx)
    {
        _dialog = dialog;
        _globalCtx = globalCtx;
        _topics = _dialog.Topics.ToDictionary(it => it.Id, it => it);
        _questionConditions = _dialog.QuestionConditions.ToDictionary(it => it.QuestionId, it => it);
        EvaluateTopics();
    }

    private void EvaluateTopics()
    {
        var nonStartTopics = new HashSet<string>();
        foreach (var topic in _dialog.Topics)
        {
            switch (topic)
            {
                case Answer ea:
                    nonStartTopics.Add(topic.Id);
                    foreach (var next in ea.NextTopicIds)
                    {
                        nonStartTopics.Add(next);
                    }
                    break;
                case AnswerSwitch c:
                    nonStartTopics.Add(topic.Id);
                    foreach (var next in c.Cases)
                    {
                        nonStartTopics.Add(next.NextId);
                    }
                    break;
                case DialogAction a:
                    nonStartTopics.Add(topic.Id);
                    break;
                case QuestionCondition qc:
                    nonStartTopics.Add(topic.Id);
                    if (!qc.Evaluate(_globalCtx))
                        nonStartTopics.Add(qc.QuestionId);
                    break;
            }
        }
        _startTopics = _topics
            .Where(it => !nonStartTopics.Contains(it.Key))
            .Select(it => it.Value as Question)
            .OrderBy(it => it.Text)
            .ToList();
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
            if (_questionConditions.ContainsKey(nextTopicId) && !_questionConditions[nextTopicId].Evaluate(_globalCtx))
                continue;
            yield return GetTopic(nextTopicId) as Question;
        }
    }

    public Answer PlayerSays(Question question)
    {
        var state = GetAnswer(question);
        _state = state;
        if (_state == null)
            return null;

        if (_state.NextTopicIds.Count == 0)
            _state = null;

        if (state.Action != null)
        {
            var script = (GetTopic(state.Action) as DialogAction)?.Script ?? "";
            if (script != "")
            {
                _globalCtx.Evaluate(script);
                EvaluateTopics();
                OnStartTopicsUpdate();
            }
        }

        return state;
    }

    private Answer GetAnswer(Topic question)
    {
        if (question == null)
            return null;
        return question switch
        {
            Question q => GetAnswer(GetTopic(q.AnswerId)),
            Answer ea => ea,
            AnswerSwitch c => GetAnswer(GetTopic(c.Evaluate(_globalCtx).NextId)),
            _ => throw new System.Exception(),
        };
    }

    private Topic GetTopic(string id)
    {
        return _topics.ContainsKey(id) ? _topics[id] : null;
    }
}