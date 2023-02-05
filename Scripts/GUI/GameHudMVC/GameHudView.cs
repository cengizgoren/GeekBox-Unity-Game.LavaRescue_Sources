using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameHudView : UIView
{
    #region Fields

    [SerializeField]
    private Image fillEnergyBar;
    [SerializeField]
    private TextMeshProUGUI preparationTimeS;

    #endregion

    #region Propeties

    public Image FillEnergyBar { get => fillEnergyBar; }
    public TextMeshProUGUI PreparationTimeS { get => preparationTimeS; }

    #endregion

    #region Methods

    public override void AttachEvents()
    {
        base.AttachEvents();

        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnEnergyChanged += OnEnergyChange;
        }

        if (GameplayEvents.Instance != null)
        {
            GameplayEvents.Instance.OnMapTimeSUpdate += OnMapTimeSUpdateHandler;
        }
    }

    public override void DettachEvents()
    {
        base.DettachEvents();

        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnEnergyChanged -= OnEnergyChange;
        }

        if(GameplayEvents.Instance != null)
        {
            GameplayEvents.Instance.OnMapTimeSUpdate -= OnMapTimeSUpdateHandler;
        }
    }

    private void OnEnable()
    {
        if(PlayerManager.Instance != null)
        {
            OnEnergyChange(PlayerManager.Instance.CurrentEnergyNormalized);
        }
    }

    // Handlers.
    private void OnEnergyChange(float normalizedValue)
    {
        FillEnergyBar.fillAmount = normalizedValue;
    }

    private void OnMapTimeSUpdateHandler(float timeS)
    {
        float timeMs = timeS * Constants.SECONDS_TO_MILI_FACTOR;
        timeMs = Mathf.Clamp(timeMs, 0f, float.MaxValue);
        PreparationTimeS.SetText(timeMs.ToTimeFormatt("mm:ss:ff"));
    }

    #endregion

    #region Enums



    #endregion
}
