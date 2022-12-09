using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    [SerializeField]
    private GameObject _itemPrefab;

    void OnSceneDrag(SceneView sceneView, int index)
    {
        var item = target as Item;
        var e = Event.current;

        if (e.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            e.Use();
        }
        else if (e.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();
            e.Use();
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            var position = Physics.Raycast(ray, out var hit, 1000.0f)
                ? hit.point
                : ray.GetPoint(1);

            var Object = (GameObject)PrefabUtility.InstantiatePrefab(_itemPrefab);
            Object.transform.position = position;
            Object.name = item.Name;

            var droppedItem = Object.GetComponent<DroppedItem>();
            droppedItem.Item = new Inventory.QuantifiedItem(item, 1);
            droppedItem.InstantiateIfNecessary();
        }
    }
}