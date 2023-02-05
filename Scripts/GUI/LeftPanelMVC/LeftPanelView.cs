using UnityEngine;
using System.Collections;
using TMPro;

public class LeftPanelView : UIView
{
    #region Fields

    [SerializeField]
    private GameObject barricadeButton;
    [SerializeField]
    private TextMeshProUGUI barricadesAmmountText;

    #endregion

    #region Propeties

    public GameObject BarricadeButton { 
        get => barricadeButton;
    }
    public TextMeshProUGUI BarricadesAmmountText { 
        get => barricadesAmmountText;
    }

    private LeftPanelModel CurrentModel { get; set; }

    #endregion

    #region Methods

    public override void AttachEvents()
    {
        base.AttachEvents();

        if(MapsManager.Instance != null)
        {
            MapsManager.Instance.OnMapSpawned += OnMapSpawnedHandler;
        }
    }

    public override void DettachEvents()
    {
        base.DettachEvents();

        if (MapsManager.Instance != null)
        {
            MapsManager.Instance.OnMapSpawned -= OnMapSpawnedHandler;
        }
    }

    private void Awake()
    {
        CurrentModel = GetModel<LeftPanelModel>();

        if(MapsManager.Instance != null)
        {
            OnMapSpawnedHandler(MapsManager.Instance.CurrentMap);
        }
    }

    private void OnMapSpawnedHandler(VillageLvlMap map)
    {
        if(CurrentModel != null && map != null)
        {
            BarricadeButton.SetActive(CurrentModel.CanShowButton());
            BarricadesAmmountText.SetText(CurrentModel.GetBarricadesAmmount().ToString());

            map.OnBarricadesAmmountChanged += OnBarricadesAmmountChangedHandler;
        }
    }

    private void OnBarricadesAmmountChangedHandler()
    {
        BarricadesAmmountText.SetText(CurrentModel.GetBarricadesAmmount().ToString());
    }

    #endregion

    #region Enums



    #endregion
}
