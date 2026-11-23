using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    // 자동차 오브젝트
    GameObject carObject;

    // 깃발 오브젝트
    GameObject flagObject;

    // 거리 표시용 UI
    GameObject distanceTextObject;

    // 자동차와 깃발 사이의 거리
    float distanceLength = 0.0f;

    // 게임 시작 시 한 번 실행
    void Start()
    {
        // 자동차 오브젝트 찾기
        carObject = GameObject.Find("car");

        // 깃발 오브젝트 찾기
        flagObject = GameObject.Find("flag");

        // 거리 표시 UI 찾기
        distanceTextObject = GameObject.Find("txtDistance");

        // 오브젝트가 제대로 찾아졌는지 확인
        Debug.Log("car = " + carObject);
        Debug.Log("flag = " + flagObject);
        Debug.Log("txtDistance = " + distanceTextObject);
    }

    // 매 프레임마다 실행
    void Update()
    {
        // 자동차의 X 좌표 가져오기
        float carX = carObject.transform.position.x;

        // 깃발의 X 좌표 가져오기
        float flagX = flagObject.transform.position.x;

        // 자동차와 깃발 사이의 거리 계산
        distanceLength = flagX - carX;

        // TextMeshPro 컴포넌트 가져오기
        TMP_Text distanceText =
            distanceTextObject.GetComponent<TMP_Text>();

        // 계산된 거리를 화면에 표시
        // F2 = 소수점 둘째 자리까지 표시
        distanceText.text =
            "자동차에서 깃발까지의 거리는 "
            + distanceLength.ToString("F2")
            + "m";
    }
}