using Obi;
using System.Collections.Generic;

public class MainMenuVillageMap : VillageLvlMap
{
    #region Fields


    #endregion

    #region Propeties

    private List<ObiEmitter> SpawnedEmitters { get; set; } = new List<ObiEmitter>();

    #endregion

    #region Methods

    public override void Init(MapsManager mapsManager)
    {
        CurrentSolvers = FindObjectsOfType<ObiSolver>();
        KillLava();

        LavaRoutineHandler = MEC.Timing.RunCoroutine(_WaitAndStartLavaInLoop().CancelWith(gameObject));
    }

    public override void RunLava()
    {
        MEC.Timing.KillCoroutines(LavaRoutineHandler);

        VFXManager.Instance.DoCameraShake();

        PreparationTimeCounterS = Constants.DEFAULT_VALUE;
        foreach (ObiEmitter emitter in Emitters)
        {
            ObiEmitter newEmiter = Instantiate(emitter, emitter.transform.parent);
            newEmiter.transform.position = emitter.transform.position;
            newEmiter.gameObject.SetActive(true);
            SpawnedEmitters.Add(newEmiter);
        }

        // Wykrycie kiedy lava zastygnela w miejscu.
        SleepWatchdogRoutineHandler = MEC.Timing.RunCoroutine(_LavaSleepWatchdog().CancelWith(gameObject), MEC.Segment.SlowUpdate);
    }

    public override void KillLava()
    {
        SpawnedEmitters.ClearDestroy();
    }

    protected override void OnVillageSurvivedHandler()
    {
        if (IsInteractable == true)
        {
            IsInteractable = false;
            MEC.Timing.KillCoroutines(SleepWatchdogRoutineHandler);
            KillLava();

            // Loop, w menu lawa co X czasu leci po mapie.
            LavaRoutineHandler = MEC.Timing.RunCoroutine(_WaitAndStartLavaInLoop().CancelWith(gameObject));
        }
    }

    protected IEnumerator<float> _WaitAndStartLavaInLoop()
    {
        while (PreparationTimeCounterS < PreparationTimeS)
        {
            PreparationTimeCounterS += MEC.Timing.DeltaTime;
            yield return MEC.Timing.WaitForOneFrame;
        }

        IsInteractable = true;
        RunLava();
    }

    private void Awake()
    {
        Init(null);
    }

    private void Start()
    {
        Terrain.BuildFluidColliders();
    }

    #endregion

    #region Enums



    #endregion
}
