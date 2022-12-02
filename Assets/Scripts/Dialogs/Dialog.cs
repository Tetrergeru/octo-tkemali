using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<ExactAnswer> ExactAnswers = new List<ExactAnswer>();
    public List<Question> Questions = new List<Question>();
    public List<Condition> Conditions = new List<Condition>();
    public List<DialogAction> DialogActions = new List<DialogAction>();

    public void Clear()
    {
        ExactAnswers = new List<ExactAnswer>();
        Questions = new List<Question>();
        Conditions = new List<Condition>();
        DialogActions = new List<DialogAction>();
    }

    public void Add(ITopic topic)
    {
        switch (topic) 
        {
            case ExactAnswer v:
                ExactAnswers.Add(v);
                break;
            case Question v:
                Questions.Add(v);
                break;
            case Condition v:
                Conditions.Add(v);
                break;
            case DialogAction v:
                DialogActions.Add(v);
                break;
        }
    }

    public IEnumerable<ITopic> Topics {
        get {
            foreach(var v in ExactAnswers) yield return v;
            foreach(var v in Questions) yield return v;
            foreach(var v in Conditions) yield return v;
            foreach(var v in DialogActions) yield return v;
        }
    }
}
