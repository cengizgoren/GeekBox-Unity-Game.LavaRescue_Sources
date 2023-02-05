using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class TriggerBasedObjectAnimation : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private string doMoveTriggerName = "DoMove";

    [Space]
    [SerializeField]
    private float minDelay;
    [SerializeField]
    private float maxDelay;

    #endregion

    #region Propeties

    public string DoMoveTriggerName { get => doMoveTriggerName; }
    public float MinDelay { get => minDelay; }
    public float MaxDelay { get => maxDelay; }
    private Animator CurrentAnimator { get; set; }

    #endregion

    #region Methods

    private void Awake()
    {
        CurrentAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        MEC.Timing.RunCoroutine(_WaitAndTriggerAnimator().CancelWith(gameObject));
    }

    private IEnumerator<float> _WaitAndTriggerAnimator()
    {
        while (true)
        {
            yield return MEC.Timing.WaitForSeconds(RandomMath.RandomRangeUnity(MinDelay, MaxDelay));

            CurrentAnimator.SetTrigger(DoMoveTriggerName);
        }
    }

    #endregion

    #region Enums



    #endregion
}
