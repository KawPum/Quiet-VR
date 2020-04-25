using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour // писал Милованов Еремей
{
    public PlayerQuestStuff player_object;
    private Text[] names_output = new Text[5];
    private GameObject subject;
    public Text description;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            names_output[i] = transform.GetChild(i).GetComponent<Text>();
            names_output[i].color = Color.white;
        }
        update_Text();
    }


    void Update()
    {

        if (player_object.change)
        {
            update_Text();
            player_object.change = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) switch_Item(1);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) switch_Item(-1);
    }

    void switch_Item(int step) // сдвиг массива с предметами в зависимости от значения шага. Алгоритм сдвига взят с форума
    {
        Destroy(subject.gameObject);
        string temp;
        GameObject temp_mesh;
        if (step < 0) //move to the right
        {
            temp = player_object.items[player_object.items.Count-1];
            temp_mesh = player_object.items_mesh[player_object.items.Count - 1];
            //  Debug.Log(temp);
            for (int i = player_object.items.Count-1; i > 0; i--)
            {
                player_object.items[i] = player_object.items[i - 1];
                player_object.items_mesh[i] = player_object.items_mesh[i - 1];
            }
            // Debug.Log(temp);
            player_object.items[0] = temp;
            player_object.items_mesh[0] = temp_mesh;
        }
        else if (step > 0)
        {
            temp = player_object.items[0];
            temp_mesh = player_object.items_mesh[0];
            //  Debug.Log(temp);
            for (int i = 0; i < player_object.items.Count-1; i++)
            {
                player_object.items[i] = player_object.items[i+1];
                player_object.items_mesh[i] = player_object.items_mesh[i + 1];
            }
            player_object.items[player_object.items.Count-1] = temp;
            player_object.items_mesh[player_object.items.Count - 1] = temp_mesh;
        }
        update_Text();
    }

    public void update_Text()
    {
        foreach (Transform child in player_object.item_position.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<ObjectRotate>() != null) Destroy(child.gameObject);
        }

        for (int i = 0; i < 5; i++)
        {
            if (i >= player_object.items.Count) break;
            if (i+2<5)
            names_output[i].text = player_object.items[i];
            else names_output[i].text = player_object.items[player_object.items.Count-(5-i)];
        }

        if (player_object.items_mesh.Count > 0)
        {
            subject = Instantiate(player_object.items_mesh[0]);
            subject.transform.parent = player_object.item_position;
            subject.transform.localPosition = new Vector3(0, 0, 0);
            subject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            subject.SetActive(true);

            ObjectRotate obj = player_object.items_mesh[0].GetComponent<ObjectRotate>();
            if (obj != null) description.text = obj.description;
            
            subject.layer = 5;

            foreach (Transform child in subject.transform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 5;
            }
        }
    }

}
