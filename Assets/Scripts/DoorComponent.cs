using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DoorComponent : ReactiveComponent
{
    public bool doorOpen = false;
	public GameObject doorParts;
	//public ParticleSystem doorParticle;
	ParticleSystem doorParticle;
    public Color openColor;
    private Color defaultColor;

    public void Start(){
        GameMaster.gm.doors.Add(this);
        defaultColor = GetComponent<SpriteRenderer>().color;
		doorParticle = doorParts.GetComponent<ParticleSystem> ();
    }

    private void OnEnable()
    {
        Position = transform.position;
    }

    public override void Reaction()
    {
        //base.Reaction();
        SetOpen(true);
    }

    public void SetOpen(bool isOpen){
        doorOpen = isOpen;

        if(doorOpen){
            Debug.Log("Aaaaaaaan OPEEEENNN");
            GetComponent<SpriteRenderer>().color = openColor;
            doorParticle.Stop();
			//doorParts.SetActive (false);
            GetComponent<Collider2D>().enabled = false;
        }

        else{
            GetComponent<SpriteRenderer>().color = defaultColor;
            doorParticle.Play();
			//doorParts.SetActive (true);
            GetComponent<Collider2D>().enabled = true;
        }
    }
}
