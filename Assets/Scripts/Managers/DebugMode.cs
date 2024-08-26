using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    bool debugModeOn = false;

    [SerializeField] GameObject player;

    [SerializeField]Transform[] transportLocations;
    [SerializeField] GameObject[] enemiesPrefabs;
    [SerializeField] GameObject debugMenuUI;
    //List<GameObject> yoyos;

    [SerializeField] GameObject post1;
    [SerializeField] GameObject post2;
    [SerializeField] GameObject post3;

    public static bool mobileShield = false;
    void Start()
    {
        mobileShield = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            debugModeOn = true;
            debugMenuUI.SetActive(true);
        }

        if (debugModeOn)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                post1.SetActive(true);
                post2.SetActive(false);
                post3.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                post1.SetActive(false);
                post2.SetActive(true);
                post3.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                post1.SetActive(false);
                post2.SetActive(false);
                post3.SetActive(true);
            }



            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                HealthSystem.Heal(1);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                HealthSystem.TakeDamage(1);
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Chest.gotGotJar = true;
            }

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                KeyInventory.AddKey();
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                KeyInventory.RemoveKey();
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                mobileShield = !mobileShield;
            }


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (transportLocations.Length>0)
                {
                    player.transform.position = transportLocations[0].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (transportLocations.Length > 1)
                {
                    player.transform.position = transportLocations[1].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (transportLocations.Length > 2)
                {
                    player.transform.position = transportLocations[2].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (transportLocations.Length > 3)
                {
                    player.transform.position = transportLocations[3].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (transportLocations.Length > 4)
                {
                    player.transform.position = transportLocations[4].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (transportLocations.Length > 5)
                {
                    player.transform.position = transportLocations[5].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (transportLocations.Length > 6)
                {
                    player.transform.position = transportLocations[6].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                if (transportLocations.Length > 7)
                {
                    player.transform.position = transportLocations[7].position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (transportLocations.Length > 8)
                {
                    player.transform.position = transportLocations[8].position;
                }
            }


            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (enemiesPrefabs.Length > 0)
                {
                    Instantiate(enemiesPrefabs[0], new Vector3(player.transform.position.x, player.transform.position.y+3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                if (enemiesPrefabs.Length > 1)
                {
                    Instantiate(enemiesPrefabs[1], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                if (enemiesPrefabs.Length > 2)
                {
                    Instantiate(enemiesPrefabs[2], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                if (enemiesPrefabs.Length > 3)
                {
                    Instantiate(enemiesPrefabs[3], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (enemiesPrefabs.Length > 4)
                {
                    Instantiate(enemiesPrefabs[4], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                if (enemiesPrefabs.Length > 5)
                {
                    Instantiate(enemiesPrefabs[5], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                if (enemiesPrefabs.Length > 6)
                {
                    Instantiate(enemiesPrefabs[6], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                if (enemiesPrefabs.Length > 7)
                {
                    Instantiate(enemiesPrefabs[7], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                if (enemiesPrefabs.Length > 8)
                {
                    Instantiate(enemiesPrefabs[8], new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z), Quaternion.identity);
                }
            }
        }
    }
}
