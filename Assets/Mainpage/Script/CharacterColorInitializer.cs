using UnityEngine;

public class CharacterColorInitializer : MonoBehaviour
{
    [Header("캐릭터 부위 스프라이트 렌더러들")]
    public SpriteRenderer[] characterParts; // 여기에 모든 캐릭터 부위의 SpriteRenderer를 연결

    void Start()
    {
        LoadAllCharacterColors();
    }

    void LoadAllCharacterColors()
    {
        if (characterParts == null || characterParts.Length == 0)
        {
            Debug.LogWarning("Character parts array is empty or not assigned. Please assign SpriteRenderers to the characterParts array in the Inspector.");
            // 만약 배열에 직접 할당하기 어렵다면, 태그나 이름으로 찾을 수도 있습니다.
            // 예: characterParts = FindObjectsOfType<SpriteRenderer>(); // 모든 SpriteRenderer를 찾음 (비효율적일 수 있음)
            // 또는 특정 부모 아래의 자식들을 찾아서 필터링
            // characterParts = GetComponentsInChildren<SpriteRenderer>();
            return;
        }

        foreach (SpriteRenderer part in characterParts)
        {
            if (part == null) continue; // null 체크

            string keyPrefix = "CharColor_" + part.gameObject.name + "_";

            if (PlayerPrefs.HasKey(keyPrefix + "H"))
            {
                float h = PlayerPrefs.GetFloat(keyPrefix + "H");
                float s = PlayerPrefs.GetFloat(keyPrefix + "S");
                float v = PlayerPrefs.GetFloat(keyPrefix + "V");
                part.color = Color.HSVToRGB(h, s, v);
            }
            else
            {
                Debug.Log($"No saved color found for {part.gameObject.name}. Using default color.");
                // 저장된 색상이 없으면, 해당 SpriteRenderer의 기본 색상을 유지합니다.
            }
        }
    }

    // (선택 사항) 저장된 모든 색상 데이터를 초기화하는 함수
    public void ClearAllSavedColors()
    {
        // PlayerPrefs.DeleteAll(); // 모든 PlayerPrefs 데이터를 삭제 (주의해서 사용)
        // 특정 키만 삭제하려면:
        // foreach (SpriteRenderer part in characterParts)
        // {
        //     if (part == null) continue;
        //     string keyPrefix = "CharColor_" + part.gameObject.name + "_";
        //     PlayerPrefs.DeleteKey(keyPrefix + "H");
        //     PlayerPrefs.DeleteKey(keyPrefix + "S");
        //     PlayerPrefs.DeleteKey(keyPrefix + "V");
        // }
        // PlayerPrefs.Save();
        // Debug.Log("All character colors cleared from PlayerPrefs.");
    }
}