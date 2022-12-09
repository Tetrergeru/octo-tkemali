using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 1)]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<Item> _items;

    public IEnumerable<Item> Items
    {
        get
        {
#if UNITY_EDITOR
            _items = AssetDatabase.FindAssets($"t:{typeof(Item).Name}").
                Select(AssetDatabase.GUIDToAssetPath).
                Select(AssetDatabase.LoadAssetAtPath<Item>).
                ToList();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)));
#endif
            return _items;
        }
    }
}