using UnityEngine;

public class PlayerMove_Server : MonoBehaviour
{
    public float moveSpeed; // 이동 속도
    public float rotationSpeed; // 회전 속도

    public Vector3 targetPosition; // 목표 위치
    private Quaternion targetRotation; // 목표 회전
    Vector3 lookDirection;
    //[SerializeField]
    Rigidbody rigid;
    S_BroadcastMove pkt;
    bool isMoving;
    float move;
    public float speed;
    bool isfixed = false;
    [SerializeField]
    Other_PlayerController opc;

    private void Start()
    {
        rotationSpeed = 0.45f;

    }

    public void SetValue()
    {
        rigid.transform.localPosition = new Vector3(rigid.transform.localPosition.x, 301f, rigid.transform.localPosition.z);
        targetPosition = rigid.transform.localPosition;
        isfixed = true;
    }

    public void MovePlayer(S_BroadcastMove packet)
    {
        if (!NetPlayerManager.Instance.isBossLoad)
        {
            //Debug.Log("받음 : " + Time.time);

            if (NetPlayerManager.Instance.isBossLoad == true)
                return;

            rigid = GetComponentInChildren<Rigidbody>();
            pkt = packet;
            isMoving = packet.isMoving;

            move = packet.moveSpeed;

            lookDirection = new Vector3(packet.directionX, packet.directionY, packet.directionZ).normalized;

            Vector3 Predict = Vector3.zero;

            if (packet.moveSpeed == 5.0f)
            {
                moveSpeed = 0.047f;
                speed = 0.6665f;
                Predict = Vector3.ClampMagnitude(lookDirection, 1f) * 0.0665f * (0.033f / 0.013f);
            }
            else if (packet.moveSpeed == 10.0f)
            {
                moveSpeed = 0.06f;
                speed = 1.333f;
                Predict = Vector3.ClampMagnitude(lookDirection, 1f) * 0.1333f * (0.033f / 0.013f);
            }
            else if (packet.moveSpeed == 0.0f)
            {

            }

            // Vector3 Predict = Vector3.ClampMagnitude(lookDirection, 1f) * speed 
            targetPosition = new Vector3(packet.posX + Predict.x, packet.posY, packet.posZ + Predict.z);
        }
    }

    private void FixedUpdate()
    {
        if (NetPlayerManager.Instance.isBossLoad)
        {
            targetPosition = rigid.transform.localPosition;
        }
        else
        {
            if (rigid != null)
            {
                rigid.transform.forward = Vector3.Lerp(rigid.transform.forward, lookDirection, rotationSpeed);

                if (move != 0 || opc.playerManager.isJumping)
                {
                    //rigid.transform.localPosition = Vector3.Lerp(rigid.transform.localPosition, new Vector3(targetPosition.x, rigid.transform.localPosition.y, targetPosition.z), moveSpeed);
                    rigid.transform.localPosition = Vector3.MoveTowards(rigid.transform.localPosition, new Vector3(targetPosition.x, rigid.transform.localPosition.y, targetPosition.z), speed);
                }
                else
                {
                    rigid.transform.localPosition = Vector3.MoveTowards(rigid.transform.localPosition, new Vector3(targetPosition.x, rigid.transform.localPosition.y, targetPosition.z), speed);
                    //rigid.transform.localPosition = Vector3.Lerp(rigid.transform.localPosition, new Vector3(targetPosition.x, rigid.transform.localPosition.y, targetPosition.z), moveSpeed);
                }
            }
        }
    }
}