using System;
using System.Collections;
using DG.Tweening;
using Food;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blender
{
    public class Blender : MonoBehaviour
    {
        private static readonly int AnimationOpenTriggerName = Animator.StringToHash("Open");
        private static readonly int AnimationCloseTriggerName = Animator.StringToHash("Close");
        
        public event Action<Ingredient> IngredientMixed;
        public event Action ShakeCompleted; 

        [SerializeField] private float _shakeTime;
        [SerializeField] private float _shakeStrength; 
        [SerializeField] private IngredientsMixer _ingredientsMixer;
        [SerializeField] private float _maxHeight;
        [SerializeField] private MeshRenderer _liquidMat;
        [SerializeField] private ParticleSystem _shakeEffect;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _timeBeforeLidClose;
        [SerializeField] private GameState _gameState;
        
        private Coroutine _shakeCoroutine;
        private Coroutine _waitForCloseCoroutine;

        public void OpenLid()
        {
            if (_waitForCloseCoroutine != null)
            {
                StopCoroutine(_waitForCloseCoroutine);
                _waitForCloseCoroutine = null;
            }
            else
            {
                _animator.SetTrigger(AnimationOpenTriggerName);
            }
            
            _waitForCloseCoroutine = StartCoroutine(WaitBeforeCloseRoutine());
        }
        
        public void Shake()
        {
            if(_gameState.GameSateType != GameSateType.Ordering) return;
            
            _gameState.GameSateType = GameSateType.Shaking;
            
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            
            _shakeCoroutine = StartCoroutine(ShakeRoutine());
        }

        public void Clean()
        {
            _liquidMat.sharedMaterial.SetFloat("_Fill", 0f);
            
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            
            _ingredientsMixer.gameObject.SetActive(false);
            _ingredientsMixer.transform.localPosition = Vector3.zero;
        }
        
        public void UpdateLiquidColor(Color color)
        {
            _liquidMat.sharedMaterial.SetColor("_Color", color);

            var main = _shakeEffect.main;
            main.startColor = color;
        }

        private void Awake()
        {
            _ingredientsMixer.IngredientMixed += OnIngredientMixed;
            Clean();
        }

        private IEnumerator ShakeRoutine()
        {
            _ingredientsMixer.gameObject.SetActive(true);
            _ingredientsMixer.transform.DOLocalMoveY(_maxHeight, _shakeTime);
            
            transform.DOShakeRotation(_shakeTime, _shakeStrength, fadeOut:false);
            
            _shakeEffect.Play();
            
            var fillProgress = 0f;
            var startTime = Time.time;
            while (fillProgress < 1f)
            {
                fillProgress = (Time.time - startTime) / _shakeTime;
                _liquidMat.sharedMaterial.SetFloat("_Fill", fillProgress);
                yield return null;
            }
            
            _shakeEffect.Stop();
            
            ShakeCompleted?.Invoke();
        }

        private IEnumerator WaitBeforeCloseRoutine()
        {
            yield return new WaitForSeconds(_timeBeforeLidClose);
            
            _animator.SetTrigger(AnimationCloseTriggerName);

            _waitForCloseCoroutine = null;
        }

        private void OnIngredientMixed(Ingredient ingredient)
        {
            IngredientMixed?.Invoke(ingredient);
        }
    }
}