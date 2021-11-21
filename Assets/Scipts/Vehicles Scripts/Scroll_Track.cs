using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Track : MonoBehaviour {

    [SerializeField]
    private float scrollSpeed = 0.05f;

    private float offset = 0.0f;
    private Renderer r;

    public bool movingForward;
    public bool movingBackward;

    void Start()
    {
        r = GetComponent<Renderer>();
    }

    public void StoppedMoving()
    {
        movingForward = false;
        movingBackward = false;
    }
    

    void Update()
    {
        if (movingForward)
        {
            offset = (offset + Time.deltaTime * scrollSpeed) % 1f;
            r.material.SetTextureOffset("_MainTex", new Vector2(offset, 0f));
        }

        if (movingBackward)
        {
            offset = (offset + Time.deltaTime * scrollSpeed) % 1f;
            r.material.SetTextureOffset("_MainTex", new Vector2(-offset, 0f));
        }
        
    }
}
