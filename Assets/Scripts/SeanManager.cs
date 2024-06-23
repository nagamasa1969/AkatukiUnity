using UnityEngine;
using UnityEngine.SceneManagement;

public class SeanManager : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void OnclickEndButton()
    {
        SceneManager.LoadScene("Main");
    }
}
