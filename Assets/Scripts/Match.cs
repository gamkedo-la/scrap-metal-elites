using System.Collections;
using UnityEngine;

public class Match : MonoBehaviour {
    public SpawnPointRuntimeSet playerSpawns;
    public SpawnPointRuntimeSet enemySpawns;
    public BotRuntimeSet allBots;

    public GameObject playerBot;
    public GameObject enemyBot;
    public int countdownTicks = 3;
    public bool debug = false;

    public StringEvent bannerFade;
    public StringEvent bannerMessage;
    public GameEvent bannerClear;

    private bool matchStarted = false;
    private GameObject spawnedPlayer;
    private GameObject spawnedEnemy;

    private bool winnerDeclared = false;
    private GameObject winningBot;

    void OnBotDeath(GameObject bot) {
        if (bot != null) {
            if (debug) Debug.Log("Bot died: " + bot.name);
        }

        // declare a winner
        if (bot == playerBot) {
            winningBot = spawnedEnemy;
        } else {
            winningBot = spawnedPlayer;
        }
        winnerDeclared = true;
    }

    GameObject SpawnBot(
        GameObject botPrefab,
        SpawnPoint spawnPoint,
        Vector3 lookAt,
        string name
    ) {
        var rotation = Quaternion.LookRotation(lookAt - spawnPoint.transform.position);
        var botGo = Object.Instantiate(botPrefab, spawnPoint.transform.position, rotation);
        botGo.name = name;
        var health = botGo.GetComponent<BotHealth>();
        if (health != null) {
            health.onDeath.AddListener(OnBotDeath);
        }
        return botGo;
    }

    IEnumerator StatePrepare() {
        if (debug) Debug.Log("StatePrepare");
        // choose spawn points
        var enemySpawnPoint = enemySpawns.PickRandom();
        var playerSpawnPoint = playerSpawns.PickRandom();

        // spawn bots
        spawnedPlayer = SpawnBot(playerBot, playerSpawnPoint, enemySpawnPoint.transform.position, "player");
        spawnedEnemy = SpawnBot(enemyBot, enemySpawnPoint, playerSpawnPoint.transform.position, "enemy");

        // TRANSITION: Countdown
        yield return StartCoroutine(StateCountdown());
    }

    IEnumerator StateCountdown() {
        if (debug) Debug.Log("StateCountdown");
        var startTime = Time.fixedTime;
        var lastTick = countdownTicks;
        if (debug) Debug.Log("Tick: " + lastTick);
        if (bannerFade != null) bannerFade.Raise(lastTick.ToString());

        var currentDelta = Time.fixedTime - startTime;
        while (currentDelta < (float) countdownTicks) {
            var currentTick = countdownTicks - Mathf.FloorToInt(currentDelta);
            if (currentTick != lastTick) {
                lastTick = currentTick;
                if (debug) Debug.Log("Tick: " + lastTick);
                if (bannerFade != null) bannerFade.Raise(lastTick.ToString());
            }
            // wait until next frame;
            yield return null;
            currentDelta = Time.fixedTime - startTime;
        }
        lastTick = 0;
        if (debug) Debug.Log("Tick: " + lastTick);
        if (bannerFade != null) bannerFade.Raise("Start!");

        // TRANSITION: Play
        yield return StartCoroutine(StatePlay());
    }

    IEnumerator StatePlay() {
        if (debug) Debug.Log("StatePlay");

        // FIXME: hack for now... manually apply control scripts/set targets/ai mode
        spawnedPlayer.AddComponent<BotDriveController>();
        spawnedPlayer.AddComponent<ActuatorController>();

        var ai = spawnedEnemy.AddComponent<AIController>();
        ai.target = spawnedPlayer;
        ai.moodNow = AIMood.aggressive;

        // wait for a winner to be declared
        while (!winnerDeclared) {
            yield return null;
        }

        // TRANSITION: Finish
        yield return StartCoroutine(StateFinish());
    }

    IEnumerator StateFinish() {
        if (debug) Debug.Log("StateFinish");

        // declare winner
        var msg = winningBot.name + " wins ... Yay!!!";
        if (debug) Debug.Log(msg);
        if (bannerMessage != null) bannerMessage.Raise(msg);
        yield return null;

        // disable bots
        for (var i=allBots.Items.Count-1; i>=0; i--) {
            allBots.Items[i].gameObject.SetActive(false);
        }
    }

    void Update() {
        // start the state engine
        if (!matchStarted) {
            StartCoroutine(StatePrepare());
            matchStarted = true;
        }
    }

}
