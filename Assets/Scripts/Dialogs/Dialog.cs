using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<Answer> Answers = new List<Answer>();
    public List<Question> Questions = new List<Question>();
    public List<AnswerSwitch> AnswerSwitches = new List<AnswerSwitch>();
    public List<DialogAction> DialogActions = new List<DialogAction>();
    public List<QuestionCondition> QuestionConditions = new List<QuestionCondition>();

    public void Clear()
    {
        Answers = new List<Answer>();
        Questions = new List<Question>();
        AnswerSwitches = new List<AnswerSwitch>();
        DialogActions = new List<DialogAction>();
        QuestionConditions = new List<QuestionCondition>();
    }

    public void Add(Topic topic)
    {
        switch (topic)
        {
            case Answer v:
                Answers.Add(v);
                break;
            case Question v:
                Questions.Add(v);
                break;
            case AnswerSwitch v:
                AnswerSwitches.Add(v);
                break;
            case DialogAction v:
                DialogActions.Add(v);
                break;
            case QuestionCondition v:
                QuestionConditions.Add(v);
                break;
        }
    }

    public IEnumerable<Topic> Topics
    {
        get
        {
            foreach (var v in Answers) yield return v;
            foreach (var v in Questions) yield return v;
            foreach (var v in AnswerSwitches) yield return v;
            foreach (var v in DialogActions) yield return v;
            foreach (var v in QuestionConditions) yield return v;
        }
    }

#if UNITY_EDITOR
    public void Persist()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)));
    }
#endif
}
