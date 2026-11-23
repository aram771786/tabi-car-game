
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public GameObject hintPopup;

    // 현재 힌트창이 열려 있는지 확인
    public bool IsHintOpen { get; private set; }

    void Start()
    {
        hintPopup.SetActive(false);
        IsHintOpen = false;
    }

    // 힌트 버튼 클릭
    public void OpenHint()
    {
        hintPopup.SetActive(true);
        IsHintOpen = true;
    }

    // 힌트창 닫기
    public void CloseHint()
    {
        hintPopup.SetActive(false);
        IsHintOpen = false;
    }

    // 팝업 바깥을 클릭했을 때
    public void ClickOutside()
    {
        if (IsHintOpen)
        {
            CloseHint();
        }
    }
}
