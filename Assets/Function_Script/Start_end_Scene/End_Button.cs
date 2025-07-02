using UnityEngine;
using UnityEngine.SceneManagement;

public class End_Button : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
