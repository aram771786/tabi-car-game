using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리 네임스페이스 추가 필수

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        // 이동할 씬의 정확한 이름을 적습니다.
        SceneManager.LoadScene("game"); 
    }
}
