using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : Handler
{
    protected const float gkMaxRangeSpawn = 5f;

    [System.Serializable]
    public class waveSet
    {
        public Enemy[] mobList;
        public int mobLimit;            // how many enemies are present at one time
        public float spawnDelay;        // how fast each monster spawns
        public float spawnAmountMin;    // how many monsters spawn each time - floor
        public float spawnAmountMax;    // how many monsters spawn each time - ceiling
    }

    public waveSet[] Waves;             // sets the waves to be spawned
    public Transform[] spawnPoints;
    public Gatekeeper[] gatekeeperSet;  // sets of gatekeepers to create;

    [SerializeField]
    protected GameUnit enemyTarget;
    
    protected List<Gatekeeper> gatekeepers;  // list of gatekeepers in a stage;
    protected waveSet currentWave;      // is assigned the current wave
    protected Enemy mob;                // is the instantiated mob;
    protected int enemyCount, waveID = 0;
    protected bool isGenerating = false;
    

    // getters and setters
    public GameUnit Target {
        get { return enemyTarget; }
        set { enemyTarget = value; }
    }

    public List<Gatekeeper> Gatekeepers {
        get { return gatekeepers; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        initializeHandler();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // initialize handler items
    protected void initializeHandler() {
        gatekeepers = new List<Gatekeeper>();
    }
    
    // spawn Gatekeepers for initializing a stage
    public bool spawnGateKeepers(List<Transform> spawnPoints, int gateType) {
        bool success = false;
        Gatekeeper tempGK;
        
        if (gateType > 0) gateType--;
        Debug.Log("EnemyHandler: Creating Gatekeepers: " + gateType + " - " + gatekeeperSet.Length);
        if (gateType < gatekeeperSet.Length) {
            for (var i = 0; i < spawnPoints.Count; i++) {
                Debug.Log("EnemyHandler: Adding Gatekeepers... " + i);
                // spawned enemy is a gatekeeper subclass. Subtract 1 from gateType to ensure it matches the stage (stage 1 = entry 0)
                tempGK = (Gatekeeper) spawnEnemy(gatekeeperSet[gateType], 
                                    spawnPoints[i].position.x - gkMaxRangeSpawn, 
                                    spawnPoints[i].position.x + gkMaxRangeSpawn, 
                                    spawnPoints[i].position.y - gkMaxRangeSpawn,
                                    spawnPoints[i].position.y + gkMaxRangeSpawn);
                if (tempGK != null) gatekeepers.Add(tempGK);
            }

            Debug.Log("GateKeepers added.");
            success = true;
        }
        return success;
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
            Enemy nextToLoad = currentWave.mobList[Random.Range(0, currentWave.mobList.Length)];   
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

     // spawns an enemy from the moblist around a certain enemy at minimum x & y ranges:
    public Enemy spawnEnemy(Gatekeeper gk, float xRange, float yRange, int areaID) {
        Enemy nextToLoad, mobCreated;
        currentWave = Waves[areaID];
        nextToLoad = currentWave.mobList[Random.Range(0, currentWave.mobList.Length)];
        
        Debug.Log("Enemyhandle: Spawning monster for " + gk);
        float tempX = Random.Range(gk.transform.position.x - xRange, gk.transform.position.x + xRange);
        float tempY = Random.Range(gk.transform.position.y - yRange, gk.transform.position.y + yRange);
        mobCreated = Instantiate(nextToLoad, new Vector2(tempX, tempY), Quaternion.identity);
        if (mobCreated != null) {
            mobCreated.CurrentTarget = enemyTarget;
            mobCreated.UpdateTarget();
        }
        return mobCreated;
    }

    // spawns an enemy in between minimum x & y ranges:
    public Enemy spawnEnemy(Enemy mobtype, float minX, float maxX, float minY, float maxY) {
        float tempX = Random.Range(minX, maxX);
        float tempY = Random.Range(minY,maxY);
        return Instantiate(mobtype, new Vector2(tempX, tempY), Quaternion.identity);
    }

    // spawns an enemy at specified position but default no rotation
    public Enemy spawnEnemy(Enemy mobtype, Vector2 position) {
        return Instantiate(mobtype, position, Quaternion.identity);
    } 

    // spawns an enmy at specified position and specific rotation
    public Enemy spawnEnemy(Enemy mobtype, Vector2 position, Quaternion rotation) {
        return Instantiate(mobtype, position, rotation);
    } 

}
