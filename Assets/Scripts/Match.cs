using System.Collections;
using UnityEngine;

public class Match : MonoBehaviour {
    [Header("External Component References")]
    public CameraController cameraController;

    [Header("Prefabs")]
    public GameObject pausePanelPrefab;
    public GameObject donePanelPrefab;

    [Header("State Variables")]
    public SpawnPointRuntimeSet playerSpawns;
    public SpawnPointRuntimeSet enemySpawns;
    public BotRuntimeSet allBots;
    public GameInfo gameInfo;
    public AIConfig defaultAIConfig;

    [Header("Events")]
    public StringEvent timerMessage;
    public StringEvent bannerFade;
    public StringEvent bannerMessage;
    public GameEvent bannerClear;
    public GameRecordEvent gameEventChannel;
    public StringEvent wantDoneConfirm;     // event used to trigger match complete modal for player confirmation
    public GameEvent doneConfirmed;         // event used by player confirmation modal when player clears modal
    public GameEvent matchFinished;         // event raised when match is complete
    public StringEvent pauseActionSelected; // event used by pause modal to identify action selected

    [Header("Match Config")]
    public int countdownTicks = 3;
    public int matchTime = 180;
    public bool debug = false;

    [HideInInspector]
    public GameObject spawnedPlayer;
    [HideInInspector]
    public GameObject spawnedEnemy;

    private bool matchStarted = false;
    private bool winnerDeclared = false;
    private GameObject winningBot;
    private MatchInfo matchInfo;
    private PlayerInfo playerInfo;
    private int timerTick = 0;

    void OnBotDeath(GameObject bot) {
        if (bot != null) {
            if (debug) Debug.Log("Bot died: " + bot.name);
        }

        // declare a winner
        DeclareWinner((bot == spawnedPlayer) ? spawnedEnemy : spawnedPlayer);
    }

