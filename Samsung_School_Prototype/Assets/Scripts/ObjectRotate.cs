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

    public float minScale = 0.7f;
    public float maxScale = 2f;


    public float rotationMax= 180f;

    public Quaternion startRotation;

    public float wheel = 1;
    //Quaternion gyroscope;
    Vector3 angles;

    //public float wheel = 1;

    void Start()
    {
        //Input.gyro.enabled = true;
        //Input.gyro.enabled = true;
        inventoryScript = FindObjectOfType<Inventory>();
        if (ball != null)
        {
            ballRB = ball.GetComponent<Rigidbody>();
            ballRB.useGravity = false; //отключаем гравитацию. Если гравитация будет всегда включена, то, когда игрок возьмёт паззл, шарик улетит за пределы объекта
        }
        //transform.gameObject.GetComponent<ObjectRotate>().enabled = false;
        //startRotation = Input.gyro.attitude;
        //for (int i = 0; i < 3; i++) rotPhone[i] = 0;
    }

    void Update()
    {
        //gyroscope = Input.gyro.attitude;
        //angles = Input.acceleration;
        //print(transform.name);
        Vector3 scale = new Vector3(inventoryScript.scale, inventoryScript.scale, inventoryScript.scale);
        transform.localScale = scale;

        if (transform.parent != null) inHand = true;
        //transform.rotation = gyroscope;
        //gyroscope= new Quaternion(gyroscope.w, gyroscope.z, -gyroscope.x, -gyroscope.y);
        //angles = gyroscope.eulerAngles;


        //transform.Rotate(inventoryScript.rotation * Time.deltaTime);
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(inventoryScript.rotation), 5f * Time.deltaTime);
        
        transform.eulerAngles = new Vector3(0,  -inventoryScript.rotation.x, inventoryScript.rotation.y);

        //if (startGyro.x == 0 || startGyro.y == 0 || startGyro.z == 0)
        //{
        //    startGyro = Input.acceleration;
        //}
        //else
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        //rot[i] = (Mathf.Abs(startGyro[i]) - Mathf.Abs(gyro[i] + startGyro[i])) * Mathf.Abs(1 / startGyro[i]) * (gyro.x / Mathf.Abs(gyro[i]));
        //        //rot[i] = gyro[i] * 180f;
        //        //print(Input.gyro.gravity[i]);
        //    }

        //}


        //transform.rotation = Quaternion.Euler(rot.z, 0, 0) ;
        //transform.rotation = Quaternion.Euler(-angles.y - 90, angles.x, angles.z-90);
        //transform.rotation = Quaternion.Euler(-angles.y, angles.x, 0);
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(rot.y, rot.x, transform.localEulerAngles.z), 0.2f);
        //stransform.localEulerAngles = new Vector3(rot.y, rot.x, transform.localEulerAngles.z);

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
