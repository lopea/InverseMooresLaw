using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FadeOnDistance), typeof(FadeText))]
public class FadeText : MonoBehaviour
{
    FadeOnDistance _fade;
    Text _text;

    void Awake()
    {
        _fade = GetComponent<FadeOnDistance>();
        _text = GetComponent<Text>();
    }

    void Update()
    {
        var color = _text.color;
        color.a = _fade.Value;
        
        _text.color = color;
    }
}
