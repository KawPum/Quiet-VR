using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    bool lastScale = false;

    public float scale = 0.5f;

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
        switch (Input.touches.Length)
        {


            case 1:
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        touchStartPosition = Input.touches[0].position.x;
                        break;
                    case TouchPhase.Ended:
                        if (!lastScale) { 
                            Debug.Log("Height " + Screen.height + " Width " + Screen.width);
                            Debug.Log(touchStartPosition);
                            if (Input.touches[0].position.x - touchStartPosition > Screen.width / 30f) inv_left = true;
                            else if (touchStartPosition - Input.touches[0].position.x > Screen.width / 30f) inv_right = true;
                            Debug.Log("right " + inv_right);
                            Debug.Log("left " + inv_left);
                            scale = 0.5f;
                        }
                        lastScale = false;
                        break;
                }
                break;

            case 2:
                if (Input.touches[1].phase == TouchPhase.Began)
                {
                    touchStartPosition = (float)Math.Pow((Math.Pow(Input.touches[0].position.x - Input.touches[1].position.x, 2) + Math.Pow(Input.touches[0].position.y - Input.touches[1].position.y, 2)), 0.5);
                    lastScale = true;
                }
                else if ((Input.touches[1].phase == TouchPhase.Moved) || (Input.touches[0].phase == TouchPhase.Moved))
                {
                    scale += (float)(Math.Pow((Math.Pow(Input.touches[0].position.x - Input.touches[1].position.x, 2) + Math.Pow(Input.touches[0].position.y - Input.touches[1].position.y, 2)), 0.5) - touchStartPosition) * 0.005f;
                    if (scale < 0.5f) scale = 0.5f;
                    else if (scale > 1.5f) scale = 1.5f;
                }
                break;
        }
    }

    void switch_Item(int step)
    {
        player_object.items_mesh[0].GetComponent<ObjectRotate>().enabled = false;
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
        player_object.items_mesh[0].transform.localScale = new Vector3(scale, scale, scale);
        player_object.items_mesh[0].GetComponent<ObjectRotate>().enabled = true;
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
            //Debug.Log("i    " + names_output[i].text);
            if (i+2<5) names_output[i].text = player_object.items[i];
            else names_output[i].text = player_object.items[player_object.items.Count-(5-i)];
        }

        if (player_object.items_mesh.Count > 0)
        {
            subject = Instantiate(player_object.items_mesh[0]);
            subject.transform.parent = player_object.item_position;
            subject.transform.localPosition = new Vector3(0, 0, 0);
            //subject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            subject.SetActive(true);

            ObjectRotate obj = player_object.items_mesh[0].GetComponent<ObjectRotate>();
            if (obj != null)description.text = obj.description;
            
            subject.layer = 5;

            foreach (Transform child in subject.transform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 5;
            }
        }
    }

}
