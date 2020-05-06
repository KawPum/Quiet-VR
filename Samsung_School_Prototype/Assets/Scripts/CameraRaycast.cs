using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraRaycast : MonoBehaviour 
{
    Camera camera;
    public int toastDuration = 3;
    GUIStyle style;
    string text = "";
    string toastText = "";
    int toastTime;
    GameObject lastHit = null;
    RigidbodyFirstPersonController player;
    PlayerQuestStuff quest;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        player = transform.parent.GetComponent<RigidbodyFirstPersonController>();
        quest = transform.parent.GetComponent<PlayerQuestStuff>();
        toastTime = toastDuration;
        style = new GUIStyle(GUI.skin.window);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject hitObject = null;

        RaycastHit hit;
        if ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 30f)))
        {
            hitObject = hit.transform.gameObject;
            ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
            ComputerScript comp = hitObject.GetComponent<ComputerScript>();
            if (target != null)
            {
                text = target.React();
                if ((lastHit != null) && (lastHit != hitObject))
                {
                    lastHit.GetComponent<ReactiveTarget>().setOriginalColor();
                    lastHit = null;
                }
                lastHit = hitObject;
            }
            else
            {
                if (lastHit != null)
                {
                    lastHit.GetComponent<ReactiveTarget>().setOriginalColor();
                    lastHit = null;
                }
                text = "";
            }

            if (comp != null)
            {
                text = comp.React();
                if (Input.GetMouseButton(0))
                {
                    player.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    quest.enabled = false;
                    player.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    player.enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    transform.parent.transform.position = new Vector3(hitObject.transform.position.x, 
                        hitObject.transform.position.y-12f, hitObject.transform.position.z + 16f);
                    transform.parent.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    text = "";
                }

                if (player.enabled == false && Input.GetKeyDown(KeyCode.Space))
                {
                    player.enabled = true;
                    quest.enabled = true;
                    Cursor.visible = false;
                    transform.parent.transform.position = new Vector3(hitObject.transform.position.x,
                        hitObject.transform.position.y-10f, transform.position.z +8f);
                    player.gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    player.gameObject.GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }
        else
        {
            if (lastHit != null)
            {
                lastHit.GetComponent<ReactiveTarget>().setOriginalColor();
                lastHit = null;
            }
            text = "";
        }
    }

    void OnGUI()
    {
        style = new GUIStyle(GUI.skin.window);
        int size = 12;
        float posX = camera.pixelWidth / 2 - size / 4;
        float posY = camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
        size = 100;
        //posX = camera.pixelWidth / 2 - size / 4;
        //posY = camera.pixelHeight / 8 - size / 2;
        GUI.Label(new Rect(posX, posY + camera.pixelHeight / 10, size, size), text);

        if (toastTime < toastDuration)
        {
            GUI.Label(new Rect(Screen.width / 1.33f - 100, 40, 200, 80), toastText, style);
            toastTime++;
        }
    }

    public void textChange(string t)
    {
        text = t;
    }

    public void Toast(string toastText1)
    {
        toastText = toastText1;
        toastTime = 0;
    }
}
