using UnityEngine;
using UnityEngine.InputSystem;

public class FlyFollower : MonoBehaviour
{
    public float moveSmoothness = 0.2f;
    public float scaleSmoothness = 0.1f;

    public Vector2 scrollRange = new Vector2(0.1f, 1f);
    public int scrollSteps = 5;

    public Sprite[] sprites; 
    int spriteIndex = 0;

    Vector2 prevMousePosition;
    Vector3 targetMousePos;

    SpriteRenderer sp;
    public ParticleSystem particles;

    Vector3 targetScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;

        targetScale = gameObject.transform.localScale;
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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

        //switch sprite
        if (Keyboard.current.aKey.wasPressedThisFrame) {
            if (spriteIndex < sprites.Length - 1) {
                spriteIndex++;
            }

            sp.sprite = sprites[spriteIndex];
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame) {
            if (spriteIndex > 0) {
                spriteIndex--;
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
    }
}
