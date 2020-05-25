using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Shaking : MonoBehaviour
{
    float accX;
    public float score;
    public float got;
    bool finished = false;
    public string reward_name;
    public GameObject reward_mesh;
   
    // Start is called before the first frame update
    void Start()
    {
        accX = Input.acceleration.x;
        got = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

        print(score);
        if (score >= 100)got = 1;
    
        if ((Input.acceleration.x - accX > 0.021f) && (got == 0))
        {
            score += 0.65f;
            transform.localRotation = Quaternion.Euler((float)Math.Sin(score) * (score/7), transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z), 5f * Time.deltaTime);
            if (transform.eulerAngles.x < 0.02f)
            {
                score = 0;
                if (got != 0)
                {
                    score = 0;
                    FindObjectOfType<RigidbodyFirstPersonController>().enabled = true;
                    StopReactive(true);
                    if (!finished)
                    {
                        finished = true;
                        FindObjectOfType<PlayerQuestStuff>().get_Reward(GetComponent<Shaking>());
                    }
                    transform.GetComponent<Shaking>().enabled = false;
                    //FindObjectOfType<PlayerQuestStuff>().StopShaking(transform.gameObject);
                }
            }
            if (score != 0)
            {
                score -= 0.01f;
            }
        }
    }

    public void StopReactive(bool state)
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            ReactiveTarget ch =  child.gameObject.GetComponent<ReactiveTarget>();
            if (ch!= null) ch.enabled = state;
        }
        Handheld.Vibrate();
        print("STOP");
    }
}