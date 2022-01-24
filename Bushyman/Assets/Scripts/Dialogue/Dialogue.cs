using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public AudioClip talksfx;

    public Sprite talkSpite;
    public Sprite idleSprite;

    public float talkSpeed;

    [TextArea(3,10)]
    public string[] sentences;

    public bool hasRequirement;
    public int reqCollectables;
    [TextArea(3, 10)]
    public string[] collectSentences;
}
