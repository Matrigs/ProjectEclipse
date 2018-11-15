using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState {
	public Dictionary<Block, Vector3> blockPosition;
	public Dictionary<ButtonComponent, ButtonComponent.buttonState> switchStatus;
	public Dictionary<DoorComponent, bool> doorStatus;
	public Dictionary<EnemyController, Vector3> enemyPosition;
	public Dictionary<EnemyController, bool> enemyActive;

	
    public GameState()
    {
		blockPosition = new Dictionary<Block, Vector3>();
		switchStatus = new Dictionary<ButtonComponent, ButtonComponent.buttonState>();
		doorStatus = new Dictionary<DoorComponent, bool>();
		enemyPosition = new Dictionary<EnemyController, Vector3>();
		enemyActive = new Dictionary<EnemyController, bool>();

		//guarda todos blocos da cena
		foreach(Block block in GameMaster.gm.blocks) blockPosition.Add(block, block.transform.position);

		//guarda todos switches da cena
		foreach(ButtonComponent switchObj in GameMaster.gm.buttons) switchStatus.Add(switchObj, switchObj.currentState);

		//guarda todas portas da cena
		foreach(DoorComponent door in GameMaster.gm.doors) doorStatus.Add(door, door.doorOpen);

		//guarda todos inimigos da cena
		foreach(EnemyController enemy in GameMaster.gm.enemies){ 
			enemyPosition.Add(enemy, enemy.transform.position);
			enemyActive.Add(enemy, enemy.gameObject.activeSelf);
		}
    }

	public void RestoreState(){
		//restaura todos blocos da cena
		Debug.Log("Blocks to reset: " + blockPosition.Count);
		foreach(KeyValuePair<Block, Vector3> block in blockPosition) block.Key.transform.position = block.Value; 

		//restaura todos switches da cena
		Debug.Log("Switches to reset: " + switchStatus.Count);
		foreach(KeyValuePair<ButtonComponent, ButtonComponent.buttonState> switchObj in switchStatus){
			if(switchObj.Key.currentState != switchObj.Value) switchObj.Key.ChangeState();
		}

		//restaura todas portas da cena
		Debug.Log("Doors to reset: " + doorStatus.Count);
		foreach(KeyValuePair<DoorComponent, bool> door in doorStatus) door.Key.SetOpen(door.Value);

		//restaura todos inimigos da cena
		Debug.Log("Enemies to reset: " + enemyPosition.Count);
		foreach(KeyValuePair<EnemyController, Vector3> enemy in enemyPosition) {
			bool alive = enemyActive[enemy.Key];
			Debug.Log(enemy.Key.gameObject + " being reseted to (" + alive + "," + enemy.Value + ")");

			enemy.Key.transform.position = enemy.Value;
			enemy.Key.gameObject.SetActive(alive);

			//restaura vida
			if(alive){
				var hurtbox = enemy.Key.GetComponent<Hurtbox>();
				hurtbox.Reset();
			}
			
			Debug.Log(enemy.Key.gameObject + " reseted to (" + enemy.Key.gameObject.activeInHierarchy + "," + enemy.Key.transform.position + ")");
		}
	}
}
