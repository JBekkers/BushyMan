using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RichTextSubstringHelper;

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
    private bool isAnimationPlaying;

    private Queue<string> sentences;
    private GameObject player;

    [Header("Dialogue Box")]
    public TMP_Text dialogueText;
    public Image NpcSprite;
    public Pickup pickup;

    [Space(10)]
    public bool completeSentence;
    public bool isSentence;

    [Space(10)]
    [Header("Dialogue animations")]
    public Animator dialogueboxAnim;
    public Animator talkSpriteAnim;
    public AudioSource talkingSfx;

    [Space(10)]
    [Header("Input")]
    //## DIALOGUE SYTEM ##
    //public DialogueTrigger testsomeshit;
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    private RaycastHit hit;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sentences = new Queue<string>();
    }

    private void Update()
    {
        GetInput();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isTalking = true;
        sentences.Clear();

        dialogueboxAnim.SetBool("IsOpen", true);
        NpcSprite.sprite = dialogue.idleSprite;
        talkingSfx.clip = dialogue.talksfx;

        checkIfRequired(dialogue);

        StartCoroutine(CheckAnimation());
    }

    private IEnumerator CheckAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        isAnimationPlaying = false;

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
        isSentence = true;
        talkingSfx.loop = true;

        //Debug.Log(sentence);
    }

    private IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        var richText = sentence;
        var maker = new RichTextSubStringMaker(richText);

        while (maker.IsConsumable())
        {
            if (completeSentence) { dialogueText.text = sentence; break; }

            maker.Consume();
            dialogueText.text = maker.GetRichText();
            yield return new WaitForSeconds(0f);
        }

        talkingSfx.loop = false;
        isSentence = false;
        completeSentence = false;
        talkSpriteAnim.SetBool("isTalking", false);
    }

    public bool isDialogue()
    {
        return isTalking;
    }
    private void EndDialogue()
    {
        dialogueboxAnim.SetBool("IsOpen", false);
        isTalking = false;
        //Debug.Log("end");
    }

    public void checkIfRequired(Dialogue dialogue)
    {
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
    }

    public void GetInput()
    {         //###### INTERACTION WITH NPC #######
        //Debug.DrawRay(player.transform.position + offset, player.transform.forward, Color.green, 5);

        if (!isTalking && Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(player.transform.position + offset, player.transform.forward, out hit, 5) && hit.transform.CompareTag("NPC"))
            {
                //Debug.Log("hit object " + hit.transform.name);
                isAnimationPlaying = true;
                hit.transform.gameObject.GetComponent<DialogueTrigger>().TriggerdDialogue();
            }
        }
        else if (isTalking && Input.GetKeyDown(KeyCode.E) && !isSentence && !isAnimationPlaying)
        { 
            DisplayNextSentence();
        }
        else if (isTalking && Input.GetKeyDown(KeyCode.E) && isSentence && !isAnimationPlaying)
        {
            completeSentence = true;
        }
    }
}
