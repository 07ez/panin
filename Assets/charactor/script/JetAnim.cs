using UnityEngine;

public class JetAnim : MonoBehaviour
{
    Animator anim;
    public Move Move;
    float Timer = 0f;
    bool isJetJumping = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Move.JetJump)
        {
            anim.SetBool("IsJump", true);
            isJetJumping = true;
            Timer = 0f; // 타이머 초기화
        }

        if (isJetJumping)
        {
            Timer += Time.deltaTime;
            if (Timer >= 0.1f)
            {
                anim.SetBool("IsJump", false);
                isJetJumping = false;
            }
        }
    }
}
