using Obi;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Obi.ObiCollider))]
public class VillageObject : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Villager[] villagers;
    [SerializeField]
    private MeshRenderer[] dynamicColorMeshes;
    [SerializeField]
    private GameObject[] objectsToEnableOnDeath;

    #endregion

    #region Propeties

    public event Action OnVillageBurned = delegate { };

    public Villager[] Villagers {
        get => villagers; 
    }
    public MeshRenderer[] DynamicColorMeshes { 
        get => dynamicColorMeshes; 
    }
    public GameObject[] ObjectsToEnableOnDeath {
        get => objectsToEnableOnDeath;
    }

    // Variables.
    public bool IsAlive { get; private set; } = true;
    private ObiSolver[] Solvers { get; set; }

    #endregion

    #region Methods

    public void OnLavaStarted()
    {
        foreach (Villager villager in Villagers)
        {
            villager.PlayPanic();
        }
    }

    private void Awake()
    {
        Solvers = FindObjectsOfType<ObiSolver>(true);
        if(Solvers.Length == Constants.DEFAULT_VALUE)
        {
            Debug.LogError("[Village] Brak ObiSolver!");
        }
    }

    private void Start()
    {
        Material terrainMat = MapsManager.Instance.CurrentMap.Terrain.material;
        DynamicColorMeshes.ForEach(x => x.sharedMaterial = terrainMat);
    }

    private void OnEnable()
    {
        Solvers.ForEach(x => x.OnCollision += SolverOnCollision);
    }

    private void OnDisable()
    {
        DetachSolversEvents();
    }

    private void HandleLavaReachVillage()
    {
        if (IsAlive == false) { return; }

        DetachSolversEvents();

        ObjectsToEnableOnDeath.ForEach(x => x.SetActive(true));
        IsAlive = false;
        OnVillageBurned();
    }

    private void DetachSolversEvents()
    {
        Solvers.ForEach(x => x.OnCollision -= SolverOnCollision);
    }

    // Handlers.
    private void SolverOnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();

        foreach (Oni.Contact contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;

                // Kolizja z aktualnym obiektem.
                if (col != null && col.gameObject == gameObject)
                {
                    HandleLavaReachVillage();
                }
            }
        }
    }

    #endregion

    #region Enums



    #endregion
}
