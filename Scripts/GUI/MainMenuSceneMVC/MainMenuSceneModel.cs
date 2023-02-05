using UnityEngine;
using System.Collections;

public class MainMenuSceneModel : UIModel
{
    #region Fields



    #endregion

    #region Propeties



    #endregion

    #region Methods

    public void LoadGameScene()
    {
        GameManager.Instance.LoadTargetScene(SceneLabel.GAME);
    }

    #endregion

    #region Enums



    #endregion
}
