using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SetMode(string mode)
    {
        PlayerPrefs.SetString("gameMode", mode);
    }
}
