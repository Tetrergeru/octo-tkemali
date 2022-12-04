using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<Answer> ExactAnswers = new List<Answer>();
    public List<Question> Questions = new List<Question>();
    public List<AnswerSwitch> Conditions = new List<AnswerSwitch>();
    public List<DialogAction> DialogActions = new List<DialogAction>();

    public void Clear()
    {
        ExactAnswers = new List<Answer>();
        Questions = new List<Question>();
        Conditions = new List<AnswerSwitch>();
        DialogActions = new List<DialogAction>();
    }

    public void Add(Topic topic)
    {
        switch (topic)
        {
            case Answer v:
                ExactAnswers.Add(v);
                break;
            case Question v:
                Questions.Add(v);
                break;
            case AnswerSwitch v:
                Conditions.Add(v);
                break;
            case DialogAction v:
                DialogActions.Add(v);
                break;
        }
    }

    public IEnumerable<Topic> Topics
    {
        get
        {
            foreach (var v in ExactAnswers) yield return v;
            foreach (var v in Questions) yield return v;
            foreach (var v in Conditions) yield return v;
            foreach (var v in DialogActions) yield return v;
        }
    }

    public void Persist()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)));
    }
}
