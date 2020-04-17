using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
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

    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
        ballRB.useGravity = false; //отключаем гравитацию. Если гравитация будет всегда включена, то, когда игрок возьмёт паззл, шарик улетит за пределы объекта
    }



    void Update()
    {
        if (transform.parent != null) inHand = true;
        if (Input.GetMouseButton(0) && inHand)
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            //rotationY = Mathf.Clamp(rotationY, minY, maxY);
            transform.localEulerAngles = new Vector3(rotationY, -rotationX, 0);
        }

        if (inHand) ballRB.useGravity = true; //если же объект в руках игрока, то включаем гравитацию шарикa

        if (ball.transform.position.x < finish.transform.position.x + 0.2f && ball.transform.position.x > finish.transform.position.x - 0.2f && ball.transform.position.z < finish.transform.position.z + 0.2f && ball.transform.position.z > finish.transform.position.z - 0.2f) //проверка на то, входит ли шарик в зону финиша
        {
            win = true;
            player = FindObjectOfType<PlayerQuestStuff>();
            player.get_Reward(this);
        }
    }
}
