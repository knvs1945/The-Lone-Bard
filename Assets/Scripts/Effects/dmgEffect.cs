using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dmgEffect : effects
{
    public static float showDmgDuration = 3;

    public TextMeshPro textRender;

    // Start is called before the first frame update
    protected override void Start()
    {
        showDuration = showDmgDuration;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // move effect upwards
        transform.Translate(transform.up * 1 * Time.deltaTime);
    }

    // change the text of the effect
    public void setText (float value) {
        textRender.text = value.ToString();
    }
}
