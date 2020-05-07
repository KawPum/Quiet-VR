using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Inventory : MonoBehaviour
{
    //private GameObject player;
    PlayerQuestStuff player_object;
    //private
    Text[] names_output = new Text[5];
    private GameObject subject;
    public Text description;

    bool inv_left = false;
    bool inv_right = false;
    float touchStartPosition = 0;

    void Start()
    {
        player_object = FindObjectOfType<PlayerQuestStuff>();
        string a = "";
        for (int i = 0; i < 5; i++)
        {
            names_output[i] =  transform.GetChild(i).GetComponent<Text>();
            names_output[i].color = Color.white;
        }
        update_Text();
    }


    void Update()
    {

        inventoryControl();

        if (player_object.change)
        {
            update_Text();
            player_object.change = false;
        }

        if (inv_right)
        {
            inv_right = false;
            switch_Item(1);
        }

        if (inv_left)
        {
            inv_left = false;
            switch_Item(-1);
        }
    }

    void inventoryControl()
    {
        if (Input.touches.Length == 1)
        {
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = Input.touches[0].position.x;
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Height " + Screen.height + " Width " + Screen.width);
                    Debug.Log(touchStartPosition);
                    if (Input.touches[0].position.x - touchStartPosition > Screen.width / 30f) inv_left = true;
                    else if (touchStartPosition - Input.touches[0].position.x > Screen.width / 30f) inv_right = true;
                    Debug.Log("right " + inv_right);
                    Debug.Log("left " + inv_left);
                    break;
            }
        }
    }

    void switch_Item(int step)
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
            if (i == player_object.items.Count) break;
            Debug.Log("i    " + names_output[i].text);
            if (i+2<5) names_output[i].text = player_object.items[i];
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
