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
    public GameObject CursorCubePrefab;
    public Material EditCursorMaterial;

    public GameObject BuildTickImage;
    public GameObject EditTickImage;
    public GameObject GridTickImage;
    

    public GameObject CursorCube;

    public bool useGrid = true;
    public bool buildMode = true;
    public bool firstClick = true;

    private void Awake()
    {
        EventSystem = FindObjectOfType<EventSystem>();
        CursorCube = Instantiate(CursorCubePrefab);
        Editor = GameObject.Find("Editor").GetComponent<Editor>();
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && firstClick) firstClick = false;
        //else
        //{
            if (Input.GetKeyDown("m")) ToggleMode();
            if (Input.GetKeyDown("g")) ToggleGrid();
            if (buildMode)
            {
                if (Input.GetMouseButtonDown(0)) SpawnShape("Cube");
                if (Input.GetMouseButtonDown(1)) SpawnShape("Ramp");

                ShowCursor();
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) Editor.ClickToSelect();

                ShowHighlighted();
            }
        //}
    }

    void SpawnShape(string shape)
    {
        Camera cameraComponent = GameObject.Find("Main Camera").GetComponent<Camera>();
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3Int gridVector;
        
        if (Physics.Raycast(ray, out hit, 100) && !EventSystem.current.IsPointerOverGameObject())
        {
            gridVector = Vector3Int.RoundToInt(hit.point);
            GameObject shapeToSpawn = null;
            if (shape == "Cube") shapeToSpawn = Instantiate(CubePrefab);
            if (shape == "Ramp") shapeToSpawn = Instantiate(RampPrefab);
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
