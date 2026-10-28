using UnityEngine;

public class ClearStage : MonoBehaviour
{ // 귀찮으니 주석 안써야지
    public GameObject[] Live;
    public GameObject[] Die;
    public GameObject[] Line;

    public GameObject Logo;
    public GameObject LogoClear;

    int ClearedStage = 0;

    void Start()
    {
        ClearedStage = PlayerPrefs.GetInt("ClearedStage", ClearedStage);
        Debug.Log("ClearedStage" + ClearedStage);
        if(ClearedStage == 12) // 게임 클리어
        {
            Debug.Log("Clear");
            Logo.SetActive(false);
            LogoClear.SetActive(true);
            for (int i = 0; i < ClearedStage; i++)
            {
                Live[i].SetActive(true);
                Line[i].SetActive(true);
                Die[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i <= ClearedStage; i++)
            {
                Live[i].SetActive(true);
                Line[i].SetActive(true);
                Die[i].SetActive(false);
            }    
        }
    }
    public void Clear(int Stage)
    {
        if (ClearedStage < Stage)
        {
            ClearedStage = Stage;
            PlayerPrefs.SetInt("ClearedStage",ClearedStage);
            PlayerPrefs.Save();
            Live[Stage].SetActive(true);
            Line[Stage].SetActive(true);
            Die[Stage].SetActive(false);
        }
    }
}
