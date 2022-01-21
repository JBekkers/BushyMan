using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int Hp;
    public int numhearts;
    private int maxHp;

    public Animator anim;
    private float waitForTimer;

    private int lifes = 3;
    public TMP_Text lifeText;
    public AudioSource gameOverSfx;
    private bool gameOver;

    public Image[] hearts;
    public Sprite fullheart;
    public Sprite emptyyheart;

    private float invincibilityTimer = 0;

    private void Start()
    {
        SetHealth();
        UpdateHealth();
        UpdateLifes();
    }

    void Update()
    {
        if (invincibilityTimer > 0) { invincibilityTimer -= Time.deltaTime; }

        if (waitForTimer >= 0) { waitForTimer -= Time.deltaTime; }
        if (waitForTimer <= 0) { anim.SetBool("isOpen", false); }

        if (anim.GetBool("isOpen") == true && DialogueManager.instance.isDialogue() == true) { anim.SetBool("isOpen", false); }

        if (Hp < 0 && lifes > 0)
        {
            lifes--;
            UpdateLifes();
            SetHealth();
        } 
        
        if (lifes <= 0 && !gameOver)
        {
            GameOver();
        }
    }

    private void UpdateHealth() 
    {     
        //hp controller and hearts display
        if (Hp > numhearts)
        {
            Hp = numhearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {

            if (i < Hp)
            {
                hearts[i].sprite = fullheart;
            }
            else
            {
                hearts[i].sprite = emptyyheart;
            }

            if (i < numhearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void UpdateLifes()
    {
        lifeText.text = "x " + lifes.ToString();
    }

    private void SetHealth()
    {
        Hp = 9;
        numhearts = 9;
        maxHp = numhearts;
        Hp = maxHp;
        UpdateHealth();
    }

    private void TakeDamage()
    {
        Hp--;
        UpdateHealth();
        //take knockback
    }

    private void GameOver()
    {
        //Debug.Log("GameOver");
        gameOverSfx.Play();
        gameOver = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit coll)
    {
        if (coll.gameObject.CompareTag("Enemy") || coll.gameObject.CompareTag("MapHazard"))
        {
            if (invincibilityTimer <= 0)
            {
                anim.SetBool("isOpen", true);
                waitForTimer = 4;
                invincibilityTimer = 2;
                TakeDamage();
            }
        }

        if (coll.gameObject.CompareTag("Life"))
        {
            lifes++;
        }
    }
}
