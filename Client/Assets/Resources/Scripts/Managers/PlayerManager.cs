using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int PlayerId { get; set; }
    bool isSelf = false;

    public testChat testChat = null;

    public Action PlayerAction = null;
    public Action WeaponEffect = null;
    public bool isJumping = false;
    public bool isGrounded = false;
    public bool isAttack = false;
    public bool isSkill = false;
    public GameObject player;
    private GameObject localPlayer;
    private Vector3 FirstPosition = Vector3.zero;
    private Vector3 Gap = Vector3.zero;
    private Rigidbody playerRigid;
    private RaycastHit hit;
    public Animator playerAnim;
    private int layermask = (1 << 10);

    public WeaponDamage weaponDamage=null;


    public TalkManager talk = new TalkManager();
    public QuestManager quest = new QuestManager();
    public void Init(bool b) 
    {
        weaponDamage = transform.gameObject.GetComponentInChildren<WeaponDamage>();
        testChat = GetComponentInChildren<testChat>();
        isSelf = b;

        if (isSelf == false)
        {
            player = GetComponentInChildren<Other_PlayerController>().gameObject;
        }
        else
        {
            player = GetComponentInChildren<PlayerController>().gameObject;
        }
       
        localPlayer = GetComponentInChildren<Rigidbody>().gameObject;
        Gap.y = player.transform.position.y - localPlayer.transform.position.y;
        playerRigid = localPlayer.GetComponent<Rigidbody>();
        // Find 는 여러 캐릭터에서 중복이기 때문에 바꿈.
        playerAnim = gameObject.GetComponentInChildren<Animator>();
        talk.Init();
        quest.Init();
    }

    public void OnUpdate()
    {
        if (!isJumping && isGrounded)
            playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0.0f, playerRigid.velocity.z);

        
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(0.2f, 0, 0), Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(0.1f, 0, 0.1f), Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position - new Vector3(0.2f, 0, 0), Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position - new Vector3(0.1f, 0, 0.1f), Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(0, 0, 0.2f), Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(-0.1f, 0, 0.1f), Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position - new Vector3(0, 0, 0.2f), Vector3.down, out hit, 1.27f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(-0.1f, 0, -0.1f), Vector3.down, out hit, 1.27f, layermask))
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
        

        if (playerRigid.velocity.y < 0)
        {
            playerRigid.AddForce(Vector3.down * 10f, ForceMode.Force);
        }

        // 캐릭터 움직임이랑 상관 x
        FirstPosition.x = localPlayer.transform.position.x;
        FirstPosition.y = localPlayer.transform.position.y + Gap.y;
        FirstPosition.z = localPlayer.transform.position.z;
        player.transform.position = FirstPosition;


        if (WeaponEffect != null)
        {
            WeaponEffect.Invoke();
        }
        
        if (isJumping && playerRigid.velocity.y <= 0)
        {
            if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 2f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(0.2f, 0, 0), Vector3.down, out hit, 1.4f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(0.1f, 0, 0.1f), Vector3.down, out hit, 1.4f, layermask) ||
                    Physics.Raycast(player.transform.position - new Vector3(0.2f, 0, 0), Vector3.down, out hit, 1.4f, layermask) ||
                    Physics.Raycast(player.transform.position - new Vector3(0.1f, 0, 0.1f), Vector3.down, out hit, 1.4f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(0, 0, 0.2f), Vector3.down, out hit, 1.4f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(-0.1f, 0, 0.1f), Vector3.down, out hit, 1.4f, layermask) ||
                    Physics.Raycast(player.transform.position - new Vector3(0, 0, 0.2f), Vector3.down, out hit, 1.4f, layermask) ||
                    Physics.Raycast(player.transform.position + new Vector3(-0.1f, 0, -0.1f), Vector3.down, out hit, 1.4f, layermask))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {               
                    playerAnim.SetBool("Jump", true);
                    playerAnim.SetBool("Land", true);
                    playerAnim.CrossFade("JumpEnd", 0.1f);

                    
                    if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position + new Vector3(0.2f, 0, 0), Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position + new Vector3(0.1f, 0, 0.1f), Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position - new Vector3(0.2f, 0, 0), Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position - new Vector3(0.1f, 0, 0.1f), Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position + new Vector3(0, 0, 0.2f), Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position + new Vector3(-0.1f, 0, 0.1f), Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position - new Vector3(0, 0, 0.2f), Vector3.down, out hit, 1.27f, layermask) ||
                   Physics.Raycast(player.transform.position + new Vector3(-0.1f, 0, -0.1f), Vector3.down, out hit, 1.27f, layermask))
                    {
                        if (hit.collider.gameObject.CompareTag("Ground"))
                        {
                            isJumping = false;
                            isGrounded = true;
                            playerAnim.SetBool("Jump", false);
                            playerAnim.SetBool("Land", false);
                        }

                    }
                    else
                    {
                        isJumping = true;
                        isGrounded = false;
                    }                   
                }
                else
                {
                    playerAnim.SetBool("Jump", true);
                    playerAnim.SetBool("Land", false);
                }
            }
        }     

    }

    public void Input() 
    {
        if (PlayerAction != null)
        {
            PlayerAction.Invoke();
        }
    }

    public GameObject findBoss()
    {
        return GameObject.Find("Boss");
    }

    public bool getIsSelf() { return isSelf; }
}
