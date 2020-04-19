using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeSecret : MonoBehaviour // писал Кузин Данил
{

    int secret = 0;
    int secretCaseOpen = 0;
    float secretCasePosition;
    int[] casesOpen = new int[6];
    GameObject wardrobeCase;
    float[] changePosition = new float[6];
    float posClose;
    float posOpen;
    int[][] whatCases =
    {
        new int[]{1, 1, 1, 0, 1, 0},
        new int[]{1, 1, 0, 1, 0, 1},
        new int[]{1, 0, 1, 1, 1, 0},
        new int[]{0, 1, 1, 1, 0, 1},
        new int[]{1, 0, 1, 0, 1, 1},
        new int[]{0, 1, 0, 1, 1, 1}
    };
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            wardrobeCase = GameObject.Find("Sphere.00" + (i + 1));
            casesOpen[i] = 0;
            changePosition[i] = wardrobeCase.transform.localPosition.z;
        }
        secretCasePosition = changePosition[0];
        posClose = changePosition[0];
        posOpen = posClose + 10f;
        open(2);
        open(3);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            wardrobeCase = GameObject.Find("Sphere.00" + (i + 1));
            wardrobeCase.transform.localPosition = Vector3.Lerp(wardrobeCase.transform.localPosition, new Vector3(wardrobeCase.transform.localPosition.x, wardrobeCase.transform.localPosition.y, changePosition[i]), 1.5f * Time.deltaTime);
            if (secret != -1)
            {
                secret += casesOpen[i];
            }
        }
        if (secret == 6)
        {
            secret = -1;
            openSecretCase();
            closeAll();
        }
        else if (secret != -1) secret = 0;
        wardrobeCase = GameObject.Find("Sphere");
        wardrobeCase.transform.localPosition = Vector3.Lerp(wardrobeCase.transform.localPosition, new Vector3(wardrobeCase.transform.localPosition.x, wardrobeCase.transform.localPosition.y, secretCasePosition), 1.5f * Time.deltaTime);
    }

    void puzzle(int ObjectId)
    {
        for (int i = 0; i < 6; i++)
        {
            Debug.Log(whatCases[ObjectId - 1][i]);
            if (whatCases[ObjectId - 1][i] == 1) open(i + 1);
        }
    }

    void closeAll()
    {
        for (int i = 0; i < 6; i++)
        {
            openCases(i + 1);
        }
    }

    void open(int ObjectId)
    {
        wardrobeCase = GameObject.Find("Sphere.00" + ObjectId);
        casesOpen[ObjectId - 1] = (casesOpen[ObjectId - 1] + 1) % 2;
        changePosition[ObjectId - 1] = posOpen * casesOpen[ObjectId - 1] - (posClose * (casesOpen[ObjectId - 1] - 1));
    }

    public void openCases(int ObjectId)
    {
        if (secret != -1)
        {
            puzzle(ObjectId);
        }
        else
        {
            open(ObjectId);
        }
    }

    public void openSecretCase()
    {
        wardrobeCase = GameObject.Find("Sphere");
        secretCaseOpen = (secretCaseOpen + 1) % 2;
        secretCasePosition = posOpen * secretCaseOpen - (posClose * (secretCaseOpen - 1));
    }

    public string getTextOpened()
    {
        if (secret == 0)
        {
            return "заперто";
        }
        else
        {
            return ("открыть/закрыть");
        }
    }

}
