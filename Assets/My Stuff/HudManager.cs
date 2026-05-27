using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HudManager : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scaleText;

    public FlexibleColorPicker bkgColorInput;
    public FlexibleColorPicker spColorInput;
    public FlexibleColorPicker pColorInput;

    [SerializeField] SpriteRenderer bkgSprite;
    ParticleSystem particles;

    [NonSerialized] public Material spriteMat;
    SpriteRenderer sprite;

    public string input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //has to be in start because flyfollower needs to run its awake first to set up the sprite renderer and material references
        FlyFollower flyFollower = FindAnyObjectByType<FlyFollower>();
        sprite = flyFollower.sp;
        particles = flyFollower.particles;

        bkgColorInput.onColorChange.AddListener(delegate { bkgColorChanged(); });
        spColorInput.onColorChange.AddListener(delegate { spColorChanged(); });
        pColorInput.onColorChange.AddListener(delegate { pColorChanged(); });
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

    public void bkgColorChanged() {
        bkgSprite.color = bkgColorInput.color;
    }

    public void spColorChanged() {
        spriteMat.SetColor("_TintColor", spColorInput.color);
    }

    public void pColorChanged() {
        var main = particles.main;
        main.startColor = pColorInput.color;
    }
}
