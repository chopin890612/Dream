using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAddListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Slider s = GetComponent<Slider>();
        s.onValueChanged.AddListener((f) => FindObjectOfType<SoundManager>().OnSliderValueChange(f, s));
    }
}
