using PlayerData;
using System;
using UnityEngine;

public class PlayerManager : SingletonSaveableManager<PlayerManager, PlayerManagerMemento>, IGameEvents
{
    #region Fields

    [SerializeField]
    private RuntimeCircleClipper clipperPrefab;

    private PlayerWallet wallet = new PlayerWallet();

    #endregion

    #region Propeties

    public event Action<float> OnEnergyChanged = delegate { };

    public PlayerWallet Wallet { 
        get => wallet; 
        private set => wallet = value;
    }

    public RuntimeCircleClipper CurrentClipper { 
        get; 
        private set;
    }

    private RuntimeCircleClipper ClipperPrefab
    {
        get => clipperPrefab;
    }

    private float DestroyedSurface { get; set; } = Constants.DEFAULT_VALUE;
    private VillageLvlMap CurrentMap { get; set; }

    public float CurrentEnergyNormalized { get; private set; } = Constants.DEFAULT_VALUE;

    #endregion

    #region Methods

    public void SetEnergy(float energyValue)
    {
        CurrentEnergyNormalized = energyValue / CurrentMap.LvlEnergy;
        CurrentEnergyNormalized = Mathf.Clamp01(CurrentEnergyNormalized);

        OnEnergyChanged(CurrentEnergyNormalized);
    }

    public void LoadNextLvl()
    {
        Wallet.SetCurrentLvlNo(Wallet.CurrentLvlNo + 1);
    }

    public void RestartLvl()
    {

    }

    public void StartLvlGame()
    {
        SetupLvlData(MapsManager.Instance.CurrentMap as VillageLvlMap);
    }

    public void StopLvlGame()
    {
        
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void LoadContent()
    {
        base.LoadContent();

        CurrentClipper = Instantiate(ClipperPrefab);
        CurrentClipper.transform.ResetParent(transform);
        CurrentClipper.onClipExecuteCallback = OnClipExecutedCallback;
    }

    public override void LoadManager(PlayerManagerMemento memento)
    {
        Wallet.Load(memento.SavedWallet);
    }

    public override void AttachEvents()
    {
        base.AttachEvents();

        GamePlayManager.Instance.OnLvlSuccess += OnLvlSuccessHandler;
    }

    public override void ResetGameData()
    {
        base.ResetGameData();
        Wallet.SetDefaultData();
    }

    protected override void DetachEvents()
    {
        base.DetachEvents();

        GamePlayManager.Instance.OnLvlSuccess -= OnLvlSuccessHandler;
    }

    private void SetupLvlData(VillageLvlMap village)
    {
        CurrentMap = village;
        CurrentClipper.terrain = village.Terrain;
        CurrentClipper.isActive = true;
        DestroyedSurface = Constants.DEFAULT_VALUE;
        SetEnergy(CurrentMap.LvlEnergy);

        CurrentMap.OnLavaStarted += OnLavaStarted;
        CurrentMap.OnVillageSurvived += OnVillageSurvived;
    }

    // HANDLERS.
    private void OnLvlSuccessHandler(int wallsBreaked)
    {
        LvlData data = new LvlData(Constants.DEFAULT_ID, wallsBreaked, Constants.DEFAULT_VALUE, Constants.DEFAULT_VALUE);
        Wallet.AddLvlData(data);
    }
    
    private void OnClipExecutedCallback(float destroyedSurface)
    {
        DestroyedSurface += destroyedSurface;
        SetEnergy(CurrentMap.LvlEnergy - DestroyedSurface);
        if(DestroyedSurface >= CurrentMap.LvlEnergy)
        {
            CurrentClipper.isActive = false;
            CurrentMap.RunLava();
        }
    }

    private void OnLavaStarted()
    {
        CurrentClipper.isActive = false;
    }

    private void OnVillageSurvived()
    {
        Debug.Log("WIN!".SetColor(Color.green));
    }

    #endregion

    #region Enums



    #endregion
}
