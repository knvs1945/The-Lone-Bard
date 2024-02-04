using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHandler : handler
{

    [System.Serializable]
    public class waveSet
    {
        public enemy[] mobList;
        public int mobLimit;            // how many enemies are present at one time
        public float spawnDelay;        // how fast each monster spawns
        public float spawnAmountMin;    // how many monsters spawn each time - floor
        public float spawnAmountMax;    // how many monsters spawn each time - ceiling
    }

    public waveSet[] Waves;             // sets the waves to be spawned
    public Transform[] spawnPoints;

    [SerializeField]
    protected gameUnit enemyTarget;
    
    protected waveSet currentWave;      // is assigned the current wave
    protected enemy mob;                // is the instantiated mob;
    protected int enemyCount, waveID = 0;
    protected bool isGenerating = false;

    // getters and setters
    public gameUnit Target {
        get { return enemyTarget; }
        set { enemyTarget = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // mobLevels - unit levels of the enemy to be generated
    public void startGenerator(int levelID, int mobLevels) {
        // don't start generating monsters if the levelID doesn't make sense 
        Debug.Log("Calling startGenerator");
        if (levelID < 0 || levelID > Waves.Length) {
            Debug.Log("Invalid levelID startGenerator");
            consoleUI.Log("Invalid levelID set");
            return;
        }

        waveID = levelID;
        if (!isGenerating) {
            isGenerating = true;
            currentWave = Waves[levelID];            
            StartCoroutine(WaveGenerator());
        }
    }

    // stop generating
    public void stopGenerator() {
        if (isGenerating) {
            isGenerating = false;
            StopCoroutine(WaveGenerator());
        }
    }

    // Generate the spawns when called
    protected IEnumerator WaveGenerator()
    {
        while (isGenerating) { 
            // enemy nextToLoad = currentWave.mobList[ Random.Range(0, currentWave.mobList.Length) ];
            enemy nextToLoad = currentWave.mobList[0];   
            //mob = Instantiate(nextToLoad, new Vector2(0,0), transform.rotation);
            mob = spawnEnemy(nextToLoad, -30, 30, -10, 10);
            if (mob != null) {
                // consoleUI.Log("Spawning mob...");
                mob.CurrentTarget = enemyTarget;
                mob.UpdateTarget();
            }
        
            yield return new WaitForSeconds(currentWave.spawnDelay);
        }
    }

    // General spawn units function here - will use polymorphs

    // spawns an enemy in between minimum x & y ranges:
    public enemy spawnEnemy(enemy mobtype, float minX, float maxX, float minY, float maxY) {
        float tempX = Random.Range(minX, maxX);
        float tempY = Random.Range(minY,maxY);
        return Instantiate(mobtype, new Vector2(tempX, tempY), Quaternion.identity);
    }

    // spawns an enemy at specified position but default no rotation
    public enemy spawnEnemy(enemy mobtype, Vector2 position) {
        return Instantiate(mobtype, position, Quaternion.identity);
    } 

    // spawns an enmy at specified position and specific rotation
    public enemy spawnEnemy(enemy mobtype, Vector2 position, Quaternion rotation) {
        return Instantiate(mobtype, position, rotation);
    } 

}
