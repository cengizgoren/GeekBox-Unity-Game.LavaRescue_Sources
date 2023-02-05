using System;
using UnityEngine;

public class DirtParticle : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private ParticleSystemRenderer particles;

    #endregion

    #region Propeties

    public ParticleSystemRenderer Particles { get => particles; }
    private Action<DirtParticle> OnStoppedCallback { get; set; } = delegate { };

    #endregion

    #region Methods

    public void SetMaterial(Material mat)
    {
        Particles.sharedMaterial = mat;
    }

    public void Init(Action<DirtParticle> onStopped)
    {
        OnStoppedCallback = onStopped;
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        OnStoppedCallback(this);
    }

    #endregion

    #region Enums



    #endregion
}
