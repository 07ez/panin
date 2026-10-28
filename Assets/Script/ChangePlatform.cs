using UnityEngine;

public class ChangePlatform : MonoBehaviour
{
    public GameObject Tile;
    public GameObject BlankTile;
    public GameObject Button;

    void Start()
    {
        Tile.SetActive(false);
        BlankTile.SetActive(true);
        Button.SetActive(true);
    }

    public void ResetMap()
    {
        Tile.SetActive(false);
        BlankTile.SetActive(true);
        Button.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Tile.SetActive(true);
            BlankTile.SetActive(false);
            Button.SetActive(false);
        }
    }

}