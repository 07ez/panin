using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGO : MonoBehaviour
{
    public GameObject Exit;
    public TextMeshPro ExitText;

    bool exitbool;

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && exitbool)
            SceneManager.LoadScene("SelectLevel");

    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.name == "Exit")
        {
            ExitText.fontSize = 9;
            ExitText.color = Color.yellow;   
            exitbool = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Exit")
        {
            ExitText.fontSize = 7;
            ExitText.color = Color.black;
            exitbool = false;
        }
    }

}
