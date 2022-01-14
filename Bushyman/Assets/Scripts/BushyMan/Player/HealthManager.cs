using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private int Hp = 9;
    private int numhearts = 9;

    public Image[] hearts;
    public Sprite fullheart;
    public Sprite emptyyheart;

    private float invincibilityTimer = 0;

    void Update()
    {
        if (invincibilityTimer > 0) { invincibilityTimer -= Time.deltaTime; }


        if (Hp < 1)
        {
            Debug.Log("lose one life");
            //have a different check to see if you are out of lifes and then gameover
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

    private void TakeDamage()
    {
        Hp--;
        UpdateHealth();
        //take knockback
    }

    private void OnControllerColliderHit(ControllerColliderHit coll)
    {
        if (coll.gameObject.CompareTag("Enemy") || coll.gameObject.CompareTag("MapHazard"))
        {
            if(invincibilityTimer <= 0)
            {
                invincibilityTimer = 2;
                TakeDamage();
            }
        }
    }
}
