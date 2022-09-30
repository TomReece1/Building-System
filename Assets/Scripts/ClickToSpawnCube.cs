using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToSpawnCube : MonoBehaviour
{
    public EventSystem EventSystem;
    public Editor Editor;
    public GameObject EditInstructions;

    public GameObject CubePrefab;
    public GameObject RampPrefab;
    public GameObject TreePrefab;
    public GameObject BridgePrefab;
    public GameObject ArchPrefab;

    public GameObject CursorCubePrefab;
    public Material EditCursorMaterial;

    public GameObject BuildTickImage;
    public GameObject EditTickImage;
    public GameObject GridTickImage;

    public GameObject TickImage1;
    public GameObject TickImage2;
    public GameObject TickImage3;
    public GameObject TickImage4;
    public GameObject TickImage5;


    public GameObject CursorCube;

    public bool useGrid = true;
    public bool buildMode = true;
    public bool firstClick = true;

    public int SelectedShapeNum = 1;

    public GameController GameController;

    private void Awake()
    {
        EventSystem = FindObjectOfType<EventSystem>();
        CursorCube = Instantiate(CursorCubePrefab);
        Editor = GameObject.Find("Editor").GetComponent<Editor>();
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (!GameController.playMode) 

        {
            if (Input.GetKeyDown("m")) ToggleMode();
            if (Input.GetKeyDown("g")) ToggleGrid();

        if (Input.GetKeyDown("1") || Input.GetKeyDown("2") || Input.GetKeyDown("3") || Input.GetKeyDown("4") || Input.GetKeyDown("5"))
        {
            TickImage1.SetActive(false);
            TickImage2.SetActive(false);
            TickImage3.SetActive(false);
            TickImage4.SetActive(false);
            TickImage5.SetActive(false);
        }

            if (Input.GetKeyDown("1"))
        {
            SelectedShapeNum = 1;
            TickImage1.SetActive(true);
        }
        if (Input.GetKeyDown("2"))
        {
            SelectedShapeNum = 2;
            TickImage2.SetActive(true);
        }
        if (Input.GetKeyDown("3"))
        {
            SelectedShapeNum = 3;
            TickImage3.SetActive(true);
        }
        if (Input.GetKeyDown("4"))
        {
            SelectedShapeNum = 4;
            TickImage4.SetActive(true);
        }
        if (Input.GetKeyDown("5"))
        {
            SelectedShapeNum = 5;
            TickImage5.SetActive(true);
        }

        if (buildMode)
            {
                if (Input.GetMouseButtonDown(0)) SpawnShape(SelectedShapeNum);

            ShowCursor();
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) Editor.ClickToSelect();

                ShowHighlighted();
            }
        }
    }

    void SpawnShape(int shapeNum)
    {
        Camera cameraComponent = GameObject.Find("Main Camera").GetComponent<Camera>();
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3Int gridVector;
        
        if (Physics.Raycast(ray, out hit, 100) && !EventSystem.current.IsPointerOverGameObject())
        {
            gridVector = Vector3Int.RoundToInt(hit.point);
            GameObject shapeToSpawn = null;
            if (shapeNum == 1) shapeToSpawn = Instantiate(CubePrefab);
            if (shapeNum == 2) shapeToSpawn = Instantiate(RampPrefab);
            if (shapeNum == 3) shapeToSpawn = Instantiate(TreePrefab);
            if (shapeNum == 4) shapeToSpawn = Instantiate(BridgePrefab);
            if (shapeNum == 5) shapeToSpawn = Instantiate(ArchPrefab);
            if (useGrid) shapeToSpawn.transform.position = gridVector;
            else shapeToSpawn.transform.position = hit.point;
        }
    }

    void ShowCursor()
    {
        Camera cameraComponent = GameObject.Find("Main Camera").GetComponent<Camera>();
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3Int gridVector;

        if (Physics.Raycast(ray, out hit, 100) && !EventSystem.current.IsPointerOverGameObject())
        {
            gridVector = Vector3Int.RoundToInt(hit.point);
                if (useGrid) CursorCube.transform.position = new Vector3(0, 0.5f, 0) + gridVector;
                else CursorCube.transform.position = new Vector3(0, 0.5f, 0) + hit.point;
        }
    }

    public void ShowHighlighted()
    {
        Camera cameraComponent = GameObject.Find("Main Camera").GetComponent<Camera>();
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)
            && hit.transform.gameObject.CompareTag("Piece")
            && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject pieceToHighlight = hit.transform.gameObject;
            CursorCube.transform.position = pieceToHighlight.transform.position + new Vector3(0f,0.5f,0f);
            CursorCube.transform.localScale = pieceToHighlight.transform.localScale + new Vector3(0.2f, 0.2f, 0.2f); 
            CursorCube.transform.rotation = pieceToHighlight.transform.rotation;
        }
    }

    void ResetCursor()
    {
        Destroy(CursorCube);
        CursorCube = Instantiate(CursorCubePrefab);
    }

    void MakeEditCursor()
    {
        CursorCube.GetComponent<Renderer>().material = EditCursorMaterial;
        
    }

    public void ToggleGrid()
    {
        if (useGrid)
        {
            useGrid = false;
            GridTickImage.SetActive(false);
        }
        else
        {
            useGrid = true;
            GridTickImage.SetActive(true);
        }
    }

    public void ToggleMode()
    {
        if (buildMode)
        {
            buildMode = false;
            Editor.DisplayEditInstructions();
            MakeEditCursor();
            EditTickImage.SetActive(true);
            BuildTickImage.SetActive(false);
        }
        else
        {
            buildMode = true;
            Editor.HideEditInstructions();
            ResetCursor();
            EditTickImage.SetActive(false);
            BuildTickImage.SetActive(true);
        }
    }



}
