using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scaleText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //canvas = FindAnyObjectByType<Canvas>(); //doesn't work for some reason, maybe because it's disabled at start?
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ToggleCanvas() {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
        return canvas.gameObject.activeSelf;
    }

    public void UpdateSpeed(float speed) {
        speedText.text = "Movement Speed (U and I keys): " + speed.ToString("F2");
    }

    public void UpdateScale(float scale) {
        scaleText.text = "Scale Speed (J and K keys): " + scale.ToString("F2");
    }
}
