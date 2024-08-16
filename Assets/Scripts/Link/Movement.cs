using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    float originalSpeed; // in case of running allowed
    bool rolling;
    float rollingSpeed;
    float rollingTimer;
    float rollingCooldown;
    float rollYPos;

    [SerializeField] PlayerZone backwardCheck;
    public static bool cantPull;

    //[SerializeField] Camera mainCamera;

    [Header("Link Model & Equipment")]

    [SerializeField] Animator anim;

    public static float hp;
    public static bool gotHit = false;
    public static int enemyHitAmount;
    float gotHitTimer;
    public static Vector2 enemyDirection;
    bool invul = false;
    float invulTimer;
    [SerializeField] GameObject linkModel; //for blinking effect
    Material linkMat;

    public static bool mud = false;

    //[SerializeField] TextMeshPro linkHp;

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
    //float gustJarWindup;
    //[SerializeField] GameObject gustJarShot;
    [SerializeField] Transform gustJarTarget;
    [SerializeField] ParticleSystem gustJarSuction;
    [SerializeField] ParticleSystem gustJarChuchuParticles;
    [SerializeField] ParticleSystem gustJarDustParticles;

    [Header("Other Stuff")]

    //[SerializeField] GameObject crosshair;
    [SerializeField] GameObject swordAndShieldOnBack;
    [SerializeField] TrailRenderer swordSlash;


    public static bool potUp = false;
    public static bool throwing = false;


    public static int push = -2; //idle

    //Transform originalTrans;
    Vector3 originalRot;

    //int directionX
    //Vector2 direction;

    public static float playerYRotation;
    static bool updateYRotation;

    public static Vector3 playerPosition;

    public static bool busy = false;

    public static bool midAction = false;

    public static bool cutScene = false;
    private static float sceneTimer;
    static bool linkRiding = false;
    static bool goToIdle = false;

    static bool stunned;
    static float stunTime;
    float stunCount;

    float DirectionX;
    float DirectionY;

    Rigidbody rigid;
    Vector3 velocityRestter;
    Vector3 knockbackPosition;
    public static bool disableGravity = false;

    [SerializeField] GameObject gameOverScreen;
    private bool deadLink;
    [SerializeField] AudioSource BGM;
    [SerializeField] GameObject deathCamera;

    private AnimatorClipInfo[] ACI;

    void Start()
    {
        goToIdle = false;
        linkMat = linkModel.GetComponent<Renderer>().material;

        gustJarCol = gustJar.GetComponent<CapsuleCollider>();

        rigid = GetComponent<Rigidbody>();

        originalSpeed = moveSpeed;

        Cursor.lockState = CursorLockMode.Locked; //confined?
        hp = 12;

        linkMat.EnableKeyword(("_EMISSION"));

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
    }

    void Update()
    {
        if (cutScene)
        {
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

        anim.SetInteger("Push", push);

        if (swordHit)
        {
            swordHitEffect.Play();
            swordHit = false;
        }

        if (swordBlocked)
        {
            swordBlockedEffect.Play();
            swordBlocked = false;
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
            sword.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            shield.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
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

        hp = HealthSystem.currentHealth;

        if (gotHit)
        {
            gotHit = false;
            rigid.AddForce(new Vector3(enemyDirection.x * 20, 0, enemyDirection.y * 20), ForceMode.Impulse);
            gotHitTimer = 0.15f;
            if (enemyShielded)
            {
                //gotHitTimer = 0.15f;
            }
            if (!invul && !enemyShielded) //checking if the enemy actually got shielded or the shield was just up
            {
                //SFXController.PlaySFX("linkOUCHHHHH");

                Sequence linkHit = DOTween.Sequence();
                //linkHit.Append(linkMat.DOColor(new Color32(255, 125, 0, 0), 0.25f));
                //linkHit.Append(linkMat.DOColor(new Color32(208, 160, 105, 255), 0.25f));

                linkHit.Append(linkMat.DOColor(new Color(1, 0, 0, 1) *10, 0.25f)); //(1, 0.5f, 0, 0) *10
                linkHit.Append(linkMat.DOColor(new Color(0.82f, 0.62f, 0.41f, 1) *1, 0.25f));



                //linkMat.SetColor("_EmissionColor", Color.red * Mathf.Pow(2, 1*Mathf.Sin(invulTimer*50)));



                //gotHitTimer = 0.15f;

                //enemyDirection.Normalize();
                //knockbackPosition = new Vector3(transform.position.x + (enemyDirection.x*5), transform.position.y,transform.position.z + (enemyDirection.y*5));
                //rigid.AddForce(new Vector3(enemyDirection.x*20,0,enemyDirection.y*20),ForceMode.Impulse);

                invulTimer = 0.5f;
                invul = true;
                HealthSystem.TakeDamage(enemyHitAmount);
            }
            enemyShielded = false;

            if (goToIdle)
            {
                anim.SetBool("Moving", false);
                anim.SetBool("Rolling", false);
                anim.SetBool("ShieldUp", false);
                anim.Play("Idle");
            }

        }

        /*
        if (gotHitTimer >= 0)
        {
            gotHitTimer -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x + (enemyDirection.x * Time.deltaTime * 20), transform.position.y, transform.position.z + (enemyDirection.y * Time.deltaTime * 20)); //originally *2 and not timedeltatime
        }

        */


        if (invul)
        {
            invulTimer -= Time.deltaTime;

            //linkMat.SetColor("_BaseColor", Color.yellow);
            //linkMat.SetColor("_EmissionColor", Color.red * Mathf.Pow(2, 1*Mathf.Sin(invulTimer*50)));

            if (invulTimer <= 0)
            {
                invul = false;
                //linkMat.SetColor("_EmissionColor", Color.black);
                //linkMat.SetColor("_BaseColor", Color.white);
            }
        }

        if (disableGravity)
        {
            rigid.useGravity = false;
        }
        else
            rigid.useGravity = true;


        if (Input.GetKey(KeyCode.Space) && busy)
        {
            //busy = true;
            //stunned = true;
            //Debug.Log(playerPosition);
            transform.position = playerPosition;
            //return; //stunned while holding space
            //rigid.isKinematic = true;
            rigid.constraints = (RigidbodyConstraints)116; //I can't believe this actually worked
            //rigid.constraints = RigidbodyConstraints.FreezePositionY;
            //rigid.freezeRotation = true;

            gustJar.SetActive(false);
            //crosshair.SetActive(false);
            gustJarUp = false;
            gustCamera = false;

        }
        else
        {
            //rigid.isKinematic = false;
            if (rigid.constraints == (RigidbodyConstraints)116)
            {
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
            }
            //rigid.constraints = RigidbodyConstraints.None;
            //rigid.freezeRotation = true;


            //rigid.constraints = RigidbodyConstraints.FreezeRotation;
        }

        /*
        else
            busy = false;
        */
        if (stunned)
        {


            stunCount += Time.deltaTime;
            if (!goToIdle)
                transform.position = playerPosition;

            if (stunCount > stunTime)
            {
                stunned = false;
                stunCount = 0;

                anim.SetBool("Rolling", false); //making sure he stops rolling when falling

                goToIdle = false;
            }



            //return; //no bothering do other movement when stunned
        }
        else
        {
            playerPosition = transform.position;
        }



        //playerPosition = transform.position;

        //Debug.Log(playerYRotation);

        if (Input.GetMouseButtonDown(0) && !gustJar.activeSelf && !rolling && !busy && !potUp && gotHitTimer < 0  && (swordTimer == 0 || swordTimer > 0.25f)) //&& !shieldUp
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

            //SFXController.PlaySFX("LinkAttack1", 1.0f);
            swordCol.enabled = true;
            swordSwing = true;
            swordTimer = 0;
            Stun(0.7f); //0.7f
            sword.SetActive(true); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            shield.SetActive(true);
            swordAndShieldOnBack.SetActive(false);
            //originalTrans.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            //originalRot = transform.eulerAngles; //???
            anim.Play("Attack", -1, 0.15f);
            //swordSlash.emitting = true;
            //anim.Play("Attack");

            if (shieldUp)
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
            //sword.transform.eulerAngles = Vector3.Lerp(new Vector3(originalTrans.eulerAngles.x, originalTrans.eulerAngles.y-90, originalTrans.eulerAngles.z), new Vector3(originalTrans.eulerAngles.x, originalTrans.eulerAngles.y+90, originalTrans.eulerAngles.z), swordTimer);
            //sword.transform.eulerAngles = Vector3.Lerp(new Vector3(originalRot.x, originalRot.y - 90, originalRot.z), new Vector3(originalRot.x, originalRot.y + 90, originalRot.z), swordTimer); // ORON PUT IN COMMENT WHEN ANIMATING
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
            if (swordTimer >= 0.7)
            {
                /*
                midAction = false;
                swordSwing = false;
                swordTimer = 0;

                */
                //swordCol.enabled = false;

                //sword.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            }
        }


        if (mud)
        {
            moveSpeed = originalSpeed * 0.6f;
        }
        else if (shieldUp || gustJarUp)
        {
            moveSpeed = originalSpeed * 0.75f; //6
        }
        else
        {
            moveSpeed = originalSpeed;
        }

        if (gustJar.activeSelf && !rolling && !midAction && !stunned && !potUp && Chest.gotGotJar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gustJar.SetActive(false);
                //crosshair.SetActive(false);
                gustJarUp = false;
                gustCamera = false;
            }

            if (Input.GetMouseButton(1))
            {
                if (gustJarRotationSpeed<=1440)
                {
                    gustJarRotationSpeed += (360 * Time.deltaTime);
                }
                gustJarSpinningComponent.Rotate(0, 0, -gustJarRotationSpeed * Time.deltaTime);

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
                if (gustJarRotationSpeed>=0)
                {
                    gustJarRotationSpeed -= (720 * Time.deltaTime);
                }
                gustJarSpinningComponent.Rotate(0, 0, -gustJarRotationSpeed * Time.deltaTime);

                gustJarUp = false;
                gustJarCol.enabled = false;
                gustJarSuction.Stop();
                gustJarChuchuParticles.Stop();
            }

            /** V1 - right mouse click shoots air
            if (Input.GetMouseButtonDown(1))
            {
                gustJarUp = false;
                if (!succed)
                {
                    Stun(0.25f);
                    Instantiate(gustJarShot, gustJar.transform.position, gustJar.transform.rotation);
                }
            }
            */

            if (Input.GetMouseButtonDown(0)) //V2 right mouse click attacks instead and puts off the gust jar
            {
                gustJar.SetActive(false);
                //crosshair.SetActive(false);
                gustJarUp = false;
                gustCamera = false;


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
                Stun(0.7f); //0.7f
                sword.SetActive(true);
                shield.SetActive(true);
                swordAndShieldOnBack.SetActive(false);
                anim.Play("Attack", -1, 0.15f);
            }

            gustJar.transform.LookAt(gustJarTarget);
            gustJarPos = gustJarHoleTrans.position;
        }

        else if (Input.GetKeyDown(KeyCode.E) && !rolling && !busy && !potUp && !stunned && !shieldUp && Chest.gotGotJar)
        {
            gustJar.SetActive(true);
            //crosshair.SetActive(true);
            gustCamera = true;
        }


        /**
        if ((Input.GetKey(KeyCode.E) ||Input.GetMouseButton(2) ) && !rolling && !busy && !potUp && !stunned && !shieldUp)
        {
            gustJarUp = true;
            gustJar.SetActive(true);
            gustJar.transform.LookAt(gustJarTarget);
            crosshair.SetActive(true);
            gustJarPos = gustJarHoleTrans.position;

            if (!gustJarSuction.isPlaying && !succed)
            {
                gustJarSuction.Play();
            }
            else if (succed)
            {
                gustJarSuction.Stop();
            }

        }
        else
        {
            if (gustJarUp)
            {
                Stun(0.25f);
                gustJarUp = false;
                gustJar.SetActive(false);
                crosshair.SetActive(false);

                if (!succed)
                {
                    Instantiate(gustJarShot, gustJar.transform.position, gustJar.transform.rotation);
                }

            }

        }
        */

        if (Input.GetMouseButtonDown(1) && !gustJar.activeSelf && !busy && !potUp  && !swordSwing && rollingTimer <= 0.1f) //shield    && swordTimer>0.2f && !rolling
        {
            midAction = true;
            shield.SetActive(true); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            sword.SetActive(true);
            swordAndShieldOnBack.SetActive(false);

            //Debug.Log("Shield up!");
            shieldCol.enabled = true;
            shieldUp = true;
            anim.Play("Block Start Anim");
            anim.SetBool("ShieldUp", true);
            SFXController.PlaySFX("ShieldOut");
        }



        if (Input.GetMouseButton(1) && !gustJar.activeSelf  && !busy && !potUp && !swordSwing && rollingTimer<=0.1f) //shield   && !rolling
        {
            midAction = true;
            shield.SetActive(true);
            sword.SetActive(true);
            swordAndShieldOnBack.SetActive(false);

            shieldCol.enabled = true;
            shieldUp = true;

            if (anim.GetCurrentAnimatorClipInfo(0).Length>0)
            {
                ACI = anim.GetCurrentAnimatorClipInfo(0);

                //Debug.Log(ACI[0].clip.name);

                if (ACI[0].clip.name == "Idle" || ACI[0].clip.name == "Attack Anim" || ACI[0].clip.name == "Rolling")
                {
                    anim.Play("Block Start Anim");
                    anim.SetBool("ShieldUp", true);
                    SFXController.PlaySFX("ShieldOut");
                }
            }


        }
        
        if (Input.GetMouseButtonUp(1) && !rolling && !busy && !potUp && !gustJar.activeSelf) //&& !swordSwing
        {
            midAction = false;
            //shield.SetActive(false); // ORON MAYBE PUT IN COMMENT WHEN ANIMATING
            shieldCol.enabled = false;
            shieldUp = false;
            anim.SetBool("ShieldUp", false);
            SFXController.PlaySFX("ShieldIn");

        }

        if (!stunned && !rolling && !busy)
        {

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //if any movement
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && rollingCooldown <= 0 && !potUp)
                {
                    invulTimer = 0.25f;
                    invul = true;

                    //making sure gust jar is not up
                    gustJar.SetActive(false);
                    //crosshair.SetActive(false);
                    gustJarUp = false;
                    gustCamera = false;

                    rollYPos = transform.position.y; //checking if link falls or goes up when rolling

                    midAction = true;
                    rolling = true;
                    SFXController.PlaySFX("LinkRoll", 0.5f);
                    anim.Play("Rolling");
                    anim.SetBool("Rolling", true);
                    rollingTimer = 0.75f; //0.3f //original was 0.25f
                }
            }
        }

        /**
        if (rolling)
        {
            rollingTimer -= Time.deltaTime; //0.75 seconds
            //6 *3 = 18 (0.75)
            //Debug.Log(rollingSpeed);
            rollingSpeed = (moveSpeed * 3 - (moveSpeed * ((1 - rollingTimer) * 2)));
            transform.Translate(Vector3.forward * Time.deltaTime * rollingSpeed);

            shieldUp = false; //making sure shield isn't up when rolling
            anim.SetBool("ShieldUp", false);

            if (rollingTimer <= 0)
            {
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
        */
        /**
        if (!stunned && !rolling && !busy)
        {

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //if any movement
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && rollingCooldown <= 0 && !potUp)
                {
                    midAction = true;
                    rolling = true;
                    SFXController.PlaySFX("LinkRoll", 0.5f);
                    anim.Play("Rolling");
                    anim.SetBool("Rolling", true);
                    rollingTimer = 0.75f; //0.3f //original was 0.25f
                }

                anim.SetBool("Moving", true);

                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

                if (Input.GetKey(KeyCode.W))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.deltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.deltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 0f, 0);
                        if (DirectionX < 0)
                            DirectionX += Time.deltaTime * 5;
                        if (DirectionY < 1)
                            DirectionY += Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.W))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.deltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.deltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 90f, 0);
                        if (DirectionX < 1)
                            DirectionX += Time.deltaTime*5;
                        if (DirectionY < 0)
                            DirectionY += Time.deltaTime*5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135f, 0);
                        if (DirectionX < 0.7)
                            DirectionX += Time.deltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.deltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 180f, 0);
                        if (DirectionX < 0)
                            DirectionX += Time.deltaTime * 5;
                        if (DirectionY > -1)
                            DirectionY -= Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    //ActionText.UpdateText("Roll");
                    if (Input.GetKey(KeyCode.S))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 225f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.deltaTime * 5;
                        if (DirectionY > -0.7)
                            DirectionY -= Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else if (Input.GetKey(KeyCode.W))
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 315f, 0);
                        if (DirectionX > -0.7)
                            DirectionX -= Time.deltaTime * 5;
                        if (DirectionY < 0.7)
                            DirectionY += Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 270f, 0);
                        if (DirectionX > -1)
                            DirectionX -= Time.deltaTime * 5;
                        if (DirectionY < 0)
                            DirectionY += Time.deltaTime * 5;
                        anim.SetFloat("DirectionX", DirectionX);
                        anim.SetFloat("DirectionY", DirectionY);
                    }
                }
            }
            else
            {
                DirectionX = DirectionX / (1+Time.deltaTime*10);
                DirectionY = DirectionY / (1 + Time.deltaTime*10);
                anim.SetFloat("DirectionX", DirectionX);
                anim.SetFloat("DirectionY", DirectionY);

                anim.SetBool("Moving", false);
            }

        }
        else
            anim.SetBool("Moving", false);
        */
    }

    private void FixedUpdate() //for all movements related to collision with rigid body
    {
        if (cutScene)
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

                transform.Translate(Vector3.forward * Time.fixedDeltaTime * moveSpeed);

                if (Input.GetKey(KeyCode.W))
                {
                    //ActionText.UpdateText("Roll");
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
                    //ActionText.UpdateText("Roll");
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
                    //ActionText.UpdateText("Roll");
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
                    //ActionText.UpdateText("Roll");
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


        if (rolling)
        {
            velocityRestter = new Vector3(0, rigid.velocity.y, 0); //1

            rollingTimer -= Time.deltaTime; //0.75 seconds
            //6 *3 = 18 (0.75)
            //Debug.Log(rollingSpeed);
            rollingSpeed = (moveSpeed * 3 - (moveSpeed * ((1 - rollingTimer) * 2)));
            //transform.Translate(Vector3.forward * Time.fixedDeltaTime * rollingSpeed); //remember

            rigid.velocity = rollingSpeed * transform.forward + velocityRestter; //2

            shieldUp = false; //making sure shield isn't up when rolling
            anim.SetBool("ShieldUp", false);

            if (Mathf.Abs(transform.position.y - rollYPos)>0.35f) //checking if link goes up or down while rolling
            {
                //rollingTimer = 0;
                rigid.velocity = Vector3.zero + velocityRestter; //3
                midAction = false;
                rolling = false;
                //anim.SetBool("Rolling", false);
                rollingCooldown = 0.10f;

                Stun(rollingTimer);
                rollingTimer = 0;
            }
            else if (rollingTimer <= 0.35f && rollingTimer>0)
            {
                //rigid.velocity = Vector3.zero + velocityRestter; //3
                //midAction = false;
                //rolling = false;
                //anim.SetBool("Rolling", false);
                //rollingCooldown = 0.10f;
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

        if (gotHitTimer >= 0) //currently strictly changes position, might need to change later with collision problems
        {
            gotHitTimer -= Time.deltaTime;
            if (gotHitTimer<0)
            {
                rigid.velocity = Vector3.zero;
            }
            //transform.position = new Vector3(transform.position.x + (enemyDirection.x * Time.fixedDeltaTime * 20), transform.position.y, transform.position.z + (enemyDirection.y * Time.fixedDeltaTime * 20)); //originally *2 and not timedeltatime

            //rigid.MovePosition(knockbackPosition);
            //rigid.position = new Vector3(transform.position.x + (enemyDirection.x * Time.fixedDeltaTime * 20), transform.position.y, transform.position.z + (enemyDirection.y * Time.fixedDeltaTime * 20)); //originally *2 and not timedeltatime
            //rigid.AddForce(knockbackPosition);
        }
        /*
        else
        {
            //rigid.ResetInertiaTensor();
            //rigid.velocity = Vector3.zero;
        }
        */
    }

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

    public static void SmallHit(Vector2 enemyDir, bool returnLinkToIdleAnimation)
    {
        enemyDirection = enemyDir;
        gotHit = true;
        goToIdle = returnLinkToIdleAnimation;
    }

    public static void UpdateYRotation()
    {
        updateYRotation = true;
    }

    public static void Scene(float timer)
    {
        cutScene = true;
        sceneTimer = timer;
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

}

