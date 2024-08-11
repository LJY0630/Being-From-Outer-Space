using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpHealingCircle : MonoBehaviour
{
    public float healing = 0;

    void Start()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine("Circle");
    }

    IEnumerator Circle() 
    {
        while (transform.localScale.x < 1.2f && transform.localScale.y < 1.2f && transform.localScale.z < 1.2f)
        {
            transform.localScale += new Vector3(Time.deltaTime * 1.4f, Time.deltaTime * 1.4f, Time.deltaTime * 1.4f);
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        while (transform.localScale.x > 0.0f && transform.localScale.y > 0.0f && transform.localScale.z > 0.0f)
        {
            transform.localScale -= new Vector3(Time.deltaTime * 0.8f, Time.deltaTime * 0.8f, Time.deltaTime * 0.8f);
            yield return null;
        }

        Destroy(gameObject);
    }
}