    void DeclareWinner(GameObject winner) {
        winningBot = winner;
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

    public static string FmtTimerMsg(bool showTimeSplit, int tick) {
        if (showTimeSplit) {
            var min = tick/60;
            var sec = tick%60;
            return min.ToString() + ":" + sec.ToString("00");
        } else {
            return tick.ToString();
        }
    }

    IEnumerator RunTimer(
        int timeout,
        StringEvent bannerEvent
    ) {
        yield return RunTimer(timeout, bannerEvent, ()=>true);
    }

    IEnumerator RunTimer(
        int timeout,
        StringEvent bannerEvent,
        System.Func<bool> conditionPredicate
    ) {
        bool showTimeSplit = (timeout > 60);
        var startTime = Time.fixedTime;
        var lastTick = timeout;
        timerTick = lastTick;
        if (debug) Debug.Log("Tick: " + lastTick);
        if (bannerEvent != null) bannerEvent.Raise(FmtTimerMsg(showTimeSplit, lastTick));
        var currentDelta = Time.fixedTime - startTime;
        while (conditionPredicate() && (currentDelta < (float) timeout)) {
            var currentTick = timeout - Mathf.FloorToInt(currentDelta);
            if (currentTick != lastTick) {
                lastTick = currentTick;
                timerTick = lastTick;
                if (debug) Debug.Log("Tick: " + lastTick);
                if (bannerEvent != null) bannerEvent.Raise(FmtTimerMsg(showTimeSplit, lastTick));
            }
            // wait until next frame;
            yield return null;
            currentDelta = Time.fixedTime - startTime;
        }
        // don't show last tick if condition was hit
        if (conditionPredicate()) {
            lastTick = 0;
            timerTick = lastTick;
            if (bannerEvent != null) bannerEvent.Raise(FmtTimerMsg(showTimeSplit, lastTick));
            if (debug) Debug.Log("Tick: " + lastTick);
        }
    }

    IEnumerator StatePrepare() {
        if (debug) Debug.Log("StatePrepare");
        // choose spawn points
        var enemySpawnPoint = enemySpawns.PickRandom();
        var playerSpawnPoint = playerSpawns.PickRandom();

        // spawn bots
        // FIXME: hack for now... manually apply control scripts/set targets/ai mode
        spawnedPlayer = SpawnBot(
            gameInfo.matchInfo.playerPrefab.prefab,
            playerSpawnPoint,
            enemySpawnPoint.transform.position,
            gameInfo.matchInfo.playerPrefab.name);
        yield return null;
        if (spawnedPlayer != null) {
            var brain = spawnedPlayer.AddComponent<HumanController>();
            brain.DisableControls();
            var materialDistributor = spawnedPlayer.GetComponent<MaterialDistributor>();
            if (materialDistributor != null) {
                materialDistributor.SetMaterials(MaterialTag.Player);
            }
        }
        spawnedEnemy = SpawnBot(
            gameInfo.matchInfo.enemyPrefabs[0].prefab,
            enemySpawnPoint,
            playerSpawnPoint.transform.position,
            gameInfo.matchInfo.enemyPrefabs[0].name);
        yield return null;
        if (spawnedEnemy != null) {
            var brain = spawnedEnemy.AddComponent<AIController>();
            brain.AssignConfig(defaultAIConfig);
            //brain.target = spawnedPlayer;
            //brain.moodNow = AIMood.aggressive;
            brain.DisableControls();
            brain.TargetPlayer();
            var materialDistributor = spawnedEnemy.GetComponent<MaterialDistributor>();
            if (materialDistributor != null) {
                materialDistributor.SetMaterials(MaterialTag.Enemy);
            }
        }

        // notify channel
        if (gameEventChannel != null) {
            gameEventChannel.Raise(GameRecord.GamePrepared());
        }

        // TRANSITION: Announcer
        StartCoroutine(StateAnnouncer());
    }

    IEnumerator StateAnnouncer() {
        if (debug) Debug.Log("StateAnnouncer");

        // enable announcer camera mode
        if (cameraController != null) {
            cameraController.WatchAnnouncer();
        }

        if (bannerFade != null) bannerMessage.Raise("ANNOUNCER INTRO - ESC TO EXIT");

        // FIXME: remove announcer timer
        yield return StartCoroutine(RunTimer(5, null, ()=>(!Input.GetKeyUp(KeyCode.Escape))));

        // TODO: add announcer voiceover mechanics/state

        // TRANSITION: Countdown
        StartCoroutine(StateCountdown());
    }

    IEnumerator StateCountdown() {
        if (debug) Debug.Log("StateCountdown");
        yield return null;

        // enable overview camera mode
        if (cameraController != null) {
            cameraController.WatchOverview();
        }

        // wait for countdown timer
        yield return StartCoroutine(RunTimer(countdownTicks, bannerFade, ()=>(!Input.GetKeyUp(KeyCode.Escape))));
        if (bannerFade != null) bannerFade.Raise("Start!");

        // TRANSITION: Play
        StartCoroutine(StatePlay());
    }

    IEnumerator StatePlay() {
        if (debug) Debug.Log("StatePlay");
        StartCoroutine(StateMatchTimer());
        yield return null;      // if match timer was skipped (escape pressed), eat the escape by waiting a frame
        StartCoroutine(PauseHandler());

        // notify channel
        if (gameEventChannel != null) {
            gameEventChannel.Raise(GameRecord.GameStarted());
        }

        // enable bot camera mode
        if (cameraController != null) {
            cameraController.WatchBots();
        }

        // enable bot controls
        spawnedPlayer.GetComponent<BotBrain>().EnableControls();
        spawnedEnemy.GetComponent<BotBrain>().EnableControls();

        // wait for a winner to be declared
        while (!winnerDeclared) {
            yield return null;
        }

        // TRANSITION: Finish
        StartCoroutine(StateFinish());
    }

    IEnumerator PauseHandler() {
        bool paused = false;
        StringEventListener listener = null;
        GameObject panelGo = null;
        string pauseAction = "";
        while (!winnerDeclared) {
            bool startPause = false;
            bool stopPause = false;
            bool concede = false;

            // handle escape key
            if (Input.GetKeyUp(KeyCode.Escape)) {
                if (paused) {
                    stopPause = true;
                } else {
                    startPause = true;
                }
            }

            // handle event listener response
            if (pauseAction == "continue") {
                stopPause = true;
            } else if (pauseAction == "concede") {
                stopPause = true;
                concede = true;
            }

            // handle start pause
            if (startPause) {
                // instantiate pause modal
                panelGo = Instantiate(pausePanelPrefab, GetCanvas().gameObject.transform);
                yield return null;      // wait a frame for panel initialization

                // add listener to panel
                pauseAction = "";
                listener = panelGo.AddComponent<StringEventListener>();
                listener.SetEvent(pauseActionSelected);
                listener.Response.AddListener((msg)=>{pauseAction = msg;});

                // actually pause
                Time.timeScale = 0f;
                paused = true;
            }

            // handle stop pause
            if (stopPause) {
                pauseAction = "";
                // tear down modal
                if (panelGo != null) {
                    Destroy(panelGo);
                }

                // continue
                Time.timeScale = 1f;
                paused = false;
            }

            // handle concede action
            if (concede) {
                pauseAction = "";
                // declaring a winner should cause pause manager to finish
                DeclareWinner(spawnedEnemy);
            }

            // wait til next frame
            yield return null;
        }
    }

    IEnumerator StateMatchTimer() {
        if (debug) Debug.Log("StateMatchTimer");
        yield return null;

        // start match timer
        yield return StartCoroutine(RunTimer(matchTime, timerMessage, ()=>(!winnerDeclared)));

        // if winner is not already declared, declare winner based on remaining health
        if (!winnerDeclared) {
            var playerHealth = spawnedPlayer.GetComponent<BotHealth>();
            var enemyHealth = spawnedEnemy.GetComponent<BotHealth>();
            if (playerHealth != null && enemyHealth != null) {
                DeclareWinner((playerHealth.healthPercent > enemyHealth.healthPercent) ? spawnedPlayer :  spawnedEnemy);

            // shouldn't happen: declare enemy winner
            } else {
                DeclareWinner(spawnedEnemy);
            }
        }
    }

    IEnumerator StateFinish() {
        if (debug) Debug.Log("StateFinish");

        // notify channel
        if (gameEventChannel != null) {
            gameEventChannel.Raise(GameRecord.GameFinished());
        }

        // declare game over
        if (bannerMessage != null) bannerMessage.Raise("Game Over!!!");
        yield return null;

        // disable bots
        for (var i=allBots.Items.Count-1; i>=0; i--) {
            allBots.Items[i].GetComponent<BotBrain>().DisableControls();
        }

        // add win/loss for player
        if (winningBot == spawnedPlayer) {
            // create score for win
            var scoreInfo = new MatchScoreInfo();
            scoreInfo.matchID = gameInfo.matchInfo.id;
            scoreInfo.time = timerTick;
            gameInfo.playerInfo.AddWin(scoreInfo);

        } else {
            gameInfo.playerInfo.AddLoss();
        }

        // setup listener for doneConfirmed event
        var confirmed = false;
        var listener = gameObject.AddComponent<GameEventListener>();
        listener.SetEvent(doneConfirmed);
        listener.Response.AddListener(()=>{confirmed = true;});

        // instantiate modal for confirmation
        var panelGo = Instantiate(donePanelPrefab, GetCanvas().gameObject.transform);
        yield return null;      // wait a frame for panel initialization

        // trigger event to notify player that match is complete,
        // causes confirmation modal to display message and wait for player to click ok
        var msg = System.String.Format("{0}:{1}", (winningBot == spawnedPlayer) ? "win" : "loss", gameInfo.playerInfo.name);
        wantDoneConfirm.Raise(msg);

        // wait for match info to be selected
        yield return new WaitUntil(() => confirmed);

        // clean up, remove listener
        Destroy(panelGo);
        Destroy(listener);

        // signal that the match is done
        if (matchFinished != null) {
            matchFinished.Raise();
        }
    }

    public void OnStartMatch() {
        if (!matchStarted) {
            StartCoroutine(StatePrepare());
            matchStarted = true;
        }
    }

    public void PlayMatch(
        PlayerInfo playerInfo,
        MatchInfo matchInfo
    ) {
        this.playerInfo = playerInfo;
        this.matchInfo = matchInfo;
        StartCoroutine(StatePrepare());
    }

    Canvas GetCanvas() {
        // canvas should always be tagged
        var canvasGo = GameObject.FindWithTag("canvas");
        if (canvasGo != null) {
            return canvasGo.GetComponent<Canvas>();
        }
        return null;
    }

}
