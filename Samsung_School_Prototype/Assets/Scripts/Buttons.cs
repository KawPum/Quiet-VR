using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Buttons : MonoBehaviour
{

    MouseLook controlScript;
    // Start is called before the first frame update
    void Start()
    {
        controlScript = FindObjectOfType<RigidbodyFirstPersonController>().mouseLook;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        controlScript.click = false;
        switch (transform.tag)
        {
            case "Inventory_Button":
                controlScript.inv_button = true;
                Debug.Log("inventory button");
                break;
            case "Down_Button":
                controlScript.down_button = 1;
                Debug.Log("down button");
                break;
        }
    }
}
