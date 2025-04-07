using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveMent : BaseManager
{
    [Header("기본 세팅")]
    public GameObject mainCamera;
    public GameObject head;
    public float intersection;
    private Rigidbody rb;
    private Animator animator;
    public AudioSource playerSound;
    public AudioSource swordSound;
    public AudioClip attackSound;
    public AudioClip[] walkSound;
    public AudioClip[] hitSound;

    [Header("플레이어 스테이터스")]
    public int maxHp;
    public int maxTempOxygen;
    public int attackDamage;
    public float moveSpeed;
    public float ItemSpeed;
    public float angleSpeed;
    public float jumpForce;

    [Header("플레이어 현 상태")]
    public int maxOxygen;
    public bool _bisStop;
    public bool _bisTarget;
    public GameObject sword;
    public GameObject flash;
    public GameObject flashLight;
    public GameObject compass;
    public float targetDuration;
    [HideInInspector] public float BoosterDuration;
    public int currentHp;
    public float currentOxygen;
    public bool _bisJump;


    float mouseX;
    float mouseY;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _bisAttack = true;
        maxOxygen = maxTempOxygen + (50 * Inventory.instance.oxygenLevel);

        mouseX = transform.eulerAngles.y;
        mouseY = mainCamera.transform.eulerAngles.x;

        animator = GetComponent<Animator>();
        currentHp = GameInstance.instance.playerHp  > 0 ? GameInstance.instance.playerHp : maxHp;
        currentOxygen = GameInstance.instance.playerOxygen > 0 ? GameInstance.instance.playerOxygen : maxTempOxygen;

        rb = GetComponent<Rigidbody>();
    }

    float whell;
    public bool _bisAttack = true;
    private void Update()
    {
        currentOxygen -= Time.deltaTime * (GameInstance.instance.currentStage < 6 ? (GameInstance.instance.currentStage + 1) / 2 : 1);

        if (targetDuration > 0)
        {
            _bisTarget = false;
            targetDuration -= Time.deltaTime;
        }
        else { 
            _bisTarget = true;
        }
        if (BoosterDuration > 0)
        {
            BoosterDuration -= Time.deltaTime;
        }
        else {
            ItemSpeed = 0;
        }
        if (!_bisStop)
        {

            Move();
            Rotation();
            Jump();
            Interaction();


            if (Input.GetMouseButtonDown(0))
            {
                if (Inventory.instance.currentItems[Inventory.instance.select].type == ItemType.expendables)
                {
                    Inventory.instance.ItemUse();
                } else if(_bisAttack)
                {
                    Debug.Log(_bisAttack);
                    if (Inventory.instance.currentItems[Inventory.instance.select].type == ItemType.equipment)
                    {
                        if (Inventory.instance.currentItems[Inventory.instance.select].use == ItemUse.Sword)
                        {
                            Attack();
                        }
                    }
                }
            }

            

            whell -= Input.GetAxis("Mouse ScrollWheel") * 10;
            whell = Mathf.Clamp(whell, 0, Inventory.instance.maxBag - 1);
            Inventory.instance.select = (int)whell;
            if (Input.GetKeyDown(KeyCode.Q) && Inventory.instance.currentItems[Inventory.instance.select].type != ItemType.equipment)
            {
                Debug.Log(Inventory.instance.currentItems[Inventory.instance.select].type);
                Inventory.instance.GiveItem();
            }
            animator.SetBool("_bisJump", !_bisJump);
            if (Inventory.instance.currentItems.Count > Inventory.instance.select)
            {
                if (Inventory.instance.currentItems[Inventory.instance.select].use == ItemUse.Sword)
                {
                    sword.gameObject.SetActive(true);
                }
                else
                {
                    sword.SetActive(false);
                }
                if (Inventory.instance.currentItems[Inventory.instance.select].use == ItemUse.FlashLight)
                {
                    flash.gameObject.SetActive(true);
                    flashLight.gameObject.SetActive(true);
                }
                else
                {
                    flash.gameObject.SetActive(false);
                    flashLight.gameObject.SetActive(false);
                }
            }
            else
            {
                sword.SetActive(false);
                flashLight.gameObject.SetActive(false);
                flash.gameObject.SetActive(false);
            }

        }
        if(currentOxygen <= 0)
        {
            gameManger.UI.GameOver(false);
        }
    }

    public void Attack()
    {
        _bisAttack = false;
        animator.SetBool("_bisAttack", true);
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys) { 
            if(Vector3.Distance(enemy.transform.position, this.transform.position) < 5f && enemy.GetComponent<BaseEnemy>() != null)
            {
                enemy.GetComponent<BaseEnemy>().Damage(attackDamage);
            }
        }
        Debug.Log("일단 여기 : " + animator.GetBool("_bisAttack"));
        StartCoroutine(AttackDelay());
    }

    public IEnumerator AttackDelay()
    {
        Debug.Log("");
        yield return null;
        animator.SetBool("_bisAttack", false);
        playerSound.clip = attackSound;
        playerSound.Play();
        swordSound.Play();
        yield return new WaitForSeconds(0.5f);
        _bisAttack = true;
    }

    public void Interaction()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Interaction");
        if(Physics.Raycast(mainCamera.transform.GetChild(0).transform.position, mainCamera.transform.GetChild(0).transform.forward, out hit, intersection, layerMask))
        {
            Debug.Log(hit.transform.gameObject);
            if (hit.transform.gameObject.GetComponent<BaseInteraction>() != null)
            {
                gameManger.UI._bisInteraction = true;
                gameManger.UI.interactionString = hit.transform.gameObject.GetComponent<BaseInteraction>().interactionText;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("하 시발");
                    hit.transform.gameObject.GetComponent<BaseInteraction>().Interaction();
                }
            } else
            {
                gameManger.UI._bisInteraction = false;
            }
        } else
        {
            gameManger.UI._bisInteraction = false;
        }
    }

    float tempX;
    float tempZ;
    bool _bisMoveSound = true;
    public void Move()
    {
        float moveX = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ?  -1 : 0;
        float moveZ = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0; ;

        Vector3 moveMent = moveX * transform.forward + moveZ * transform.right;
        moveMent.Normalize();

        if(moveMent.magnitude != 0)
        {
            animator.SetBool("_bisWalk", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("_bisRun", true);
            } else
            {
                animator.SetBool("_bisRun", false);
            }
            if (_bisMoveSound)
            {
                _bisMoveSound = false;
                playerSound.clip = walkSound[Random.Range(0, walkSound.Length)];
                playerSound.Play();
                StartCoroutine(soundDelay());
            }
        } else
        {
            animator.SetBool("_bisWalk", false);
            animator.SetBool("_bisRun", false);
        }

        
        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2 : moveSpeed;
        currentMoveSpeed = Inventory.instance.currentItems.Count >= Inventory.instance.maxBag ? currentMoveSpeed / 2 : currentMoveSpeed;
        Vector3 _move = moveMent *  (currentMoveSpeed + ItemSpeed); 
        rb.velocity = new Vector3(_move.x, rb.velocity.y, _move.z);
    }

    public IEnumerator soundDelay()
    {
        yield return new WaitForSeconds(0.4f);
        _bisMoveSound = true;
    }


    public void Rotation()
    {
        mouseX += Input.GetAxis("Mouse X") * angleSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * angleSpeed;

        mouseY = Mathf.Clamp(mouseY, -60, 20);

        this.transform.rotation = Quaternion.Euler(0, mouseX, 0);
        mainCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        mainCamera.transform.GetChild(0).gameObject.transform.LookAt(head.transform.position);
    }


    public void Jump()
    {
        if (_bisJump && Input.GetKeyDown(KeyCode.Space) && Inventory.instance.maxWeight > Inventory.instance.weight)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Damage(int _damage)
    {
        currentHp -= _damage;
        StartCoroutine(gameManger.UI.DamageEffort());
        playerSound.clip = hitSound[Random.Range(0, hitSound.Length)];
        playerSound.Play();
        if (currentHp <= 0)
        {
            gameManger.UI.GameOver(true);
        }
    }
}
