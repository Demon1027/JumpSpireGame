using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left_Hidden_Spike_Group : MonoBehaviour
{
    [Header("부모가 가지고 있는 자식 Spike들")]
    public GameObject[] Crush_Spikes;

    [Header("Spike 이동 속도")]
    public float speed = 16f;

    [Header("Spike가 날아갈 최대 거리")]
    public float travelDistance = 100f;

    [Header("목표 지점 도달 후 원위치로 돌아가기 전 대기 시간")]
    public float returnDelay = 0.5f;

    private Vector3[] originalLocalPositions;

    private void Awake()
    {
        originalLocalPositions = new Vector3[Crush_Spikes.Length];
        for (int i = 0; i < Crush_Spikes.Length; i++)
        {
            originalLocalPositions[i] = Crush_Spikes[i].transform.localPosition;

            Crush_Spikes[i].SetActive(false);
        }
    }

    public void Shoot_Spike()
    {
        for (int i = 0; i < Crush_Spikes.Length; i++)
        {
            StartCoroutine(MoveSpikeWithMovePosition(i));
        }
    }

    private IEnumerator MoveSpikeWithMovePosition(int index)
    {
        GameObject spikeObj = Crush_Spikes[index];
        Transform spikeTf = spikeObj.transform;

        Rigidbody2D rb = spikeObj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning($"[{spikeObj.name}]에 Rigidbody2D가 없습니다.");
            yield break;
        }

        spikeObj.SetActive(true);

        Vector3 dir = spikeTf.up.normalized;

        Vector3 startLocalPos = originalLocalPositions[index];
        Vector3 targetLocalPos = startLocalPos + dir * travelDistance;

        Vector2 startWorldPos = spikeTf.parent.TransformPoint(startLocalPos);
        Vector2 targetWorldPos = spikeTf.parent.TransformPoint(targetLocalPos);

        while (Vector2.Distance(rb.position, targetWorldPos) > 0.01f)
        {
            Vector2 newPos = Vector2.MoveTowards(
                rb.position,
                targetWorldPos,
                speed * Time.fixedDeltaTime
            );
            rb.MovePosition(newPos);
            yield return new WaitForFixedUpdate();
        }
        rb.MovePosition(targetWorldPos);

        yield return new WaitForSeconds(returnDelay);

        spikeTf.localPosition = startLocalPos;

        rb.position = startWorldPos;

        spikeObj.SetActive(false);
    }
}
