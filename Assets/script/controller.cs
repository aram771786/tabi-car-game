

using UnityEngine;
using UnityEngine.EventSystems;

public class controller : MonoBehaviour
{
    [Header("자동차 이동")]
    public float moveCarSpeed = 0.0f;
    public float swipeToSpeedRate = 100.0f;

    // 자동차의 감속 비율
    public const float DecelerationRate = 0.96f;

    // 마우스를 처음 누른 위치
    Vector3 StartPos;

    // 게임 시작 여부
    bool hasSwiped = false;

    // 마우스를 UI 위에서 눌렀는지 확인
    bool isPointerStartedOnUI = false;


    [Header("게임 설정")]
    public Transform flag;

    public GameObject winPanel;
    public GameObject gameOverPanel;

    // 자동차와 깃발 사이의 승리 거리
    public float winRange = 0.5f;

    // 게임 종료 여부
    bool gameFinished = false;


    [Header("시작 제한 시간")]
    public float startTimeLimit = 3.0f;

    float remainingTime;


    [Header("힌트 설정")]
    public HintManager hintManager;


    [Header("자동차 사운드")]
    public AudioSource audioSource;
    public AudioClip carStartSound;


    void Start()
    {
        Application.targetFrameRate = 60;

        // 시간 정상화
        Time.timeScale = 1.0f;

        // 시작 제한 시간 설정
        remainingTime = startTimeLimit;

        // 게임 결과창 숨기기
        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 깃발 자동 찾기
        if (flag == null)
        {
            GameObject flagObject = GameObject.Find("flag");

            if (flagObject != null)
                flag = flagObject.transform;
        }

        // 힌트 매니저 자동 찾기
        if (hintManager == null)
            hintManager = FindObjectOfType<HintManager>();

        // 자동차 오디오 소스 찾기
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // UI 위에서 누른 입력도 기록해야 하므로
        // HandleInput은 매 프레임 호출해야 함!
        HandleInput();

        // 게임 종료 시 더 이상 진행하지 않음
        if (gameFinished)
            return;

        // 힌트창이 열려 있으면 게임 정지
        if (hintManager != null && hintManager.IsHintOpen)
            return;


        // 자동차 출발 전 제한 시간
        if (!hasSwiped)
        {
            remainingTime -= Time.deltaTime;

            // 제한 시간 초과 시 게임오버
            if (remainingTime <= 0.0f)
            {
                GameOver();
            }

            return;
        }


        // 자동차 이동
        if (Mathf.Abs(moveCarSpeed) > 0.01f)
        {
            transform.Translate(
                Vector3.right * moveCarSpeed * Time.deltaTime
            );

            // 자동차 감속
            moveCarSpeed *= DecelerationRate;
        }
        else
        {
            moveCarSpeed = 0.0f;

            // 자동차가 멈추면 결과 확인
            CheckResult();
        }
    }


    // 마우스 입력 처리
    void HandleInput()
    {
        bool isOverUI = false;

        if (EventSystem.current != null)
        {
            isOverUI =
                EventSystem.current.IsPointerOverGameObject();
        }


        // 마우스를 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // UI 위에서 눌렀는지 기록
            isPointerStartedOnUI = isOverUI;

            // UI 위에서 눌렀다면 게임 입력으로 처리하지 않음
            if (isPointerStartedOnUI)
                return;

            StartPos = Input.mousePosition;
        }


        // 마우스를 뗐을 때
        if (Input.GetMouseButtonUp(0))
        {
            // UI에서 시작한 클릭이면 무시
            if (isPointerStartedOnUI || isOverUI)
            {
                isPointerStartedOnUI = false;
                return;
            }

            isPointerStartedOnUI = false;

            // 이미 자동차가 출발했다면 무시
            if (hasSwiped)
                return;

            // 게임이 끝났다면 무시
            if (gameFinished)
                return;

            // 힌트창이 열려 있다면 무시
            if (hintManager != null && hintManager.IsHintOpen)
                return;


            // 마우스를 뗀 위치
            Vector3 endPos = Input.mousePosition;

            // 드래그한 거리 계산
            float swipeLength = endPos.x - StartPos.x;

            // 자동차 속도 계산
            moveCarSpeed = swipeLength / swipeToSpeedRate;

            // 오른쪽으로 드래그한 경우에만 출발
            if (moveCarSpeed > 0.01f)
            {
                hasSwiped = true;

                // 자동차 출발 소리
                PlayCarSound();

                Debug.Log("자동차 출발!");
            }
        }
    }


    // 자동차 출발 사운드
    void PlayCarSound()
    {
        if (audioSource != null && carStartSound != null)
        {
            audioSource.PlayOneShot(carStartSound);
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 자동차 사운드가 연결되지 않았습니다.");
        }
    }


    // 승리 여부 확인
    void CheckResult()
    {
        if (flag == null)
        {
            Debug.LogError("깃발 오브젝트를 찾을 수 없습니다.");
            GameOver();
            return;
        }

        float distance =
            Mathf.Abs(flag.position.x - transform.position.x);

        Debug.Log("깃발까지 거리: " + distance);

        if (distance <= winRange)
        {
            Win();
        }
        else
        {
            GameOver();
        }
    }


    // 승리
    void Win()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        Debug.Log("게임 승리!");

        if (winPanel != null)
            winPanel.SetActive(true);
    }


    // 게임오버
    void GameOver()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        Debug.Log("게임 오버!");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
