using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestStuff : MonoBehaviour
{
    public GameObject inventory; // inventory ui, здесь ссылка на канвас, отвечающий за весь ЮИ
    private GameObject puzzle; // empty object for puzzle
    public Transform item_position; // empty object for puzzle's location
    public List<string> keys; //array of keys
    public List<string> items; // array of items
    public Text keyText; // output for keys and items
    public Text itemsText; //текст, отвечающий за отображение инфы о предметах
    public GameObject cam; //ссылка на камеру
    private ObjectInspect obj; //пустая ссылка для экземпляра класса ObjectInspect. в коде пригодится дальше


    void Update()
    {
        if (Input.GetKey(KeyCode.I) || OVRInput.Get(OVRInput.RawButton.B)) //если нажата кнопка I, то показывается инвентарь
        {
            inventory.SetActive(true);

        }
        else inventory.SetActive(false);

        if (puzzle != null && puzzle.activeSelf == false) //если объект паззла ещё в руках, а сам паззл деактивирован (головоломка решена)
        {
            items.Add(obj.reward); //прибавляем в конец списка награду за решение паззла. Obj заполнится дальше в коде
            Debug.Log("got" + items[items.Count - 1]);
            itemsText.text = ""; //обнуляем текст
            for (int i = 0; i < items.Count; i++)
            {
                itemsText.text += items[i] + "\n"; // выводим в каждую строку текста по одному элементу инвентаря с предметами
            }
            puzzle = null; //обнуляем ссылку на паззл
        }
    }

    private void OnTriggerStay(Collider other) //пока коллайдер этого объекта находится в триггере
    {
        if (other.gameObject.CompareTag("Puzzle")) //если же тэг другого объекта равен паззлу
        {
            puzzle = other.gameObject; //закидываем в паззл ссылку на этот объект

            if ((Input.GetKeyDown(KeyCode.E) || OVRInput.Get(OVRInput.RawButton.Any)) && puzzle.transform.parent != this.transform)
            {
                puzzle.transform.parent = this.transform; //паззл становится становится дочерним по отношению к объекту с этим скриптом
                puzzle.transform.position = item_position.position; //переносим паззл в место для паззла
                puzzle.transform.LookAt(cam.transform); // вращаем в сторону камеры
                puzzle.transform.Rotate(Vector3.up, 180f); //разворачиваем на 180 относительно ОУ
                puzzle.transform.Rotate(Vector3.right, -30f); // наклоняем на -30 по ОХ к игроку
                obj = puzzle.GetComponent<ObjectInspect>(); // кидаем в ссылку компонент ObjectInspect паззла
                obj.inHand = true; // сообщаем компоненту, что паззл в руках
            }
        }

        if (other.CompareTag("Key") && (Input.GetKeyDown(KeyCode.E) || OVRInput.Get(OVRInput.RawButton.RShoulder))) //если тэг объекта равен включу и нажата кнопка
        {
            keys.Add(other.name); //добавляем в конец списка с ключами новый ключ
           // Debug.Log(keys.Count + "keys capacity");
            keyText.text = ""; //обнуляем текст с ключами
            for (int i = 0; i < keys.Count; i++)
            {
                keyText.text += keys[i] + "\n"; // в каждую строку текста выводим название ключа
            }
            other.gameObject.SetActive(false); //скрываем объект ключа, типа он удалился/исчез/мы его взяли/ ну ты понял
        }

        if (other.CompareTag("Door") && (Input.GetKeyDown(KeyCode.E)||OVRInput.Get(OVRInput.RawButton.RShoulder))) //если дверь и кнопка нажата
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

}
