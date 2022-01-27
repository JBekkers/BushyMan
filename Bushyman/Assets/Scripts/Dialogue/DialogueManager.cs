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
    private bool completeSentence;
    private bool isSentence;

    [Space(10)]
    [Header("Dialogue animations")]
    public Animator dialogueboxAnim;
    public AudioSource talkingSfx;

    [Space(10)]
    [Header("Animated sprites")]
    private List<Sprite> sprites;
    private int spritePerFrame = 1;
    private bool isTalkingAnimation;

    private int index = 0;
    private int frame = 0;

    private Sprite idleSprite;

    [Space(10)]
    [Header("Input")]
    //## DIALOGUE SYTEM ##
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

        if (isTalkingAnimation) { animateSprites(); }
        else NpcSprite.sprite = idleSprite;

    }

    public void StartDialogue(Dialogue dialogue)
    {
        //when first talking to a npc set all the sentences in a queue and set all the needed sprites/soundeffects of the npc your talking to
        spritePerFrame = dialogue.talkSpeed;
        isTalking = true;
        sentences.Clear();

        dialogueboxAnim.SetBool("IsOpen", true);
        setSprites(dialogue);
        idleSprite = dialogue.idleSprite;
        talkingSfx.clip = dialogue.talksfx;

        checkIfRequired(dialogue);

        StartCoroutine(CheckAnimation());
    }

    private void setSprites(Dialogue dialogue) 
    {
        //set the sprites from the npc as the sprites that will be animated
        sprites = new List<Sprite>();

        for (int i = 0; i < dialogue.talkSprites.Length; i++)
        {
            sprites.Add(dialogue.talkSprites[i]);
        }
    }

    private IEnumerator CheckAnimation()
    {
        //check if the dialogue box sliding in animation is done playing
        yield return new WaitForSeconds(1.5f);
        isAnimationPlaying = false;

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

        //remove old sentence from queue and display a new sentence
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        //enable talking soundeffect and play sprite talking animation
        isTalkingAnimation = true;
        talkingSfx.Play();
        isSentence = true;
        talkingSfx.loop = true;
    }

    private IEnumerator TypeSentence (string sentence)
    {
        //show every letter one by one and display it in the text field
        dialogueText.text = "";
        var subStringMaker = new RichTextSubStringMaker(sentence);

        while (subStringMaker.IsConsumable())
        {
            subStringMaker.Consume();
            dialogueText.text = subStringMaker.GetRichText();

            if (completeSentence) { dialogueText.text = sentence; break; }
            yield return new WaitForSeconds(0);
        }

        //stop the talk animation from playing and stop all the talk soundeffect
        isTalkingAnimation = false;
        talkingSfx.loop = false;
        isSentence = false;
        completeSentence = false;
    }

    public bool isDialogue()
    {
        return isTalking;
    }
    private void EndDialogue()
    {
        //end of the dialogue (plays dialogue box close animation)
        dialogueboxAnim.SetBool("IsOpen", false);
        isTalking = false;
        sprites.Clear();
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
    {         
        //###### START INTERACTION WITH NPC #######

        if (!isTalking && Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(player.transform.position + offset, player.transform.forward, out hit, 5) && hit.transform.CompareTag("NPC"))
            {
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

    public void animateSprites()
    {
        //adds frame, if enough frames passed display next sprites in list
        //if the end of list is reached start from the beginning

        frame++;

        if (frame > spritePerFrame)
        {
            NpcSprite.sprite = sprites[index];
            frame = 0;
            index++;
        }

        if (index >= sprites.Count)
        {
            index = 0;
        }
    }
}
