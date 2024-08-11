using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Move_Server : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 이동 속도
    public float rotationSpeed = 180.0f; // 회전 속도

    private Vector3 targetPosition; // 목표 위치
    private Quaternion targetRotation; // 목표 회전
    Vector3 lookDirection;
    //[SerializeField]
    Rigidbody rigid;
    S_BroadcastMonsterMove pkt;

    EnemyArchorAttack eaa;

    [SerializeField]
    EnemyController ec;

    // Start is called before the first frame update
    void Start()
    {
        // 초기 위치와 회전 설정
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        eaa = transform.root.GetComponentInChildren<EnemyArchorAttack>();
    }

    void Update()
    {
        if (eaa == null)
        {
            if (rigid != null && pkt.isMoving)
            {
                Debug.Log($"{pkt.monsterId}번 몬스터서버무브 업데이트중, moveSpeed : {moveSpeed}");

                rigid.transform.position = Vector3.Lerp(rigid.transform.position, targetPosition, Time.deltaTime * moveSpeed);
                //rigid.transform.Rotate(lookDirection);
                Vector3 dir = targetPosition - transform.position;
                dir.y = 0f;
                Quaternion rot = Quaternion.LookRotation(dir.normalized);
                transform.rotation = rot;
            }
        }
        else
        {
            if (rigid != null && pkt.isMoving && !eaa.Delay)
            {
                Debug.Log($"{pkt.monsterId}번 몬스터서버무브 업데이트중, moveSpeed : {moveSpeed}");

                rigid.transform.position = Vector3.Lerp(rigid.transform.position, targetPosition, Time.deltaTime * moveSpeed);
                //rigid.transform.Rotate(lookDirection);
                Vector3 dir = targetPosition - transform.position;
                dir.y = 0f;
                Quaternion rot = Quaternion.LookRotation(dir.normalized);
                transform.rotation = rot;
            }
        }


    }

    public void MoveMonster(S_BroadcastMonsterMove packet)
    {
        Debug.Log($"{packet.monsterId}번 MoveMonster 들어옴, isMoving : {packet.isMoving}");
        rigid = GetComponent<Rigidbody>();
        pkt = packet;
        moveSpeed = packet.moveSpeed;
        targetPosition = new Vector3(packet.posX, packet.posY, packet.posZ);
        ec.moveSpeed = packet.moveSpeed;
        lookDirection = new Vector3(0, packet.directionY, 0);
    }
}