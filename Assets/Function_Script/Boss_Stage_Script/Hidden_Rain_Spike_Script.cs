using System.Collections;
using UnityEngine;

public class Hidden_Rain_Spike_Script : MonoBehaviour
{
    [Header("스파이크 오브젝트들")]
    public GameObject[] Rain_Spikes;

    [Header("리스폰 위치 범위")]
    public float groundY = 63.5f;
    public float minX = -11.5f;
    public float maxX = 19.5f;
    public float minY = 90f;
    public float maxY = 150.5f;

    public void Start_SpikeRespawnLoop()
    {
        foreach (var spike in Rain_Spikes)
        {
            spike.SetActive(true);

            var rb = spike.GetComponent<Rigidbody2D>();
    
            RespawnAtTop(spike, rb);

            StartCoroutine(SpikeFallLoop(spike, rb));
        }
    }

    private void RespawnAtTop(GameObject spike, Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
        spike.transform.position = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }

    private IEnumerator SpikeFallLoop(GameObject spike, Rigidbody2D rb)
    {
        while (true)
        {
            while (spike.transform.position.y > groundY)
                yield return null;

            yield return new WaitForFixedUpdate();

            RespawnAtTop(spike, rb);

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
    }

    public void StopAllSpikes()
    {
        StopAllCoroutines();
        foreach (var spike in Rain_Spikes)
            spike.SetActive(false);
    }
}
