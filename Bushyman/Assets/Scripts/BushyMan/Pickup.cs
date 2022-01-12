using UnityEngine;
using TMPro;

public class Pickup : MonoBehaviour
{
    private int Leafs = 0;
    public TMP_Text LeafText;
    public Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        LeafText.text = Leafs.ToString(); ;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Leaf"))
        {
            Leafs++;
            //anim.Play("LeafUI");
            Destroy(trigger.gameObject);
        }
    }
}
