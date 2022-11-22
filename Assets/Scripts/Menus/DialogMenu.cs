using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenu : MonoBehaviour
{
    public GameObject CharacterName;
    public GameObject DialogContent;
    public GameObject TopicsContent;

    private float _dialogContentOffset;

    public void LoadDialog(Dialog dialog, string npcName)
    {
        var npcNameText = CharacterName.GetComponent<TextMeshProUGUI>();
        npcNameText.text = npcName;
        npcNameText.outlineColor = Color.black;
        npcNameText.outlineWidth = 0.2f;

        foreach (var topic in dialog.Topics)
        {
            var panel = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

            var colorChanger = panel.AddComponent<OnHoverChangeColor>();
            colorChanger.DefaultColor = new Color(1, 1, 1, 0.1f);
            colorChanger.HoveredtColor = new Color(1, 1, 1, 0.5f);

            var button = panel.AddComponent<Button>();
            button.onClick.AddListener(() =>
            {
                {
                    var textObject = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));

                    var text = textObject.AddComponent<TextMeshProUGUI>();
                    text.text = topic.Query;
                    text.fontSize = 20;
                    text.color = Color.blue;
                    text.verticalAlignment = VerticalAlignmentOptions.Middle;

                    DialogContent.GetComponent<ListComponent>().AddElement(textObject);
                }
                {
                    var textObject = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));

                    var text = textObject.AddComponent<TextMeshProUGUI>();
                    text.text = topic.Response;
                    text.fontSize = 20;
                    text.color = Color.black;
                    text.verticalAlignment = VerticalAlignmentOptions.Middle;
                    DialogContent.GetComponent<ListComponent>().AddElement(textObject);
                }
            });

            var textObject = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer));
            textObject.transform.SetParent(panel.transform);

            var text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = topic.Query;
            text.fontSize = 20;
            text.color = Color.black;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;

            TopicsContent.GetComponent<ListComponent>().AddElement(panel);
        }
    }

    private void RenderTopics()
    {

    }
}
