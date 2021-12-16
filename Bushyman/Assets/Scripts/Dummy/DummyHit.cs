using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHit : MonoBehaviour
{
    public Animator anim;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerHitbox"))
        {
            anim.Play("SpinAnimation");
        }
    }
}
