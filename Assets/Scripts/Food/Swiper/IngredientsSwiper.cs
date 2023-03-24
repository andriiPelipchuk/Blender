using System.Collections.Generic;
using Tip;
using UnityEngine;

namespace Food.Swiper
{
    public class IngredientsSwiper : MonoBehaviour
    {
        [SerializeField] private float _moveForce;
        [SerializeField][Range(0, 1f)] private float _inputDeltaDeadZone;
        [SerializeField] private TipBubble _tip;
        [SerializeField] private float _idleDelayFroTip;
        [SerializeField] private GameState _gameState;
        
        private IReadOnlyList<Ingredient> _ingredients;

        private float _startSwipteInputPositionX;
        private float _lastSwipeTime;

        public void Initialize(IReadOnlyList<Ingredient> availableIngredients)
        {
            _ingredients = availableIngredients;
        }
        
        private void Update()
        {
            if(_gameState.GameSateType != GameSateType.Ordering) return;
            
            if (Time.time - _lastSwipeTime > _idleDelayFroTip)
            {
                _tip.Show();
                _lastSwipeTime = Time.time;
            }
            
            if (Input.touchCount > 0)
            {
                TouchUpdate();
            }
            else
            {
                MouseUpdate();
            }
        }

        private void MouseUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartSwipe(Input.mousePosition);
            } 
            else if (Input.GetMouseButton(0))
            {
                SwipeIngredients(Input.mousePosition);
            }
        }

        private void TouchUpdate()
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                StartSwipe(touch.position);
            }
            else
            {
                SwipeIngredients(touch.position);
            }
        }

        private void StartSwipe(Vector2 inputPosition)
        {
            _startSwipteInputPositionX = inputPosition.x;
        }
        
        private void SwipeIngredients(Vector2 inputPosition)
        {
            var currentXPosition = inputPosition.x;
            var delta = currentXPosition - _startSwipteInputPositionX;
            delta /= Screen.width;

            if (Mathf.Abs(delta) < _inputDeltaDeadZone) return;

            for (int i = 0; i < _ingredients.Count; i++)
            {
                var ingredient = _ingredients[i];
                ingredient.Rigidbody.AddForce(Vector3.right * (delta * _moveForce * Time.deltaTime));
            }

            _lastSwipeTime = Time.time;
        }
    }
}