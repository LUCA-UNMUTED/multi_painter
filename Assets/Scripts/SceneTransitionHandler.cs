
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using DilmerGames.Core.Singletons;

public class SceneTransitionHandler : Singleton<SceneTransitionHandler>
{
    /// <summary>
    /// this script allows us to switch between scenes based on buttons pushed or functions called
    /// </summary>
    static public SceneTransitionHandler sceneTransitionHandler { get; internal set; }

    [SerializeField] public string MultiplayerMainScene = "XRMultiplayerMain";
    [SerializeField] public string TutorialScene = "TutorialScene";

    [HideInInspector]
    public delegate void ClientLoadedSceneDelegateHandler(ulong clientId);
    [HideInInspector]
    public event ClientLoadedSceneDelegateHandler OnClientLoadedScene;

    [HideInInspector]
    public delegate void SceneStateChangedDelegateHandler(SceneStates newState);
    [HideInInspector]
    public event SceneStateChangedDelegateHandler OnSceneStateChanged;

    private int m_numberOfClientLoaded;

    [Header("debug")]
    [SerializeField] private bool testHost = false;
    [SerializeField] private bool testClient = false;
    [SerializeField] private bool testTutorial = false;
    [SerializeField] private bool doSkipTutorial = false;
    public bool InitializeAsHost { get; set; }

    [Header("Button to disable to avoid double click")]
    public GameObject clientbuttonDisable;//geen kans op 2x klikken;
    public GameObject CanvasToHide;

    public Vector3 canvasMoveLocation;
    private void Update()
    {
        // for testing only
        if (testHost) //Input.GetKeyDown(KeyCode.H) || 
        {
            testHost = false;
            LaunchMP(true);
        }
        if (testClient) //Input.GetKeyDown(KeyCode.C) || 
        {
            testClient = false;
            LaunchMP(false);
        }
        if (testTutorial)
        {
            LaunchTutorial();
        }
    }

    /// <summary>
    /// Example scene states
    /// </summary>
    public enum SceneStates
    {
        Init,
        Start,
        Lobby,
        Ingame
    }

    [SerializeField] private SceneStates m_SceneState;

    /// <summary>
    /// Awake
    /// If another version exists, destroy it and use the current version
    /// Set our scene state to INIT
    /// </summary>
    private void Awake()
    {
        if (sceneTransitionHandler != this && sceneTransitionHandler != null)
        {
            Debug.Log("Another scene transition handler existed, removing");
            GameObject.Destroy(sceneTransitionHandler.gameObject);
        }
        sceneTransitionHandler = this;
        SetSceneState(SceneStates.Init);
    }


    /// <summary>
    /// SetSceneState
    /// Sets the current scene state to help with transitioning.
    /// </summary>
    /// <param name="sceneState"></param>
    public void SetSceneState(SceneStates sceneState)
    {
        m_SceneState = sceneState;
        if (OnSceneStateChanged != null)
        {
            OnSceneStateChanged.Invoke(m_SceneState);
        }
    }

    /// <summary>
    /// GetCurrentSceneState
    /// Returns the current scene state
    /// </summary>
    /// <returns>current scene state</returns>
    public SceneStates GetCurrentSceneState()
    {
        return m_SceneState;
    }

    /// <summary>
    /// Initialize
    /// Loads the default main menu when started (this should always be a component added to the networking manager)
    /// </summary>
    public void InitializeMP()
    {
        if (m_SceneState == SceneStates.Init)
        {
            Debug.Log("Loading " + MultiplayerMainScene);
            SceneManager.LoadScene(MultiplayerMainScene);
        }
    }

    /// <summary>
    /// Switches to a new scene
    /// </summary>
    /// <param name="scenename"></param>
    public void SwitchScene(string scenename)
    {
        Debug.Log("Switching to " + scenename);
        if (NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
            NetworkManager.Singleton.SceneManager.LoadScene(scenename, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync(scenename);
        }
    }
    private void OnSceneEvent(SceneEvent sceneEvent)
    {
        //We are only interested by Client Loaded Scene events
        if (sceneEvent.SceneEventType != SceneEventType.LoadComplete) return;

        m_numberOfClientLoaded += 1;
        OnClientLoadedScene?.Invoke(sceneEvent.ClientId);
    }

    public bool AllClientsAreLoaded()
    {
        return m_numberOfClientLoaded == NetworkManager.Singleton.ConnectedClients.Count;
    }

    /// <summary>
    /// ExitAndLoadStartMenu
    /// This should be invoked upon a user exiting a multiplayer game session.
    /// </summary>
    public void ExitAndLoadStartMenu()
    {
        OnClientLoadedScene = null;
        SetSceneState(SceneStates.Start);
        SceneManager.LoadScene(1);
    }

    public void LaunchMP(bool asHost)
    {
        Debug.Log("Clicked at " + Time.time);
        InitializeAsHost = asHost;
        InitializeMP();
    }

    public void LaunchTutorial()
    {
        if (m_SceneState == SceneStates.Init)
        {
            if (!doSkipTutorial)
            {

                Debug.Log("Loading " + TutorialScene);
                SceneManager.LoadScene(TutorialScene, LoadSceneMode.Additive);
                clientbuttonDisable.SetActive(false);
                CanvasToHide = GameObject.FindGameObjectWithTag("StartMenu");

                canvasMoveLocation = CanvasToHide.transform.position + new Vector3(0, 100, 0);
                StartCoroutine(LerpPosition(canvasMoveLocation, 5));


                //disable button
            }
            else
            {
                Debug.Log("skipping tutorial, loading " + MultiplayerMainScene);
                SceneManager.LoadScene(MultiplayerMainScene);
            }
        }
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = CanvasToHide.transform.position;
        while (time < duration)
        {
            CanvasToHide.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        CanvasToHide.transform.position = targetPosition;
    }
}