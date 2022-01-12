using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public AudioSource talksfx;
    public Sprite[] talkSprite;
    public Sprite idleSprite;

    [TextArea(3,10)]
    public string[] sentences;
}
