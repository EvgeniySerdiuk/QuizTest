using System;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Extensions
{
    [Serializable]
    public class TweenSettings
    {
        [field: SerializeField] public float TargetValue { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public float FromValue { get; private set; }
        [field: SerializeField] public Ease Ease { get; private set; }
        [field: SerializeField] public AnimationCurve EaseCurve { get; private set; }
        [field: SerializeField] public bool IsRelative { get; private set; }
    }
}