using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public AudioClip talksfx;

    [Space(10)]
    [Header("Sprites")]
    public Sprite talkSpite;
    public Sprite idleSprite;

    public float talkSpeed;

    [Space(10)]
    [Header("Sentences")]
    [TextArea(3,10)]
    public string[] sentences;

    [Space(10)]
    [Header("Requirement check")]
    public bool hasRequirement;
    public int reqCollectables;
    [TextArea(3, 10)]
    public string[] collectSentences;
}
