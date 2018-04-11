using UnityEngine;

public class Match : MonoBehaviour {
    public SpawnPointRuntimeSet playerSpawns;
    public SpawnPointRuntimeSet enemySpawns;
    public BotRuntimeSet allBots;

    public GameObject playerBot;
    public GameObject enemyBot;

    private bool prepared = false;
    private GameObject spawnedPlayer;
    private GameObject spawnedEnemy;

    void OnBotDeath(GameObject bot) {
        if (bot != null) {
            Debug.Log("Bot died: " + bot.name);
        }

        // FIXME: move to state engine
        if (bot == playerBot) {
            Debug.Log("Enemy wins ... Boo!!!");
        } else {
            Debug.Log("Player wins ... Yay!!!");
        }
        // disable bots
        for (var i=allBots.Items.Count-1; i>=0; i--) {
            allBots.Items[i].gameObject.SetActive(false);
        }
    }

    GameObject SpawnBot(GameObject botPrefab, SpawnPoint spawnPoint, Vector3 lookAt) {
        var rotation = Quaternion.LookRotation(lookAt - spawnPoint.transform.position);
        var botGo = Object.Instantiate(botPrefab, spawnPoint.transform.position, rotation);
        var health = botGo.GetComponent<BotHealth>();
        if (health != null) {
            health.onDeath.AddListener(OnBotDeath);
        }
        return botGo;
    }

    SpawnPoint PickSpawn(SpawnPointRuntimeSet points) {
        var index = Random.Range(0, points.Items.Count);
        return points.Items[index];
    }

    void PrepareBots() {
        // choose spawn points
        var enemySpawnPoint = PickSpawn(enemySpawns);
        var playerSpawnPoint = PickSpawn(playerSpawns);

        // spawn bots
        spawnedPlayer = SpawnBot(playerBot, playerSpawnPoint, enemySpawnPoint.transform.position);
        spawnedEnemy = SpawnBot(enemyBot, enemySpawnPoint, playerSpawnPoint.transform.position);

        // FIXME: hack for now... manually apply control scripts/set targets/ai mode
        spawnedPlayer.AddComponent<BotDriveController>();
        spawnedPlayer.AddComponent<ActuatorController>();

        var ai = spawnedEnemy.AddComponent<AIController>();
        ai.target = spawnedPlayer;
        ai.moodNow = AIMood.aggressive;

    }

    void Update() {
        if (!prepared) {
            PrepareBots();
            prepared = true;
        }
    }

}
