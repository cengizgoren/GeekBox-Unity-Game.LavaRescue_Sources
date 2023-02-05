using UnityEngine;
using UnityEngine.Events;

public class LvlEventsListener : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private UnityEvent onLavaStarted = new UnityEvent();

    #endregion

    #region Propeties



    #endregion

    #region Methods

    private void Start()
    {
        MapsManager.Instance.CurrentMap.OnLavaStarted += OnLavaStartedHandler;
    }

    private void OnDestroy()
    {
        if(MapsManager.Instance != null && MapsManager.Instance.CurrentMap != null)
        {
            MapsManager.Instance.CurrentMap.OnLavaStarted -= OnLavaStartedHandler;
        }
    }

    private void OnLavaStartedHandler()
    {
        if(onLavaStarted != null)
        {
            onLavaStarted.Invoke();
        }
    }

    #endregion

    #region Enums



    #endregion
}
