using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scaleText;
    public TMP_InputField colorInput;

    [SerializeField] SpriteRenderer bkgSprite;

    public string input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    public void colorTextChanged() {
        input = colorInput.text;
        if (input.Length == 0 || input.Length % 6 != 0) {
            return;
        }

        input = "#" + colorInput.text; // Add the '#' character to the beginning of the input

        Color newColor;

        if (ColorUtility.TryParseHtmlString(input, out newColor)) {
            bkgSprite.color = newColor;
        }
    }
}
