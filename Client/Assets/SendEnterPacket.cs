using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEnterPacket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        C_RequestEnterGame p = new C_RequestEnterGame();
        NetPlayerManager.Instance.Session.Send(p.Write());

        Destroy(gameObject, 5f);
    }

    
}
