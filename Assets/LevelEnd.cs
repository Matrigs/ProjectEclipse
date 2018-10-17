using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    public GameObject NextLevel;
    public Transform NextLevelSpawnPoint;
	public Animator fadeAnimator;

    private List<GameObject> _playersOnTrigger =  new List<GameObject>();

    private void OnEnable()
    {
        //_levelEndTrigger = GetComponent<Collider2D>();
    }

    private void OnPlayersLevelEnd(List<GameObject> _playersOnTrigger)
    {
        if(_playersOnTrigger.Count == 2) // If the number of player in the Level End trigger is Equal to 2
        {
			fadeAnimator.SetTrigger ("FadeOut");
			transform.parent.gameObject.SetActive(false); // Turn off the whole current Level
            if (!NextLevel.activeInHierarchy) // If the next level is turned off
            {
                NextLevel.SetActive(true); //Turn it on
                foreach (GameObject go in _playersOnTrigger)
                {
                    go.transform.position = NextLevelSpawnPoint.position; // Teleport players to the Spawn point of the NextLevel
                }
                _playersOnTrigger.Clear();
            } 
			fadeAnimator.SetTrigger ("FadeIn");
        }
    }

    void OnTriggerExit2D()
    {
        _playersOnTrigger.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Other" + other.name + "Tag" + other.tag);
        if (!_playersOnTrigger.Contains(other.gameObject) && other.tag == "Player")
        {
            _playersOnTrigger.Add(other.gameObject);
            OnPlayersLevelEnd(_playersOnTrigger);
        }
    }

}
