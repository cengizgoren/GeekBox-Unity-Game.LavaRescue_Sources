using System;
using UnityEngine;

public class GameplayEvents : ManagerSingletonBase<GameplayEvents>
{
    #region Fields

    #endregion

    #region Propeties

    public event Action<int> OnBallStopOnWall = delegate { };
    public event Action OnBallBreakWall = delegate { };
    public event Action<float> OnMapTimeSUpdate = delegate { };
    public event Action OnBarricadeAdded = delegate { };
    public event Action OnLavaStarted = delegate { };

    /// <summary>
    /// vector3 - worldposition; int - ilosc
    /// </summary>
    public event Action<Vector3, int> OnCollectMoney = delegate { };

    #endregion

    #region Methods

    public void NotifyOnLavaStarted()
    {
        OnLavaStarted();
    }

    public void NotifyBarricadeAdd()
    {
        OnBarricadeAdded();
    }

    public void NotifyBallStopOnWall(int breakedWalls)
    {
        OnBallStopOnWall(breakedWalls);
    }

    public void NotifyBallBreakWall()
    {
        OnBallBreakWall();
    }

    public void NotifyOnCollectMoney(Vector3 worldPosition, int ammount)
    {
        OnCollectMoney(worldPosition, ammount);
    }

    public void NotifyOnMapTimeSUpdate(float timeS)
    {
        OnMapTimeSUpdate(timeS);
    }

    #endregion

    #region Enums



    #endregion
}
