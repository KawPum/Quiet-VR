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
    Transform cubePuzzle;

    // Start is called before the first frame update
    void Start()
    {
        door = gameObject.GetComponent<DoorScript>();
        parent = transform.parent;
        if (!transform.CompareTag("Puzzle")) col = transform.gameObject.GetComponent<Renderer>().material.color;
        else
        {
            foreach(Transform i in transform.GetComponentsInChildren<Transform>())
            {
                if(i.gameObject.name == "Cube (1)")
                {
                    cubePuzzle = i;
                    break;
                }
            }
            col = cubePuzzle.gameObject.GetComponent<Renderer>().material.color;
        }
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
        if (!transform.CompareTag("Puzzle")) transform.gameObject.GetComponent<Renderer>().material.color = col;
        else cubePuzzle.gameObject.GetComponent<Renderer>().material.color = col;
    }


    public string React()
    {
        if(!transform.CompareTag("Puzzle"))transform.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.96f, 0.321f, 0);
        else cubePuzzle.gameObject.GetComponent<Renderer>().material.color = col;
        if ((transform.parent != null) && (parent.gameObject.tag == "Wardrobe"))
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


        Debug.Log(transform.tag);
        //if (transform.CompareTag("Puzzle")) //если же тэг другого объекта равен паззлу
        //{
        //    puzzle = transform.gameObject; //закидываем в паззл ссылку на этот объект
        //    if (controlScript.click)
        //    {
        //        items.Add(puzzle.gameObject.name);
        //        cam.Toast("Получен: " + puzzle.gameObject.name);
        //        puzzle.transform.position = new Vector3(0f, 0f, 0f);
        //        foreach (Transform child in puzzle.GetComponentsInChildren<Transform>())
        //        {
        //            child.gameObject.layer = 5;
        //        }
        //        obj = puzzle.GetComponent<ObjectRotate>(); // кидаем в ссылку компонент ObjectInspect паззла
        //        items_mesh.Add(puzzle);
        //        obj.inHand = true; // сообщаем компоненту, что паззл в руках
        //                           //inventory.update_Text();
        //        change = true;
        //    }
        //}

        //if (other.CompareTag("Key") && controlScript.click) //если тэг объекта равен включу и нажата кнопка
        //{
        //    items.Add(other.name); //добавляем в конец списка с ключами новый ключ
        //    items_mesh.Add(other.gameObject);
        //    other.GetComponent<ObjectRotate>().inHand = true;
        //    cam.Toast("Получен: " + other.name);
        //    other.SetActive(false); //скрываем объект ключа, типа он удалился/исчез/мы его взяли
        //                            //inventory.update_Text();
        //    change = true;
        //}

        //if (other.CompareTag("Door") && controlScript.click) //если дверь и кнопка нажата
        //{
        //    string name = "";
        //    for (int i = 0; i < other.gameObject.name.Length - 5; i++) //у двери должно быть имя типа "*_Door", перебираем каждую букву имени объекта двери кроме последних 5
        //    {
        //        name += other.gameObject.name[i]; //записываем в стрингу имя двери без _Door
        //    }
        //    for (int i = 0; i < items.Count; i++)
        //    {
        //        if (items[i] == name) //если имя ключа совпадает с именем двери. Имена соответствующих ключей и дверей должны отличаться только последними 5ю символами _Door
        //        {
        //            items.Remove(name); //удаляем ключ из списка
        //            items_mesh.Remove(items_mesh[i]);
        //            cam.Toast("Использован: " + name);
        //            other.GetComponent<DoorScript>().closed = false;
        //        }
        //    }
        //}

        return "";
    }

}