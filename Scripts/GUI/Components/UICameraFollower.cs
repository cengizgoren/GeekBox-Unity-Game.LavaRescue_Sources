using UnityEngine;
using System.Collections;

public class UICameraFollower : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Vector3 rotationModifier = Vector3.one;

    #endregion

    #region Propeties

    public Vector3 RotationModifier { 
        get => rotationModifier; 
    }

    private Camera CachedCamera { get; set; }

    #endregion

    #region Methods

    private void Awake()
    {
        CachedCamera = Camera.main;
    }

    private void LateUpdate()
    {
        Quaternion cameraRotation = CachedCamera.transform.rotation;
        transform.LookAt(transform.position + cameraRotation * Vector3.forward, cameraRotation * Vector3.up);
    }

    #endregion

    #region Enums



    #endregion
}
