using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //## singleton ## 
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

    public Image NpcSprite;
    public Pickup pickup;
    public bool completeSentence;

    public Animator dialogueboxAnim;
    public Animator talkSpriteAnim;

    public AudioSource talkingSfx;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isTalking = true;
        sentences.Clear();

        dialogueboxAnim.SetBool("IsOpen", true);
        NpcSprite.sprite = dialogue.idleSprite;
        talkingSfx.clip = dialogue.talksfx;

        // check if the npc has a required leafs check
        //if true check if requirement is met then display lines if not display normal lines
        if (!dialogue.hasRequirement)
        {
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
        } 
        else if (dialogue.hasRequirement)
        {
            if (pickup.getGoldenLeafs() < dialogue.reqCollectables)
            {
                foreach (string sentence in dialogue.sentences)
                {
                    sentences.Enqueue(sentence);
                }
            }
            else if (pickup.getGoldenLeafs() >= dialogue.reqCollectables)
            {
                foreach (string sentence in dialogue.collectSentences)
                {
                    sentences.Enqueue(sentence);
                }
            }
        }

        StartCoroutine(CheckAnimation());
    }

    private IEnumerator CheckAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        //Debug.Log("Starting to animate text");
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

        talkSpriteAnim.SetBool("isTalking", true);

        talkingSfx.Play();
        talkingSfx.loop = true;

        //Debug.Log(sentence);
    }

    private IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        string newsentence = sentence;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;

            if (completeSentence) { sentence = newsentence; break; }
        }
        talkingSfx.loop = false;
        talkSpriteAnim.SetBool("isTalking", false);
    }

    private void EndDialogue()
    {
        dialogueboxAnim.SetBool("IsOpen", false);
        isTalking = false;
        //Debug.Log("end");
    }

    public bool isDialogue()
    {
        //Debug.Log(isTalking);
        return isTalking;
    }
}
