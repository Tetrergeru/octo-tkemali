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

        var addAnswerButton = new Button(() => _graphView.AddAnswerNode());
        addAnswerButton.text = "Add Answer";
        toolbar.Add(addAnswerButton);

        var addQuestionButton = new Button(() => _graphView.AddQuestionNode());
        addQuestionButton.text = "Add Question";
        toolbar.Add(addQuestionButton);

        var addActionButton = new Button(() => _graphView.AddActionNode());
        addActionButton.text = "Add Action";
        toolbar.Add(addActionButton);

        var addConditionButton = new Button(() => _graphView.AddAnswerSwitchNode());
        addConditionButton.text = "Add Answer Switch";
        toolbar.Add(addConditionButton);

        var addQuestionConditionButton = new Button(() => _graphView.AddQuestionConditionNode());
        addQuestionConditionButton.text = "Add Question Condition";
        toolbar.Add(addQuestionConditionButton);

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
