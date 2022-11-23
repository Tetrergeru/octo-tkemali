using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class DialogGraph : EditorWindow
{
    public Dialog TargetObject;
    
    private DialogGraphView _graphView;

    [MenuItem("Graph/Dialog Graph")]
    public static void OpenDialogGraphWindow()
    {
        var window = GetWindow<DialogGraph>();
        window.titleContent = new UnityEngine.GUIContent("Dialog Graph");
    }

    private void OnEnable()
    {
        MakeGraphView();
        MakeToolbar();
    }

    private void MakeGraphView()
    {
        _graphView = new DialogGraphView
        {
            name = "Dialog Graph ff",
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void MakeToolbar()
    {
        var toolbar = new Toolbar();

        var makeNodeButton = new Button(() => _graphView.AddNode());
        makeNodeButton.text = "Add Node";

        toolbar.Add(makeNodeButton);

        rootVisualElement.Add(toolbar);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
        _graphView = null;
    }
}
