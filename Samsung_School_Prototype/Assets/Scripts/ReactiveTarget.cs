using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ReactiveTarget : MonoBehaviour
{
    public int ObjectId;
    public float deltaPostion = 10f;
    Transform parent;
    Color col;
    public int opened = 0;
    float angle;
    float changePosition;
    float posOpen;
    float posClose;
    DoorScript door;
    MouseLook controlScript;

    // Start is called before the first frame update
    void Start()
    {
        door = gameObject.GetComponent<DoorScript>();
        parent = transform.parent;
        col = transform.gameObject.GetComponent<Renderer>().material.color;
        angle = transform.eulerAngles.y;
        posClose = transform.localPosition.z;
        posOpen = posClose + deltaPostion;
        changePosition = posOpen * opened - (posClose * (opened - 1));
        controlScript = FindObjectOfType<RigidbodyFirstPersonController>().mouseLook;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.tag != "WardrobeShake"))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z), 2f * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, changePosition), 2f * Time.deltaTime);
        }
    }

    public void setOriginalColor()
    {
        transform.gameObject.GetComponent<Renderer>().material.color = col;
    }


    public string React()
    {
        
        transform.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.96f, 0.321f, 0);
        if (parent.gameObject.tag == "Wardrobe")
        {
            if (ObjectId != 0)
            {
                if (controlScript.click)
                {
                    parent.GetComponent<WardrobeSecret>().openCases(ObjectId);
                }
                else return ("открыть/закрыть");
            }
            else
            {
                string text = parent.GetComponent<WardrobeSecret>().getTextOpened();
                if ((text == "открыть/закрыть") && controlScript.click)
                {
                    parent.GetComponent<WardrobeSecret>().openSecretCase();
                }
                if (text == "заперто") transform.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.48f, 0.46f, 0);
                return text;
            }
        }

        if (transform.tag == "Door")
        {
            if (door == null && controlScript.click)
            {
                opened = (opened + 1) % 2;
                angle -= ((ObjectId - 0.5f) * 180) * (opened - 0.5f) * 2;
            }
            else if (door != null)
            {
                if (!door.closed && controlScript.click)
                {
                    opened = (opened + 1) % 2;
                    angle -= ((ObjectId - 0.5f) * 180) * (opened - 0.5f) * 2;
                }
                else if (door.closed)
                {
                    transform.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.48f, 0.46f, 0);
                    return "Заперто! Нужен ключ.";
                }
            }
            return "открыть/зкарыть";
        }
        if (transform.tag == "Case")
        {
            if (controlScript.click)
            {
                opened = (opened + 1) % 2;
                changePosition = posOpen * opened - (posClose * (opened - 1));
            }
            return ("открыть/закрыть");
        }

        if (transform.tag == "WardrobeShake")
        {
            if (controlScript.click)
            {
                FindObjectOfType<RigidbodyFirstPersonController>().enabled = false;
                parent.GetComponent<Shaking>().enabled = true;
                parent.GetComponent<Shaking>().score = 0;
                parent.GetComponent<Shaking>().got = 0;
                parent.GetComponent<Shaking>().StopReactive(false);
            }
            return ("трясти");
        }

        return "";
    }

}