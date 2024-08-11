using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnSkell : MonoBehaviour
{
    [SerializeField]
    private GameObject skeleton;

    [SerializeField]
    private Transform portal;

    [SerializeField]
    private BossSound bossSound;

    // Start is called before the first frame update
    void Start()
    {
        portal.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Spawn() 
    {
        StartCoroutine("SpawnSkell");
    }

    IEnumerator SpawnSkell() 
    {
        Debug.Log("Portal Open");

        bossSound.BossPatern3Sound();
        while (portal.localScale.x < 1.0f && portal.localScale.y < 1.0f && portal.localScale.z < 1.0f)
        {
            portal.localScale += new Vector3(Time.deltaTime * 0.4f, Time.deltaTime * 0.4f, Time.deltaTime * 0.4f);
            yield return null;
        }

        GameObject spawn = Instantiate(skeleton, transform.position, Quaternion.identity);
        spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
        spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
        spawn.GetComponent<EnemyController>().viewAngle = 360f;
        spawn.GetComponent<EnemyController>().viewRadius = 1000f;
        spawn.GetComponent<EnemyController>().monsterId = SpawnManager.instance.currentWCount;
        SpawnManager.instance.monsterList.Add(SpawnManager.instance.currentWCount, spawn);
        SpawnManager.instance.currentWCount++;
        yield return new WaitForSeconds(1.5f);

        while (portal.localScale.x > 0.0f && portal.localScale.y > 0.0f && portal.localScale.z > 0.0f)
        {
            portal.localScale -= new Vector3(Time.deltaTime * 0.4f, Time.deltaTime * 0.4f, Time.deltaTime * 0.4f);
            yield return null;
        }

        portal.localScale = Vector3.zero;
    }
}
