using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GlobalCtx
{
    public Dictionary<string, string> Data = new Dictionary<string, string>();
    public Dictionary<string, Item> AllItems;
    public Inventory PlayerInventory;

    public GlobalCtx(Inventory playerInventory)
    {
        PlayerInventory = playerInventory;
        AllItems = AssetDatabase.FindAssets($"t:{typeof(Item).Name}").
            Select(AssetDatabase.GUIDToAssetPath).
            Select(AssetDatabase.LoadAssetAtPath<Item>).
            ToDictionary(it => it.Id, it => it);
    }

    public string Evaluate(string expr)
    {
        return new Evaluator(expr, this).Evaluate();
    }

    public string EvaluateExpression(string expr)
    {
        return new Evaluator(expr, this).EvaluateExpression();
    }

    public string Get(string name) => Data.ContainsKey(name) ? Data[name] : "";
    public string Set(string name, string value) => Data[name] = value;
}
