using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XO_GameObject : MonoBehaviour
{
    public float targetScaleAmount = 10f;
    public float scaleDuration = 0.2f;
    private Vector3 initialScale;
    private void Awake()
    {
        
        initialScale = transform.localScale;

        StartCoroutine(ScaleObjectOverTime());
    }
    IEnumerator ScaleObjectOverTime()
    {
        Vector3 targetScale = initialScale * targetScaleAmount;
        float elapsedTime = 0f;
        while (elapsedTime < scaleDuration)
        {
            float t = elapsedTime / scaleDuration;
            Vector3 currentScale = Vector3.Lerp(initialScale, targetScale, t);
            transform.localScale = currentScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
