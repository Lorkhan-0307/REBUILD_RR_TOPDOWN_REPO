using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorTextureArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;

    private int currentFrame;
    private float frameTimer;

    public enum CursorType
    {
        Arrow,
        RangeAttack
    }
    

    private void Start()
    {
        SwitchToArrowCursor();
    }

    private void Update()
    {
        /*
        frameTimer -= Time.deltaTime;
        if(frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTextureArray[currentFrame], new Vector2(0, 0), CursorMode.Auto);
        }*/
    }

    public void SwitchToRangeAttackCursor()
    {
        Cursor.SetCursor(cursorTextureArray[1], new Vector2(16, 16), CursorMode.Auto);
    }

    public void SwitchToArrowCursor()
    {
        Cursor.SetCursor(cursorTextureArray[0], new Vector2(0, 0), CursorMode.Auto);
    }
}
