using UnityEngine;

namespace CursorSystem.FollowersObj
{
    public class CursorBarricadeFollowerObject : CursorFollowerObject<BarricadeObject>
    {
        #region Fields



        #endregion

        #region Propeties



        #endregion

        #region Methods

        protected override void RefreshVisualization()
        {
            base.RefreshVisualization();

            AttachedObject.RefreshProbes();
        }

        public override void OnMouseRelease()
        {
            base.OnMouseRelease();

            if(AttachedObject.CanBuild == true)
            {
                Transform parent = MapsManager.Instance.CurrentMap.transform;
                BarricadeObject newBarridace = CreateObject(parent);
                newBarridace.SetSelectionMode(true);
                newBarridace.transform.position = AttachedObject.transform.position;

                GameplayEvents.Instance.NotifyBarricadeAdd();
                SelectingManager.Instance.SelectObject(newBarridace);
            }
        }

        protected override BarricadeObject CreateObject(Transform parent)
        {
            BarricadeObject newBarricade = GameObject.Instantiate(WorldObjectsSettings.Instance.BarricadePrefab);
            newBarricade.transform.ResetParent(parent);
            newBarricade.SetSelectionMode(false);

            return newBarricade;
        }

        #endregion

        #region Enums



        #endregion
    }
}
