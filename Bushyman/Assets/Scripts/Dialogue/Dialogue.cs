using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public AudioClip talksfx;
    public Animation talkAnim;
    public Sprite idleSprite;

    [TextArea(3,10)]
    public string[] sentences;
}
