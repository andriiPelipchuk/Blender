using System;
using Food.Data;
using UnityEngine;

namespace Food
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ingredient : MonoBehaviour, IComparable
    {
        public event Action<Ingredient> WasClicked;
        public event Action<Ingredient> WasDestroyed;

        [SerializeField] private IngredientData _data;

        public Rigidbody Rigidbody { get; private set; }

        public IngredientData Data => _data;
        
        private void OnEnable()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnMouseDown()
        {
            WasClicked?.Invoke(this);
        }

        public void DestroyIngredient()
        {
            WasDestroyed?.Invoke(this);
        }

        public int CompareTo(object other)
        {
            if (other is Ingredient otherIngredient)
            {
                return String.Compare(Data.Name, otherIngredient.Data.Name, StringComparison.Ordinal);
            }

            return -1;
        }
    }
}