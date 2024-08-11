using System;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Transform player; 
    public Vector3 boxCenter; 
    public Vector3 boxSize = new Vector3(1, 1, 1); 
    public LayerMask layerMask; 
    [HideInInspector]
    public bool keyChest;
    [HideInInspector]
    public bool bossKeyChest;
    [HideInInspector]
    public bool mapChest;
    [HideInInspector]
    public bool compassChest;
    [HideInInspector]
    public bool gustJarChest;
    [HideInInspector]
    public bool heartPieceChest;
    [HideInInspector]
    public bool rupeeChest;
    public static bool gotGotJar = false;

    private bool playerInBox = false;
    private bool chestOpened = false;
    private bool textUpdated = false;
    public PauseMenu pauseMenu;
    Animator chestAnim;

    private void Start() 
    {
        chestAnim = GetComponent<Animator>();    
    }

    void Update()
    {
        CheckPlayerInBox();
        if (playerInBox && Input.GetKeyDown(KeyCode.Space) && !chestOpened)
        {
            chestOpened = true;
            OnChestOpened();
        }
    }

    void CheckPlayerInBox()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position + boxCenter, boxSize / 2, Quaternion.identity, layerMask);
        playerInBox = false;
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform == player)
            {
                playerInBox = true;
                if (!chestOpened)
                {   
                ActionText.UpdateText("Open");
                textUpdated = true;
                }
                break;
            }
        }
        if (!playerInBox && textUpdated)
        {
            ActionText.UpdateText("");
            textUpdated = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + boxCenter, Quaternion.identity, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    void OnChestOpened()
    {
        if (keyChest)
        {
            //KeyInventory.Key++;
            KeyInventory.AddKey();
            chestOpened = true;
            ActionText.UpdateText("");
            Debug.Log("you got " + KeyInventory.Key);
            Dialogue.StartDialogue(0);
            chestAnim.SetTrigger("Open");
        }

        if (bossKeyChest)
        {
             KeyInventory.bossKey++;
            chestOpened = true;
            ActionText.UpdateText("");
            Debug.Log("you got " + KeyInventory.bossKey);
            chestAnim.SetTrigger("Open");
        }

        if (gustJarChest)
        {
            chestOpened = true;
            gotGotJar = true;
            ActionText.UpdateText("");
            Debug.Log("you got the Gust Jar");
            Dialogue.StartDialogue(1);
            chestAnim.SetTrigger("Open");
        }

        if (mapChest)
        {
            chestOpened = true;
            pauseMenu.gotMap();
            ActionText.UpdateText("");
            chestAnim.SetTrigger("Open");
        }
    }
}
