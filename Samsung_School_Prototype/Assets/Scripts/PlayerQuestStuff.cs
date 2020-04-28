using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerQuestStuff : MonoBehaviour 
{
    public GameObject UICamera; // inventory ui, здесь ссылка на канвас, отвечающий за весь ЮИ
    private GameObject puzzle; // empty object for puzzle
    public Transform item_position; // empty object for puzzle's location
    public  List<string> keys; //array of keys
    public  List<string> items; // array of items names
    public  List<GameObject> items_mesh; // array of 3d items
    public List<string> items_description;
    public Text keyText; // output for keys and items
    public Text itemsText; //текст, отвечающий за отображение инфы о предметах
    public CameraRaycast cam; //ссылка на камеру
    private ObjectRotate obj; //пустая ссылка для экземпляра класса ObjectInspect. в коде пригодится дальше
    private RigidbodyFirstPersonController rb_move;
    public bool change = false;
    public Inventory inventory;

    private void Start()
    {
        rb_move = GetComponent<RigidbodyFirstPersonController>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //если нажата кнопка I, то показывается инвентарь
        {
            UICamera.SetActive(!UICamera.activeSelf);
            rb_move.enabled = !rb_move.enabled;
            //inventory.update_Text();
            change = true;
        }

        if (obj != null && obj.win) //если объект паззла ещё в руках, а сам паззл деактивирован (головоломка решена)
        {
            items.Remove(puzzle.gameObject.name);
            items_description.Remove(obj.description);
            items.Add(obj.reward.gameObject.name); //прибавляем в конец списка награду за решение паззла. Obj заполнится дальше в коде
           
            Debug.Log("got" + items[items.Count - 1]);

            items_mesh.Add(obj.reward);
            Destroy(puzzle);
            obj.win = false;
            //inventory.update_Text();
            change = true;
        }

        RaycastHit hit;
        if ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 30f)))
        {
            GameObject other = hit.transform.gameObject;
            if (other.gameObject.CompareTag("Puzzle")) //если же тэг другого объекта равен паззлу
            {
                puzzle = other.gameObject; //закидываем в паззл ссылку на этот объект
                if (Input.GetMouseButtonDown(0))
                {
                    items.Add(puzzle.gameObject.name);
                    cam.Toast("Получен: " + puzzle.gameObject.name);
                    puzzle.transform.position = new Vector3(0f, 0f, 0f);
                    foreach (Transform child in puzzle.GetComponentsInChildren<Transform>())
                    {
                        child.gameObject.layer = 5;
                    }
                    obj = puzzle.GetComponent<ObjectRotate>(); // кидаем в ссылку компонент ObjectInspect паззла
                    items_mesh.Add(puzzle);
                    obj.inHand = true; // сообщаем компоненту, что паззл в руках
                    //inventory.update_Text();
                    change = true;
                }
            }

            if (other.CompareTag("Key") && Input.GetMouseButtonDown(0)) //если тэг объекта равен включу и нажата кнопка
            {
                items.Add(other.name); //добавляем в конец списка с ключами новый ключ
                items_mesh.Add(other.gameObject);
                other.GetComponent<ObjectRotate>().inHand = true;
                cam.Toast("Получен: " + other.name);
                other.SetActive(false); //скрываем объект ключа, типа он удалился/исчез/мы его взяли
                //inventory.update_Text();
                change = true;
            }

            if (other.CompareTag("Door") && Input.GetMouseButtonDown(0)) //если дверь и кнопка нажата
            {
                string name = "";
                for (int i = 0; i < other.gameObject.name.Length - 5; i++) //у двери должно быть имя типа "*_Door", перебираем каждую букву имени объекта двери кроме последних 5
                {
                    name += other.gameObject.name[i]; //записываем в стрингу имя двери без _Door
                }
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == name) //если имя ключа совпадает с именем двери. Имена соответствующих ключей и дверей должны отличаться только последними 5ю символами _Door
                    {
                        items.Remove(name); //удаляем ключ из списка
                        items_mesh.Remove(items_mesh[i]);
                        cam.Toast("Использован: " + name);
                        other.GetComponent<DoorScript>().closed = false;
                    }
                }
            }
        }
    }

    public void get_Reward(ObjectRotate new_obj)
    {
        items[0] = new_obj.reward.gameObject.name;
        items_mesh[0] = new_obj.reward.gameObject;

        Debug.Log("got" + items[items.Count - 1]);

        obj.win = false;
        change = true;
        //inventory.update_Text();
    }

    public void get_Reward(Shaking shkaf)
    {
        cam.Toast("Получен: " + shkaf.reward_name);
        items.Add(shkaf.reward_name);
        items_mesh.Add(shkaf.reward_mesh);
        //shkaf.reward_mesh.SetActive(false);
        change = true;
        shkaf.enabled = false;
        Debug.Log("Shaking   " + shkaf.enabled);
    }

    public void StopShaking(GameObject gO)
    {
        ///gO.GetComponent<Shaking>().enabled = false;
        Debug.Log("Shaking   " + gO.GetComponent<Shaking>().enabled);
    }

}
