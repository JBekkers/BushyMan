using UnityEngine;
using TMPro;

public class Pickup : MonoBehaviour
{
    private int Leafs = 0;
    public TMP_Text LeafText;
    public Animator anim;
    private float waitForPickup;

    private void Update()
    {
        //waitforpickup is a float that counts down to keep the UI menu down for a set amount of seconds
        //if there is no leaf picked up within those set amount of seconds it will animate back off screen again
        if (waitForPickup >= 0) { waitForPickup -= Time.deltaTime; }
        if(waitForPickup <= 0) { anim.SetBool("isOpen", false); }

        LeafText.text = Leafs.ToString(); ;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Leaf"))
        {
            Leafs++;
            anim.SetBool("isOpen",true);
            waitForPickup = 4;
            Destroy(trigger.gameObject);
        }
    }
}
