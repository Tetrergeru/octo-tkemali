using System.Text;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UniqueId : MonoBehaviour
{
    public string Id;

    void Start()
    {
        if (Id == null || Id == "")
            RegenerateId();
    }

    public void RegenerateId()
    {
        Id = RandomId(10);
    }

    public static string RandomId(int length)
    {
        const string symbols = "0123456789abcdef";
        var res = new StringBuilder();
        for (var i = 0; i < length; i++)
            res.Append(symbols[Random.Range(0, symbols.Length)]);
        return res.ToString();
    }
}
