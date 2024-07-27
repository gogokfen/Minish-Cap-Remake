using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] Transform pos1way1;
    [SerializeField] Transform pos2way1;
    [SerializeField] Transform pos1way2;
    [SerializeField] Transform pos2way2;
    [SerializeField] GameObject player;
    [SerializeField] GameObject door;
    [SerializeField] GameObject doorCamera;
    [SerializeField] Animator walkAnim;
    [SerializeField] DoorZone way1;
    [SerializeField] DoorZone way2;
    [SerializeField] GameObject Lock;
    public int playerYRotationWay1 = 0;
    public int playerYRotationWay2 = 0;
    public bool lockedDoor;
    void Update()
    {
        if (way1.inside)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !Movement.potUp && !Movement.midAction)
            {
                if (!lockedDoor)
                {
                    //walkAnim.Play("Idle");
                    //Movement.playerYRotation = playerYRotationWay1;
                    //Movement.UpdateYRotation();
                    Tween Down = door.transform.DOMoveY(-5, 1.5f);
                    Down.SetEase(Ease.InQuint);
                    Tween Move = player.transform.DOMove(pos2way1.transform.position, 1);
                    Tween Up = door.transform.DOMoveY(3, 2);
                    Sequence doorSequence = DOTween.Sequence();
                    //doorSequence.OnStart(() => Movement.playerYRotation = playerYRotationWay1);
                    //doorSequence.OnStart(() => Movement.UpdateYRotation());
                    doorSequence.AppendCallback(() => walkAnim.SetBool("Moving", false));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.OnStart(() => Movement.Scene(6));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.rotation =Quaternion.Euler(new Vector3(player.transform.eulerAngles.x, playerYRotationWay1, player.transform.eulerAngles.z));
                    player.transform.position = pos1way1.position;
                    doorSequence.Append(Down);
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(Move);
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.Append(Up);
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));

                    //doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    //player.transform.position = pos1way1.position;
                    //doorSequence.Append(door.transform.DOMoveY(-5, 2)).SetEase(Ease.InCubic);
                    //doorSequence.Append(door.transform.DOMoveY(-5, 1.5f, false)).SetEase(Ease.Unset);
                    //doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    //doorSequence.Append(player.transform.DOMove(pos2way1.transform.position, 1));
                    //doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    //doorSequence.Append(door.transform.DOMoveY(3f, 1)).SetEase(Ease.OutExpo);
                    //doorSequence.Append(door.transform.DOMoveY(3, 2,false).SetEase(Ease.Unset)); //.SetEase(Ease.OutCubic)
                    //doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                    ActionText.UpdateText("");
                }
                else if (lockedDoor && KeyInventory.Key >= 1)
                {
                    lockedDoor = false;
                    KeyInventory.RemoveKey();
                    Lock.SetActive(false);
                }
            }
        }

        if (way2.inside)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !lockedDoor && !Movement.midAction)
            {
                if (!lockedDoor)
                {
                    //Movement.playerYRotation = playerYRotationWay2;
                    //Movement.UpdateYRotation();
                    Tween Down = door.transform.DOMoveY(-5, 1.5f);
                    Down.SetEase(Ease.InQuint);
                    Tween Move = player.transform.DOMove(pos2way2.transform.position, 1);
                    Tween Up = door.transform.DOMoveY(3, 2);
                    Sequence doorSequence = DOTween.Sequence();
                    doorSequence.AppendCallback(() => walkAnim.SetBool("Moving", false));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.AppendCallback(() => Movement.Scene(6));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.rotation =Quaternion.Euler(new Vector3(player.transform.eulerAngles.x, playerYRotationWay2, player.transform.eulerAngles.z));
                    player.transform.position = pos1way2.position;
                    doorSequence.Append(Down);
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(Move);
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.Append(Up);
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                    // Movement.playerYRotation = playerYRotationWay2;
                    // Movement.UpdateYRotation();
                    // Sequence doorSequence = DOTween.Sequence();
                    // doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    // player.transform.position = pos1way2.position;
                    // //doorSequence.Append(door.transform.DOMoveY(-5, 2));
                    // doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    // doorSequence.Append(player.transform.DOMove(pos2way2.transform.position, 1));
                    // doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    // //doorSequence.Append(door.transform.DOMoveY(3f, 1));
                    // doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                    // ActionText.UpdateText("");
                }
                else if (lockedDoor && KeyInventory.Key >= 1)
                {
                    lockedDoor = false;
                    KeyInventory.RemoveKey();
                    Lock.SetActive(false);
                }
            }
        }
    }
}
