using UnityEngine;

public class DummyHit : MonoBehaviour
{
    public Animator anim;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("PlayerHitbox"))
        {
            Debug.Log("got hit");
            anim.SetTrigger("Spin");

        }
    }
}