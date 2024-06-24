using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    private bool inside;
    [SerializeField] GameObject player;
    [SerializeField] GameObject door;
    [SerializeField] Animator walkAnim;
    void Update()
    {
        if (inside)
        {   
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Movement.playerYRotation = 0;
                Movement.UpdateYRotation();
                Sequence doorSequence = DOTween.Sequence();
                player.transform.position = pos1.position;
                doorSequence.Append(door.transform.DOMoveY(-5,2));
                doorSequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
                doorSequence.Append(player.transform.DOMove(pos2.transform.position, 1));
                doorSequence.AppendCallback(() => walkAnim.Play("Idle"));
                doorSequence.Append(door.transform.DOMoveY(3f,1));
                ActionText.UpdateText("");
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag.Equals ("Player"))
        {
            inside = true;
            ActionText.UpdateText("Open");
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag.Equals ("Player"))
        {
            ActionText.UpdateText("");
            inside = false;
        }
    }
}
