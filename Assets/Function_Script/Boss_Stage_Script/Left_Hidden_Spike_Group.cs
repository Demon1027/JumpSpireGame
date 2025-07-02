using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left_Hidden_Spike_Group : MonoBehaviour
{
    [Header("�θ� ������ �ִ� �ڽ� Spike��")]
    public GameObject[] Crush_Spikes;

    [Header("Spike �̵� �ӵ�")]
    public float speed = 16f;

    [Header("Spike�� ���ư� �ִ� �Ÿ�")]
    public float travelDistance = 100f;

    [Header("��ǥ ���� ���� �� ����ġ�� ���ư��� �� ��� �ð�")]
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
            Debug.LogWarning($"[{spikeObj.name}]�� Rigidbody2D�� �����ϴ�.");
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
