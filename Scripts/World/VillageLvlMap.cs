using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Obi;
using System.Collections.Generic;
using System;
using System.Linq;

public class VillageLvlMap : LvlMap
{
    #region Fields

    [Title("Settings")]
    [SerializeField]
    private float lvlEnergy;
    [SerializeField]
    private int barricadesToUse = 0;
    [SerializeField]
    private float preparationTimeS = 45f;
    [SerializeField]
    private float watchDogTime = 60f;

    [Space]
    [SerializeField]
    private DestructibleTerrain terrain;
    [SerializeField]
    private VillageObject villageObj;
    [SerializeField]
    private Obi.ObiEmitter[] emitters;

    [SerializeField, InfoBox("x - left, y - right, z - bottom")]
    private Vector3 mapBounds = Vector3.one;

    [Header("Debug")]
    [SerializeField]
    private Vector3[] _frustumCorners = new Vector3[4];

    #endregion

    #region Propeties

    public event Action OnLavaStarted = delegate { };
    public event Action OnVillageSurvived = delegate { };
    public event Action OnBarricadesAmmountChanged = delegate { };

    public float LvlEnergy {
        get => LvlEnergy1;
    }
    public DestructibleTerrain Terrain {
        get => terrain;
    }
    public float LvlEnergy1 { 
        get => lvlEnergy;
    }
    public float PreparationTimeS {
        get => preparationTimeS;
    }
    public float WatchDogTime {
        get => watchDogTime;
    }
    public VillageObject VillageObj {
        get => villageObj; 
    }
    public ObiEmitter[] Emitters { 
        get => emitters;
    }
    public Vector3 MapBounds {
        get => mapBounds;
        private set => mapBounds = value;
    }
    public int BarricadesToUse {
        get => barricadesToUse;
        private set => barricadesToUse = value;
    }

    //Variables.
    protected float PreparationTimeCounterS { get; set; } = Constants.DEFAULT_VALUE;
    protected MEC.CoroutineHandle LavaRoutineHandler { get; set; }
    protected MEC.CoroutineHandle SleepWatchdogRoutineHandler { get; set; }
    protected Obi.ObiSolver[] CurrentSolvers { get; set; }

    #endregion

    #region Methods

    public override void Init(MapsManager mapsManager)
    {
        base.Init(mapsManager);

        CurrentSolvers = FindObjectsOfType<ObiSolver>();

        VillageObj.OnVillageBurned += OnVillageBurnedHandler;
        KillLava();

        LavaRoutineHandler = MEC.Timing.RunCoroutine(_WaitAndStartLava().CancelWith(gameObject));
    }

    public virtual void RunLava()
    {
        MEC.Timing.KillCoroutines(LavaRoutineHandler);

        Terrain.BuildFluidColliders();

        PreparationTimeCounterS = Constants.DEFAULT_VALUE;
        VillageObj.OnLavaStarted();
        OnLavaStarted();
        GameplayEvents.Instance.NotifyOnLavaStarted();

        foreach (ObiEmitter emitter in Emitters)
        {
            emitter.gameObject.SetActive(true);
        }

        // Wykrycie kiedy lava zastygnela w miejscu.
        SleepWatchdogRoutineHandler = MEC.Timing.RunCoroutine(_LavaSleepWatchdog().CancelWith(gameObject), MEC.Segment.SlowUpdate);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        KillLava();
    }

    protected override void AttachEvents()
    {
        base.AttachEvents();

        GameplayEvents.Instance.OnBarricadeAdded += OnBarricadeAddedHandler;
    }

    protected override void DetachEvents()
    {
        base.DetachEvents();

        if(GameplayEvents.Instance != null)
        {
            GameplayEvents.Instance.OnBarricadeAdded += OnBarricadeAddedHandler;
        }
    }

    protected virtual void OnVillageSurvivedHandler()
    {
        if (IsInteractable == true)
        {
            IsInteractable = false;

            FreezLava();
            GamePlayManager.Instance.LvlSuccess();
            OnVillageSurvived();

            MEC.Timing.KillCoroutines(SleepWatchdogRoutineHandler);
        }
    }

