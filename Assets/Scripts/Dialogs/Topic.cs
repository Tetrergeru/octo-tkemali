using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Topic
{
    public string Id;
    public string Answer;
    public List<Question> Questions = new List<Question>();
    public Vector2 Position;

    [Serializable]
    public struct Question
    {
        public string Text;
        public string NextTopicId;
    }
}
