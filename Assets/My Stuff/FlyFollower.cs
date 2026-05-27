using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FlyFollower : MonoBehaviour
{
    public float moveSmoothness = 0.2f;
    public float scaleSmoothness = 0.1f;

    public Vector2 scrollRange = new Vector2(0.1f, 1f);
    public int scrollSteps = 5;

    public Sprite[] sprites; 

    public Material GreyscaleMat;
    Material originalMat;

    [SerializeField] bool defaultGrey = true;

    int spriteIndex = 0;

    Vector2 prevMousePosition;
    Vector3 targetMousePos;

    public SpriteRenderer sp;
    public ParticleSystem particles;

    Vector3 targetScale;

    HudManager hudManager;
    InputField[] hexInputs;

    void Awake()
    {
        Cursor.visible = false;

        targetScale = gameObject.transform.localScale;
        sp = GetComponent<SpriteRenderer>();

        //hud manager setup
        hudManager = FindAnyObjectByType<HudManager>();
        hexInputs = new InputField[] {
            hudManager.bkgColorInput.GetComponentInChildren<InputField>(),
            hudManager.spColorInput.GetComponentInChildren<InputField>()
        };

        hudManager.UpdateSpeed(moveSmoothness);
        hudManager.UpdateScale(scaleSmoothness);

        originalMat = sp.material;

        //default to grey
        if (defaultGrey)
        {
            sp.material = GreyscaleMat;
            hudManager.spriteMat = sp.material;
        }

        // Texture2D loadedTexture = 
        //     LoadPNG("C:\\Users\\Student\\Documents\\GitHub\\StarterPlatformer\\assets\\tilemap_packed.png");
        
        // sp.sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        //toggle cursor visibility
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            if (Cursor.visible) {
                Cursor.visible = false;
            }
            else {
                Cursor.visible = true;
            }
        }

        //IMPORTANT: PAUSE INPUT WHILE CURSOR IS VISISBLE
        if (Cursor.visible) {
            return;
        }
        //ALL INPUT BELOW THIS

        //movement
        Vector2 rawMousePosition = Mouse.current.position.ReadValue();
        if (prevMousePosition != rawMousePosition) {
            prevMousePosition = rawMousePosition;
            //movement
            targetMousePos = Camera.main.ScreenToWorldPoint(rawMousePosition);
            targetMousePos.z = 0; //prevents clipping
        }
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetMousePos, moveSmoothness);

        //scroll
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (scrollValue != 0)
        {
            int direction = scrollValue > 0 ? 1 : -1;

            float newScale = targetScale.x 
                    + direction * ((scrollRange.y - scrollRange.x) / scrollSteps);
            newScale = Mathf.Clamp(newScale, scrollRange.x, scrollRange.y);
            
            targetScale = new Vector3(newScale, newScale, newScale);
        }
        gameObject.transform.localScale = 
            Vector3.Lerp(gameObject.transform.localScale, targetScale, scaleSmoothness);   

        //ALL INPUTS THAT YOU WANT PAUSED WHILE TYPING GO BELOW!!!
        if (hexInputs[0].isFocused || hexInputs[1].isFocused) {
            return;
        }

        //switch sprite
        if (Keyboard.current.aKey.wasPressedThisFrame) {
            if (spriteIndex < sprites.Length - 1) {
                spriteIndex++;
            }
            else {
                spriteIndex = 0;
            }

            sp.sprite = sprites[spriteIndex];
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame) {
            if (spriteIndex > 0) {
                spriteIndex--;
            }
            else {
                spriteIndex = sprites.Length - 1;
            }

            sp.sprite = sprites[spriteIndex];
        }

        //hide
        if (Keyboard.current.sKey.wasPressedThisFrame) {
            sp.enabled = !sp.enabled;

            if (!sp.enabled) {
                particles.Stop();
            }
            else {
                particles.Play();
            }
        }

        //particles on/off
        if (Keyboard.current.xKey.wasPressedThisFrame) {
            if (particles.isPlaying) {
                particles.Stop();
            }
            else {
                particles.Play();
            }
        }

        //hide only sprite
        if (Keyboard.current.wKey.wasPressedThisFrame) {
            if (sp.sprite == null) {
                sp.sprite = sprites[spriteIndex];
            }
            else {
                sp.sprite = null;
            }
        }

        //greyscale
        if (Keyboard.current.bKey.wasPressedThisFrame) {
            if (sp.material == originalMat) {
                sp.material = GreyscaleMat;

                hudManager.spriteMat = sp.material;
                hudManager.spColorChanged(); //new material, color got reset
            }
            else {
                sp.material = originalMat;
            }
        }

        //hud
        if (Keyboard.current.hKey.wasPressedThisFrame) {
            hudManager.ToggleCanvas();
        }

        //hud smoothing
        if (Keyboard.current.uKey.wasPressedThisFrame) {
            changeMoveSmoothness(-0.02f);
        }
        else if (Keyboard.current.iKey.wasPressedThisFrame) {
            changeMoveSmoothness(0.02f);
        }

        if (Keyboard.current.jKey.wasPressedThisFrame) {
            changeScaleSmoothness(-0.02f);
        }
        else if (Keyboard.current.kKey.wasPressedThisFrame) {
            changeScaleSmoothness(0.02f);
        }
    }

    void changeMoveSmoothness(float delta) {
        moveSmoothness += delta;
        moveSmoothness = Mathf.Clamp(moveSmoothness, 0.01f, 1f);

        hudManager.UpdateSpeed(moveSmoothness);
    }

    void changeScaleSmoothness(float delta) {
        scaleSmoothness += delta;
        scaleSmoothness = Mathf.Clamp(scaleSmoothness, 0.01f, 1f);

        hudManager.UpdateScale(scaleSmoothness);
    }

    public static Texture2D LoadPNG(string filePath) {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath)) 	{
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
