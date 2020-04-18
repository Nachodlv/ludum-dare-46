using System;
using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactables
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Turnable: Interactable
    {
        [SerializeField, ColorUsage(true, true)] private Color workingColor;
        [SerializeField, ColorUsage(true, true)] private Color notWorkingColor;
        [SerializeField] private float minimumSecondsBeforeBreaking;
        [SerializeField] private float maximumSecondsBeforeBreaking;
        
        public event Action OnStartWorking;
        public event Action OnStopWorking;
        
        private SpriteRenderer _spriteRenderer;
        private Material _material;
        private bool _isWorking = true;
        private Coroutine _coroutine;
        
        protected override void Awake()
        {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _material = _spriteRenderer.material;
            _material.color = workingColor;
            CalculateBreakingTime();
        }

        public override void Interact(Interactor interactor)
        {
            if (_isWorking) return;
            base.Interact(interactor);
            _material.color = workingColor;
            OnStartWorking?.Invoke();
            CalculateBreakingTime();
        }

        private void CalculateBreakingTime()
        {
            var breakingTime = Random.Range(minimumSecondsBeforeBreaking, maximumSecondsBeforeBreaking);
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(WaitForBreaking(breakingTime));
        }

        private IEnumerator WaitForBreaking(float breakingTime)
        {
            yield return new WaitForSeconds(breakingTime);
            _isWorking = false;
            _material.color = notWorkingColor;
            OnStopWorking?.Invoke();
        }
    }
}