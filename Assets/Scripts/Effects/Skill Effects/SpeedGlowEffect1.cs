using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// effect used by SpeedBuff1
public class SpeedGlowEffect1 : Effects
{
    public GameObject target;
    public float GlowBrightness = 5.0f;
    private Renderer _renderer;
    private Color baseColor, targetColor;
    private bool isGlowing = false;

    void Start()
    {
        
    }

    void Update()
    {

        if (isGlowing) {
            // float glow = Mathf.PingPong(Time.time, GlowBrightness);
            // _renderer.material.SetFloat("_EmissionIntensity", glow);
            
            // change the color to blue and back
            float t = Mathf.PingPong(Time.time, 1);
            Color newColor = Color.Lerp(baseColor, targetColor, t); // Interpolate between the base color and the target color
            _renderer.material.color = newColor; // Set the new color
        }
    }

    public void startGlow()
    {
        _renderer = target.GetComponent<Renderer>();
        baseColor = _renderer.material.color; // The base color of the sprite
        targetColor = Color.blue; // The color you want to ping-pong to
        isGlowing = true;
    }

}
