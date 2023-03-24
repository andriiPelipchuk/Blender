using UnityEngine;

namespace Food.Data
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "Data/Ingredient Data")]
    public class IngredientData : ScriptableObject
    {
        [SerializeField] private Material _colorMaterial;
        
        public string Name;
        public Color Color => _colorMaterial.color;
    }
}