    private void OnVillageBurnedHandler()
    {
        if(IsInteractable == true)
        {
            IsInteractable = false;

            GamePlayManager.Instance.LvlFailed();
            Debug.Log("Wioska sie jara!".SetColor(Color.red));

            MEC.Timing.KillCoroutines(SleepWatchdogRoutineHandler);
        }
    }

    private void OnBarricadeAddedHandler()
    {
        BarricadesToUse--;
        OnBarricadesAmmountChanged();
    }

    public void FreezLava()
    {
        CurrentSolvers.ForEach(x => x.isPlaying = false);
    }

    public virtual void KillLava()
    {
        foreach (ObiEmitter emitter in Emitters)
        {
            emitter.gameObject.SetActive(false);
        }
    }

    protected IEnumerator<float> _WaitAndStartLava()
    {
        while (PreparationTimeCounterS < PreparationTimeS)
        {
            PreparationTimeCounterS += MEC.Timing.DeltaTime;
            GameplayEvents.Instance.NotifyOnMapTimeSUpdate(PreparationTimeS - PreparationTimeCounterS);

            yield return MEC.Timing.WaitForOneFrame;
        }

        RunLava();
    }

    protected IEnumerator<float> _LavaSleepWatchdog()
    {
        Obi.ObiSolver currentSolver = CurrentSolvers[Constants.DEFAULT_VALUE];

        float sleepTreshold = currentSolver.parameters.sleepThreshold;
        Vector3 leftBound = currentSolver.transform.InverseTransformPoint(new Vector3(MapBounds.x, 0f));
        Vector3 rightBound = currentSolver.transform.InverseTransformPoint(new Vector3(MapBounds.y, 0f));
        Vector3 bottomBound = currentSolver.transform.InverseTransformPoint(new Vector3(0f, MapBounds.z));

        yield return MEC.Timing.WaitForSeconds(1f);
        Debug.Log("_LavaSleepWatchdog - Started!");

        bool allSleep = false;
        int actorsCount = Constants.DEFAULT_VALUE;
        while (true)
        {
            UnityEngine.Profiling.Profiler.BeginSample("_LavaSleepWatchdog");

            allSleep = true;
            for (int x = 0; x < CurrentSolvers.Length; x++)
            {
                currentSolver = CurrentSolvers[x];

                // Dla kazdego solvera w jednej klatce.
                for (int i = currentSolver.allocParticleCount - 1; i >= 0; i--)
                {
                    Vector3 position = currentSolver.positions[i];
                    if (position.x < leftBound.x || position.x > rightBound.x || position.y < bottomBound.y)
                    {
                        ObiSolver.ParticleInActor pa = currentSolver.particleToActor[i];
                        ObiEmitter emitter = pa.actor as ObiEmitter;
                        if (emitter != null)
                        {
                            emitter.life[pa.indexInActor] = 0;
                            continue;
                        }
                    }

                    Vector3 velocity = currentSolver.velocities[i];
                    float particleKineticEnergy = 0.5f * Vector3.SqrMagnitude(velocity);
                    if (particleKineticEnergy > sleepTreshold)
                    {
                        allSleep = false;
                    }
                }

                // Aktualna liczba czastek na scenie.
                actorsCount += currentSolver.actors.Count;

                yield return MEC.Timing.WaitForOneFrame;
            }

            if(allSleep == true || actorsCount == Constants.DEFAULT_VALUE)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                break;
            }

            UnityEngine.Profiling.Profiler.EndSample();

            yield return MEC.Timing.WaitForOneFrame;
        }

