using System.Collections;
using UnityEngine;

[System.Serializable]
public class MatchInfo {
    public NamedPrefab playerPrefab;
    public NamedPrefab[] enemyPrefabs;
    // FIXME: todo
    // player/enemy spawn points
    // # of enemies at a time
    // list of hazards
    // hazard spawn points
    // arena choice
}

public class Match : MonoBehaviour {
    [Header("External Component References")]
    public CameraController cameraController;

    [Header("State Variables")]
    public SpawnPointRuntimeSet playerSpawns;
    public SpawnPointRuntimeSet enemySpawns;
    public BotRuntimeSet allBots;

    [Header("Events")]
    public StringEvent bannerFade;
    public StringEvent bannerMessage;
    public GameEvent bannerClear;
    public GameRecordEvent gameEventChannel;

    [Header("Match Config")]
    public int countdownTicks = 3;
    public bool debug = false;

    private bool matchStarted = false;
    [HideInInspector]
    public GameObject spawnedPlayer;
    [HideInInspector]
    public GameObject spawnedEnemy;

    private bool winnerDeclared = false;
    private GameObject winningBot;
    private MatchInfo matchInfo;

    void OnBotDeath(GameObject bot) {
        if (bot != null) {
            if (debug) Debug.Log("Bot died: " + bot.name);
        }

        // declare a winner
        if (bot == spawnedPlayer) {
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

    IEnumerator RunTimer(
        int timeout,
        StringEvent bannerFade
    ) {
        var startTime = Time.fixedTime;
        var lastTick = timeout;
        if (debug) Debug.Log("Tick: " + lastTick);
        if (bannerFade != null) bannerFade.Raise(lastTick.ToString());
        var currentDelta = Time.fixedTime - startTime;
        while ((currentDelta < (float) timeout) && !Input.GetKeyUp(KeyCode.Escape)) {
            var currentTick = timeout - Mathf.FloorToInt(currentDelta);
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
    }

    IEnumerator StatePrepare() {
        if (debug) Debug.Log("StatePrepare");
        // choose spawn points
        var enemySpawnPoint = enemySpawns.PickRandom();
        var playerSpawnPoint = playerSpawns.PickRandom();

        // spawn bots
        // FIXME: hack for now... manually apply control scripts/set targets/ai mode
        spawnedPlayer = SpawnBot(
            matchInfo.playerPrefab.prefab,
            playerSpawnPoint,
            enemySpawnPoint.transform.position,
            matchInfo.playerPrefab.name);
        yield return null;
        if (spawnedPlayer != null) {
            var brain = spawnedPlayer.AddComponent<HumanController>();
            brain.controlsActive = false;
            var materialDistributor = spawnedPlayer.GetComponent<MaterialDistributor>();
            if (materialDistributor != null) {
                materialDistributor.SetMaterials(MaterialTag.Player);
            }
        }
        spawnedEnemy = SpawnBot(
            matchInfo.enemyPrefabs[0].prefab,
            enemySpawnPoint,
            playerSpawnPoint.transform.position,
            matchInfo.enemyPrefabs[0].name);
        yield return null;
        if (spawnedEnemy != null) {
            var brain = spawnedEnemy.AddComponent<AIController>();
            brain.target = spawnedPlayer;
            brain.moodNow = AIMood.aggressive;
            brain.controlsActive = false;
            var materialDistributor = spawnedEnemy.GetComponent<MaterialDistributor>();
            if (materialDistributor != null) {
                materialDistributor.SetMaterials(MaterialTag.Enemy);
            }
        }

        // notify channel
        if (gameEventChannel != null) {
            gameEventChannel.Raise(GameRecord.GamePrepared());
        }

        // TRANSITION: Countdown
        yield return StartCoroutine(StateAnnouncer());
    }

    IEnumerator StateAnnouncer() {
        if (debug) Debug.Log("StateAnnouncer");

        // enable announcer camera mode
        if (cameraController != null) {
            cameraController.WatchAnnouncer();
        }

        if (bannerFade != null) bannerMessage.Raise("ANNOUNCER INTRO - ESC TO EXIT");

        // FIXME: remove announcer timer
        yield return StartCoroutine(RunTimer(5, null));

        // TODO: add announcer voiceover mechanics/state

        // TRANSITION: Countdown
        yield return StartCoroutine(StateCountdown());
    }

    IEnumerator StateCountdown() {
        if (debug) Debug.Log("StateCountdown");
        yield return null;

        // enable overview camera mode
        if (cameraController != null) {
            cameraController.WatchOverview();
        }

        // wait for countdown timer
        yield return StartCoroutine(RunTimer(countdownTicks, bannerFade));
        if (bannerFade != null) bannerFade.Raise("Start!");

        // TRANSITION: Play
        yield return StartCoroutine(StatePlay());
    }

    IEnumerator StatePlay() {
        if (debug) Debug.Log("StatePlay");

        // notify channel
        if (gameEventChannel != null) {
            gameEventChannel.Raise(GameRecord.GameStarted());
        }

        // enable bot camera mode
        if (cameraController != null) {
            cameraController.WatchBots();
        }

        // enable bot controls
        spawnedPlayer.GetComponent<BotBrain>().controlsActive = true;
        spawnedEnemy.GetComponent<BotBrain>().controlsActive = true;

        // wait for a winner to be declared
        while (!winnerDeclared) {
            yield return null;
        }

        // TRANSITION: Finish
        yield return StartCoroutine(StateFinish());
    }

    IEnumerator StateFinish() {
        if (debug) Debug.Log("StateFinish");

        // notify channel
        if (gameEventChannel != null) {
            gameEventChannel.Raise(GameRecord.GameFinished());
        }

        // declare winner
        var msg = winningBot.name + " wins ... Yay!!!";
        if (debug) Debug.Log(msg);
        if (bannerMessage != null) bannerMessage.Raise(msg);
        yield return null;

        // disable bots
        for (var i=allBots.Items.Count-1; i>=0; i--) {
            allBots.Items[i].GetComponent<BotBrain>().controlsActive = false;
        }
    }

    public void StartMatch(
        MatchInfo matchInfo
    ) {
        this.matchInfo = matchInfo;
        StartCoroutine(StatePrepare());
    }

}
