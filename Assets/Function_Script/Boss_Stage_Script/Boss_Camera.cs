using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Camera : MonoBehaviour
{
    public Audio_Manager_Script AMS;
    private Camera CM;
    private Vector3 originalPos;
    private void Awake()
    {
        originalPos = transform.localPosition;
        CM = GetComponent<Camera>();
    }
    public void Set_Boss_Camera_Off()
    {
        CM.enabled = false;
    }
    public void Set_Boss_Camera_On()
    {
        CM.enabled = true;
    }

    public void Up_Down_Camera_Effect(float duration, float magnitude)
    {
        StopAllCoroutines();
        AMS.Crush_Sound_Effect();
        originalPos = transform.localPosition;
        StartCoroutine(DampedShake(Vector2.up, duration, magnitude));
    }

    private IEnumerator DampedShake(Vector2 direction, float duration, float magnitude)
    {
        AMS.Quake_Sound_Effect(duration);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float damper = 1f - (elapsed / duration);

            float strength = Mathf.Sin(elapsed * 40f) * magnitude * damper;

            Vector3 offset = new Vector3(direction.x * strength, direction.y * strength, 0f);

            transform.localPosition = originalPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    public IEnumerator Shake(float E_Time, float scale)
    {
        AMS.Quake_Sound_Effect(E_Time);
        Vector3 Original_Position = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < E_Time)
        {
            float FixedX = Random.Range(-1, 1) * scale;
            float FixedY = Random.Range(-1, 1) * scale;

            transform.localPosition = Original_Position + new Vector3(FixedX, FixedY, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = Original_Position;
    }


}
