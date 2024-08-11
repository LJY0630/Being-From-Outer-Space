using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartUICall : MonoBehaviour
{
    [SerializeField]
    private BoxCollider BossEnter;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            // 아더가 아니어야 가능
            collision.gameObject.transform.root.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            
            if (collision.gameObject.transform.root.GetComponentInChildren<BossEnterUI>() != null)
                collision.transform.root.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<BossEnterUI>().SetPanel(true);
            else
                collision.gameObject.transform.root.GetComponentInChildren<Rigidbody>().transform.localPosition = new Vector3(2503.6f, 0f,
                collision.gameObject.transform.root.GetComponentInChildren<Rigidbody>().transform.localPosition.z);
        }
    }
}
