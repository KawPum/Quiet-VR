using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ReactiveTarget : MonoBehaviour
{
    public int ObjectId;
    public float deltaPostion = 10f;
    Transform parent;
    //Color col;
    public int opened = 0;
    float angle;
    float changePosition;
    float posOpen;
    float posClose;
    DoorScript door;
    MouseLook controlScript;
    Transform changeColorObject;
    Material originalMaterial;
    PlayerQuestStuff playerQuestStuff;
    GameObject inv_add;

    // Start is called before the first frame update
    void Start()
    {
        door = gameObject.GetComponent<DoorScript>();
        parent = transform.parent;
        changeColorObject = getChangeColorObject();
        originalMaterial = changeColorObject.gameObject.GetComponent<Renderer>().material;
        angle = transform.eulerAngles.y;
        posClose = transform.localPosition.z;
        posOpen = posClose + deltaPostion;
        changePosition = posOpen * opened - (posClose * (opened - 1));
        controlScript = FindObjectOfType<RigidbodyFirstPersonController>().mouseLook;
        playerQuestStuff = FindObjectOfType<PlayerQuestStuff>();
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
        changeColorObject.gameObject.GetComponent<Renderer>().material = originalMaterial;
    }

    Transform getChangeColorObject()
    {
        if (!transform.CompareTag("Puzzle")) return transform;
        else
        {
            foreach (Transform i in transform.GetComponentsInChildren<Transform>())
            {
                if (i.gameObject.tag == "ChangeColorElement") return i;
            }
        }
        return null;
    }


    public string React()
    {
        changeColorObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
        changeColorObject.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.96f, 0.321f, 0);

        if ((transform.parent != null) && (parent.gameObject.tag == "Wardrobe"))
        {
            if (ObjectId != 0)
            {
                return ("открыть/закрыть");
            }
            else
            {
                string text = parent.GetComponent<WardrobeSecret>().getTextOpened();
                if (text == "заперто") changeColorObject.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.48f, 0.46f, 0);
                return text;
            }
        }


        switch (transform.tag)
        {
            case "Door":
                if ((door != null) && (door.closed))
                {
                    changeColorObject.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.48f, 0.46f, 0);
                    return "Заперто! Нужен ключ.";
                }
                return "открыть/зкарыть";

            case "Case":
                return ("открыть/закрыть");

            case "WardrobeShake":
                return ("трясти");

            case "Puzzle":
            case "Photo":
                return "Взять";
        }
        return "";
    }

    public void clickReact()
    {
        switch (transform.tag)
        {

            case "Door":
                string name = "";
                for (int i = 0; i < transform.name.Length - 5; i++) //у двери должно быть имя типа "*_Door", перебираем каждую букву имени объекта двери кроме последних 5
                {
                    name += transform.name[i]; //записываем в стрингу имя двери без _Door
                }
                for (int i = 0; i < playerQuestStuff.items.Count; i++)
                {
                    if (playerQuestStuff.items[i] == name) //если имя ключа совпадает с именем двери. Имена соответствующих ключей и дверей должны отличаться только последними 5ю символами _Door
                    {
                        playerQuestStuff.items.Remove(name); //удаляем ключ из списка
                        playerQuestStuff.items_mesh.Remove(playerQuestStuff.items_mesh[i]);
                        playerQuestStuff.cam.Toast("Использован: " + name);
                        transform.GetComponent<DoorScript>().closed = false;
                    }
                }
                if (door == null)
                {
                    opened = (opened + 1) % 2;
                    angle -= ((ObjectId - 0.5f) * 180) * (opened - 0.5f) * 2;
                }
                else if (door != null && !door.closed)
                {
                    opened = (opened + 1) % 2;
                    angle -= ((ObjectId - 0.5f) * 180) * (opened - 0.5f) * 2;
                }
                break;


            case "Case":
                opened = (opened + 1) % 2;
                changePosition = posOpen * opened - (posClose * (opened - 1));
                break;

            case "WardrobeShake":
                FindObjectOfType<RigidbodyFirstPersonController>().enabled = false;
                parent.GetComponent<Shaking>().enabled = true;
                parent.GetComponent<Shaking>().score = 0;
                parent.GetComponent<Shaking>().got = 0;
                parent.GetComponent<Shaking>().StopReactive(false);
                break;

            case "Puzzle":
            case "Photo":
                inv_add = transform.gameObject; //закидываем в obj ссылку на этот объект
                inv_add.transform.position = new Vector3(0f, 0f, 0f);
                playerQuestStuff.items.Add(inv_add.gameObject.name);
                playerQuestStuff.cam.Toast("Получен: " + inv_add.gameObject.name);
                foreach (Transform child in inv_add.GetComponentsInChildren<Transform>())
                {
                    child.gameObject.layer = 5;
                }
                playerQuestStuff.obj = inv_add.GetComponent<ObjectRotate>(); // кидаем в ссылку компонент ObjectInspect паззла
                playerQuestStuff.items_mesh.Add(inv_add);
                playerQuestStuff.obj.inHand = true; // сообщаем компоненту, что паззл в руках
                                                    //inventory.update_Text();
                playerQuestStuff.change = true;
                break;

            case "Key":
                playerQuestStuff.items.Add(transform.name); //добавляем в конец списка с ключами новый ключ
                playerQuestStuff.items_mesh.Add(transform.gameObject);
                transform.GetComponent<ObjectRotate>().inHand = true;
                playerQuestStuff.cam.Toast("Получен: " + transform.name);
                transform.gameObject.SetActive(false); //скрываем объект ключа, типа он удалился/исчез/мы его взял
                playerQuestStuff.change = true;
                break;
        }

        if ((transform.parent != null) && (parent.gameObject.tag == "Wardrobe"))
        {
            if (ObjectId != 0)
            {
                    parent.GetComponent<WardrobeSecret>().openCases(ObjectId);
            }
            else
            {
                string text = parent.GetComponent<WardrobeSecret>().getTextOpened();
                if (text == "открыть/закрыть")
                {
                    parent.GetComponent<WardrobeSecret>().openSecretCase();
                }
            }
        }

    }
}