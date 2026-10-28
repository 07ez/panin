using UnityEngine;

public class ReverseAnim : MonoBehaviour
{
    public int gravity = 1;
    public GameObject[] reverseobj;
    Animator[] Anim;
    void Start()
    {
        Anim = new Animator[reverseobj.Length];

        for (int i = 0; i < reverseobj.Length; i++)
        {
            Anim[i] = reverseobj[i].GetComponent<Animator>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            gravity *= -1;
            for (int i = 0; i < reverseobj.Length; i++)
            {
                Anim[i].SetInteger("Back", gravity);
                reverseobj[i].GetComponent<ReverseAnim>().gravity = gravity;   
            }
        }
    }
}
