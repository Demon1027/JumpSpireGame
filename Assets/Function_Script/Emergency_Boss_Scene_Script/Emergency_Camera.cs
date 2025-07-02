using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emergency_Camera : MonoBehaviour
{
    public Sound_Manager_Script_Emergency SMSE;
    public Camera Main_C;
    public GameObject Boss;
    public Camera Boss_C;
    public IEnumerator Shake(float E_Time,float scale)
    {
        Vector3 Original_Position = transform.localPosition;

        float elapsed = 0.0f;

        while(elapsed < E_Time)
        {
            float FixedX = Random.Range(-1, 1) * scale;
            float FixedY = Random.Range(-1, 1) * scale;

            transform.localPosition = Original_Position + new Vector3(FixedX, FixedY,-10);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = Original_Position;
    }


    private void Start()
    {
        Invoke("Camara_Shake", 4.5f);
        Invoke("OnBoss", 7.5f);
    }

    void Camara_Shake()
    {
        SMSE.Quake_Sound_Effect(2);
        StartCoroutine(Camera.main.GetComponent<Emergency_Camera>().Shake(2f, 0.35f));
    }

    void OnBoss()
    {
        Boss.SetActive(true);
        Main_C.enabled = false;
        Boss_C.enabled = true;
    }
}
