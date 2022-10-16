using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeOut(RawImage image)
    {
        StartCoroutine(ChangeOut(image, 0.01f));        
    }
    private IEnumerator ChangeOut(RawImage image, float tickTime)
    {
        while (image.color.a > 0)
        {
            float transparent = image.color.a;
            transparent -= 0.01f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, transparent);
            yield return new WaitForSeconds(tickTime);
        }
    }

    public void FadeIn(RawImage image)
    {
        StartCoroutine(ChangeIn(image, 0.01f));
    }
    private IEnumerator ChangeIn(RawImage image, float tickTime)
    {
        while (image.color.a < 1)
        {
            float transparent = image.color.a;
            transparent += 0.01f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, transparent);
            yield return new WaitForSeconds(tickTime);
        }
    }
}
