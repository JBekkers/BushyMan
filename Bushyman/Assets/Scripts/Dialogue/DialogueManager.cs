using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //singleton
    public static DialogueManager instance { get; private set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool isTalking;

    private Queue<string> sentences;
    public TMP_Text dialogueText;

    public Image npcSprite;
    public Animator anim;

    public AudioSource talkingSfx;
    private bool canDisplay;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        anim.SetBool("IsOpen", true);

        sentences.Clear();
        npcSprite.sprite = dialogue.idleSprite;
        talkingSfx.clip = dialogue.talksfx;
        isTalking = true;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);

        DisplayNextSentence();

    }


    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            dialogueText.text = "";
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        talkingSfx.Play();
        talkingSfx.loop = true;

        //Debug.Log(sentence);
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";


        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;        
        }

        talkingSfx.loop = false;
    }

    void EndDialogue()
    {
        anim.SetBool("IsOpen", false);
        isTalking = false;
        //Debug.Log("end");
    }

    public bool isDialogue()
    {
        //Debug.Log(isTalking);
        return isTalking;
    }
}
