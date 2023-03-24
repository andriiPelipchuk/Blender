using System.Collections.Generic;
using UnityEngine;

namespace Food.Data
{
    [CreateAssetMenu(fileName = "RecipeData", menuName = "Data/Recipe Data")]
    public class RecipeData : ScriptableObject
    {
        public string Name;
        public List<Ingredient> Ingredients;
    }
}