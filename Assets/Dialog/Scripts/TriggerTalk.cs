using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTalk : MonoBehaviour {

    public GameObject DialogObject;
    public int DialogNumber;

    private bool talkActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!talkActivated)
        {
            talkActivated = true;
            DialogObject.GetComponent<ManageText>().startDialog(DialogNumber);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
