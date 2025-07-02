using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark_Emergency : MonoBehaviour
{
    public GameObject[] Emergency_Marks;

    private List<SpriteRenderer[]> markRenderersList = new List<SpriteRenderer[]>();

    public float Delay_Time = 0.2f;

    private void Awake()
    {
        foreach (GameObject markGroup in Emergency_Marks)
        {
            SpriteRenderer[] renderers = markGroup.GetComponentsInChildren<SpriteRenderer>(true);
            markRenderersList.Add(renderers);
        }
    }

    public void Start_Emergency_Mark_Coroutine(int Direction)
    {
        StartCoroutine(Mark(Delay_Time, Direction));
    }

    private IEnumerator Mark(float delay, int Dic)
    {
        int Up_Count = 0;
        if (Dic <= 0 || Dic > markRenderersList.Count)
        {
            Debug.LogWarning("잘못된 방향 값: " + Dic);
            yield break;
        }

        SpriteRenderer[] renderers = markRenderersList[Dic - 1];

        if (renderers.Length == 0)
        {
            Debug.LogError($"[{Emergency_Marks[Dic - 1].name}] 안에 SpriteRenderer가 없습니다!");
            yield break;
        }

        if(Dic == 1)
        {
            Up_Count = 1;
        }

        for (int i = 0; i < 2 + Up_Count; i++)
        {
            foreach (var sr in renderers)
                sr.enabled = true;

            yield return new WaitForSeconds(delay);

            foreach (var sr in renderers)
                sr.enabled = false;

            yield return new WaitForSeconds(delay);
        }

        Up_Count = 0;
    }
}
