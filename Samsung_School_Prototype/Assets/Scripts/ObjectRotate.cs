using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour // писал Милованов Еремей
{
    public GameObject finish; //ссылка на объект, играющий роль финишной точки
    public GameObject ball; // ссылка на объект шарика
    public bool inHand = false; //буль, отвечающий за то, находится ли объект в руках игрока или нет
    private Rigidbody ballRB; // ригидбоди шарика
    public GameObject reward; //награда за решение головоломки
    public string path;
    public bool win = false;
    public PlayerQuestStuff player;
    public string description;
    Inventory inventoryScript;

    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minZ = -45.0f;
    public float maxZ = 45.0f;

    public float sensX = 100.0f;
    public float sensZ = 100.0f;

    public float minScale = 0.7f;
    public float maxScale = 2f;

    float rotationZ = 0.0f;
    float rotationX = 0.0f;

    //public float wheel = 1;

    void Start()
    {
        inventoryScript = FindObjectOfType<Inventory>();
        if (ball != null)
        {
            ballRB = ball.GetComponent<Rigidbody>();
            ballRB.useGravity = false; //отключаем гравитацию. Если гравитация будет всегда включена, то, когда игрок возьмёт паззл, шарик улетит за пределы объекта
        }
        //transform.gameObject.GetComponent<ObjectRotate>().enabled = false;
    }

    void Update()
    {
        //print(transform.name);
        Vector3 scale = new Vector3(inventoryScript.scale, inventoryScript.scale, inventoryScript.scale);
        transform.localScale = scale;
        if (transform.parent != null) inHand = true;

        //if (Input.GetMouseButton(0) && inHand)         //код для вращения взят с одного из форумов
        //{
        //    rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        //    rotationZ += Input.GetAxis("Mouse Y") * sensZ * Time.deltaTime;
        //    transform.localEulerAngles = new Vector3(0, -rotationX, rotationZ);
        //}

        

        if (inHand && ball!=null) ballRB.useGravity = true; //если же объект в руках игрока, то включаем гравитацию шарикa

        if (ball!= null && ball.transform.position.x < finish.transform.position.x + 0.2f && ball.transform.position.x > finish.transform.position.x - 0.2f && ball.transform.position.z < finish.transform.position.z + 0.2f && ball.transform.position.z > finish.transform.position.z - 0.2f) //проверка на то, входит ли шарик в зону финиша
        {
            win = true;
            player = FindObjectOfType<PlayerQuestStuff>();
            player.get_Reward(this);
        }
    }
}
