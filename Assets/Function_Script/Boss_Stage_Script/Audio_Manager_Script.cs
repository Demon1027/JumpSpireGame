using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager_Script : MonoBehaviour
{
    public AudioSource Main_BGM;
    public AudioSource Hidden_BGM;
    public AudioSource Crush_Sound;
    public AudioSource Jump_Sound;
    public AudioSource Coin_Sound;
    public AudioSource Quake_Sound;
    public AudioSource Boss_Hit_Sound;
    public AudioSource Gameover_Sound;
    public AudioSource Player_Hit_Sound;
    public AudioSource Boss_Cub_die_Sound;
    public AudioSource Loop_Quake_sound;
    public AudioSource Clear_Sound;

    private void Start()
    {
        StartCoroutine(Start_Main_BGM());
    }

    private IEnumerator Start_Main_BGM()
    {
        yield return new WaitForSeconds(0.8f);
        Main_BGM.Play();
    }
    public void Stop_Main_BGM()
    {
        Main_BGM.Stop();
    }

    public void Start_Hidden_BGM()
    {
        StartCoroutine(C_Start_Hidden_BGM());
    }
    public void Stop_Hidden_BGM()
    {
        Hidden_BGM.Stop();
    }
    private IEnumerator C_Start_Hidden_BGM()
    {
        yield return new WaitForSeconds(0.8f);
        Quake_Sound.Play();
        Quake_Sound.loop = true;
        Hidden_BGM.Play();
    }

    public void Coin_Sound_Effect()
    {
        StartCoroutine(Start_Coin_Sound_Effect());
    }

    private IEnumerator Start_Coin_Sound_Effect()
    {
        Coin_Sound.Play();
        yield return new WaitForSeconds(0.01f);
    }

    public void Jump_Sound_Effect()
    {
        StartCoroutine(Start_Jump_Sound_Effect());
    }

    private IEnumerator Start_Jump_Sound_Effect()
    {
        Jump_Sound.Play();
        yield return new WaitForSeconds(0.01f);
    }

    public void Crush_Sound_Effect()
    {
        StartCoroutine(Start_Crush_Sound_Effect());
    }

    private IEnumerator Start_Crush_Sound_Effect()
    {
        Crush_Sound.time = 0.3f;
        Crush_Sound.Play();
        yield return new WaitForSeconds(0.001f);
        StartCoroutine(StopAfterSeconds(Crush_Sound, 0.25f));
    }

    private IEnumerator StopAfterSeconds(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        source.Stop();
    }

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

    public void GameOver_Sound_Effect()
    {
        Gameover_Sound.Play();
    }

    public void Boss_Hit_Sound_Effect()
    {
        Boss_Hit_Sound.Play();
    }

    public void Player_Hit_Sound_Effect()
    {
        Player_Hit_Sound.Play();
    }

    public void Boss_Cub_Die_Sound_Effect()
    {
        Boss_Cub_die_Sound.Play();
    }

    public void Loop_Quake_Sound_Effect()
    {
        Loop_Quake_sound.Play();
    }

    public void Stop_Loop_Quake_Sound_Effect()
    {
        Loop_Quake_sound.Stop();
    }

    public void Clear_Sound_Effect()
    {
        Clear_Sound.Play();
    }
}
