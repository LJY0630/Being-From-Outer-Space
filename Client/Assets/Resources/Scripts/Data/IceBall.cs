using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    public Vector3 playerVector;

    public Transform ownplayer;

    [SerializeField]
    public PlayerStat playerStat;

    public void ThrowBall() 
    {
        StartCoroutine("Ball");
    }

    private void Update()
    {
        transform.GetComponent<Rigidbody>().velocity = playerVector.normalized * 10.0f;
    }

    IEnumerator Ball() 
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
