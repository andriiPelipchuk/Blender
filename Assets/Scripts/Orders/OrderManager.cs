using Food;
using Food.Data;
using Orders.Bubble;
using Orders.Popup;
using UnityEngine;

namespace Orders
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private RecipeData[] _recipes;
        [SerializeField] private Blender.Blender _blender;
        [SerializeField] private IngredientsManager _ingredientsManager;
        [SerializeField] private OrderCompletePopup _completePopup;
        [SerializeField] private OrderBubble _orderBubble;
        [SerializeField] private GameState _gameState;

        private Order _currentOrder;
        private int _currentOrderIndex;

        public void BeginOrders()
        {
            _currentOrderIndex = 0;
            
            StartNewOrder(_currentOrderIndex);
        }
        
        private void Awake()
        {
            _blender.IngredientMixed += OnIngredientMixed;
            _blender.ShakeCompleted += OnShakeCompleted;

            _completePopup.RestartClicked += Restart;
            _completePopup.NextClicked += StartNextOrder;
        }

        private void StartNextOrder()
        {
            _currentOrderIndex++;
            _currentOrderIndex %= _recipes.Length;
            
            StartNewOrder(_currentOrderIndex);
        }

        private void Restart()
        {
            StartNewOrder(_currentOrderIndex);
        }

        private void StartNewOrder(int orderIndex)
        {
            if (_currentOrder != null)
            {
                _currentOrder.ColorChanged -= OnColorChanged;
            }
            
            _ingredientsManager.Restart();
            _blender.Clean();
            
            _ingredientsManager.Activate();
            
            _currentOrder = new Order(_recipes[orderIndex]);
            _currentOrder.ColorChanged += OnColorChanged;
            
            _orderBubble.Show(_currentOrder.GetTargetColor());

            _gameState.GameSateType = GameSateType.Ordering;
        }

        private void OnColorChanged(Color color)
        {
            _blender.UpdateLiquidColor(color);
        }

        private void OnIngredientMixed(Ingredient ingredient)
        {
            _currentOrder.AddIngredient(ingredient);
        }
        
        private void OnShakeCompleted()
        {
            _gameState.GameSateType = GameSateType.Popup;
            
            var result = _currentOrder.SuccessRate;
            _completePopup.OpenPopup(result);
            
            _currentOrder.ColorChanged -= OnColorChanged;
            _currentOrder = null;
        }
    }
}