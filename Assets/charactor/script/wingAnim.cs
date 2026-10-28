using UnityEngine;

public class wingAnim : MonoBehaviour
{
    Animator anim;
    public Move Move;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        anim.SetBool("Isfly", !Move.isGrounded);
    }
}
