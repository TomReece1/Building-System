using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Camera MainCamera;
    public GameObject Player;
    

    public ClickToSpawnCube ClickToSpawnCube;

    public bool playMode = false;


    void Awake()
    {
        ClickToSpawnCube = GameObject.Find("GameController").GetComponent<ClickToSpawnCube>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);

        if (Input.GetKeyDown("return"))
        {
            playMode = true;
            MainCamera.GetComponent<CinemachineBrain>().enabled = true;
            Player.GetComponent<ThirdPersonController>().MoveSpeed = 2;
            Player.GetComponent<ThirdPersonController>().SprintSpeed = 4;

            ClickToSpawnCube.CursorCube.SetActive(false);
            



        }

    }



}
