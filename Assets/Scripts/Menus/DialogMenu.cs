using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenu : MonoBehaviour
{
    public GameObject CharacterName;
    public GameObject DialogContent;
    public GameObject DialogView;
    public GameObject TopicsContent;

    private float _dialogContentOffset;
    private Dialog _currentDialog;
    private int _numberOfButtons;
    private bool _choiceRequired;

    public void LoadDialog(Dialog dialog, string npcName)
    {
        _currentDialog = dialog;

        var npcNameText = CharacterName.GetComponent<TextMeshProUGUI>();
        npcNameText.text = npcName;
        npcNameText.outlineColor = Color.black;
        npcNameText.outlineWidth = 0.2f;

        var prevTopics = new Dictionary<string, string>();
        foreach (var topic in _currentDialog.Topics)
        {
            foreach (var question in topic.Questions)
            {
                prevTopics[question.NextTopicId] = topic.Id;
            }
        }

        foreach (var topic in _currentDialog.Topics)
        {
            if (prevTopics.ContainsKey(topic.Id))
                continue;
            foreach (var question in topic.Questions)
                RenderQuestionButton(question);
        }
    }

    private void RenderQuestionButton(Topic.Question question)
    {
        var panel = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));
        panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var colorChanger = panel.AddComponent<OnHoverChangeColor>();
        colorChanger.DefaultColor = new Color(1, 1, 1, 0.1f);
        colorChanger.HoveredtColor = new Color(1, 1, 1, 0.5f);

        var button = panel.AddComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (_choiceRequired) return;
            AskQuestion(question);
        });

        var textObject = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer));
        textObject.transform.SetParent(panel.transform);

        var text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = question.Text;
        text.fontSize = 20;
        text.color = Color.black;
        text.verticalAlignment = VerticalAlignmentOptions.Middle;

        TopicsContent.GetComponent<ListComponent>().AddElement(panel);
    }

    private void AskQuestion(Topic.Question question)
    {
        AddText(question.Text, Color.blue);
        var topic = GetTopicById(question.NextTopicId);
        AddText(topic.Answer, Color.black);

        _choiceRequired = topic.Questions.Count != 0;

        foreach (var nextQuestion in topic.Questions)
        {
            AddChoiceButton(nextQuestion);
        }

        DialogView.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    private void AddText(string text, Color color)
    {
        var textObject = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));

        var textMesh = textObject.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.fontSize = 20;
        textMesh.color = color;
        textMesh.verticalAlignment = VerticalAlignmentOptions.Middle;

        DialogContent.GetComponent<ListComponent>().AddElement(textObject);
    }

    private void AddChoiceButton(Topic.Question question)
    {
        _numberOfButtons++;

        var panel = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));
        panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var colorChanger = panel.AddComponent<OnHoverChangeColor>();
        colorChanger.DefaultColor = new Color(1, 1, 1, 0.1f);
        colorChanger.HoveredtColor = new Color(1, 1, 1, 0.5f);

        var button = panel.AddComponent<Button>();
        button.onClick.AddListener(() =>
        {
            RemoveButtons();
            AskQuestion(question);
        });

        var textObject = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer));
        textObject.transform.SetParent(panel.transform);

        var textMesh = textObject.AddComponent<TextMeshProUGUI>();
        textMesh.text = question.Text;
        textMesh.fontSize = 20;
        textMesh.color = Color.black;
        textMesh.verticalAlignment = VerticalAlignmentOptions.Middle;

        DialogContent.GetComponent<ListComponent>().AddElement(panel);
    }

    private void RemoveButtons()
    {
        for (var i = 0; i < _numberOfButtons; i++)
        {
            DialogContent.GetComponent<ListComponent>().DeleteLastElement();
        }
        _numberOfButtons = 0;
    }

    private Topic GetTopicById(string id)
        => _currentDialog.Topics.Find(it => it.Id == id);
}
