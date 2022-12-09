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
#if UNITY_EDITOR
                Object = (GameObject)PrefabUtility.InstantiatePrefab(Item.Item.Prefab);
#else
                Object = Instantiate(Item.Item.Prefab);
#endif
                Object.transform.parent = this.transform;
                Object.transform.localPosition = new Vector3();
                foreach (var transform in Object.GetComponentsInChildren<Transform>())
                    transform.tag = "Activator";

#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }
        else
        {
            DestroyImmediate(Object);
        }
    }
}
