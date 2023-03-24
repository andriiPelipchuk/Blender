using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Food.Spawner
{
    public class IngredientsSpawnZone : MonoBehaviour
    {
        public event Action<Ingredient> IngredientEntered;
        public event Action<Ingredient> IngredientLeft;

        [SerializeField] private Vector3 _spawnPositionOffset;
        [SerializeField] private BoxCollider _boxZone;

        public Vector3 GetRandomPositionInSpawnZone()
        {
            var bounds = _boxZone.bounds;
            var randomX = Random.Range(bounds.min.x, bounds.max.x);
            var randomY = Random.Range(bounds.min.y, bounds.max.y);
            var randomZ = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(randomX, randomY, randomZ) + _spawnPositionOffset;
        }

        private void OnTriggerEnter(Collider other)
        {
            var ingredient = other.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                IngredientEntered?.Invoke(ingredient);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var ingredient = other.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                IngredientLeft?.Invoke(ingredient);
            }
        }
    }
}