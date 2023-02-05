using GeekBox.Scripts.Generic;
using UnityEngine;

public class CameraController : SingletonBase<CameraController>
{
    #region Fields

    [SerializeField]
    private Camera currentCamera;

    #endregion

    #region Propeties

    public Camera CurrentCamera {
        get => currentCamera;
    }

    #endregion

    #region Methods



    #endregion

    #region Enums



    #endregion
}
