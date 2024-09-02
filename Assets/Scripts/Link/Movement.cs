using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations.Rigging;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    float originalSpeed; // in case of running allowed
    //---------Rolling----------//
    bool rolling;
    float rollingSpeed;
    float rollingTimer;
    float rollingCooldown;
    float rollYPos;

    static bool stopRolling = false;

    [SerializeField] PlayerZone backwardCheck;
    public static bool cantPull;

    [Header("Link Model & Equipment")]

    [SerializeField] Animator anim;
    //---------Geting Hit----------//
    public static float hp;
    public static bool gotHit = false;
    public static int enemyHitAmount;
    float gotHitTimer;
    public static Vector2 enemyDirection;
    bool invul = false;
    float invulTimer;

    [SerializeField] Material[] linkBlinkMats;

    public static bool mud = false;
    //---------Sword & Shield----------//
    [SerializeField] GameObject sword;
    bool swordSwing = false;
    float swordTimer = 0;
    BoxCollider swordCol;
    public static bool swordHit = false;
    [SerializeField] ParticleSystem swordHitEffect;
    public static bool swordBlocked = false;
    [SerializeField] ParticleSystem swordBlockedEffect;

    [SerializeField] GameObject shield;
    public static bool shieldUp = false;
    public static bool enemyShielded = false;
    BoxCollider shieldCol;

    //---------Gust Jar----------//
    [Header("Gust Jar")]
    [SerializeField] GameObject gustJar;
    CapsuleCollider gustJarCol;
    float gustJarRotationSpeed = 0;
    [SerializeField] Transform gustJarSpinningComponent;
    public static bool gustJarUp = false;
    public static bool gustCamera = false;
    public static bool succed = false;
    public static bool dustSucced = false;
    [SerializeField] Transform gustJarHoleTrans;
    public static Vector3 gustJarPos;
    [SerializeField] Transform gustJarTarget;
    [SerializeField] ParticleSystem gustJarSuction;
    [SerializeField] ParticleSystem gustJarChuchuParticles;
    [SerializeField] ParticleSystem gustJarDustParticles;

    float gustJarSoundTimer = 0;
    bool gustJarLoopSound = false;
    bool gustJarSoundStop = false;

    [Header("Other Stuff")]

    [SerializeField] GameObject swordAndShieldOnBack;
    [SerializeField] TrailRenderer swordSlash;

    //---------States Set By Link's Interactions----------//
    public static bool potUp = false;
    public static bool throwing = false;

    public static int push = -2; //pillar pushing idle state

    public static float playerYRotation;
    static bool updateYRotation;

    public static Vector3 playerPosition;

    public static bool busy = false;

    public static bool midAction = false;

    public static bool cutScene = false;
    private static float sceneTimer;
    static bool linkRiding = false;
    static bool goToIdle = false;
    static bool goToCombat = false;

    static bool stunned;
    static float stunTime;
    float stunCount;

    float DirectionX;
    float DirectionY;

    Rigidbody rigid;
    Vector3 velocityRestter;
    public static bool disableGravity = false;

    //---------UI----------//
    [SerializeField] GameObject gameOverScreen;
    private bool deadLink;
    [SerializeField] AudioSource BGM;
    [SerializeField] GameObject deathCamera;

    private AnimatorClipInfo[] ACI;

    //---------Ground Check----------//
    RaycastHit player;
    [SerializeField] LayerMask mask;
    bool grounded = false;

    [SerializeField] Rig headRig;
    [SerializeField] GameObject gustJarUI;

    void Start() //making sure no static bool is on by accident from previous run
    {
        stopRolling = false;

        goToIdle = false;

        goToCombat = false;

        gustJarCol = gustJar.GetComponent<CapsuleCollider>();

        rigid = GetComponent<Rigidbody>();

        originalSpeed = moveSpeed;

        Cursor.lockState = CursorLockMode.Locked; //confined?
        hp = 12;

        swordCol = sword.GetComponent<BoxCollider>();
        shieldCol = shield.GetComponent<BoxCollider>();

        potUp = false;

        throwing = false;

        cantPull = false;

        succed = false;

        dustSucced = false;

        gustJarUp = false;

        gustCamera = false;

        mud = false;

        disableGravity = false;

        midAction = false;

        cutScene = false;

        busy = false;

        shieldUp = false;

        enemyShielded = false;

        gotHit = false;

        Chest.gotGotJar = false;

        linkRiding = false;
    }

    void Update()
    {
        if (cutScene) //link doesn't act during a cutscene, at least not from this script
        {
            midAction = false;
            rolling = false;
            if (busy)
            {
                busy = false;
                anim.SetBool("Moving", false);
                anim.SetBool("Rolling", false);
                anim.SetBool("ShieldUp", false);
                anim.Play("Idle");
            }
            if (linkRiding)
            {
                sword.SetActive(false);
                shield.SetActive(false);
                swordAndShieldOnBack.SetActive(true);
                if (goToIdle)
                {
                    anim.SetBool("Moving", false);
                    anim.SetBool("Rolling", false);
                    anim.SetBool("ShieldUp", false);
                    anim.Play("Idle");
                    goToIdle = false;
                }
                return;
            }
            sceneTimer -= Time.deltaTime;
            if (sceneTimer <= 0)
                cutScene = false;
            else
                return;

        }
        if (PauseMenu.paused)
        {
            return;
        }
        if (HealthSystem.currentHealth <= 0)
        {
            if (!deadLink)
            {
                anim.Play("Death");
                SFXController.PlaySFX("DeathScreenSound", 0.55f);
                deathCamera.SetActive(true);
                gameOverScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                BGM.Stop();
            }
            deadLink = true;
            return;
        }

        if (gustJar.activeSelf) //head movement & gust jar UI during gust jar usage
        {
            headRig.weight = 1;
            gustJarUI.SetActive(true);
        }
        else
        {
            headRig.weight = 0;
            gustJarUI.SetActive(false);
        }

        if (stopRolling)
        {
            stopRolling = false;
            midAction = false;
            rolling = false;
            anim.SetBool("Rolling", false);
        }

        if (Physics.Raycast(transform.position, -Vector3.up, out player, 10, mask)) //ground check
        {
            if (player.distance > 0.375)
            {
                grounded = false;
            }
            else
                grounded = true;
        }

        anim.SetInteger("Push", push); //setting the pillar pushing state

        hp = HealthSystem.currentHealth; //updating link's hp according the script taking care of the UI as well

        if (swordHit) //playing the hitting visual when someting set the "swordHit" variable to true
        {
            swordHitEffect.Play();
            swordHit = false;
        }

        if (swordBlocked) //same for the blocking
        {
            swordBlockedEffect.Play();
            swordBlocked = false;
            SFXController.PlaySFX("SwordBonk", 0.8f);
        }

        if (potUp)
        {
            sword.SetActive(false);
            shield.SetActive(false);
            swordAndShieldOnBack.SetActive(true);
            if (stunned)
            {
                anim.Play("Pot Lift");
            }
        }

        if (throwing)
        {
            throwing = false;
            anim.SetTrigger("Throwing");
        }

        if (busy || rolling)
        {
            sword.SetActive(false);
            shield.SetActive(false);
            swordAndShieldOnBack.SetActive(true);
        }

        if (backwardCheck.immoveable)
        {
            cantPull = true;
        }
        else
            cantPull = false;

        if (updateYRotation)
        {
            updateYRotation = false;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerYRotation, transform.eulerAngles.z);
        }
        else
            playerYRotation = transform.eulerAngles.y;

        if (gotHit)
        {
            gotHit = false;
            rigid.AddForce(new Vector3(enemyDirection.x * 20, 0, enemyDirection.y * 20), ForceMode.Impulse); //originally was done without rigidbody, interactions with things while knockbacked was rough
            gotHitTimer = 0.15f;
            if (enemyShielded)
            {
                SFXController.PlaySFX("ShieldBonk", 0.5f);
            }
            if (!invul && !enemyShielded) //checking if the enemy actually got shielded or the shield was just up
            {
                for (int i = 0; i < linkBlinkMats.Length; i++) //making his whole body light up when taking damage
                {
                    Sequence linkHit = DOTween.Sequence();
                    linkBlinkMats[i].EnableKeyword(("_EMISSION"));
                    linkHit.Append(linkBlinkMats[i].DOColor(new Color(1, 0, 0, 1) * 1, "_EmissionColor", 0.25f));
                    linkHit.Append(linkBlinkMats[i].DOColor(new Color(0, 0, 0, 0) * 1, "_EmissionColor", 0.25f));
                }

                invulTimer = 0.5f;
                invul = true;
                HealthSystem.TakeDamage(enemyHitAmount);
                anim.Play("TakeDamage");
                int randomSFX = Random.Range(1, 4);

                switch (randomSFX)
                {
                    case 1:
                        SFXController.PlaySFX("LinkHurt1", 0.55f);
                        break;
                    case 2:
                        SFXController.PlaySFX("LinkHurt2", 0.55f);
                        break;
                    case 3:
                        SFXController.PlaySFX("LinkHurt3", 0.55f);
                        break;
                }
            }
            enemyShielded = false;
            if (goToCombat)
            {
                anim.SetBool("Moving", false);
                anim.SetBool("Rolling", false);
                anim.SetBool("ShieldUp", false);
                anim.Play("Attack Blocked", -1, 0.7f);
            }
        }

        if (invul)
        {
            invulTimer -= Time.deltaTime;

            if (invulTimer <= 0)
            {
                invul = false;
            }
        }

        if (disableGravity)
        {
            rigid.useGravity = false;
        }
        else
            rigid.useGravity = true;


        if (Input.GetKey(KeyCode.Space) && busy) //used mainly for pushing pillars, prevents weird gravity interactions
        {
            transform.position = playerPosition;

            rigid.constraints = (RigidbodyConstraints)116; //I can't believe this actually worked

            gustJar.SetActive(false);
            gustJarUp = false;
            gustCamera = false;
        }
        else
        {
            if (rigid.constraints == (RigidbodyConstraints)116)
            {
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        if (stunned) //using the "stunned" bool when affecting the players position from other scripts
        {
            stunCount += Time.deltaTime;
            if (!(goToIdle || goToCombat))
            {
                transform.position = playerPosition;
            }

            if (stunCount > stunTime)
            {
                stunned = false;
                stunCount = 0;

                anim.SetBool("Rolling", false); //making sure he stops rolling when falling

                goToIdle = false;
                goToCombat = false;
            }
        }
        else
        {
            playerPosition = transform.position;
        }

        if (Input.GetMouseButtonDown(0) && !gustJar.activeSelf && !rolling && !busy && !potUp && gotHitTimer < 0 && (swordTimer == 0 || swordTimer > 0.25f)) //Attack
        {
            midAction = true;
            int randomSFX = Random.Range(1, 5);

            switch (randomSFX)
            {
                case 1:
                    SFXController.PlaySFX("LinkAttack1", 0.55f);
                    break;
                case 2:
                    SFXController.PlaySFX("LinkAttack2", 0.55f);
                    break;
                case 3:
                    SFXController.PlaySFX("LinkAttack3", 0.55f);
                    break;
                case 4:
                    SFXController.PlaySFX("LinkAttack4", 0.55f);
                    break;
            }
            swordCol.enabled = true;
            swordSwing = true;
            swordTimer = 0;
            Stun(0.7f);
            sword.SetActive(true);
            shield.SetActive(true);
            swordAndShieldOnBack.SetActive(false);
            int randomAnim = Random.Range(1, 4);
            switch (randomAnim) //multiple attack animations play randomly
            {
                case 1:
                    anim.Play("Attack1", -1, 0.15f);
                    break;
                case 2:
                    anim.Play("Attack2", -1, 0.15f);
                    break;
                case 3:
                    anim.Play("Attack3", -1, 0.15f);
                    break;
            }
            if (shieldUp) //making sure shield is not up when attacking
            {
                shieldCol.enabled = false;
                shieldUp = false;
                anim.SetBool("ShieldUp", false);
                SFXController.PlaySFX("ShieldIn");
            }
        }

        if (swordSwing)
        {
            swordTimer += (Time.deltaTime);
            if (swordTimer >= 0.02)
            {
                swordSlash.emitting = true;
            }
            if (swordTimer >= 0.15)
            {
                swordSlash.emitting = false;
            }
            if (swordTimer >= 0.35)
            {
                swordCol.enabled = false;

                midAction = false;
                swordSwing = false;
            }
        }

        if (mud) //movement speed reduction while stepping on mud, having shield or gust jar active
        {
            moveSpeed = originalSpeed * 0.6f;
        }
        else if (shieldUp || gustJarUp)
        {
            moveSpeed = originalSpeed * 0.75f;
        }
        else
        {
            moveSpeed = originalSpeed;
        }

        if (gustJar.activeSelf && !rolling && !midAction && !stunned && !potUp && Chest.gotGotJar) //gust jar behaviour
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gustJar.SetActive(false);
                gustJarUp = false;
                gustCamera = false;
                anim.SetBool("HoldGustJar", false);

                gustJarSoundStop = false;
                gustJarLoopSound = false;
                SFXController.StopSFX();
                gustJarSoundTimer = 0;
            }

            if (Input.GetMouseButton(1))
            {
                if (gustJarRotationSpeed <= 1440)
                {
                    gustJarRotationSpeed += (360 * Time.deltaTime);
                }
                gustJarSpinningComponent.Rotate(0, 0, -gustJarRotationSpeed * Time.deltaTime);

                if (!gustJarUp)
                {
                    SFXController.PlaySFX("SuctionStart", 0.7f);
                    gustJarSoundStop = true;
                }

                gustJarSoundTimer += Time.deltaTime;

                if (gustJarSoundTimer >= 1f && !gustJarLoopSound) //looping the gust jar sound after it starts
                {
                    gustJarLoopSound = true;
                    SFXController.PlaySFX("SuctionLoop", 0.5f, true);
                }

                gustJarUp = true;
                gustJarCol.enabled = true;
                if (!gustJarSuction.isPlaying && !succed)
                {
                    gustJarSuction.Play();
                }
                else if (succed)
                {
                    gustJarSuction.Stop();
                }

                if (!gustJarDustParticles.isPlaying && dustSucced)
                {
                    gustJarDustParticles.Play();
                    dustSucced = false;
                }
            }
            else
            {
                if (gustJarSoundStop)
                {
                    gustJarSoundStop = false;
                    gustJarLoopSound = false;
                    SFXController.StopSFX();
                    SFXController.PlaySFX("SuctionEnd", 0.7f);
                    gustJarSoundTimer = 0;
                }

                if (gustJarRotationSpeed >= 0)
                {
                    gustJarRotationSpeed -= (720 * Time.deltaTime);
                }
                gustJarSpinningComponent.Rotate(0, 0, -gustJarRotationSpeed * Time.deltaTime);

                gustJarUp = false;
                gustJarCol.enabled = false;
                gustJarSuction.Stop();
                gustJarChuchuParticles.Stop();
            }

            if (Input.GetMouseButtonDown(0)) //V2 right mouse click attacks instead and puts off the gust jar
            {
                gustJarSoundStop = false;
                gustJarLoopSound = false;
                SFXController.StopSFX();
                gustJarSoundTimer = 0;

                gustJar.SetActive(false);
                gustJarUp = false;
                gustCamera = false;
                anim.SetBool("HoldGustJar", false);

                midAction = true;
                int randomSFX = Random.Range(1, 5);
                switch (randomSFX)
                {
                    case 1:
                        SFXController.PlaySFX("LinkAttack1", 0.55f);
                        break;
                    case 2:
                        SFXController.PlaySFX("LinkAttack2", 0.55f);
                        break;
                    case 3:
                        SFXController.PlaySFX("LinkAttack3", 0.55f);
                        break;
                    case 4:
                        SFXController.PlaySFX("LinkAttack4", 0.55f);
                        break;
                }
                int randomAnim = Random.Range(1, 4);
                switch (randomAnim)
                {
                    case 1:
                        anim.Play("Attack1", -1, 0.15f);
                        break;
                    case 2:
                        anim.Play("Attack2", -1, 0.15f);
                        break;
                    case 3:
                        anim.Play("Attack3", -1, 0.15f);
                        break;
                }

                if (goToCombat)
                {
                    swordCol.enabled = false;
                }
                else
                    swordCol.enabled = true;

                swordSwing = true;
                swordTimer = 0;
                Stun(0.7f);
                sword.SetActive(true);
                shield.SetActive(true);
                swordAndShieldOnBack.SetActive(false);
            }

            gustJar.transform.LookAt(gustJarTarget);
            gustJarPos = gustJarHoleTrans.position;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                gustJarSoundStop = false;
                gustJarLoopSound = false;
                SFXController.StopSFX();
                gustJarSoundTimer = 0;
            }
        }

        else if (Input.GetKeyDown(KeyCode.E) && !rolling && !busy && !potUp && !stunned && !shieldUp && Chest.gotGotJar)
        {
            gustJar.SetActive(true);
            anim.SetBool("HoldGustJar", true);
            sword.SetActive(false);
            shield.SetActive(false);
            swordAndShieldOnBack.SetActive(true);
            gustCamera = true;
        }

        if (Input.GetMouseButtonDown(1) && !gustJar.activeSelf && !busy && !potUp && !swordSwing && rollingTimer <= 0.1f)// Shield behaviour
        {
            midAction = true;
            shield.SetActive(true);
            sword.SetActive(true);
            swordAndShieldOnBack.SetActive(false);

            shieldCol.enabled = true;
            shieldUp = true;
            anim.Play("Block Start Anim");
            anim.SetBool("ShieldUp", true);
            SFXController.PlaySFX("ShieldOut");
        }



        if (Input.GetMouseButton(1) && !gustJar.activeSelf && !busy && !potUp && !swordSwing && rollingTimer <= 0.1f)// making it possible to use shield right when another action ends
        {
            midAction = true;
            shield.SetActive(true);
            sword.SetActive(true);
            swordAndShieldOnBack.SetActive(false);

            shieldCol.enabled = true;
            shieldUp = true;

            if (anim.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                ACI = anim.GetCurrentAnimatorClipInfo(0);

                if (ACI[0].clip.name == "Idle" || ACI[0].clip.name == "Attack Anim" || ACI[0].clip.name == "Rolling" || ACI[0].clip.name == "Attack2" || ACI[0].clip.name == "Attack3") //making sure the sound and animation don't loop infinitely
                {
                    anim.Play("Block Start Anim");
                    anim.SetBool("ShieldUp", true);
                    SFXController.PlaySFX("ShieldOut");
                }
            }
        }

        if (Input.GetMouseButtonUp(1) && !rolling && !busy && !potUp && !gustJar.activeSelf)//shield release
        {
            midAction = false;
            shieldCol.enabled = false;
            shieldUp = false;
            anim.SetBool("ShieldUp", false);
            SFXController.PlaySFX("ShieldIn");
        }

        if (!stunned && !rolling && !busy)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //if any movement
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && rollingCooldown <= 0 && !potUp && grounded) //rolling start behaviour
                {
                    invulTimer = 0.25f;
                    invul = true;

                    gustJar.SetActive(false);
                    gustJarUp = false;
                    gustCamera = false;

                    rollYPos = transform.position.y; //checking if link falls or goes up when rolling

                    midAction = true;
                    rolling = true;
                    SFXController.PlaySFX("LinkRoll", 0.5f);
                    anim.SetBool("HoldGustJar", false);
                    anim.Play("Rolling");
                    anim.SetBool("Rolling", true);
                    rollingTimer = 0.75f;
                }
            }
        }
    }

    private void FixedUpdate() //for all movements related to collision with rigid body
    {
        if (cutScene) //during cutscenes, pause and death these don't occur
        {
            return;
        }
        if (PauseMenu.paused)
        {
            return;
        }
        if (HealthSystem.currentHealth <= 0)
        {
            return;
        }

        if (!stunned && !rolling && !busy)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //if any movement
            {
                anim.SetBool("Moving", true);

                transform.Translate(Vector3.forward * Time.fixedDeltaTime * moveSpeed); //link just goes forward, really simple

                if (Input.GetKey(KeyCode.W)) //making multiple buttons/directions clicking sequences possible for best player feel
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.fixedDeltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.fixedDeltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 0f, 0);
                        if (DirectionX < 0)
                            DirectionX += Time.fixedDeltaTime * 5;
                        if (DirectionY < 1)
                            DirectionY += Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.fixedDeltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.fixedDeltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 90f, 0);
                        if (DirectionX < 1)
                            DirectionX += Time.fixedDeltaTime * 5;
                        if (DirectionY < 0)
                            DirectionY += Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.fixedDeltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.fixedDeltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 180f, 0);
                        if (DirectionX < 0)
                            DirectionX += Time.fixedDeltaTime * 5;
                        if (DirectionY > -1)
                            DirectionY -= Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.fixedDeltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.W))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.fixedDeltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 270f, 0);
                        if (DirectionX > -1)
                            DirectionX -= Time.fixedDeltaTime * 5;
                        if (DirectionY < 0)
                            DirectionY += Time.fixedDeltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
            }
            else
            {
                DirectionX = DirectionX / (1 + Time.fixedDeltaTime * 10);
                DirectionY = DirectionY / (1 + Time.fixedDeltaTime * 10);
                anim.SetFloat("DirectionX", DirectionX);
                anim.SetFloat("DirectionY", DirectionY);

                anim.SetBool("Moving", false);
            }
        }
        else
            anim.SetBool("Moving", false);


        if (rolling) //Rolling behaviour
        {
            velocityRestter = new Vector3(0, rigid.velocity.y, 0);

            rollingTimer -= Time.deltaTime; //0.75 seconds

            rollingSpeed = (moveSpeed * 3 - (moveSpeed * ((1 - rollingTimer) * 2)));

            rigid.velocity = rollingSpeed * transform.forward + velocityRestter;

            shieldUp = false; //making sure shield isn't up when rolling
            anim.SetBool("ShieldUp", false);

            if (Mathf.Abs(transform.position.y - rollYPos) > 0.35f) //checking if link goes up or down while rolling
            {
                rigid.velocity = Vector3.zero + velocityRestter;
                midAction = false;
                rolling = false;
                rollingCooldown = 0.10f;

                Stun(rollingTimer);
                rollingTimer = 0;
            }
            else if (rollingTimer <= 0)
            {
                rigid.velocity = Vector3.zero + velocityRestter; //3
                midAction = false;
                rolling = false;
                anim.SetBool("Rolling", false);
                rollingCooldown = 0.10f;
            }
        }
        else
        {
            rollingCooldown -= Time.deltaTime;
        }

        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;
            if (gotHitTimer < 0)
            {
                rigid.velocity = Vector3.zero; //restting momentum when knockback timer ends
            }
        }
    }

    //-----------------Functions to be called from other scripts to affect link without a serialize field------------------//

    public static void Stun(float newStunTime)
    {
        stunned = true;
        stunTime = newStunTime;
    }

    public static void SmallHit(Vector2 enemyDir)
    {
        enemyDirection = enemyDir;
        gotHit = true;
    }

    public static void SmallHit(Vector2 enemyDir, bool returnLinkToCombatAnimation)
    {
        enemyDirection = enemyDir;
        gotHit = true;
        goToCombat = returnLinkToCombatAnimation;
    }

    public static void UpdateYRotation()
    {
        updateYRotation = true;
    }

    public static void Scene(float timer)
    {
        cutScene = true;
        sceneTimer = timer;

        busy = true;
    }

    public static void BarrelRiding(bool riding, bool GoToIdle)
    {
        if (riding)
        {
            linkRiding = true;
            cutScene = true;
            if (GoToIdle)
            {
                goToIdle = true;
            }
        }
        else
        {
            cutScene = false;
            linkRiding = false;
        }
    }
    public static void BarrelRiding(bool riding)
    {
        if (riding)
        {
            linkRiding = true;
            cutScene = true;
        }
        else
        {
            cutScene = false;
            linkRiding = false;
        }
    }

    public static void StopRolling()
    {
        stopRolling = true;
    }
}