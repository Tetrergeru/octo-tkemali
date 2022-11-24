using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class DialogGraph : EditorWindow
{
    private Dialog _targetObject;

    private DialogGraphView _graphView;

    [MenuItem("Graph/Dialog Graph")]
    public static void OpenDialogGraphWindow()
    {
        var window = GetWindow<DialogGraph>();
        window.titleContent = new UnityEngine.GUIContent("Dialog Graph");
    }

    public void Load(Dialog targetObject)
    {
        _targetObject = targetObject;
        _graphView.Load(_targetObject);
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

        var saveButton = new Button(() => _graphView.Save(_targetObject));
        saveButton.text = "Save";
        toolbar.Add(saveButton);

        rootVisualElement.Add(toolbar);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
        _graphView = null;
    }
}
