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
    public GameObject cam; //ссылка на камеру
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
        }

        if (obj.win) //если объект паззла ещё в руках, а сам паззл деактивирован (головоломка решена)
        {
            items.Remove(puzzle.gameObject.name);
            items_description.Remove(obj.description);
            items.Add(obj.reward.gameObject.name); //прибавляем в конец списка награду за решение паззла. Obj заполнится дальше в коде
           
            Debug.Log("got" + items[items.Count - 1]);

            items_mesh.Add(obj.reward);
            Destroy(puzzle);
            obj.win = false;
            inventory.update_Text();
        }
    }

    private void OnTriggerStay(Collider other) //пока коллайдер этого объекта находится в триггере
    {
        if (other.gameObject.CompareTag("Puzzle")) //если же тэг другого объекта равен паззлу
        {
            puzzle = other.gameObject; //закидываем в паззл ссылку на этот объект
            if (Input.GetKeyDown(KeyCode.E))
            {
                items.Add(puzzle.gameObject.name);
                puzzle.transform.position = new Vector3(0f, 0f, 0f);
                foreach (Transform child in puzzle.GetComponentsInChildren<Transform>())
                {
                    child.gameObject.layer = 5;
                }
                obj = puzzle.GetComponent<ObjectRotate>(); // кидаем в ссылку компонент ObjectInspect паззла
                items_mesh.Add(puzzle);
               //puzzle.transform.parent = item_position;
               // puzzle.transform.localPosition = new Vector3(0, 0, 0);
               // puzzle.layer = 5;
               // foreach (Transform child in puzzle.transform.GetComponentsInChildren<Transform>())
               // {
              //      child.gameObject.layer = 5;
              //  }
                obj.inHand = true; // сообщаем компоненту, что паззл в руках
                inventory.update_Text();
            }
        }

        if (other.CompareTag("Key") && Input.GetKeyDown(KeyCode.E)) //если тэг объекта равен включу и нажата кнопка
        {
            keys.Add(other.name); //добавляем в конец списка с ключами новый ключ
            items_mesh.Add(other.gameObject);
           // Debug.Log(keys.Count + "keys capacity");
            keyText.text = ""; //обнуляем текст с ключами
            for (int i = 0; i < keys.Count; i++)
            {
                keyText.text += keys[i] + "\n"; // в каждую строку текста выводим название ключа
            }
            other.gameObject.SetActive(false); //скрываем объект ключа, типа он удалился/исчез/мы его взяли/ ну ты понял
        }

        if (other.CompareTag("Door") && Input.GetKeyDown(KeyCode.E)) //если дверь и кнопка нажата
        {
            string name = ""; 
            for (int i = 0; i < other.gameObject.name.Length-5; i++) //у двери должно быть имя типа "*_Door", перебираем каждую букву имени объекта двери кроме последних 5
            {
                name += other.gameObject.name[i]; //записываем в стрингу имя двери без _Door
            }
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i] == name) //если имя ключа совпадает с именем двери. Имена соответствующих ключей и дверей должны отличаться только последними 5ю символами _Door
                {
                    keys.Remove(name); //удаляем ключ из списка
                    items_mesh.Remove(other.gameObject);
                    other.gameObject.SetActive(false); //деактивируем дверь, чтобы она была невидимой
                    Debug.Log(keys.Count + "keys capacity");
                    keyText.text = ""; //обнуляем текст
                    for (int j = 0; j < keys.Count; j++)
                    {
                        keyText.text += keys[j] + "\n"; //в каждую строку выводим ключ
                    }
                }
            }
        }
    }

    public void get_Reward(ObjectRotate new_obj)
    {
        //items.Remove(new_obj.gameObject.name);
        //items_mesh.Remove(new_obj.gameObject);
        items[0] = new_obj.reward.gameObject.name;
        items_mesh[0] = new_obj.reward.gameObject;
     //   items.Add(new_obj.reward.gameObject.name); //прибавляем в конец списка награду за решение паззла. Obj заполнится дальше в коде

        Debug.Log("got" + items[items.Count - 1]);

      //  items_mesh.Add(new_obj.reward);
      //  Destroy(new_obj.gameObject);
        obj.win = false;
        change = true;
    }

}
