using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMP_Text textObject;
    public float moveSpeed = 10.0f;
    public float fadeSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        textObject.text = StatManager.instance.baseAtt.ToString();
        StartCoroutine(MoveAndFade());
    }

    IEnumerator MoveAndFade()
    {
        float timer = 0.0f;

        while (timer < 1.0f)
        {

            Vector3 currentPosition = textObject.rectTransform.position;
            textObject.rectTransform.position = new Vector3(currentPosition.x, currentPosition.y + moveSpeed * Time.deltaTime, currentPosition.z);


            Color textColor = textObject.color;
            textColor.a = Mathf.Lerp(1.0f, 0.0f, timer);
            textObject.color = textColor;

            timer += fadeSpeed * Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);

    }
}
