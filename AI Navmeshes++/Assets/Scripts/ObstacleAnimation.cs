using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAnimation : MonoBehaviour
{
    [SerializeField] private float maxX = 5f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            while (transform.position.x < xOffset + maxX)
            {
                transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0f, 0f);
                yield return new WaitForEndOfFrame();
            }
            while (transform.position.x > xOffset - maxX)
            {
                transform.position = transform.position - new Vector3(speed * Time.deltaTime, 0f, 0f);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
