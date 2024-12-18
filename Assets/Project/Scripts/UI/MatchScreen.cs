using System;
using DG.Tweening;
using Project.Scripts.EventBuss;
using Project.Scripts.Extensions;
using Project.Scripts.Match;
using Project.Scripts.Services;
using Project.Scripts.SpecificStates;
using Project.Scripts.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace Project.Scripts.UI
{
    public struct RestartButtonOnClick
    {
    }

    public class MatchScreen : MonoBehaviour, 
        IListener<ChooseWinConditionId>, 
        IListener<ExitState<MatchState>>,
        IListener<StartState<StartMatchState>>,
        IListener<UniqueValuesEnded>
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private Button restartButton;

        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup loaderFadeCanvasGroup;
        [SerializeField] private CanvasGroup restartWindowCanvasGroup;
        [SerializeField] private CanvasGroup restartSessionCanvasGroup;

        [SerializeField] private TweenSettings fadeSettings;

        private string _titleTextFormat;
        private IEventBuss _eventBuss;

        [Inject]
        public void Construct(IEventBuss eventBuss, MatchConfig config)
        {
            _eventBuss = eventBuss;
            _titleTextFormat = config.TitleTextFormat;

            loaderFadeCanvasGroup.Hide();
            restartWindowCanvasGroup.Hide();

            _eventBuss.AddListener<ChooseWinConditionId>(this);
            _eventBuss.AddListener<ExitState<MatchState>>(this);
            _eventBuss.AddListener<StartState<StartMatchState>>(this);
            _eventBuss.AddListener<UniqueValuesEnded>(this);
        }

        public void Execute(ChooseWinConditionId value)
        {
            title.text = string.Format(_titleTextFormat, value.ConditionId);
        }

        public void Execute(UniqueValuesEnded value)
        {
            restartSessionCanvasGroup.Show();
        }

        public void Execute(StartState<StartMatchState> value)
        {
            Fade(loaderFadeCanvasGroup, 0);
            Fade(titleCanvasGroup, 1);
        }

        public void Execute(ExitState<MatchState> value)
        {
            restartButton.onClick.AddListener(RestartButtonOnClick);
            Fade(restartWindowCanvasGroup, 1, () => { restartWindowCanvasGroup.Show(); });
        }

        private void RestartButtonOnClick()
        {
            restartButton.onClick.RemoveListener(RestartButtonOnClick);
            Fade(loaderFadeCanvasGroup, 1, () =>
            {
                restartWindowCanvasGroup.Hide();
                _eventBuss.Execute(new RestartButtonOnClick());
            });
        }

        private void Fade(CanvasGroup canvasGroup, int targetValue, Action onComplete = null)
        {
            canvasGroup.DOFade(targetValue, fadeSettings.Duration)
                .SetDelay(fadeSettings.Delay)
                .SetEase(fadeSettings.Ease)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}