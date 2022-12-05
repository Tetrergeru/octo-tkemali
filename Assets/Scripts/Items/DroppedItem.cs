using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DroppedItem : MonoBehaviour
{
    public Inventory.QuantifiedItem Item;

    [SerializeField]
    private GameObject Object;

#if UNITY_EDITOR
    void Start()
    {
        InstantiateIfNecessary();
    }

    void Update()
    {
        InstantiateIfNecessary();
    }
#endif

    public void InstantiateIfNecessary()
    {
        if (Item.Item != null)
        {
            if (Object == null)
            {
                Object = (GameObject)PrefabUtility.InstantiatePrefab(Item.Item.Prefab);
                Object.transform.parent = this.transform;
                Object.transform.localPosition = new Vector3();
                foreach (var transform in Object.GetComponentsInChildren<Transform>())
                    transform.tag = "Activator";
                EditorUtility.SetDirty(this);
            }
        }
        else
        {
            DestroyImmediate(Object);
        }
    }
}
