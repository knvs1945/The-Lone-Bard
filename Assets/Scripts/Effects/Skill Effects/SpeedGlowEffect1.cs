using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// effect used by SpeedBuff1
public class SpeedGlowEffect1 : Effects
{
    public GameObject target;
    public float GlowBrightness = 5.0f;
    private Renderer _renderer;
    private bool isGlowing = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (isGlowing) {
            float glow = Mathf.PingPong(Time.time, GlowBrightness);
            _renderer.material.SetFloat("_EmissionIntensity", glow);
        }
    }

    public void startGlow()
    {
        _renderer = target.GetComponent<Renderer>();
        isGlowing = true;
    }

}
