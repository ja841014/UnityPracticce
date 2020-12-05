using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    float originalSize;
    // you want to be able to access our UIHealthBar script from any other script without needing a reference.
    // you can write UIHealthBar.instance in any script and it will call that get property. The set property is private because we don’t want people to be able to change it from outside that script
    public static UIHealthBar instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //apart from getting the size on screen with rect.width and setting the size 
        originalSize = mask.rectTransform.rect.width;
    }
    // Your  code will call SetValue when the Health changes to a value between 0 and 1 (1 full health, 0.5 half health and so on), and this will change the size of our mask, which in turn will hide the right part of your Health bar.
    public void SetValue(float value)
    {				    
        //  anchor from code with SetSizeWithCurrentAnchors. 
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}