using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardOpenClose : MonoBehaviour // писал Кузин Данил
{

    GameObject door;
    int[] openedDoors;
    float[] angles;

    // Start is called before the first frame update
    void Start()
    {
        openedDoors = new int[transform.childCount];
        angles = new float[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            openedDoors[i] = -1;
            angles[i] = transform.GetChild(i).transform.eulerAngles.y;
            Debug.Log(angles[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            door = transform.GetChild(i).gameObject;
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, Quaternion.Euler(door.transform.eulerAngles.x, angles[i], door.transform.eulerAngles.z), 0.03f);
        }
    }

    public void openClose(int ObjectId, GameObject door1)
    {
        int i = 0;
        while (transform.GetChild(i).name != door1.name) i++;
        openedDoors[i] *= -1;
        angles[i] -= ((ObjectId - 0.5f) * 180) * openedDoors[i];
        //door1.transform.rotation = Quaternion.Euler(door1.transform.eulerAngles.x, angles[i], door1.transform.eulerAngles.z);
    }
}