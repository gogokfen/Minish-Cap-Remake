using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChuChu : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public int hp;
    float moveTimer;
    float moveAnim;
    bool move = false;
    Vector3 direction;

    float jumpTimer;
    [SerializeField] float jumpCooldown = 6;
    bool jumping = false;
    bool jumped = false;
    Vector3 halfWayPoint;

    [HideInInspector]
    public bool vulnerable = false;
    [HideInInspector]
    public bool fallen = false;
    float waddleTime;
    int fallDirection = 0;
    [HideInInspector]
    public bool wakingUp = false;

    float attackTimer;
    [SerializeField] float attackCooldown = 5;
    bool attacking = false;
    int attackRoll;
    bool spat = false;
    [SerializeField] GameObject targetedSpit;
    [SerializeField] Transform spitPos;
    [SerializeField] GameObject shakeSpit;
    [SerializeField] Transform shakeSpot;
    float shakeTimer;
    Vector3 shakeDir;
    Quaternion lastGFXRot;

    [SerializeField] Transform link;
    [SerializeField] GameObject GFX;

    public Animator anim;

    float dyingTimer = 6.5f;
    [HideInInspector]
    public bool dying = false;

    public ParticleSystem dyingEffect;

    void Update()
    {
        if (dying)
        {
            dyingTimer -= Time.deltaTime;
            if (dyingTimer <= 0)
            {
                dying = false;
                SFXController.PlaySFX("EnemyPoof", 0.7f);
                Die();
            }
        }
        else
        {
            moveTimer += Time.deltaTime;

            if (!vulnerable && !attacking)
            {
                GFX.transform.LookAt(link);
                GFX.transform.eulerAngles = new Vector3(0, GFX.transform.eulerAngles.y, 0);
            }

            if (!vulnerable)
            {
                if (!attacking)
                {
                    jumpTimer += Time.deltaTime;
                }
                if (!jumping)
                {
                    attackTimer += Time.deltaTime;
                }
            }

            if (moveTimer >= 1 && !vulnerable && !jumping && !attacking)
            {
                moveTimer = 0;
                direction = (Movement.playerPosition - transform.position);
                direction.Normalize();

                direction *= 10;

                direction.y = 0;
                int randomSFX = Random.Range(1, 6);

                switch (randomSFX)
                {
                    case 1:
                        SFXController.PlaySFX("ChuChuStep1", 0.55f);
                        break;
                    case 2:
                        SFXController.PlaySFX("ChuChuStep2", 0.55f);
                        break;
                    case 3:
                        SFXController.PlaySFX("ChuChuStep3", 0.55f);
                        break;
                    case 4:
                        SFXController.PlaySFX("ChuChuStep4", 0.55f);
                        break;
                    case 5:
                        SFXController.PlaySFX("ChuChuStep5", 0.55f);
                        break;
                }
                move = true;
            }
            if (move)
            {
                moveAnim += Time.deltaTime;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                if (moveAnim >= 0.5)
                {
                    move = false;
                    moveAnim = 0;
                }
            }


            if (attackTimer >= attackCooldown && !vulnerable)
            {
                if (!attacking)
                {
                    attackRoll = Random.Range(0, 2);
                    if (attackRoll == 0)
                    {
                        lastGFXRot = GFX.transform.rotation;
                        anim.SetBool("Shaking", true);
                    }

                    if (attackRoll == 1)
                    {
                        anim.SetBool("Spitting", true);
                    }
                }
                attacking = true;

                if (attackRoll == 1) //spit attack
                {
                    if (attackTimer >= attackCooldown + 2)
                    {
                        GFX.transform.LookAt(link);
                        GFX.transform.eulerAngles = new Vector3(0, GFX.transform.eulerAngles.y, 0);
                        spat = false;
                        anim.SetBool("Spitting", false);
                        attacking = false;
                        attackTimer = 0;
                    }
                    else if (attackTimer >= attackCooldown + 1)
                    {
                        if (!spat)
                        {
                            GameObject tempSpit = Instantiate(targetedSpit, spitPos.position, transform.rotation);
                            tempSpit.GetComponent<ChuChuTargetedSpit>().spitTarget = link; //making the spit know the target
                        }
                        spat = true;
                    }
                }

                else if (attackRoll == 0) //shake attack
                {
                    if (attackTimer >= attackCooldown + 2)
                    {
                        attacking = false;
                        attackTimer = 0;
                        anim.SetBool("Shaking", false);
                    }
                    if (shakeTimer > 0)
                    {
                        shakeTimer -= Time.deltaTime;
                    }
                    else
                    {
                        shakeDir = new Vector3(Random.Range(-45, 15), Random.Range(0, 360), 0);
                        GameObject tempShake = Instantiate(shakeSpit, shakeSpot.position, Quaternion.Euler(shakeDir));
                        shakeTimer = 0.05f; //attacks when this goes to 0
                        GFX.transform.rotation = lastGFXRot;
                        shakeDir = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
                    }
                }
            }

            if (jumpTimer >= jumpCooldown) //V2
            {
                if (!jumping && !attacking)
                {
                    jumping = true;
                    anim.SetBool("Jumping", true);
                }
                if (jumpTimer >= jumpCooldown + 1 && !jumped)
                {
                    Jump();
                }
            }
            if (vulnerable) //v2
            {
                waddleTime += Time.deltaTime;
                if (waddleTime < 5)
                {
                    direction = (Movement.playerPosition - transform.position);
                    transform.Translate(new Vector3(direction.x, 0, 0) * moveSpeed * Time.deltaTime); //moves only in one direction during the waddle
                }
            }
        }

    }

    public void Waddle()
    {
        waddleTime = 0;

        GFX.transform.eulerAngles = new Vector3(0, GFX.transform.eulerAngles.y, 0); //making sure he is positioned correctly

        fallDirection = Random.Range(1, 3);
        if (fallDirection == 2) //falls either left or right
        {
            fallDirection = -1;
        }
        Sequence vulnerableSeq = DOTween.Sequence();

        CapsuleCollider physicalCC = GetComponent<CapsuleCollider>(); 

        Tween fall = transform.DORotate(Vector3.zero, 1);
        fall.SetDelay(6);
        fall.OnStart(() =>
        {
            GFX.transform.eulerAngles = new Vector3(0, GFX.transform.eulerAngles.y, 0);

            physicalCC.direction = 0;
            if (fallDirection == 1) //making sure the physical hitbox can still be interacted with the new animations
            {
                physicalCC.center = new Vector3(-4, 1, 0);
            }
            else
            {
                physicalCC.center = new Vector3(4, 1, 0);
            }

            if (fallDirection == 1)
            {
                anim.SetBool("FallingR", true);
            }
            else
            {
                anim.SetBool("FallingL", true);
            }

        });
        Tween wakeUp = transform.DORotate(Vector3.zero, 1);
        fall.OnComplete(() =>
        {
            anim.SetBool("FallingR", false);
            anim.SetBool("FallingL", false);

            anim.SetBool("Fallen", true);
            anim.SetBool("FallenL", true);

            fallen = true;
        });
        wakeUp.OnComplete(() =>
        {
            wakingUp = true;
        });
        vulnerableSeq.Append(fall);
        vulnerableSeq.AppendInterval(4);
        vulnerableSeq.Append(wakeUp);
        vulnerableSeq.AppendInterval(1);
        vulnerableSeq.OnComplete(() =>
        {
            physicalCC.direction = 1;
            physicalCC.center = new Vector3(0, 3, 0);

            anim.SetBool("Fallen", false);
            anim.SetBool("FallenL", false);

            vulnerable = false;
            fallen = false;
            wakingUp = false;

            anim.SetBool("Waddling", false);
        });
    }

    void Jump()
    {
        jumped = true;

        halfWayPoint = new Vector3((Movement.playerPosition.x + transform.position.x) / 2, 6, (Movement.playerPosition.z + transform.position.z) / 2); 

        Tween up = transform.DOMove(halfWayPoint, 0.5f); // .SetEase(Ease.OutQuad)
        Tween down = transform.DOMove(Movement.playerPosition, 0.5f); //.SetEase(Ease.InQuad)

        Sequence jump = DOTween.Sequence();

        jump.SetEase(Ease.InQuad);

        jump.Append(up);
        jump.Append(down);
        jump.OnComplete(() =>
        {
            jumpTimer = 0;
            jumping = false;
            jumped = false;

            anim.SetBool("Jumping", false);
        });
    }

    public void Die()
    {
        dyingEffect.transform.SetParent(null);
        dyingEffect.transform.rotation = Quaternion.identity;
        dyingEffect.Play();
        Destroy(gameObject);
        Destroy(dyingEffect.gameObject, 3);
    }
}
