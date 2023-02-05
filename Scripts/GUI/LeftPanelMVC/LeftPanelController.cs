using UnityEngine;

[RequireComponent(typeof(LeftPanelModel), typeof(LeftPanelView))]
public class LeftPanelController : UIController
{
    #region Fields



    #endregion

    #region Propeties



    #endregion

    #region Methods

    public void OnBarricadeButtonTouch()
    {
        GetModel<LeftPanelModel>().TryAttachBarricadeFollower();
    }

    #endregion

    #region Enums



    #endregion
}
