using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DoorComponent : ReactiveComponent
{
    public bool doorOpen = false;
    public Color openColor;

    private void OnEnable()
    {
        Position = transform.position;
    }

    public override void Reaction()
    {
        //base.Reaction();
        doorOpen = true;
        Debug.Log("Aaaaaaaan OPEEEENNN");
        GetComponent<SpriteRenderer>().color = openColor;
        GetComponent<Collider2D>().enabled = false;
    }
}
