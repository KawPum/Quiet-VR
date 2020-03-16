using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInspect : MonoBehaviour
{
    public float turnSpeed = 60f; //скорость вращения объекта
    public GameObject finish; //ссылка на объект, играющий роль финишной точки
    public GameObject ball; // ссылка на объект шарика
    public bool inHand = false; //буль, отвечающий за то, находится ли объект в руках игрока или нет
    private Rigidbody ballRB; // ригидбоди шарика
    public string reward; //награда за решение головоломки
 
    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
        ballRB.useGravity = false; //отключаем гравитацию. Если гравитация будет всегда включена, то, когда игрок возьмёт паззл, шарик улетит за пределы объекта
    }

    // Update is called once per frame
    void Update()
    { //тут вращение объекта по ОУ и ОХ
        if (this.transform.parent != null)
        {
            if (Input.GetKey(KeyCode.I))
            {
                transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime);
            }

            else if (Input.GetKey(KeyCode.K))
            {
                transform.Rotate(Vector3.right, -turnSpeed * Time.deltaTime);
            }

            else if (Input.GetKey(KeyCode.J))
            {
                transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
            }

            else if (Input.GetKey(KeyCode.L))
            {
                transform.Rotate(Vector3.forward, -turnSpeed * Time.deltaTime);
            }

            if (inHand) ballRB.useGravity = true; //если же объект в руках игрока, то включаем гравитацию шарика
        }

            

        if (ball.transform.position.x<finish.transform.position.x+0.2f && ball.transform.position.x>finish.transform.position.x-0.2f && ball.transform.position.z<finish.transform.position.z+0.2f && ball.transform.position.z > finish.transform.position.z - 0.2f) //проверка на то, входит ли шарик в зону финиша
        {
            this.gameObject.SetActive(false);
        }

    }

}
