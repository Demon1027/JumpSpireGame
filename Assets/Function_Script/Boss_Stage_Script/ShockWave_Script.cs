using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave_Script : MonoBehaviour
{
    public Boss Boss_Object;
    public GameObject[] Wave_Spikes;
    public float travelDistance = 1.75f;
    public float duration = 0.2f;
    private Vector3[] originalLocalPositions;
    private void Awake()
    {
        originalLocalPositions = new Vector3[Wave_Spikes.Length];
        for (int i = 0; i < Wave_Spikes.Length; i++)
        {
            originalLocalPositions[i] = Wave_Spikes[i].transform.localPosition;

            Wave_Spikes[i].SetActive(false);
        }
    }

    public void ShockWave_Spike(bool Big_ShockWave)
    {
        for (int i = 0; i < Wave_Spikes.Length; i++)
        {
            StartCoroutine(Move_ShockWave_Spike(Boss_Object.transform, i,Big_ShockWave));
        }
    }

    private IEnumerator Move_ShockWave_Spike(Transform Boss_Pos, int index, bool Big_SW)
    {
        int Big_Effect_Dis = 1;
        if (Big_SW == true)
        {
            Big_Effect_Dis = 2;
        }
        else
        {
            Big_Effect_Dis = 1;
        }
        GameObject spikeObj = Wave_Spikes[index];
        Transform spikeTf = spikeObj.transform;
        float Dis = Boss_Pos.transform.position.x - spikeTf.position.x;
        if (Dis < 0)
        {
            Dis *= -1;
        }
        Rigidbody2D rb = spikeObj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            yield break;
        }
        yield return new WaitForSeconds(Dis / 15);
        spikeObj.SetActive(true);

        Vector3 startPos = spikeTf.position;
        Vector3 targetPos = startPos + ((Vector3.up * Big_Effect_Dis) * travelDistance);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            spikeTf.position = Vector3.Lerp(startPos, targetPos, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            spikeTf.position = Vector3.Lerp(targetPos, startPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        spikeTf.position = startPos;
    }
}
