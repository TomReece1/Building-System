using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Editor : MonoBehaviour
{
    public Material SelectedCursorMaterial;


    public GameObject EditInstructions;
    public ClickToSpawnCube ClickToSpawnCube;
    public AudioSource AudioSource;

    public GameObject SelectedObject;

    public GameController GameController;

    void Awake()
    {
        ClickToSpawnCube = GameObject.Find("GameController").GetComponent<ClickToSpawnCube>();
        AudioSource = GetComponent<AudioSource>();
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (!ClickToSpawnCube.buildMode && !GameController.playMode)
        {
            if (Input.GetKey("t"))
            {
                if (Input.GetKeyDown("d")) ExtendMe(1, 0, 0);
                if (Input.GetKeyDown("a")) ExtendMe(-1, 0, 0);
                if (Input.GetKeyDown("w")) ExtendMe(0, 0, 1);
                if (Input.GetKeyDown("s")) ExtendMe(0, 0, -1);
                if (Input.GetKeyDown("e")) ExtendMe(0, 1, 0);
                if (Input.GetKeyDown("q")) ExtendMe(0, -1, 0);
            }
            else if (Input.GetKey("r"))
            {
                if (Input.GetKeyDown("d")) RotateMe(0, 45, 0);
                if (Input.GetKeyDown("a")) RotateMe(0, -45, 0);
                if (Input.GetKeyDown("w")) RotateMe(45, 0, 0);
                if (Input.GetKeyDown("s")) RotateMe(-45, 0, 0);
                if (Input.GetKeyDown("e")) RotateMe(0, 0, 45);
                if (Input.GetKeyDown("q")) RotateMe(0, 0, -45);
            }
            else
            {
                if (Input.GetKeyDown("d")) MoveMe(1, 0, 0);
                if (Input.GetKeyDown("a")) MoveMe(-1, 0, 0);
                if (Input.GetKeyDown("w")) MoveMe(0, 0, 1);
                if (Input.GetKeyDown("s")) MoveMe(0, 0, -1);
                if (Input.GetKeyDown("e")) MoveMe(0, 1, 0);
                if (Input.GetKeyDown("q")) MoveMe(0, -1, 0);

                if (Input.GetKeyDown(KeyCode.Delete)) DeleteMe();
            }
        }
    }

    public void ClickToSelect()
    {
        Camera cameraComponent = GameObject.Find("Main Camera").GetComponent<Camera>();
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)
            && hit.transform.gameObject.CompareTag("Piece")
            && !EventSystem.current.IsPointerOverGameObject())
        {
            SelectedObject = hit.transform.gameObject;
            ClickToSpawnCube.CursorCube.GetComponent<Renderer>().material = SelectedCursorMaterial;

        }
    }

    public void DisplayEditInstructions()
    {
        EditInstructions.SetActive(true);
    }

    public void HideEditInstructions()
    {
        EditInstructions.SetActive(false);
    }

    public void MoveMe(float amountX, float amountY, float amountZ)
    {
        SelectedObject.transform.position += new Vector3(amountX, amountY, amountZ);
        ClickToSpawnCube.CursorCube.transform.position = SelectedObject.transform.position + new Vector3(0f, 0.5f, 0f);
        AudioSource.Play();
    }

    public void ExtendMe(float amountX, float amountY, float amountZ)
    {
        //Y amount always 0, for some reason scaling up in Y automatically moves the position, I'm guessing because the floor is solid and can't overlap.
        SelectedObject.transform.position += new Vector3(amountX / 2, 0, amountZ / 2);
        SelectedObject.transform.localScale += new Vector3(amountX, amountY, amountZ);
        //If we don't set Y to 0 above then we need this line to cancel it out after.
        //if (amountY != 0) SelectedObject.transform.position += new Vector3(0, -amountY/2, 0);

        //The cursor doesn't interact with the solid floor however, so should be transformed up.
        //But no amount of transforming up will work because of the ClickToSpawnCube.update calling ShowHighlighted();
        //The position of the cursor moves from the same point each time

        ClickToSpawnCube.CursorCube.transform.position = SelectedObject.transform.position + new Vector3(0, 0.5f, 0);
        ClickToSpawnCube.CursorCube.transform.localScale = SelectedObject.transform.localScale;
        ClickToSpawnCube.CursorCube.transform.rotation = SelectedObject.transform.rotation;

        AudioSource.Play();
    }
    

    public void RotateMe(float amountX, float amountY, float amountZ)
    {
        SelectedObject.transform.Rotate(amountX, amountY, amountZ, 0f);
        AudioSource.Play();
    }

    public void DeleteMe()
    {
        Destroy(SelectedObject);
        //SelectedObject = null;

        AudioSource.Play();
    }

}