        OnVillageSurvivedHandler();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_frustumCorners[0], _frustumCorners[1]);
        Gizmos.DrawLine(_frustumCorners[1], _frustumCorners[2]);
        Gizmos.DrawLine(_frustumCorners[2], _frustumCorners[3]);
        Gizmos.DrawLine(_frustumCorners[3], _frustumCorners[0]);

        Gizmos.color = Color.cyan;
        Vector3 leftBottomPoint = new Vector3(MapBounds.x, MapBounds.z);
        Vector3 leftUpperPoint = new Vector3(MapBounds.x, 100f);
        Vector3 righBottomPoint = new Vector3(MapBounds.y, MapBounds.z);
        Vector3 righUpperPoint = new Vector3(MapBounds.y, 100f);
        Vector3 bottomLeftPoint = new Vector3(MapBounds.x, MapBounds.z);
        Vector3 bottomRightPoint = new Vector3(MapBounds.y, MapBounds.z);

        Gizmos.DrawLine(leftBottomPoint, leftUpperPoint);
        Gizmos.DrawLine(righBottomPoint, righUpperPoint);
        Gizmos.DrawLine(bottomLeftPoint, bottomRightPoint);
    }

    [Button]
    private void CalculateCamera()
    {
        Scene gameScene = SceneManager.GetSceneByBuildIndex(2);
        GameObject[] sceneobjects = gameScene.GetRootGameObjects();

        Camera cam = null;
        for (int i = 0; i < sceneobjects.Length; i++)
        {
            cam = sceneobjects[i].GetComponentInChildren<Camera>();
            break;
        }

        float frustumHeight;
        float frustumWidth;

        if (cam.orthographic == false)
        {
            frustumHeight = 2.0f * Mathf.Abs(cam.transform.position.z) * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            frustumWidth = frustumHeight * cam.aspect;
        }
        else
        {
            frustumHeight = cam.orthographicSize * 2;
            frustumWidth = frustumHeight * Screen.width / Screen.height;
        }

        float halfWidth = frustumWidth / 2;
        float halfHeight = frustumHeight / 2;

        _frustumCorners[0] = new Vector3(cam.transform.position.x - halfWidth, cam.transform.position.y + halfHeight, 0f);
        _frustumCorners[1] = new Vector3(cam.transform.position.x + halfWidth, cam.transform.position.y + halfHeight, 0f);
        _frustumCorners[3] = new Vector3(cam.transform.position.x - halfWidth, cam.transform.position.y - halfHeight, 0f);
        _frustumCorners[2] = new Vector3(cam.transform.position.x + halfWidth, cam.transform.position.y - halfHeight, 0f);
    }

    [Button]
    private void CalculateCameraIso()
    {
        if(Application.isPlaying == false) { Debug.Log("Runtime-Only!".SetColor(Color.red)); return; }

        // get the 2 camera-corner rays
        Ray topRightRay = Camera.main.ViewportPointToRay(Vector3.zero);
        Ray bottomLeftRay = Camera.main.ViewportPointToRay(Vector3.one);

        // find the corners of the terrain-rect 
        RaycastHit topRightRH;
        RaycastHit bottomLeftRH;
        Rect result;

        Terrain.XZPlane.Raycast(topRightRay, out float distance);
        Vector3 worldTopHitPositon = topRightRay.GetPoint(distance);

        Terrain.XZPlane.Raycast(bottomLeftRay, out distance);
        Vector3 worldBottomHitPositon = bottomLeftRay.GetPoint(distance);

        result = Rect.MinMaxRect(worldTopHitPositon.x, worldTopHitPositon.y, worldBottomHitPositon.x, worldBottomHitPositon.y);

        _frustumCorners = new Vector3[4];
        _frustumCorners[0] = new Vector3(result.center.x - result.width / 2f, result.center.y + result.height / 2f, 0f);
        _frustumCorners[1] = new Vector3(result.center.x + result.width / 2f, result.center.y + result.height / 2f, 0f);
        _frustumCorners[3] = new Vector3(result.center.x - result.width / 2f, result.center.y - result.height / 2f, 0f);
        _frustumCorners[2] = new Vector3(result.center.x + result.width / 2f, result.center.y - result.height / 2f, 0f);

        foreach (var element in _frustumCorners)
        {
            Debug.Log(element.ToString());
        }
    }

        #endif

        #endregion

        #region Enums



        #endregion
}
