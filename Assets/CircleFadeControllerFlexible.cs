using UnityEngine;
using System.Collections;

public class CircleFadeControllerFlexible : MonoBehaviour
{
    public enum FadeType { In, Out }

    public Material fadeMaterial;
    public Transform playerTransform;
    public float duration = 2f;

    private float fadeStart;
    private float fadeEnd;

    public void Set_On() => gameObject.SetActive(true);
    public void Set_Off() => gameObject.SetActive(false);

    public void StartFade(FadeType type)
    {
        Vector2 center = Camera.main.WorldToViewportPoint(playerTransform.position);
        fadeMaterial.SetVector("_Center", center);

        if (type == FadeType.In)
        {
            // 중심부터 밝아짐 (흑색 → 게임화면)
            fadeMaterial.SetFloat("_Invert", 0f);
            fadeStart = 1.5f;
            fadeEnd = -0.2f;
        }
        else // FadeType.Out
        {
            // 중심부터 어두워짐 (게임화면 → 흑색)
            fadeMaterial.SetFloat("_Invert", 0f);
            fadeStart = -0.2f;
            fadeEnd = 1.5f;
        }

        fadeMaterial.SetFloat("_Cutoff", fadeStart);
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float t = 0f;
        while (t < 1f)
        {
            float cutoff = Mathf.Lerp(fadeStart, fadeEnd, t);
            fadeMaterial.SetFloat("_Cutoff", cutoff);
            t += Time.deltaTime / duration;
            yield return null;
        }

        fadeMaterial.SetFloat("_Cutoff", fadeEnd);
    }
}