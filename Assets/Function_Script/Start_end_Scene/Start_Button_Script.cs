using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Button_Script : MonoBehaviour
{
    private AudioSource AS;
    public AudioClip AC;
    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }

    public void OnStartButtonClicked()
    {
        AS.PlayOneShot(AC);
        Invoke("GoNextScene", 0.5f);
    }

    private void GoNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
