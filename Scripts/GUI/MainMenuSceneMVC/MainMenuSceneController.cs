using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MainMenuSceneModel), typeof(MainMenuSceneView))]
public class MainMenuSceneController : UIController
{
    #region Fields



    #endregion

    #region Propeties



    #endregion

    #region Methods

    public void OnStartGameButtonClick()
    {
        GetModel<MainMenuSceneModel>().LoadGameScene();
    }

    #endregion

    #region Enums



    #endregion
}
