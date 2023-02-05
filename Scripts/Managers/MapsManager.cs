using System;
using System.Diagnostics;
using UnityEngine;

public class MapsManager : ManagerSingletonBase<MapsManager>, IGameEvents
{
    #region Fields

    private const string CONTAINER_TAG = "World_Container";

    #endregion

    #region Propeties

    public event Action<VillageLvlMap> OnMapSpawned = delegate { };

    //Variables.
    public VillageLvlMap CurrentMap { get; private set; }
    public Obi.ObiSolver CurrentSolver { get; private set; }
    private Transform SceneWorldContainer { get; set; }

    private MapGeneratorSettings GeneratorSettings { get; set; }

    #endregion

    #region Methods

    public void LoadNextLvl()
    {
        LoadNextMap();
    }

    public void RestartLvl()
    {
        ResetCurrentRoad();
    }

    public override void Initialize()
    {
        base.Initialize();

        GeneratorSettings = MapGeneratorSettings.Instance;
    }

    public override void LoadContent()
    {
        base.LoadContent();

        SceneWorldContainer = GameObject.FindGameObjectWithTag(CONTAINER_TAG).transform;
        CurrentSolver = FindObjectOfType<Obi.ObiSolver>();

        LoadNextMap();
    }

    public void StopLvlGame()
    {
        
    }

    public void StartLvlGame()
    {
        
    }

    private void ResetCurrentRoad()
    {
        SpawnMap();
    }

    private void LoadNextMap()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        SpawnMap();

        UnityEngine.Debug.LogFormat("Mapa wygenerowana: {0}ms", watch.ElapsedMilliseconds);
    }

    private void SpawnMap()
    {
        if(CurrentMap != null)
        {
            Destroy(CurrentMap.gameObject);
        }

        SpawnConcreteLvlMap();
    }

    private void SpawnConcreteLvlMap()
    {
        VillageLvlMap concreteMapPrefab = GeneratorSettings.GetMapForLvl(PlayerManager.Instance.Wallet.NextLvlNo) as VillageLvlMap;

        CurrentMap = Instantiate(concreteMapPrefab);
        CurrentMap.transform.ResetParent(SceneWorldContainer);

        CurrentMap.Init(this);
        OnMapSpawned(CurrentMap);
    }

    public override void AttachEvents()
    {
        base.AttachEvents();

        SaveLoadManager.Instance.OnResetCompleted += ResetGame;
    }

    protected override void DetachEvents()
    {
        base.DetachEvents();

        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.OnResetCompleted -= ResetGame;
        }
    }

    private void ResetGame()
    {
        SpawnMap();
    }

    #endregion

    #region Enums



    #endregion
}
