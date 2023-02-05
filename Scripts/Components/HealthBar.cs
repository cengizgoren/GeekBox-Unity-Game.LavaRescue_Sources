using UnityEngine;
using System.Collections;
using TMPro;

public class HealthBar : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private bool disableOnZero = true;

    [Space]
    [SerializeField]
    private Transform slider;
    [SerializeField]
    private TextMeshPro hpInfoText;

    #endregion

    #region Propeties

    public Transform Slider { get => slider; }
    public TextMeshPro HpInfoText { get => hpInfoText; }
    public bool DisableOnZero { get => disableOnZero; }
    private float MaxHealth { get; set; }

    #endregion

    #region Methods

    public void Init(int maxHealth)
    {
        MaxHealth = maxHealth;
        RefreshBar(MaxHealth);
    }

    public void RefreshBar(float currentHealth)
    {
        float healthNormalized = currentHealth / MaxHealth;
        Slider.transform.localScale = new Vector3(healthNormalized, Slider.localScale.y, Slider.localScale.z);
        HpInfoText.SetText(currentHealth.ToString());

        if(DisableOnZero == true)
        {
            gameObject.SetActive(!(currentHealth <= 0f));
        }
    }

    #endregion

    #region Enums



    #endregion
}
