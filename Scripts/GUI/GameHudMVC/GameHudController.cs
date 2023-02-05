using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameHudModel), typeof(GameHudView))]
public class GameHudController : UIController
{
    #region Fields



    #endregion

    #region Propeties



    #endregion

    #region Methods

    public void OnResetButtonClick()
    {
        GetModel<GameHudModel>().ResetLvl();
    }

    #endregion

    #region Enums



    #endregion
}
