using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMain : MonoBehaviour
{
    public Transform camTarget;
    public float camSpeed = 1;

    public float cameraBoundsLeft, cameraBoundsRight, cameraBoundsUp, cameraBoundsDown; 

    // Start is called before the first frame update
    void Start()
    {
        transform.position = camTarget.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if (camTarget != null) {
            float limX = Mathf.Clamp(camTarget.position.x, cameraBoundsLeft, cameraBoundsRight);
            float limY = Mathf.Clamp(camTarget.position.y, cameraBoundsDown, cameraBoundsUp);
            transform.position = Vector2.Lerp(transform.position, new Vector2(limX, limY), camSpeed);
            // transform.position = Vector2.Lerp(transform.position, camTarget.position, camSpeed); // enable if no need for boundaries
        }
    }
}
