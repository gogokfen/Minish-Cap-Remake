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
    bool opened = false;
    void Update()
    {
        if (way1.inside)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !Movement.potUp && !Movement.midAction && !opened)
            {
                if (!lockedDoor)
                {
                    opened = true;
                    Tween Down = door.transform.DOMoveY(-5, 1f); 
                    Down.SetEase(Ease.InQuint);
                    Tween Move = player.transform.DOMove(pos2way1.transform.position, 0.75f); 
                    Sequence doorSequence = DOTween.Sequence();
                    doorSequence.AppendCallback(() => walkAnim.SetBool("Moving", false));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.OnStart(() => Movement.Scene(3));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.rotation =Quaternion.Euler(new Vector3(player.transform.eulerAngles.x, playerYRotationWay1, player.transform.eulerAngles.z));
                    player.transform.position = pos1way1.position;
                    doorSequence.Append(Down);
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(Move);
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                    ActionText.UpdateText("");
                }
                else if (lockedDoor && KeyInventory.Key >= 1)
                {
                    lockedDoor = false;
                    KeyInventory.RemoveKey();
                    Lock.SetActive(false);
                    opened = true;
                    Tween Down = door.transform.DOMoveY(-5, 1f); 
                    Down.SetEase(Ease.InQuint);
                    Tween Move = player.transform.DOMove(pos2way1.transform.position, 0.75f); 
                    Sequence doorSequence = DOTween.Sequence();
                    doorSequence.AppendCallback(() => walkAnim.SetBool("Moving", false));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.OnStart(() => Movement.Scene(3));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.rotation =Quaternion.Euler(new Vector3(player.transform.eulerAngles.x, playerYRotationWay1, player.transform.eulerAngles.z));
                    player.transform.position = pos1way1.position;
                    doorSequence.Append(Down);
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(Move);
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                    ActionText.UpdateText("");
                }
            }
        }
        if (way2.inside)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !lockedDoor && !Movement.midAction && !opened)
            {
                if (!lockedDoor)
                {
                    opened = true;
                    Tween Down = door.transform.DOMoveY(-5, 1f); 
                    Down.SetEase(Ease.InQuint);
                    Tween Move = player.transform.DOMove(pos2way2.transform.position, 0.75f); 
                    Sequence doorSequence = DOTween.Sequence();
                    doorSequence.AppendCallback(() => walkAnim.SetBool("Moving", false));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.AppendCallback(() => Movement.Scene(3));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.rotation =Quaternion.Euler(new Vector3(player.transform.eulerAngles.x, playerYRotationWay2, player.transform.eulerAngles.z));
                    player.transform.position = pos1way2.position;
                    doorSequence.Append(Down);
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(Move);
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                }
                else if (lockedDoor && KeyInventory.Key >= 1)
                {
                    lockedDoor = false;
                    KeyInventory.RemoveKey();
                    Lock.SetActive(false);
                    opened = true;
                    Tween Down = door.transform.DOMoveY(-5, 1f); 
                    Down.SetEase(Ease.InQuint);
                    Tween Move = player.transform.DOMove(pos2way2.transform.position, 0.75f); 
                    Sequence doorSequence = DOTween.Sequence();
                    doorSequence.AppendCallback(() => walkAnim.SetBool("Moving", false));
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.AppendCallback(() => Movement.Scene(3));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(true));
                    player.transform.rotation =Quaternion.Euler(new Vector3(player.transform.eulerAngles.x, playerYRotationWay2, player.transform.eulerAngles.z));
                    player.transform.position = pos1way2.position;
                    doorSequence.Append(Down);
                    doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                    doorSequence.Append(Move);
                    doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                    doorSequence.AppendCallback(() => doorCamera.SetActive(false));
                }
            }
        }
    }
}
