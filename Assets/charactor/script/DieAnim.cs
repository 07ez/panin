using System.Collections;
using UnityEngine;

public class DieAnim : MonoBehaviour
{
    Vector3 finalPosition; // 마지막에 이동할 위치

    // 컴포넌트
    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        // 컴포넌트 받아오기
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Die()
    {
        FinalPos();
        StartCoroutine(DieAnimation());
    }

    void FinalPos()
    {
        string PlayerName = gameObject.name;
        GameObject StartposObj = GameObject.Find("StartPos" + PlayerName);
        finalPosition = StartposObj.transform.position;
    }

    IEnumerator DieAnimation()
    {
        anim.SetBool("IsDie", true); // 죽는 애니메이션 활성화
        rb.simulated = false;        // RigidBody2D 끄기

        Vector3 startPos = transform.position;      // 첫 시작 위치
        Vector3 upPos = startPos + Vector3.up * 5;  // 처음 올라가기

        // 1초 동안 위로 이동
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            if (rb.simulated) // rb가 켜져있을때
            {
                rb.simulated = false;            // 다시 RigidBody2D 끄기
            }
            transform.position = Vector3.Lerp(startPos, upPos, t);
            yield return null;
        }

        // 1초 동안 처음 위치로 이동
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            if (rb.simulated) // rb가 켜져있을때
            {
                rb.simulated = false;            // 다시 RigidBody2D 끄기
            }
            transform.position = Vector3.Lerp(upPos, finalPosition, t);
            yield return null;
        }

        // 도착 후 딱 맞게 위치 조정
        transform.position = finalPosition;
        
        if (!rb.simulated) // rb가 꺼져있을때
        {
            rb.simulated = true;            // 다시 RigidBody2D 켜기
        }
        anim.SetBool("IsDie", false);   // 애니메이션 비활성화
    }
}
