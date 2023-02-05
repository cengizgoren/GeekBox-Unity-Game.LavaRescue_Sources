using UnityEngine;

public class DirtParticleHandler : MonoBehaviour
{
    #region Fields

    private DestructibleTerrain currentTerrain = null;

    #endregion

    #region Propeties

    private VFXManager CachedManager { get; set; }

    // Shortcuts.
    public DestructibleTerrain CurrentTerrain { 
        get {
            if(currentTerrain == null)
            {
                currentTerrain = MapsManager.Instance.CurrentMap.Terrain;
            }

            return currentTerrain;
        }
    }

    #endregion

    #region Methods

    public void TriggerParticles(Vector3 worldPosition)
    {
        CachedManager.DoDirtParticle(worldPosition, CurrentTerrain.material);
    }

    private void Awake()
    {
        CachedManager = VFXManager.Instance;
    }

    #endregion

    #region Enums



    #endregion
}
