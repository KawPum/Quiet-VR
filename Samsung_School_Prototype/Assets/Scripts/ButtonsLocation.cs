using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsLocation : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        float scale = 0;
        float x = 0;
        float y = 0;
        switch (transform.name)
        {
            case "MobileJoystick":
                scale = Screen.height * 0.00215f;
                x = Screen.width * 0.15f;
                y = Screen.height * 0.3f;
                break;
            case "Down_button":
                scale = Screen.height * 0.06f;
                x = Screen.width * 0.9f;
                y = Screen.height * 0.2f;
                break;
            case "Inv_Button":
                scale = Screen.height * 0.06f;
                x = Screen.width * 0.90f;
                y = Screen.height * 0.8f;
                break;
            case "InvRight":
                scale = Screen.height * 0.06f;
                x = Screen.width * 0.90f;
                y = Screen.height * 0.5f;
                break;
            case "InvLeft":
                scale = Screen.height * 0.06f;
                x = Screen.width * 0.10f;
                y = Screen.height * 0.5f;
                break;
            case "flashlightButton":
                scale = Screen.height * 0.06f;
                x = Screen.width * 0.90f;
                y = Screen.height * 0.5f;
                break;
        }
        transform.position = new Vector3(x, y, 0);
        transform.localScale = new Vector3(scale, scale, 0);
        transform.GetComponent<ButtonsLocation>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
