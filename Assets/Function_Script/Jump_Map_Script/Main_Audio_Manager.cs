using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Audio_Manager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource Jump;
    public AudioSource Next;
    private AudioClip JAC;
    private AudioClip NAC;
    private void Awake()
    {
        NAC = GetComponent<AudioClip>();
        JAC = GetComponent<AudioClip>();
        JAC = Jump.clip;
        NAC = Next.clip;
    }
    private void Start()
    {
        StartCoroutine(Wait_BGM_Play());
    }

    private IEnumerator Wait_BGM_Play()
    {
        yield return new WaitForSeconds(0.7f);
        BGM.Play();
    }

    public void Jump_Sound()
    {
        Jump.PlayOneShot(JAC);
    }
    public void Next_sound()
    {
        Next.PlayOneShot(NAC);
    }
}
