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
    public ObjectRotate obj; //пустая ссылка для экземпляра класса ObjectInspect. в коде пригодится дальше
    private RigidbodyFirstPersonController rb_move;
    public bool change = false;
    public Inventory inventory;
    MouseLook controlScript;

    private void Start()
    {
        rb_move = GetComponent<RigidbodyFirstPersonController>();
        controlScript = FindObjectOfType<RigidbodyFirstPersonController>().mouseLook;
    }


    void Update()
    {
        if (controlScript.inv_button) //если нажата кнопка I, то показывается инвентарь
        {
            UICamera.SetActive(!UICamera.activeSelf);
            rb_move.enabled = !rb_move.enabled;
            inventory.enabled = !inventory.enabled;
            inventory.scale = 0.5f;
            items_mesh[0].GetComponent<ObjectRotate>().enabled = !items_mesh[0].GetComponent<ObjectRotate>().enabled;
            //inventory.update_Text();
            change = true;
            controlScript.inv_button = false;
            controlScript.setRotateTouchFalse();
        }

        if (obj != null && obj.win) //если объект паззла ещё в руках, а сам паззл деактивирован (головоломка решена)
        {
            items.Remove(puzzle.gameObject.name);
            items_description.Remove(obj.description);
            items.Add(obj.reward.gameObject.name); //прибавляем в конец списка награду за решение паззла. Obj заполнится дальше в коде

            Debug.Log("got" + items[items.Count - 1]);

            items_mesh.Add(obj.reward);
            Destroy(puzzle);
            inventory.scale = 0.5f;
            items_mesh[0].GetComponent<ObjectRotate>().enabled = true;
            obj.win = false;
            //inventory.update_Text();
            change = true;
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
