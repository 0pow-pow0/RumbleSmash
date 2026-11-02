using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class BootstrapMultiplayerScene : MonoBehaviour
{

    [field: Header("Level"), SerializeField]
    string levelWrapperName;
    
    [SerializeField]
    Goal player1Goal;
    [SerializeField]
    Goal player2Goal;
    [SerializeField]
    GameObject[] player1Pivots;
    [SerializeField]
    GameObject[] player2Pivots;

    [SerializeField]
    GameObject spawnpointPlayer1;
    [SerializeField]
    GameObject spawnpointPlayer2;
    [SerializeField]
    GameObject spawnpointBall;



    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Light directionalLight;


    [SerializeField]
    GameObject arena;
    [SerializeField]
    GameObject platforms;
    [SerializeField]
    GameObject background;
    
    void InitializeLevel()
    {
        // Contains everything
        GameObject levelWrapper = new GameObject(levelWrapperName);

        player1Goal = Instantiate(player1Goal, levelWrapper.transform);
        player2Goal = Instantiate(player2Goal, levelWrapper.transform);

        GameObject[] instantiatedPlayer1Pivots = new GameObject[player1Pivots.Length];
        for(int i = 0; i < instantiatedPlayer1Pivots.Length; i++)
        {
            instantiatedPlayer1Pivots[i] = Instantiate(player1Pivots[i], levelWrapper.transform);
        }
        player1Pivots = instantiatedPlayer1Pivots;
        
        GameObject[] instantiatedPlayer2Pivots = new GameObject[player2Pivots.Length];
                for(int i = 0; i < instantiatedPlayer2Pivots.Length; i++)
        {
            instantiatedPlayer2Pivots[i] = Instantiate(player2Pivots[i], levelWrapper.transform);
        }
        player2Pivots = instantiatedPlayer2Pivots;

        player1Goal.movementPivots = player1Pivots;
        player2Goal.movementPivots = player2Pivots;

        spawnpointPlayer1 = Instantiate(spawnpointPlayer1, levelWrapper.transform);
        spawnpointPlayer2 = Instantiate(spawnpointPlayer2, levelWrapper.transform);
        spawnpointBall = Instantiate(spawnpointBall, levelWrapper.transform);

        mainCamera = Instantiate(mainCamera, levelWrapper.transform);
        directionalLight = Instantiate(directionalLight, levelWrapper.transform);
        arena = Instantiate(arena, levelWrapper.transform);
        platforms = Instantiate(platforms, levelWrapper.transform);
        background = Instantiate(background, levelWrapper.transform);
    }

    [field: Header("Entities"), SerializeField]
    string entitiesWrapperName;
    [SerializeField]
    Ball ball;

    void InitializeEntities()
    {
        GameObject entitiesWrapperPrefabCreated = new GameObject(entitiesWrapperName);
        
        ball = Instantiate(ball, entitiesWrapperPrefabCreated.transform);
    }

    [field: Header("UI"), SerializeField]
    Canvas canvas;
    [SerializeField]
    UIRoundAnimations uiRoundAnimations;
    [SerializeField]
    UI_InputHandlingScreen ui_InputHandlingScreen;
    [SerializeField]
    UIPausePanel uiPausePanel;
    [SerializeField]
    UIAnimationManager uiAnimationManager;
    [SerializeField]
    UIScoreboard uiScoreBoard;
    [SerializeField]
    UIPreMatchAnimations uiPreMatchAnimation;
    [SerializeField]
    UIMatchAnimations uiMatchAnimations;
    [SerializeField]
    UIInGame uiInGame;

    void InitializeUI()
    {
        canvas = Instantiate(canvas);

        uiRoundAnimations = Instantiate(uiRoundAnimations, canvas.transform);
        ui_InputHandlingScreen = Instantiate(ui_InputHandlingScreen, canvas.transform);
        uiPausePanel = Instantiate(uiPausePanel, canvas.transform);
        uiAnimationManager = Instantiate(uiAnimationManager, canvas.transform);
        uiScoreBoard = Instantiate(uiScoreBoard, canvas.transform);
        uiPreMatchAnimation = Instantiate(uiPreMatchAnimation, canvas.transform);
        uiMatchAnimations = Instantiate(uiMatchAnimations, canvas.transform);
        uiInGame = Instantiate(uiInGame, canvas.transform);
    }


    [field: Header("Managers"), SerializeField]
    InputManagerLogic inputManager;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    MatchManager matchManager;
    RoundManager roundManager;

    [SerializeField]
    GameObject eventSystemPrefab;

    [SerializeField]
    UIScreensManagerPow uiScreensManagerPowPrefab;

    [SerializeField]
    GlobalParticleManagerPow globalParticleManagerPow;

    void InitializeManagers()
    {
        inputManager = Instantiate(inputManager);
        gameManager = Instantiate(gameManager);
        //gameManager.ball = ball;
        //gameManager.player1Goal = player1Goal;
        //gameManager.player2Goal = player2Goal;

        matchManager = Instantiate(matchManager);
        roundManager = matchManager.GetComponentInChildren<RoundManager>();
        // roundManager.spawnpointBall = spawnpointBall;
        // roundManager.spawnpointPlayer1 = spawnpointPlayer1;
        // roundManager.spawnpointPlayer2 = spawnpointPlayer2;

        eventSystemPrefab = Instantiate(eventSystemPrefab);
        uiScreensManagerPowPrefab = Instantiate(uiScreensManagerPowPrefab);
        globalParticleManagerPow = Instantiate(globalParticleManagerPow);
    }

    void InitializeManagersEntitiesReferences()
    {
        gameManager.ball = ball;
        gameManager.player1Goal = player1Goal;
        gameManager.player2Goal = player2Goal;
    }

    void InitializeManagersLevelReferences()
    {
        
        roundManager.spawnpointBall = spawnpointBall;
        roundManager.spawnpointPlayer1 = spawnpointPlayer1;
        roundManager.spawnpointPlayer2 = spawnpointPlayer2;
    }

    void Initiator()
    {
        InitializeManagers();
        InitializeEntities();
        InitializeManagersEntitiesReferences();
        InitializeLevel();
        InitializeManagersLevelReferences();
        InitializeUI();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initiator();
    }

    

    // Update is called once per frame
    void Update()
    {
    }
}
