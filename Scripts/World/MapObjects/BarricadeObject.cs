using UnityEngine;
using System.Collections;

public class BarricadeObject : MonoBehaviour, ISelectable
{
    #region Fields

    [SerializeField]
    private MeshRenderer currentRenderer;
    [SerializeField]
    private Material buildNoAllowedMat;
    [SerializeField]
    private SphereCollider[] probes;
    [SerializeField]
    private LayerMask buildLayer;
    [SerializeField]
    private Collider selectionCollider;
    [SerializeField]
    private BarricadeSliderUI sliderUI;

    #endregion

    #region Propeties

    public SphereCollider[] Probes {
        get => probes; 
    }
    public MeshRenderer CurrentRenderer { 
        get => currentRenderer; 
    }

    // Variables.
    public bool CanBuild { get; set; } = false;
    public LayerMask BuildLayer { get => buildLayer; }
    private Material CachedOriginalMat { get; set; }
    public Material BuildNoAllowedMat { get => buildNoAllowedMat; }
    public Collider SelectionCollider { get => selectionCollider; }
    public BarricadeSliderUI SliderUI { get => sliderUI; }

    public int ID => GetHashCode();

    // Buffors.
    private Collider[] _collisionsBuffer = new Collider[4];

    #endregion

    #region Methods

    public void SetSelectionMode(bool isSelectable)
    {
        SelectionCollider.enabled = isSelectable;
    }

    public void RotateOverZAxis(float targetDegree)
    {
        CurrentRenderer.transform.rotation = Quaternion.Euler(CurrentRenderer.transform.rotation.eulerAngles.x,
            CurrentRenderer.transform.rotation.eulerAngles.y, targetDegree);
    }

    public void RefreshProbes()
    {
        int positiveProbes = Constants.DEFAULT_VALUE;
        for (int i = 0; i < Probes.Length; i++)
        {
            int hits = Physics.OverlapSphereNonAlloc(Probes[i].transform.position, probes[i].radius, _collisionsBuffer, BuildLayer.value);
            if(hits > Constants.DEFAULT_VALUE)
            {
                positiveProbes++;
            }
        }

        CanBuild = positiveProbes == Probes.Length;
        RefreshMaterial();
    }

    private void RefreshMaterial()
    {
        CurrentRenderer.sharedMaterial = CanBuild == true ? CachedOriginalMat : BuildNoAllowedMat;
    }

    private void Awake()
    {
        CachedOriginalMat = CurrentRenderer.sharedMaterial;
        SliderUI.Init(this);
    }

    public void OnSelected()
    {
        SliderUI.gameObject.SetActive(true);
    }

    public void OnDeselected()
    {
        SliderUI.gameObject.SetActive(false);
    }

    public bool IDEqual(int otherId)
    {
        return otherId == ID;
    }

    #endregion

    #region Enums



    #endregion
}
