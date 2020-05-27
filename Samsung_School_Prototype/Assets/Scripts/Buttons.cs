using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Buttons : MonoBehaviour
{

    MouseLook controlScript;
    public Inventory inventoryScript = null;
    public GameObject flashlight;
    // Start is called before the first frame update
    void Start()
    {
        //inventoryScript = FindObjectOfType<Inventory>();
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
            case "Inv_Right":
                inventoryScript.inv_right = true;
                inventoryScript.rotation = new Vector3(0, 0, 0);
                Debug.Log("Inv_right");
                break;
            case "Inv_Left":
                inventoryScript.inv_left = true;
                inventoryScript.rotation = new Vector3(0, 0, 0);
                Debug.Log("Inv_left");
                break;
            case "flashlightButton":
                flashlight.SetActive(!flashlight.activeSelf);
                controlScript.flashlight = true;
                Debug.Log("flashligh");
                break;
        }
    }
}
