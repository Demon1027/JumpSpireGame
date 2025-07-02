using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager_Script_Emergency : MonoBehaviour
{
    public AudioSource Quake_Sound;
    public AudioSource Crush_Sound;
    public AudioSource Jump_Sound;
    public void Quake_Sound_Effect(float time)
    {
        StartCoroutine(Start_Quake_Sound_Effect(time));
    }

    private IEnumerator Start_Quake_Sound_Effect(float time)
    {
        Quake_Sound.Play();
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(QuakeStopAfterSeconds(Quake_Sound, time));
    }
    private IEnumerator QuakeStopAfterSeconds(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        source.Stop();
    }

    public void Crush_Sound_Effect()
    {
        Crush_Sound.Play();
    }
    public void Jump_Sound_Effect()
    {
        Jump_Sound.Play();
    }
}
