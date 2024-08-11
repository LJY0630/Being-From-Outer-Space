using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : BaseController
{
    [SerializeField]
    public Stat stat;

    [SerializeField]
    private GameObject crystalcontrol;

    [SerializeField]
    private GameObject[] attackcrystal;

    [SerializeField]
    private BossDownLaser[] downLaser;

    [SerializeField]
    private BossSpawnSkell[] spawnPortal;

    [SerializeField]
    private BoxCollider box;

    [SerializeField]
    private GameObject DeadLight;

    [SerializeField]
    private GameObject FadeOut;

    [SerializeField]
    private BossSound bossSound;

    private float EndPos = 0;
    private float Rotation = 0;
    private bool Delay = false;
    private bool Dying = false;


    public int randomAction = 0;
    public bool isRecv = false;

    C_RandBossPatern c_RandBossPatern = new C_RandBossPatern();

    public S_BroadcastRandBossPatern recvPacket = null;

    public override void Init() // 초기 보스 설정
    {
        //return;
        for (int i = 0; i < attackcrystal.Length; i++)
        {
            attackcrystal[i].GetComponent<BossCrystal>().Activate(false);
        }
        DeadLight.transform.localScale = Vector3.zero;
        Color alpha = FadeOut.GetComponent<Image>().color;
        alpha.a = 0.0f;
        FadeOut.GetComponent<Image>().color = alpha;
        DeadLight.SetActive(false);
        FadeOut.SetActive(false);
        if(NetPlayerManager.Instance.isHost) // 호스트면 조금 늦게 실행
            Invoke("DelayStartThink", 2f);
        else
            StartCoroutine("Think");
    }

    public void DelayStartThink() // 패턴 실행
    {
        StartCoroutine("Think");
    }

    // Update is called once per frame
    void Update()
    {
        //return;
        if (stat.isDead) 
        {

            if (!Dying) // 죽으면 모든 행동 정지
            {
                StopAllCoroutines();
                Delay = false;
                StartCoroutine("Dead");
            }
        }

    }

    IEnumerator Think() // 패턴을 판단함
    {
        yield return new WaitForSeconds(0.5f);

        // 이 부분만 동기화 시키면 될듯

        if (NetPlayerManager.Instance.isHost)
        {
            randomAction = Random.Range(0, 3);

            
            c_RandBossPatern.paternNum = randomAction;
            if(randomAction==0||randomAction==2)
                NetPlayerManager.Instance.Session.Send(c_RandBossPatern.Write());

            switch (randomAction)
            {
                case 0:
                    StartCoroutine("Patern1");
                    break;
                case 1:
                    StartCoroutine("Patern2");
                    break;
                case 2:
                    StartCoroutine("Patern3");
                    break;
            }

        }
        else
        {
            if (isRecv)
            {
                switch (recvPacket.paternNum)
                {
                    case 0:
                        StartCoroutine("Patern1");
                        break;
                    case 1:
                        StartCoroutine("Patern2_Server");
                        break;
                    case 2:
                        StartCoroutine("Patern3");
                        break;
                }
                isRecv = false;
            }
            else
            {
                StartCoroutine("Think");
            }
        }

    }

    IEnumerator Patern1()  // 첫번째(레이저) 패턴
    {
        Debug.Log("Start Patern1");
        
        for (int i = 0; i < attackcrystal.Length; i++)
        {
            attackcrystal[i].GetComponent<BossCrystal>().setAttack();
        }


        while (crystalcontrol.transform.localPosition.y > -2.63f)
        {
            crystalcontrol.transform.localPosition -= new Vector3(0, Time.deltaTime * 3.0f, 0);
            yield return null;
        }

        bossSound.Loop(true);
        bossSound.BossPatern1Sound();

        for (int i = 0; i < attackcrystal.Length; i++)
        {
            attackcrystal[i].GetComponent<BossCrystal>().Activate(true);
            attackcrystal[i].GetComponent<BossCrystal>().Laser(new Vector3(175, 0, 0));
            attackcrystal[i].GetComponent<BossCrystal>().setCollider(true);
        }

        yield return new WaitForSeconds(1.5f);

        while (Rotation < 720f) 
        {
            Rotation += Time.deltaTime * 50f;
            crystalcontrol.transform.rotation = Quaternion.Euler(new Vector3(0, Rotation, 0));
            yield return null;
        }
        bossSound.audioSource.Pause();
        bossSound.Loop(false);

        for (int i = 0; i < attackcrystal.Length; i++)
        {
            attackcrystal[i].GetComponent<BossCrystal>().Laser(new Vector3(0, 0, 0));
            attackcrystal[i].GetComponent<BossCrystal>().Activate(false);
            attackcrystal[i].GetComponent<BossCrystal>().setCollider(false);
        }

        yield return new WaitForSeconds(1.5f);

        while (crystalcontrol.transform.localPosition.y < 0f)
        {
            crystalcontrol.transform.localPosition += new Vector3(0, Time.deltaTime * 3.0f, 0);
            yield return null;
        }

        for (int i = 0; i < attackcrystal.Length; i++)
        {
            attackcrystal[i].GetComponent<BossCrystal>().setIdle();
        }

        Rotation = 0;
        EndPos = 0;

        StartCoroutine("Think");
    }

    IEnumerator Patern2()  // 두번째(레이저) 패턴
    {
        Debug.Log("Start Patern2");

        for (int i = 0; i < downLaser.Length; i++)
        {
            downLaser[i].transform.position = new Vector3(Random.Range(15f, 82.5f), 1.5f, Random.Range(13f, 85f));
            C_RandBossPatern.DownLaserPos pos = new C_RandBossPatern.DownLaserPos();
            pos.posX = downLaser[i].transform.position.x;
            pos.posZ = downLaser[i].transform.position.z;
            c_RandBossPatern.downLaserPoss.Add(pos);
        }
        NetPlayerManager.Instance.Session.Send(c_RandBossPatern.Write());
        c_RandBossPatern.downLaserPoss.Clear();
        for(int i=0;i<downLaser.Length;i++)
        {
            downLaser[i].Shoot();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(15f);
        StartCoroutine("Think");
    }

    IEnumerator Patern2_Server()
    {
        Debug.Log("Start Server Patern2");

        for (int i = 0; i < downLaser.Length; i++)
        {
            downLaser[i].transform.position = new Vector3(recvPacket.downLaserPoss[i].posX, 1.5f, recvPacket.downLaserPoss[i].posZ);
            downLaser[i].Shoot();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(15f);
        StartCoroutine("Think");
    }

    IEnumerator Patern3() // 3번째 패턴
    {
        Debug.Log("Start Patern3");

        for (int i = 0; i < 2; i++) //spawnPortal.Length
        {
            spawnPortal[i].Spawn();
        }
        SpawnManager.instance.isSpawn = true;
        yield return new WaitForSeconds(10f);
        StartCoroutine("Think");
    }

    private void OnTriggerStay(Collider other)
    {

        if (!stat.isDead) // 플레이어에게 공격을 입음
        {
            if (other.gameObject.tag == "PowerWeapon" || other.gameObject.tag == "MagicWeapon" || other.gameObject.tag == "HealWeapon")
            {
                if (other.TryGetComponent<IceBall>(out IceBall ice))
                {
                    if (NetPlayerManager.Instance._playerManager.PlayerId
                            == ice.ownplayer.GetComponent<PlayerManager>().PlayerId)
                    {
                        if (!Delay)
                        {
                            Delay = true;
                            stat.OnAttacked(other.gameObject.transform);
                            StartCoroutine("stopHit");
                        }
                    }
                }
                else if (other.TryGetComponent<Lighting>(out Lighting light))
                {
                    Debug.Log("asdasdasdasd");
                    if (NetPlayerManager.Instance._playerManager.PlayerId
                            == light.ownplayer.GetComponent<PlayerManager>().PlayerId)
                    {
                        if (!Delay)
                        {
                            Delay = true;
                            stat.OnAttacked(other.gameObject.transform);
                            StartCoroutine("stopHit");
                        }
                    }
                }
                else
                {
                    if (NetPlayerManager.Instance._playerManager.PlayerId
                            == other.transform.root.GetComponent<PlayerManager>().PlayerId)
                    {
                        if (!Delay)
                        {
                            Delay = true;
                            stat.OnAttacked(other.gameObject.transform);
                            StartCoroutine("stopHit");
                        }
                    }
                }

            }
        }
    }

    IEnumerator stopHit() // 맞을 때 처리
    {
        bossSound.HitSound();
        yield return new WaitForSeconds(0.5f);
        Delay = false;
    }

    IEnumerator Dead() // 죽었을 때 작업, 엔딩 씬으로 전환
    {
        Dying = true;
        bossSound.DeadSound();
        gameObject.layer = 8;
        DeadLight.SetActive(true);
        FadeOut.SetActive(true);
        State = Define.State.Die;
        while (FadeOut.GetComponent<Image>().color.a < 1f) 
        {
            Color alpha = FadeOut.GetComponent<Image>().color;
            alpha.a = Mathf.Lerp(0, 1, Time.deltaTime * 0.2f);
            FadeOut.GetComponent<Image>().color += alpha;
            DeadLight.transform.localScale += new Vector3(Time.deltaTime * 10f, Time.deltaTime * 10f, Time.deltaTime * 10f);
            yield return null;
        }

        if (NetPlayerManager.Instance._playerManager != null && NetPlayerManager.Instance._playerManager.getIsSelf())
        {
            if (NetPlayerManager.Instance._playerManager.quest.questId == 105)
            {
                NetPlayerManager.Instance._playerManager.quest.boss = true;
            }

        }
        Manager.Scene.LoadEndScene();
        Destroy(gameObject);
    }
}
