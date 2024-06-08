using UnityEngine;

public class EnemyMouvement : MonoBehaviour
{
    private float shrinkSpeed = 0.17f; 
    private float growSpeed = 0.17f; 
    private float minSize = 0f; 
    private float maxSize = 0.4f; 
    public float invisibleDuration = 10f; 

    private bool isShrinking = false;
    private bool isStopped = false; 
    private float invisibleTimer = 0f;

    public int Index; 

    public void SetIndex(int newIndex)
    {
        Index = newIndex;
    }

    void Update()
    {
        if (isStopped) return; 
        if (isShrinking)
        {
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

          if (transform.localScale.x <= minSize)
            {
                transform.localScale = Vector3.zero;

              invisibleTimer += Time.deltaTime;

                if (invisibleTimer >= invisibleDuration)
                {
                    invisibleTimer = 0f;
                    isShrinking = false;
                }
            }
        }
        else
        {
      transform.localScale += Vector3.one * growSpeed * Time.deltaTime;

            if (transform.localScale.x >= maxSize)
            {
                isShrinking = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStopped = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStopped = false; 
        }
    }
}
