using System;
using DG.Tweening;
using Project.Scripts.Extensions;
using UnityEngine;

namespace Project.Scripts.Cell
{
    public class CellView : MonoBehaviour
    {
        public Action<string, CellView> OnCellClicked;

        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private Collider2D mainCollider;

        [SerializeField] private ParticleSystem winParticle;
        [SerializeField] private TweenSettings showCellSetting;
        [SerializeField] private TweenSettings falseCellSetting;
        [SerializeField] private TweenSettings trueCellSetting;

        private string _id;

        public void Construct(CellData cellData, Vector2 cellSize)
        {
            transform.localScale = cellSize;

            if (cellData.Icon.pivot.y > 0)
            {
                icon.transform.Rotate(Vector3.forward, -90);
            }

            icon.sprite = cellData.Icon;
            _id = cellData.Id;
        }


        public void ShowEnableEffect()
        {
            var targetScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(targetScale, showCellSetting.Duration).SetDelay(showCellSetting.Delay)
                .SetEase(showCellSetting.Ease);
        }

        public void ShowFalseCellEffect()
        {
            icon.transform.DOPunchPosition(Vector3.left * falseCellSetting.TargetValue, falseCellSetting.Duration)
                .SetDelay(falseCellSetting.Delay).SetEase(falseCellSetting.Ease);
        }

        public void ShowTrueCellEffect(Action onComplete = null)
        {
            InactiveClick();

            winParticle.Play();
            icon.transform.DOScale(trueCellSetting.TargetValue, trueCellSetting.Duration)
                .SetRelative(trueCellSetting.IsRelative)
                .SetDelay(trueCellSetting.Delay)
                .SetEase(trueCellSetting.EaseCurve)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void InactiveClick()
        {
            mainCollider.enabled = false;
        }

        private void OnMouseUpAsButton()
        {
            OnCellClicked?.Invoke(_id, this);
        }
    }
}