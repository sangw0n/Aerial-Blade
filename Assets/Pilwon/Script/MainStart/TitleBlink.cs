using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TitleBlink : MonoBehaviour
{
    public TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();    
    }

    private void Start()
    {
        StartCoroutine(textblink());
    }

    IEnumerator textblink()
    {
        yield return new WaitForSeconds(1.5f);
        while (true)
        {
            Color color = text.color;
            while (text.color.a < 1)
            {
                color.a += Time.deltaTime;
                text.color = color;
                yield return null;
            }
            while (text.color.a > 0)
            {
                color.a -= Time.deltaTime;
                text.color = color;
                yield return null;
            }
        }
    }
}
