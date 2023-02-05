using UnityEngine;
using System.Collections;
using CursorSystem.FollowersObj;

public class LeftPanelModel : UIModel
{
    #region Fields



    #endregion

    #region Propeties



    #endregion

    #region Methods

    public bool CanShowButton()
    {
        return GetBarricadesAmmount() > Constants.DEFAULT_VALUE;
    }

    public int GetBarricadesAmmount()
    {
        return MapsManager.Instance.CurrentMap.BarricadesToUse;
    }

    public bool CanBuild()
    {
        return GetBarricadesAmmount() > Constants.DEFAULT_VALUE;
    }

    public void TryAttachBarricadeFollower()
    {
        if(CanBuild() == true)
        {
            CursorManager.Instance.SetFollowerObject(new CursorBarricadeFollowerObject());
        }
    }

    #endregion

    #region Enums



    #endregion
}
