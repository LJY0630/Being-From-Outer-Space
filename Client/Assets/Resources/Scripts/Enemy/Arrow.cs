using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 shotVector;

    [SerializeField]
    public Stat Enemystat;

    public void ShotArrow()
    {
        StartCoroutine("Shot");
    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(shotVector);
        transform.forward = shotVector;
        transform.GetComponent<Rigidbody>().velocity = shotVector * 10.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
        {
            Debug.Log("Why");
            Destroy(gameObject);
        }
    }
}
