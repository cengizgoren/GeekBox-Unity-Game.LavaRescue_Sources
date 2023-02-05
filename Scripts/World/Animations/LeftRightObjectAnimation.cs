using UnityEngine;
using System.Collections;

public class LeftRightObjectAnimation : MonoBehaviour
{

    #region Fields

    [SerializeField]
    private Transform leftPoint;
    [SerializeField]
    private Transform rightPoint;
    [SerializeField]
    private Transform targetObject;

    [Space]
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool randomStartPosition = true;
    [SerializeField]
    private bool randomDirection = true;

    #endregion

    #region Propeties

    public Transform LeftPoint { get => leftPoint; }
    public Transform RightPoint { get => rightPoint; }
    public float Speed { get => speed; }
    public bool RandomStartPosition { get => randomStartPosition; }
    public bool RandomDirection { get => randomDirection; }
    private int Direction { get; set; } = 1;
    public Transform TargetObject { get => targetObject; }


    #endregion

    #region Methods

    private void Start()
    {
        if (RandomDirection == true)
        {
            Direction = (UnityEngine.Random.Range(0, 10) / 2) == 0 ? -1 : 1;
        }

        if(RandomStartPosition == true)
        {
            Vector3 newPosition = new Vector3(TargetObject.localPosition.x, TargetObject.localPosition.y, UnityEngine.Random.Range(LeftPoint.localPosition.z, RightPoint.localPosition.z));
            TargetObject.localPosition = newPosition;
        }
    }

    private void Update()
    {
        float step = Direction * Speed * Time.deltaTime;
        Vector3 newPosition = new Vector3(TargetObject.localPosition.x, TargetObject.localPosition.y, TargetObject.localPosition.z + step);

        if (newPosition.z >= RightPoint.localPosition.z)
        {
            newPosition.z = RightPoint.localPosition.z;
            Direction = -1;
        }
        else if (newPosition.z <= LeftPoint.localPosition.z)
        {
            newPosition.z = LeftPoint.localPosition.z;
            Direction = 1;
        }

        TargetObject.localPosition = newPosition;
    }

    private void OnDrawGizmos()
    {
        // Dla celow debugowych.
        BoxCollider colider = TargetObject.GetComponent<BoxCollider>();

        if (colider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(LeftPoint.position + colider.center, colider.size);
            Gizmos.DrawWireCube(RightPoint.position + colider.center, colider.size);
        }
    }

    #endregion

    #region Enums



    #endregion
}
