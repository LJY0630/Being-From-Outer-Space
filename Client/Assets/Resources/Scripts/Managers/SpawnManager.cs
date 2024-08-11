using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance= new SpawnManager();

    [SerializeField]
    private GameObject Warrior;

    [SerializeField]
    private GameObject Archor;

    [SerializeField]
    private GameObject Orc;

    [SerializeField]
    private GameObject Orge;

    public Dictionary<int, GameObject> monsterList = new Dictionary<int, GameObject>();

    public int spawnWCount;

    public int currentWCount;

    public int currentOCount;

    public int currentOgCount;

    public int spawnACount;

    public int spawnOCount;

    public int currentACount;

    public int spawnOgCount;

    public bool isSpawn = false;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        currentWCount = 0;
        currentACount = 0;
        currentOCount = 0;
        currentOgCount = 0;

        spawnWCount = 2;
        spawnACount = 2;
        spawnOCount = 2;
        spawnOgCount = 2;
    }

    

    public int FindIndexMonster(int id)
    {
        foreach(GameObject go in monsterList.Values)
        {
            
            EnemyController c = go.GetComponent<EnemyController>();
            if (c.monsterId == id)
                return id;
        }
        return -1;
    }

    public void Damaged(S_BroadcastMonsterDamaged packet)
    {
        int monsterID = FindIndexMonster(packet.monsterId);
        if(monsterID==-1)
        {
            Debug.Log($"몬스터를 찾지 못했음! id : {packet.monsterId}");
            return;
        }
        // 데미지 입히기 -> enemyController에 함수 만들어서
        //Debug.Log($"몬스터 패킷 : {packet.damaged}");
        monsterList[monsterID].GetComponent<EnemyController>().Damaged(packet.damaged);
    }

    public void SetTargetPosById(S_BroadcastMonsterMove pkt)
    {

        Vector3 vec = new Vector3(pkt.posX, pkt.posY, pkt.posZ);
        Vector3 vec2 = new Vector3(pkt.directionX, pkt.directionY, pkt.directionZ);

        //monsterList[pkt.monsterId].GetComponent<EnemyController>().nav.SetDestination(vec);
        //monsterList[pkt.monsterId].GetComponent<EnemyController>().nav.isStopped = !pkt.isMoving;
        //monsterList[pkt.monsterId].GetComponent<EnemyController>().nav.speed = pkt.moveSpeed;

        GameObject go = null;
        if(monsterList.TryGetValue(pkt.monsterId,out go))
        {
            monsterList[pkt.monsterId].GetComponent<EnemyController>().targetPos_server = vec;
            monsterList[pkt.monsterId].GetComponent<EnemyController>().targetRot_server = vec2;
            //monsterList[pkt.monsterId].GetComponent<EnemyController>().recvMoveSpeed = pkt.moveSpeed;
            monsterList[pkt.monsterId].GetComponent<EnemyController>().recvMoveSpeed = pkt.moveSpeed;
            //Debug.Log($"{pkt.monsterId}번 몬스터 무브 패킷 moveSpeed : {pkt.moveSpeed}");
            monsterList[pkt.monsterId].GetComponent<EnemyController>().isMoving = pkt.isMoving;

            // 위에것들만 바꾸는게 아니고 뭔가 애니메이션 관련된 변수도 패킷값에 따라 바꿔야 할듯.

            monsterList[pkt.monsterId].GetComponent<Monster_Move_Server>().MoveMonster(pkt);
            //Debug.Log("몬스터 아이디 찾음!");
        }
        else
        {
            Debug.Log($"Id : {pkt.monsterId} 없음!");
        }

        
    }

    public void SetEmptyMonsterList()
    {
        isSpawn = false;
        monsterList.Clear();
        currentACount = 0;
        currentWCount = 0;
        currentOCount = 0;
        currentOgCount = 0;
    }

    public void AttackByMonsterId(S_BroadcastMonsterAttack p)
    {
        GameObject go = null;
        if(monsterList.TryGetValue(p.monsterId,out go))
        {
            // 공격시키기
            if (p.monsterId < 100)
                go.GetComponentInChildren<EnemyAttack>().MonsterAttack();
            else if (p.monsterId < 200)
                go.GetComponentInChildren<EnemyArchorAttack>().MonsterAttack(p.targetPlayerId);
            else if (p.monsterId < 300)
                go.GetComponentInChildren<EnemyOrcAttack>().MonsterAttack();
            else if(p.monsterId < 400)
                go.GetComponentInChildren<EnemyOrgeAttack>().MonsterAttack();
        }
    }

    public void InitF()
    {
        if (currentWCount < spawnWCount)
        {
            for (; currentWCount < spawnWCount;)
            {
                GameObject spawn = Instantiate(Warrior, new Vector3(Random.Range(-281.8f, -238.7f), -30.20987f, Random.Range(4.7f, 68f)), Quaternion.identity);
                spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
                spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
                while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
                {
                    // Debug.Log("What");
                    spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-281.8f, -238.7f), -32.543f, Random.Range(4.7f, 68f));
                    spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-281.8f, -238.7f), -32.543f, Random.Range(4.7f, 68f));
                }
                //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
                // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
                spawn.GetComponent<EnemyController>().monsterId = currentWCount;
                monsterList.Add(currentWCount, spawn);
                currentWCount++;
                
            }
        }

        if (currentACount < spawnACount)
        {
            for (; currentACount < spawnACount;)
            {
                GameObject spawn = Instantiate(Archor, new Vector3(Random.Range(-238.7f, -209f), -30.20987f, Random.Range(4.7f, 68f)), Quaternion.identity);
                spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
                spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
                while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
                {
                    // Debug.Log("What");
                    spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-238.7f, -205.7f), -32.543f, Random.Range(4.7f, 76f));
                    spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-238.7f, -205.7f), -32.543f, Random.Range(4.7f, 76f));
                }
                //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
                // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
                spawn.GetComponent<EnemyController>().monsterId = currentACount+100;
                monsterList.Add(currentACount + 100, spawn);
                currentACount++;
                
            }
        }

        if (currentOCount < spawnOCount)
        {
            for (; currentOCount < spawnOCount;)
            {
                GameObject spawn = Instantiate(Orc, new Vector3(Random.Range(-145.4f, -114.6f), -30.20987f, Random.Range(3.0f, 76.5f)), Quaternion.identity);
                spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
                spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
                while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
                {
                    // Debug.Log("What");
                    spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-145.4f, -114.6f), -32.543f, Random.Range(3.0f, 76.5f));
                    spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-145.4f, -114.6f), -32.543f, Random.Range(3.0f, 76.5f));
                }
                //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
                // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
                spawn.GetComponent<EnemyController>().monsterId = currentOCount + 200;
                monsterList.Add(currentOCount + 200, spawn);
                currentOCount++;

            }
        }

        if (currentOgCount < spawnOgCount)
        {
            for (; currentOgCount < spawnOgCount;)
            {
                GameObject spawn = Instantiate(Orge, new Vector3(Random.Range(-45.6f, -10.3f), -30.20987f, Random.Range(0.4f, 76.5f)), Quaternion.identity);
                spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
                spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
                while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
                {
                    // Debug.Log("What");
                    spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-45.6f, -10.3f), -30.20987f, Random.Range(0.4f, 76.5f));
                    spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-45.6f, -10.3f), -30.20987f, Random.Range(0.4f, 76.5f));
                }
                //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
                // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
                spawn.GetComponent<EnemyController>().monsterId = currentOgCount + 300;
                monsterList.Add(currentOgCount + 300, spawn);
                currentOgCount++;

            }
        }

        isSpawn = true;
    }

    // 호스트가 아닐 때
    public void Init(S_BroadcastMonsterSpawn p)
    {
        Debug.Log($"몬스터 인잇 카운트 : {p.monsterIds.Count}");
        for(int i=0;i<p.monsterIds.Count;i++)
        {
            // 아이디에 따라서 워리어, 아처 몬스터 생성

            float posX = p.positionXs[i].posX;
            float posZ = p.positionZs[i].posZ;

            // 워리어 몬스터 생성
            if (p.monsterIds[i].monsterid < 100)
            {
                GameObject spawn = Instantiate(Warrior, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
                spawn.GetComponent<EnemyController>().monsterId = p.monsterIds[i].monsterid;
                Debug.Log($"몬스터 아이디 : {p.monsterIds[i].monsterid}");
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(p.wayPoints[i].way0_X, p.wayPoints[i].way0_Y, p.wayPoints[i].way0_Z);
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(p.wayPoints[i].way1_X, p.wayPoints[i].way1_Y, p.wayPoints[i].way1_Z);
                monsterList.Add(p.monsterIds[i].monsterid, spawn);
                currentWCount++;
            }
            // 아처 몬스터 생성
            else if (p.monsterIds[i].monsterid < 200)
            {
                GameObject spawn = Instantiate(Archor, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
                spawn.GetComponent<EnemyController>().monsterId = p.monsterIds[i].monsterid;
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(p.wayPoints[i].way0_X, p.wayPoints[i].way0_Y, p.wayPoints[i].way0_Z);
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(p.wayPoints[i].way1_X, p.wayPoints[i].way1_Y, p.wayPoints[i].way1_Z);
                Debug.Log($"몬스터 아이디 : {p.monsterIds[i].monsterid}");
                monsterList.Add(p.monsterIds[i].monsterid, spawn);
                currentACount++;
            }
            else if (p.monsterIds[i].monsterid < 300)
            {
                GameObject spawn = Instantiate(Orc, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
                spawn.GetComponent<EnemyController>().monsterId = p.monsterIds[i].monsterid;
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(p.wayPoints[i].way0_X, p.wayPoints[i].way0_Y, p.wayPoints[i].way0_Z);
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(p.wayPoints[i].way1_X, p.wayPoints[i].way1_Y, p.wayPoints[i].way1_Z);
                Debug.Log($"몬스터 아이디 : {p.monsterIds[i].monsterid}");
                monsterList.Add(p.monsterIds[i].monsterid, spawn);
                currentOCount++;
            }
            else 
            {
                GameObject spawn = Instantiate(Orge, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
                spawn.GetComponent<EnemyController>().monsterId = p.monsterIds[i].monsterid;
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(p.wayPoints[i].way0_X, p.wayPoints[i].way0_Y, p.wayPoints[i].way0_Z);
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(p.wayPoints[i].way1_X, p.wayPoints[i].way1_Y, p.wayPoints[i].way1_Z);
                Debug.Log($"몬스터 아이디 : {p.monsterIds[i].monsterid}");
                monsterList.Add(p.monsterIds[i].monsterid, spawn);
                currentOgCount++;
            }
        }
        isSpawn = true;
    }

    public void DeadMonster(S_BroadcastMonsterDead packet)
    {
        monsterList[packet.monsterId].GetComponent<EnemyController>().hitPlayerId = packet.killPlayerId;
        monsterList[packet.monsterId].GetComponent<EnemyController>().DiedMonster_Server();
        
        if(NetPlayerManager.Instance.isHost)
        {
            if(NetPlayerManager.Instance.inBoss==false)
            // 리스폰 코루틴 시작
            StartCoroutine("RespawnMonster",packet);
        }

    }

    IEnumerator RespawnMonster(S_BroadcastMonsterDead packet)
    {
        yield return new WaitForSeconds(5f);
        GameObject spawn = null;
        int mid = -1;
        if (packet.monsterId < 100)
        {
            spawn = Instantiate(Warrior, new Vector3(Random.Range(-281.8f, -238.7f), -30.20987f, Random.Range(4.7f, 68f)), Quaternion.identity);
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
            {
                // Debug.Log("What");
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-281.8f, -238.7f), -32.543f, Random.Range(4.7f, 68f));
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-281.8f, -238.7f), -32.543f, Random.Range(4.7f, 68f));
            }
            //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
            // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
            currentWCount++;

            if (monsterList.ContainsKey(currentWCount))
                currentWCount += 10;
            spawn.GetComponent<EnemyController>().monsterId = currentWCount;
            monsterList.Add(currentWCount, spawn);
            mid = currentWCount;
        }
        else if (packet.monsterId < 200)
        {
            spawn = Instantiate(Archor, new Vector3(Random.Range(-238.7f, -209f), -30.20987f, Random.Range(4.7f, 68f)), Quaternion.identity);
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
            {
                // Debug.Log("What");
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-238.7f, -205.7f), -32.543f, Random.Range(4.7f, 76f));
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-238.7f, -205.7f), -32.543f, Random.Range(4.7f, 76f));
            }
            //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
            // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
            currentACount++;
            if (monsterList.ContainsKey(currentACount + 100))
                currentACount += 10;
            spawn.GetComponent<EnemyController>().monsterId = currentACount + 100;
            monsterList.Add(currentACount + 100, spawn);
            mid = currentACount + 100;
        }
        else if ((packet.monsterId < 300))
        {
            spawn = Instantiate(Orc, new Vector3(Random.Range(-145.4f, -114.6f), -30.20987f, Random.Range(3.0f, 76.5f)), Quaternion.identity);
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
            {
                // Debug.Log("What");
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-145.4f, -114.6f), -32.543f, Random.Range(3.0f, 76.5f));
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-145.4f, -114.6f), -32.543f, Random.Range(3.0f, 76.5f));
            }
            //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
            // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
            currentOCount++;
            if (monsterList.ContainsKey(currentOCount + 200))
                currentOCount += 10;
            spawn.GetComponent<EnemyController>().monsterId = currentOCount + 200;
            monsterList.Add(currentOCount + 200, spawn);
            mid = currentOCount + 200;
        }
        else 
        {
            spawn = Instantiate(Orge, new Vector3(Random.Range(-45.6f, -10.3f), -30.20987f, Random.Range(0.4f, 76.5f)), Quaternion.identity);
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            while (Vector3.Distance(spawn.GetComponent<EnemyController>().waypoints[0], spawn.GetComponent<EnemyController>().waypoints[1]) <= 10)
            {
                // Debug.Log("What");
                spawn.GetComponent<EnemyController>().waypoints[0] = new Vector3(Random.Range(-45.6f, -10.3f), -32.543f, Random.Range(0.4f, 76.5f));
                spawn.GetComponent<EnemyController>().waypoints[1] = new Vector3(Random.Range(-45.6f, -10.3f), -32.543f, Random.Range(0.4f, 76.5f));
            }
            //Debug.Log(spawn.GetComponent<EnemyController>().waypoints[0]);
            // Debug.Log(spawn.GetComponent<EnemyController>().waypoints[1]);
            currentOgCount++;
            if (monsterList.ContainsKey(currentOgCount + 300))
                currentOgCount += 10;
            spawn.GetComponent<EnemyController>().monsterId = currentOgCount + 300;
            monsterList.Add(currentOgCount + 300, spawn);
            mid = currentOgCount + 300;
        }

        // 패킷 전송
        C_RespawnMonster p = new C_RespawnMonster();
        p.monsterId = mid;
        p.posX = spawn.transform.position.x;
        p.posY = spawn.transform.position.y;
        p.posZ = spawn.transform.position.z;
        NetPlayerManager.Instance.Session.Send(p.Write());
    }

    public void RespawnMonsterServer(S_BroadcastRespawnMonster p)
    {
        float posX = p.posX;
        float posZ = p.posZ;

        // 워리어 몬스터 생성
        if (p.monsterId < 100)
        {
            GameObject spawn = Instantiate(Warrior, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
            spawn.GetComponent<EnemyController>().monsterId = p.monsterId;
            Debug.Log($"몬스터 아이디 : {p.monsterId}");
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            monsterList.Add(p.monsterId, spawn);
            currentWCount++;
        }
        // 아처 몬스터 생성
        else if (p.monsterId < 200)
        {
            GameObject spawn = Instantiate(Archor, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
            spawn.GetComponent<EnemyController>().monsterId = p.monsterId;
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            Debug.Log($"몬스터 아이디 : {p.monsterId}");
            monsterList.Add(p.monsterId, spawn);
            currentACount++;
        }
        else if (p.monsterId < 300)
        {
            GameObject spawn = Instantiate(Orc, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
            spawn.GetComponent<EnemyController>().monsterId = p.monsterId;
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            Debug.Log($"몬스터 아이디 : {p.monsterId}");
            monsterList.Add(p.monsterId, spawn);
            currentOCount++;
        }
        else 
        {
            GameObject spawn = Instantiate(Orge, new Vector3(posX, -30.20987f, posZ), Quaternion.identity);
            spawn.GetComponent<EnemyController>().monsterId = p.monsterId;
            spawn.GetComponent<EnemyController>().waypoints[0] = Vector3.zero;
            spawn.GetComponent<EnemyController>().waypoints[1] = Vector3.zero;
            Debug.Log($"몬스터 아이디 : {p.monsterId}");
            monsterList.Add(p.monsterId, spawn);
            currentOgCount++;
        }
    }

}
