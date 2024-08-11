using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    [SerializeField]
    private ShopUI shop;

    [SerializeField]
    private SkillUI skillUI;

    [SerializeField]
    private InventotyUI inven;

    [SerializeField]
    PlayerHit playerhit;

    [SerializeField]
    public PlayerStat playerstat;

    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float runSpeed;

    [SerializeField]
    float jumpPower;

    [SerializeField]
    WeaponEffect weapon;

    [SerializeField]
    private Transform CameraArm;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private CapsuleCollider collider;

    [SerializeField]
    private TalkUI talk;

    [SerializeField]
    private GameObject Head;

    [SerializeField]
    private GameObject Body;

    [SerializeField]
    private GameObject Weapon;

    [SerializeField]
    private GameObject Leg;

    [SerializeField]
    private GameObject Foot;

    Vector3 lastDistance;

    bool isInput = false;
    bool isGetKey = false;
    bool isFirst = true;
    Vector3 lastmoveDir = Vector3.zero;

    public float curSpeed;
    public bool Dying = false;
    private bool isMove = false;
    private bool isAirMove = false;
    private bool isPressSpace = false;
    private bool pressattack = false;
    private bool pressskill1 = false;
    private bool pressskill2 = false;
    private float sendrate = 0.25f;

    private Vector2 moveInput;
    private Vector3 lookForward;
    private Vector3 lookRight;
    private Vector3 moveDir;


    // Start is called before the first frame update
    public override void Init()
    {
        // �÷��� ���� ����ȭ �غ�� �ʿ��� ������Ʈ ����, ����
        playerManager = transform.parent.GetComponentInChildren<PlayerManager>();
        playerManager.PlayerAction -= OnAction;
        playerManager.PlayerAction += OnAction;
        if (playeras != null)
        {
            Debug.Log("PlayerController in playeras �� �ƴ�");
            playeras.playerSound = base.sound;
            playeras.weaponcollider.enabled = false;
        }

        isInput = false;

        StopCoroutine("moveTime");
        StartCoroutine("moveTime");
    }

    // �÷��� ���� ���, ����, ����ġ, �κ��丮, ���� ��� ����
    public void InvenInit(S_PlayerInfo packet)
    {
        Debug.Log($"�����۰��� : {packet.itemss.Count}");
        transform.parent.GetComponent<PlayerStat>().Gold = packet.Money;
        transform.parent.GetComponent<PlayerStat>().Level = packet.Level;
        transform.parent.GetComponent<PlayerStat>().GetExp = packet.exp;
        transform.parent.GetComponent<PlayerStat>().SetStat(transform.parent.GetComponent<PlayerStat>().Level);
        ItemDataBase itemData = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>();

        Debug.Log(Head.GetComponentInChildren<EquipDrag>().previousItem == null);

        for (int i = 0; i < packet.itemss.Count; i++)
        {
            Item getItem = itemData.FindItem(int.Parse(packet.itemss[i].itemId));

            if (getItem == null)
            {
                Debug.Log("Cant Find!");
            }
            else
            {
                for (int j = 0; j < packet.itemss[i].cnt; j++)
                {

                    if (getItem.itemType == ItemType.Equipment)
                    {
                        if (packet.itemss[i].isEquipped)
                        {
                            ItemEffectEquipment effect = (ItemEffectEquipment)getItem.efts[0];

                            if (effect.itemcode == 1 && Head.GetComponentInChildren<EquipDrag>().previousItem == null)
                            {
                                if (getItem.Use(transform.root))
                                {
                                    Head.GetComponent<Slot>().item = getItem;
                                    Head.GetComponent<Slot>().UpdateSlotUI();
                                    Head.GetComponent<ChangItem>().SetItem(getItem.itemName);
                                    Head.GetComponentInChildren<EquipDrag>().previousItem = getItem;
                                    C_Equipped c_Equipped = new C_Equipped();
                                    c_Equipped.itemId = Head.GetComponent<Slot>().item.itemcode.ToString();
                                    NetPlayerManager.Instance.Session.Send(c_Equipped.Write());
                                }
                            }
                            else if (effect.itemcode == 2 && Body.GetComponentInChildren<EquipDrag>().previousItem == null)
                            {
                                if (getItem.Use(transform.root))
                                {
                                    Body.GetComponent<Slot>().item = getItem;
                                    Body.GetComponent<Slot>().UpdateSlotUI();
                                    Body.GetComponent<ChangItem>().SetItem(getItem.itemName);
                                    Body.GetComponentInChildren<EquipDrag>().previousItem = getItem;
                                    C_Equipped c_Equipped = new C_Equipped();
                                    c_Equipped.itemId = Body.GetComponent<Slot>().item.itemcode.ToString();
                                    NetPlayerManager.Instance.Session.Send(c_Equipped.Write());
                                }
                            }
                            else if (effect.itemcode == 3 && Leg.GetComponentInChildren<EquipDrag>().previousItem == null)
                            {
                                if (getItem.Use(transform.root))
                                {
                                    Leg.GetComponent<Slot>().item = getItem;
                                    Leg.GetComponent<Slot>().UpdateSlotUI();
                                    Leg.GetComponent<ChangItem>().SetItem(getItem.itemName);
                                    Leg.GetComponentInChildren<EquipDrag>().previousItem = getItem;
                                    C_Equipped c_Equipped = new C_Equipped();
                                    c_Equipped.itemId = Leg.GetComponent<Slot>().item.itemcode.ToString();
                                    NetPlayerManager.Instance.Session.Send(c_Equipped.Write());
                                }
                            }
                            else if (effect.itemcode == 4 && Foot.GetComponentInChildren<EquipDrag>().previousItem == null)
                            {
                                if (getItem.Use(transform.root))
                                {
                                    Foot.GetComponent<Slot>().item = getItem;
                                    Foot.GetComponent<Slot>().UpdateSlotUI();
                                    Foot.GetComponent<ChangItem>().SetItem(getItem.itemName);
                                    Foot.GetComponentInChildren<EquipDrag>().previousItem = getItem;
                                    C_Equipped c_Equipped = new C_Equipped();
                                    c_Equipped.itemId = Foot.GetComponent<Slot>().item.itemcode.ToString();
                                    NetPlayerManager.Instance.Session.Send(c_Equipped.Write());
                                }
                            }
                            else
                            {
                                transform.parent.GetChild(1).GetComponent<Inventory>().AddItem(getItem);
                            }
                        }
                        else
                        {
                            transform.parent.GetChild(1).GetComponent<Inventory>().AddItem(getItem);
                        }
                    }
                    else if (getItem.itemType == ItemType.Weapon)
                    {
                        if (packet.itemss[i].isEquipped && Weapon.GetComponentInChildren<WeaponDrag>().previousItem == null)
                        {
                            if (getItem.Use(transform.root))
                            {
                                Weapon.GetComponent<Slot>().item = getItem;
                                Weapon.GetComponent<Slot>().UpdateSlotUI();
                                Weapon.GetComponent<WeaponChanger>().WeaponChange(getItem.itemName);
                                Weapon.GetComponentInChildren<WeaponDrag>().previousItem = getItem;
                                C_Equipped c_Equipped = new C_Equipped();
                                c_Equipped.itemId = Weapon.GetComponent<Slot>().item.itemcode.ToString();
                                NetPlayerManager.Instance.Session.Send(c_Equipped.Write());
                            }
                        }
                        else
                        {
                            transform.parent.GetChild(1).GetComponent<Inventory>().AddItem(getItem);
                        }
                    }
                    else
                    {
                        transform.parent.GetChild(1).GetComponent<Inventory>().AddItem(getItem);
                    }
                }
            }
        }
        transform.parent.GetComponent<PlayerStat>().Hp = transform.parent.GetComponent<PlayerStat>().MaxHp;
        transform.parent.GetComponent<PlayerStat>().Mp = transform.parent.GetComponent<PlayerStat>().MaxMp;
    }

    // �÷��� ���� ����Ʈ ��Ȳ ����
    public void QuestInit(S_QuestInfo packet)
    {
        //Debug.Log($"����Ʈ ���� ���� �Ϸ� : ����Ʈ ���� : {packet.questInfos.Count}");
        //packet.playerId

        // List ����
        //packet.questInfos[0].questId
        //packet.questInfos[0].questState �� => 0 = ������, 1 = ������ 2 = �Ϸ�
        transform.parent.GetComponentInChildren<PlayerManager>().quest.questId = packet.questInfos[0].questId;
        transform.parent.GetComponentInChildren<TalkUI>().QuestText.text = transform.parent.GetComponentInChildren<PlayerManager>().quest.CheckQuest();
        Debug.Log(transform.parent.GetComponentInChildren<PlayerManager>().quest.questId);
    }

    // Ű �Է� ���� �ൿ�� �������� ����� ó���ϴ� ��
    private void FixedUpdate()
    {
        if (playerhit.isHit)
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        }

        if (playeras != null)
        {
            if (pressattack)
            {
                pressattack = false;
                playeras.Delay = true;
                playerManager.isAttack = true;
                playeras.attack = true;
                State = Define.State.Attack;
                playeras.AttackActive();
                curSpeed = 0;
            }

            if (pressskill1)
            {
                pressskill1 = false;
                anim.applyRootMotion = true;
                playeras.Delay = true;
                playerManager.isSkill = true;
                playeras.skill1 = true;
                State = Define.State.Skill1;
                playerstat.Mp -= playeras.skill1mana;
                playeras.Skill1MoveActive();
                curSpeed = 0;
            }

            if (pressskill2)
            {
                pressskill2 = false;
                anim.applyRootMotion = true;
                playeras.Delay = true;
                playerManager.isSkill = true;
                playeras.skill2 = true;
                State = Define.State.Skill2;
                playerstat.Mp -= playeras.skill2mana;
                playeras.Skill2MoveActive();
                curSpeed = 0;
            }
        }

        if (isMove)
        {
            Move();

        }

        if (isAirMove)
        {
            AirMove();
        }

        if (isPressSpace)
        {
            isPressSpace = false;
            Jump();
            playerManager.isJumping = true;
            anim.applyRootMotion = false;
        }
    }

    // �÷��̾��� �Է��� ó���ϴ� ��
    void OnAction()
    {
        // ī�޶� �������� ���� ���� ����
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        lookForward = new Vector3(CameraArm.forward.x, 0f, CameraArm.forward.z).normalized;
        lookRight = new Vector3(CameraArm.right.x, 0f, CameraArm.right.z).normalized;
        moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        // ���� ������ ���� ����
        if (!playerstat.isDead)
        {
            // �¾��� �� ���� ���� ���ݰ� ��ų ����
            if (playerhit.isHit)
            {
                if (playeras != null)
                {
                    playeras.StopAll();
                    playeras.Delay = false;
                    if (playeras.weaponcollider != null)
                    {
                        playeras.weaponcollider.enabled = false;
                    }
                }
                playerManager.isAttack = false;
                playerManager.isSkill = false;
                curSpeed = 0;
                anim.applyRootMotion = false;
            }

            // Ư�� �ൿ, UI�� ������ �������� �۵�
            if (!(playerManager.isJumping) && (playerManager.isGrounded) && !(playerManager.isAttack) && !(playerManager.isSkill) && !playerhit.isHit && !shop.isShopOn && !talk.isTexting)
            {
                // ����
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isPressSpace = true;
                    isInput = true;
                    sound.JumpSound();
                    State = Define.State.Jump;
                } // ��ų 1
                else if (Input.GetKeyDown(KeyCode.Alpha1) && playeras != null && playerstat.Mp >= playeras.skill1mana && !playeras.skill1)
                {
                    if (!playeras.Delay)
                    {
                        pressskill1 = true;
                        isMove = false;
                        isAirMove = false;
                    }
                } // ��ų 2
                else if (Input.GetKeyDown(KeyCode.Alpha2) && playeras != null && playerstat.Mp >= playeras.skill2mana && !playeras.skill2)
                {
                    if (!playeras.Delay)
                    {
                        pressskill2 = true;
                        isMove = false;
                        isAirMove = false;
                    }
                }
            }

            // ���콺�� �������� Ư�� �ൿ, UI�� ������ �������� �۵�
            if (Input.GetKeyDown(KeyCode.Mouse0) && !(playerManager.isJumping) && !(playerManager.isSkill) && !playerhit.isHit && !inven.isInventory && !shop.isShopOn && !talk.isTexting)
            {    
                // ����
                if (playeras != null && !playeras.Delay && !playeras.attack)
                {
                    pressattack = true;
                    isMove = false;
                    isAirMove = false;
                }
            }

            // W, A, S, D Ű�� ������ �� Ư�� �ൿ�� ���� �ʰ� UI�� ���� �ʾ��� �� �۵�
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !(playerManager.isJumping) && !shop.isShopOn && !talk.isTexting)
            {
                // Ư�� �ൿ ���� �ƴϸ�
                if (!(playerManager.isJumping) && !(playerManager.isAttack) && !(playerManager.isSkill) && !playerhit.isHit)
                {
                    // �޸��� �Ǵ�
                    if (Input.GetKey(KeyCode.LeftShift))
                        curSpeed = runSpeed;
                    else
                        curSpeed = walkSpeed;

                    if (curSpeed == walkSpeed)
                    {
                        State = Define.State.Walk;
                    }
                    else if (curSpeed == runSpeed)
                    {
                        State = Define.State.Running;
                    }
                }

                Debug.DrawRay(transform.position, moveDir.normalized * 10.5f, Color.red);

                // ������ �� �ִ����� �Ǵ�
                if (!(playerManager.isAttack) && !(playerManager.isSkill) && !playerhit.isHit)
                {
                    if ((moveDir != Vector3.zero))
                    {
                        // �޸���, �ȱ� �Ǵ� �� �Ҹ� ���
                        if (!playerManager.isJumping)
                        {
                            if (curSpeed == walkSpeed)
                            {
                                sound.WalkSound();
                            }
                            else if (curSpeed == runSpeed)
                            {
                                sound.RunSound();
                            }
                        }
                         // �÷��̾� ȸ���ϴ� �� ����, ȸ�� �ڿ�������
                        Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized);
                        playerTransform.transform.rotation = Quaternion.Lerp(playerTransform.transform.rotation, viewRot, Time.deltaTime * 20);
                        isMove = true;
                    }
                } // �ƴϸ� �������� ����
                else
                {
                    isMove = false;
                }
            }
            else // �������� �ƴ� ����
            {
                isMove = false;

                if (playerManager.isJumping && !(playerManager.isAttack) && !(playerManager.isSkill)) // �����̸鼭 ������ ���� �Ǵ�
                {
                    isAirMove = true;
                }
                else
                {
                    isAirMove = false;
                    curSpeed = 0;
                }

                if ((!(playerManager.isJumping) && !(playerManager.isAttack) && !(playerManager.isSkill) && !playerhit.isHit)) // �ƹ� �͵� �ƴ� ���� ��, ������ �ִ� ����
                {
                    State = Define.State.Idle;
                    curSpeed = 0;
                }
            }
        }
        else if (playerstat.isDead) // ������
        {
            if (!Dying) // ���� �� �ѹ��� ������, �÷��̾��� ��� ���� �����ϰ� �������� �Ѿ
            {
                Debug.Log("HeisDead");
                if (playeras != null)
                {
                    playeras.StopAll();
                    playeras.Delay = false;
                    playeras.playerSound = sound;
                    if (playeras.weaponcollider != null)
                    {
                        playeras.weaponcollider.enabled = false;
                    }
                    anim.applyRootMotion = false;
                }
                playerManager.isAttack = false;
                playerManager.isSkill = false;
                StartCoroutine("Dead");
            }
        }
    }

    private void Move() // �÷��̾ �����̴� �Լ�
    {
        rigidbody.MovePosition(playerTransform.transform.position + Vector3.ClampMagnitude(moveDir, 1f) * curSpeed * Time.fixedDeltaTime);
    }

    // ���� ���¿��� �����������ϴ� �Լ�
    private void AirMove()
    {
        rigidbody.MovePosition(playerTransform.transform.position + Vector3.ClampMagnitude(playerTransform.transform.forward, 1f) * curSpeed * Time.fixedDeltaTime);
    }

    private void Jump() // ���� �Լ�
    {
        C_Jump p = new C_Jump();
        NetPlayerManager.Instance.Session.Send(p.Write());
        rigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
    }

    IEnumerator Dead() // ���� �Լ�
    {
        C_Dead c_Dead = new C_Dead();
        NetPlayerManager.Instance.Session.Send(c_Dead.Write());
        Dying = true;
        if (playeras != null)
        {
            playeras.skill1 = false;
            playeras.skill2 = false;
        }
        skillUI.SetStart();
        State = Define.State.Idle;
        State = Define.State.Die;
        gameObject.layer = 8;
        curSpeed = 0;

        sound.DieSound();

        yield return new WaitForSeconds(4.28f);
        /*
        for (int i = 0; i < transform.root.GetChild(1).childCount; i++) 
        {
           Destroy(transform.root.GetChild(1).GetChild(i).gameObject);
        }
        */
        if (playerstat.isDead)
        {
            transform.root.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void StopMoveTime() // ����ȭ �Ͻ� ����
    {
        StopAllCoroutines();
    }

    public void StartMoveTime() // �Ͻ� ���� �� �ٽ� ����ȭ
    {
        StopCoroutine("moveTime");
        StartCoroutine("moveTime");
    }

    IEnumerator moveTime() // �÷��̾� ����ȭ�� �ʿ��� �������� ������ �Լ�
    {
        //Debug.Log(rigidbody.transform.localPosition);

        // Debug.Log("�۵���!");
        //yield return null;
        isInput = false;
        C_Move c_Move = new C_Move();
        c_Move.isMoving = isMove;// �����϶� isMove = true

        lastmoveDir = rigidbody.transform.forward.normalized;

        c_Move.directionX = rigidbody.transform.forward.x;
        c_Move.directionY = rigidbody.transform.forward.y;
        c_Move.directionZ = rigidbody.transform.forward.z;

        if (isMove)
        {
            c_Move.posX = rigidbody.gameObject.transform.localPosition.x;
            c_Move.posY = rigidbody.gameObject.transform.localPosition.y;
            c_Move.posZ = rigidbody.gameObject.transform.localPosition.z;
        }
        else if (isAirMove)
        {
            c_Move.posX = rigidbody.gameObject.transform.localPosition.x;
            c_Move.posY = rigidbody.gameObject.transform.localPosition.y;
            c_Move.posZ = rigidbody.gameObject.transform.localPosition.z;
        }
        else
        {
            c_Move.posX = rigidbody.gameObject.transform.localPosition.x;
            c_Move.posY = rigidbody.gameObject.transform.localPosition.y;
            c_Move.posZ = rigidbody.gameObject.transform.localPosition.z;
        }

        c_Move.moveSpeed = curSpeed;

        NetPlayerManager.Instance.Session.Send(c_Move.Write());
        //Debug.Log("C_Move�� Y���� : " + c_Move.directionY);
        yield return new WaitForSeconds(0.033f); // 33ms�� ���� �ֱ� ����

        StartCoroutine("moveTime"); // �ֱ� �ݺ�
    }
}