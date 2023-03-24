using System;
using Food;
using UnityEngine;

namespace Blender
{
    public class IngredientsMixer : MonoBehaviour
    {
        public event Action<Ingredient> IngredientMixed;

        private void OnTriggerEnter(Collider other)
        {
            var ingredient = other.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                ingredient.DestroyIngredient();
                IngredientMixed?.Invoke(ingredient);
            }
        }
    }
}