using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden_State_Camera_Shake : MonoBehaviour
{
    Vector3 originalPos;
    Coroutine quakeRoutine;
    public Audio_Manager_Script AMS;

    private void Awake() => originalPos = transform.localPosition;

    public void StartQuake(float magnitude, float interval)
    {
        AMS.Loop_Quake_Sound_Effect();
        if (quakeRoutine != null) return;
        quakeRoutine = StartCoroutine(QuakeLoop(magnitude, interval));
    }

    public void StopQuake()
    {
        if (quakeRoutine == null) return;
        StopCoroutine(quakeRoutine);
        quakeRoutine = null;
        transform.localPosition = originalPos;
    }

    private IEnumerator QuakeLoop(float mag, float interval)
    {
        while (true)
        {
            Vector2 offset = Random.insideUnitCircle * mag;
            transform.localPosition = originalPos + (Vector3)offset;
            yield return new WaitForSeconds(interval);
        }
    }
}
