using UnityEngine;
using TMPro; 

public class StartTextFlicker : MonoBehaviour
{
    private TextMeshProUGUI startText;
    public float flickerSpeed = 1.5f; 

    void Awake()
    {
        startText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (startText == null) return;

        float alpha = Mathf.PingPong(Time.time * flickerSpeed, 1f);
        Color color = startText.color;
        color.a = alpha;
        startText.color = color;
    }
}