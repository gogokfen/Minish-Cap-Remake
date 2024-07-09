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
    public int playerYRotationWay1 = 0;
    public int playerYRotationWay2 = 0;
    public bool lockedDoor;
    void Update()
    {
        if (way1.inside)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!lockedDoor)
                {
                    Movement.playerYRotation = playerYRotationWay1;
                    Movement.UpdateYRotation();
                    Sequence doorSequence = DOTween.Sequence();
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.position = pos1way1.position;
                    //doorSequence.Append(door.transform.DOMoveY(-5, 2)).SetEase(Ease.InCubic);
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(player.transform.DOMove(pos2way1.transform.position, 1));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    //doorSequence.Append(door.transform.DOMoveY(3f, 1)).SetEase(Ease.OutExpo);
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                    ActionText.UpdateText("");
                }
                else if (lockedDoor && KeyInventory.Key >= 1)
                {
                    lockedDoor = false;
                    KeyInventory.RemoveKey();
                }
            }
        }

        if (way2.inside)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !lockedDoor)
            {
                if (!lockedDoor)
                {
                    Movement.playerYRotation = playerYRotationWay2;
                    Movement.UpdateYRotation();
                    Sequence doorSequence = DOTween.Sequence();
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.position = pos1way2.position;
                    doorSequence.Append(door.transform.DOMoveY(-5, 2));
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(player.transform.DOMove(pos2way2.transform.position, 1));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.Append(door.transform.DOMoveY(3f, 1));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                    ActionText.UpdateText("");
                }
                else if (lockedDoor && KeyInventory.Key >= 1)
                {
                    lockedDoor = false;
                    KeyInventory.RemoveKey();
                }
            }
        }
    }
}
