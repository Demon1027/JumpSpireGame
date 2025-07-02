using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Boss_Stage_Camera : MonoBehaviour
{
    Vector3 fixedCamPos;
    private Camera cam;
    public Audio_Manager_Script AMS;
    public Boss_Stage_Player BSP;
    private Vector3 baseLocalPos;
    bool Stop_pos = false;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        baseLocalPos = transform.localPosition;
    }
    private void LateUpdate()
    {
        if (Stop_pos == true)
        {
            transform.position = transform.position = fixedCamPos;
        }
    }
    public void Up_Down_Camera_Effect(float duration, float magnitude)
    {
        StopAllCoroutines();
        AMS.Crush_Sound_Effect();
        StartCoroutine(DampedShake(Vector2.up, duration, magnitude));
    }
    public void Left_Camera_Effect(float duration, float magnitude)
    {
        StopAllCoroutines();
        AMS.Crush_Sound_Effect();
        StartCoroutine(DampedShake(Vector2.right, duration, magnitude));
    }

    public void Right_Camera_Effect(float duration, float magnitude)
    {
        StopAllCoroutines();
        AMS.Crush_Sound_Effect();
        StartCoroutine(DampedShake(Vector2.left, duration, magnitude));
    }

    private IEnumerator DampedShake(Vector2 direction, float duration, float magnitude)
    {
        AMS.Quake_Sound_Effect(duration);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float damper = 1f - (elapsed / duration);
            float strength = Mathf.Sin(elapsed * 40f) * magnitude * damper;
            transform.localPosition = baseLocalPos + (Vector3)direction * strength;

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = baseLocalPos;
    }

    public IEnumerator Shake(float duration, float scale)
    {
        AMS.Quake_Sound_Effect(duration);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * scale;
            float y = Random.Range(-1f, 1f) * scale;
            transform.localPosition = baseLocalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = baseLocalPos;
    }

    public void Set_Camera_Off() => cam.enabled = false;
    public void Set_Camera_On() => cam.enabled = true;
    public void Set_Camera_Pos_Stop()
    {
        Stop_pos = true;
        fixedCamPos = new Vector3(BSP.transform.position.x, BSP.transform.position.y, transform.position.z);
    }
}
