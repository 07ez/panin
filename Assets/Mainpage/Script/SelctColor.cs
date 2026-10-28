using UnityEngine;

public class SelctColor : MonoBehaviour
{
    [Header("플레이어")]
    public SpriteRenderer[] Charater;

    public HsvColorChanger colorChanger;
    string[] Charatername =
    {
        "head1",
        "hair1",
        "body1",
        "arm1",
        "pants1",
        "head2",
        "hair2",
        "body2",
        "arm2",
        "pants2",
    };

    bool[] a = new bool[10];

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < Charatername.Length; i++)
            {
                if (a[i])
                {
                    colorChanger.targetSpriteRenderer = Charater[i];
                }
            }
        }    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < Charatername.Length; i++)
        {
            if (other.gameObject.name == Charatername[i])
            {
                a[i] = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        for (int i = 0; i < Charatername.Length; i++)
        {
            if (other.gameObject.name == Charatername[i])
            {
                a[i] = false;
            }
        }
    }
}
