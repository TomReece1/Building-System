using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFreeCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<FreeFlyCamera>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            //ToggleFree();
        }
    }

    public void ToggleFree()
    {
        if (GetComponent<FreeFlyCamera>().enabled == true)
        {
            GetComponent<FreeFlyCamera>().enabled = false;
        }
        else
        {
            GetComponent<FreeFlyCamera>().enabled = true;
        }
    }
}
