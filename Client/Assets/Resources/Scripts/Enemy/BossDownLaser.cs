using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class BossDownLaser : MonoBehaviour
{
    [SerializeField]
    private VolumetricLineBehavior laser;

    [SerializeField]
    private Transform circle;

    [SerializeField]
    private CapsuleCollider capsule;

    [SerializeField]
    private BossSound bossSound;

    private RaycastHit hit;
    private int layermask = (1 << 6);

    // Start is called before the first frame update
    void Start()
    {
        circle.localScale = Vector3.zero;
        laser.StartPos = Vector3.zero;
        laser.EndPos = Vector3.zero;
        laser.gameObject.SetActive(false);
        capsule.enabled = false;
    }

    private void Update()
    {

    }

    public void Shoot() 
    {
        StartCoroutine("shootlaser");
    }

    IEnumerator shootlaser() 
    {
        while (circle.localScale.x < 1.0f && circle.localScale.y < 1.0f && circle.localScale.z < 1.0f) 
        {
            circle.localScale += new Vector3(Time.deltaTime * 0.2f, Time.deltaTime * 0.2f, Time.deltaTime * 0.2f);
            yield return null;
        }

        bossSound.BossPatern2Sound();
        laser.gameObject.SetActive(true);
        laser.EndPos = new Vector3(50f, 0, 0);
        capsule.enabled = true;


        yield return new WaitForSeconds(5f);

        laser.EndPos = new Vector3(0, 0, 0);
        laser.gameObject.SetActive(false);
        capsule.enabled = false;

        yield return new WaitForSeconds(1.5f);

        while (circle.localScale.x > 0.0f && circle.localScale.y > 0.0f && circle.localScale.z > 0.0f)
        {
            circle.localScale -= new Vector3(Time.deltaTime * 0.2f, Time.deltaTime * 0.2f, Time.deltaTime * 0.2f);
            yield return null;
        }

        laser.StartPos = Vector3.zero;
        laser.EndPos = Vector3.zero;
    }

    public void Activate(bool set)
    {
        gameObject.SetActive(set);
    }
}
