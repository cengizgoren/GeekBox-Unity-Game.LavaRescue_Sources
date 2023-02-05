using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class BarricadeSliderUI : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float maxRotationAngle = 45f;

    #endregion

    #region Propeties

    public float MaxRotationAngle { 
        get => maxRotationAngle;
    }

    private BarricadeObject Barricade { get; set; }

    #endregion

    #region Methods

    public void Init(BarricadeObject barricade)
    {
        Barricade = barricade;
        GetComponent<Canvas>().worldCamera = CameraController.Instance.CurrentCamera;
    }

    public void OnSliderValueChanged(float value)
    {
        Barricade.RotateOverZAxis(-value * MaxRotationAngle);
    }

    #endregion

    #region Enums



    #endregion
}
