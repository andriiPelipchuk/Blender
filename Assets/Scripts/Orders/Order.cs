using System;
using System.Collections.Generic;
using System.Linq;
using Food;
using Food.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace Orders
{
    public class Order
    {
        public event Action<Color> ColorChanged; 

        private RecipeData _recipe;

        private List<Ingredient> _addedIngredients;

        private Color _resultColor;

        public float SuccessRate
        {
            get
            {
                var matchedElements = 0f;

                var addedIngredientsTemp = new List<Ingredient>(_addedIngredients);
                for (var i = 0; i < _recipe.Ingredients.Count; i++)
                {
                    var targetIngredient = _recipe.Ingredients[i];

                    var addedIngredient = addedIngredientsTemp.Find(x => x.Data == targetIngredient.Data);
                    if (addedIngredient != null)
                    {
                        matchedElements++;
                        addedIngredientsTemp.Remove(addedIngredient);
                    }
                }

                var max = Mathf.Max(_recipe.Ingredients.Count, _addedIngredients.Count);
                var rate = matchedElements / max;
                
                return rate;
            }
        }

        public Order(RecipeData recipe)
        {
            _recipe = recipe;

            _addedIngredients = new List<Ingredient>();
            
            _resultColor = Color.black;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            _addedIngredients.Add(ingredient);

            _resultColor = Color.black;

            for (int i = 0; i < _addedIngredients.Count; i++)
            {
                _resultColor += _addedIngredients[i].Data.Color;
            }

            _resultColor /= _addedIngredients.Count;
            
            ColorChanged?.Invoke(_resultColor);
        }

        public Color GetTargetColor()
        {
            var targetColor = Color.black;

            for (int i = 0; i < _recipe.Ingredients.Count; i++)
            {
                targetColor += _recipe.Ingredients[i].Data.Color;
            }

            targetColor /= _recipe.Ingredients.Count;

            return targetColor;
        }
    }
}