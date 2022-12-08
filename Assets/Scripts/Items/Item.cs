using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public string Name;

    public ItemType Type;

    public float Weight;

    public float Price;

    public string Id;

    public GameObject Prefab;

    void Awake()
    {
        if (Id == null || Id == "")
            Id = UniqueId.RandomId(10);
        EditorUtility.SetDirty(this);
    }

    public bool Consume(PlayerController playerController)
    {
        if (Type == ItemType.Food)
        {
            switch (Id)
            {
                case "apple":
                    playerController.PlayerAttributes.Fatigue += 10;
                    return true;
                case "invisibility_potion":
                    var visibility = playerController.GetComponent<PlayerVisibility>();
                    visibility.InvisibilityTime = visibility.MaxInvisibilityTime;
                    return true;
            }
        }

        return false;
    }
}