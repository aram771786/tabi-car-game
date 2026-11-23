using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    // 홈 화면으로 돌아가기
    public void GoHome()
    {
        SceneManager.LoadScene("Main");
    }
}