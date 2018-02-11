using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaveController : MonoBehaviour {
    public int wave;
    public int numRemaining;
    public float timeBetweenWaves = 10f;
    public int spawnRemaining;
    public int startingWave = 1;
    float waveTimer;
    float timer = 5f;
	float currentTimer;
    bool waveOver = false;
	public Transform origin;
	// Use this for initialization
	void Start () {
        startWave(startingWave);
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0 && spawnRemaining > 0 && numRemaining - spawnRemaining < 25) {
			SpawnGrunt ();
            spawnRemaining--;
            timer = currentTimer;
		}
        if(numRemaining == 0)
        {
            waveTimer -= Time.deltaTime;
            if(waveTimer <= 0)
            {
                startWave(wave + 1);
            }
        }
	}

    public void OnEnemyDied()
    {
        numRemaining--;
    }

    public void startWave(int num)
    {
        NetworkController.singleton.RpcSendMsgToAll("Wave " + num + " starting");   
        wave = num;
        numRemaining = Mathf.CeilToInt(10f * Mathf.Pow(1.2f, num));
        spawnRemaining = numRemaining;
        currentTimer = 20f / numRemaining;
        timer = currentTimer;
        waveTimer = timeBetweenWaves;
    }

    


	public void SpawnGrunt(){
		Vector3 position = new Vector3 (Random.Range (-30f, 30f), 31.27f, Random.Range (-30f, 30f));
		position += origin.position;
		NetworkServer.Spawn ((GameObject)GameObject.Instantiate(Resources.Load ("Spawn_Effect"), position, Quaternion.identity));
	}
}
