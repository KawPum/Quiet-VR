using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerScript : MonoBehaviour // писал Милованов Еремей
{
    public string password = "1995";
    public GameObject password_screen;
    private IEnumerator coroutine;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wow");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Wow issa Player wow so wow");
        }
    }

    public string React()
    {
        transform.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0.96f, 0.321f, 0);
        return "сесть за компьютер";
    }

    public void CheckPassword(Text input_field)
    {
        if (input_field.text == password)
        {
            password_screen.SetActive(false);
        }
        else
        {
            coroutine = HideInput(input_field);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator HideInput(Text input_field)
    {
        input_field.transform.parent.GetComponent<InputField>().text = "";
        input_field.enabled = false;
        input_field.transform.parent.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(1f);
        input_field.enabled = true;
        input_field.transform.parent.GetComponent<Image>().color = Color.white;
    }
}
