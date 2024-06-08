using UnityEngine;
using System.Collections;
public class Coins : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(0.7f, 0.7f, 0.7f); 
    public float duration = 1f;

    void Start()
    {
        StartCoroutine(AnimateCoin());
    }

    private IEnumerator AnimateCoin()
    {
        Vector3 initialScale = transform.localScale;

        while (true)
        {
            yield return ScaleOverTime(transform, initialScale, targetScale, duration);

          yield return ScaleOverTime(transform, targetScale, initialScale, duration);
        }
    }

    private IEnumerator ScaleOverTime(Transform coinTransform, Vector3 fromScale, Vector3 toScale, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float progress = (Time.time - startTime) / duration;
            coinTransform.localScale = Vector3.Lerp(fromScale, toScale, progress);
            yield return null;
        }

        coinTransform.localScale = toScale;
    }
}