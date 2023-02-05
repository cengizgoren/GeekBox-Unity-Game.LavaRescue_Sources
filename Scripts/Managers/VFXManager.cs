using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;

public class VFXManager : ManagerSingletonBase<VFXManager>, IGameEvents
{
    #region Fields

    [SerializeField]
    private DirtParticle dirtParticlePrefab;

    [Title("Camera Shake")]
    [SerializeField]
    private float cameraShakeDrationS = 0.15f;
    [SerializeField]
    float cameraShakeStrength = 3;
    [SerializeField]
    int cameraShakeVibrato = 10;
    [SerializeField]
    float cameraShakeRandomness = 90;
    [SerializeField]
    bool cameraShakeFadeOut = true;

    #endregion

    #region Propeties

    public DirtParticle DirtParticlePrefab { get => dirtParticlePrefab; }
    private Stack<DirtParticle> DirtParticlesPool { get; set; } = new Stack<DirtParticle>();

    #endregion

    #region Methods

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < 50; i++)
        {
            DirtParticle deathParticle = Instantiate(DirtParticlePrefab);
            deathParticle.transform.ResetParent(transform, false);
            deathParticle.Init((obj) => { DirtParticlesPool.Push(obj); });

            DirtParticlesPool.Push(deathParticle);
        }
    }

    public void DoDirtParticle(Vector3 position, Material mat)
    {
        DirtParticle particle = DirtParticlesPool.Count > 0 ? DirtParticlesPool.Pop() : null;
        if (particle == null)
        {
            particle = Instantiate(DirtParticlePrefab);
            particle.transform.ResetParent(transform, false);
            particle.Init((obj) => { DirtParticlesPool.Push(obj); });
        }

        particle.transform.position = position;
        particle.SetMaterial(mat);
        particle.gameObject.SetActive(true);
    }

    [Button]
    public void DoCameraShake()
    {
        Camera currentCamera = CameraController.Instance.CurrentCamera;
        currentCamera.DOShakePosition(cameraShakeDrationS, cameraShakeStrength, cameraShakeVibrato, cameraShakeRandomness, cameraShakeFadeOut);
    }

    public void DoSuccessCameraVFX()
    {
        
    }

    public void DoConfettiVFX()
    {
        
    }

    public void LoadNextLvl()
    {

    }

    public void RestartLvl()
    {

    }

    public void StopLvlGame()
    {

    }

    public void StartLvlGame()
    {

    }

    public override void AttachEvents()
    {
        base.AttachEvents();

        GameplayEvents.Instance.OnLavaStarted += DoCameraShake;
    }

    protected override void DetachEvents()
    {
        base.DetachEvents();

        GameplayEvents.Instance.OnLavaStarted -= DoCameraShake;
    }

    #endregion

    #region Enums



    #endregion
}
