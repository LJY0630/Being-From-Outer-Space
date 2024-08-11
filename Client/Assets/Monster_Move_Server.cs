using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Move_Server : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �̵� �ӵ�
    public float rotationSpeed = 180.0f; // ȸ�� �ӵ�

    private Vector3 targetPosition; // ��ǥ ��ġ
    private Quaternion targetRotation; // ��ǥ ȸ��
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
        // �ʱ� ��ġ�� ȸ�� ����
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
                Debug.Log($"{pkt.monsterId}�� ���ͼ������� ������Ʈ��, moveSpeed : {moveSpeed}");

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
                Debug.Log($"{pkt.monsterId}�� ���ͼ������� ������Ʈ��, moveSpeed : {moveSpeed}");

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
        Debug.Log($"{packet.monsterId}�� MoveMonster ����, isMoving : {packet.isMoving}");
        rigid = GetComponent<Rigidbody>();
        pkt = packet;
        moveSpeed = packet.moveSpeed;
        targetPosition = new Vector3(packet.posX, packet.posY, packet.posZ);
        ec.moveSpeed = packet.moveSpeed;
        lookDirection = new Vector3(0, packet.directionY, 0);
    }
}