using System;
using UnityEngine;

namespace Food
{
    public class IngredientDeadZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var ingredient = other.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                ingredient.DestroyIngredient();
            }
        }
    }
}