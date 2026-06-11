using UnityEngine;

[System.Serializable]
public class ChatAnswer
{
    public string answerText;
    public float lovometerPoints;
}

[System.Serializable]
public class ChatNode
{
    [TextArea(2, 4)]
    public string npcMessage;
    public ChatAnswer[] answers;
    public bool isLastNode;
}