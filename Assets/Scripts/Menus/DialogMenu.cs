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
    private DialogManager _currentDialog;
    private int _numberOfButtons;
    private ListComponent _startTopics;

    public void LoadDialog(Dialog dialog, string npcName, GlobalCtx globalCtx)
    {
        _startTopics = TopicsContent.GetComponent<ListComponent>();
        _currentDialog = new DialogManager(dialog, globalCtx);

        var npcNameText = CharacterName.GetComponent<TextMeshProUGUI>();
        npcNameText.text = npcName;
        npcNameText.outlineColor = Color.black;
        npcNameText.outlineWidth = 0.2f;

        RerenderStartTopics();
        _currentDialog.OnStartTopicsUpdate = () => RerenderStartTopics();
    }

    private void RerenderStartTopics()
    {
        _startTopics.DestroyAll();
        foreach (var question in _currentDialog.StartTopics())
        {
            RenderQuestionButton(question);
        }
    }

    private void RenderQuestionButton(Question question)
    {
        var panel = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));
        panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var colorChanger = panel.AddComponent<OnHoverChangeColor>();
        colorChanger.DefaultColor = new Color(1, 1, 1, 0.1f);
        colorChanger.HoveredtColor = new Color(1, 1, 1, 0.5f);

        var button = panel.AddComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (_currentDialog.InConversation)
            {
                return;
            }
            AskQuestion(question);
        });

        var textObject = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer));
        textObject.transform.SetParent(panel.transform);

        var text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = question.Text;
        text.fontSize = 20;
        text.color = Color.black;
        text.verticalAlignment = VerticalAlignmentOptions.Middle;

        _startTopics.AddElement(panel);
    }

    private void AskQuestion(Question question)
    {
        AddText(question.Text, Color.blue);
        var topic = _currentDialog.PlayerSays(question);
        AddText(topic.Text, Color.black);

        foreach (var nextQuestion in _currentDialog.WhatToSay())
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

    private void AddChoiceButton(Question question)
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
}
