using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "WorldObjectsSettings.asset", menuName = "Settings/WorldObjectsSettings")]
public class WorldObjectsSettings : ScriptableObject
{
    #region Fields

    private static WorldObjectsSettings instance;

    [SerializeField]
    private BarricadeObject barricadePrefab;

    #endregion

    #region Propeties

    public static WorldObjectsSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<WorldObjectsSettings>("Settings/WorldObjectsSettings");
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public BarricadeObject BarricadePrefab {
        get => barricadePrefab;
    }

    #endregion

    #region Methods



    #endregion

    #region Enums



    #endregion
}
