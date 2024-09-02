using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClimbCheck : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] float maxHightDiff;

    [SerializeField]float smoothSpeed = 5;
    bool smooth = false;
    float groundY;

    void Update()
    {
        if (smooth)
        {
            playerPos.DOMove(new Vector3(playerPos.position.x, groundY, playerPos.position.z),smoothSpeed).SetEase(Ease.InCirc); //consider disabling gravity on trigger enter and reactivating on ontriggerexit
            smooth = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.position.y > playerPos.position.y && (other.transform.position.y - playerPos.position.y< maxHightDiff))
        {
            smooth = true;

            groundY = playerPos.position.y + ((other.transform.position.y - playerPos.position.y) * 2);
        }
    }
}
