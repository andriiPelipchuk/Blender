using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Food.Spawner
{
    public class IngredientsSpawner : MonoBehaviour
    {
        public event Action<Ingredient> IngredientSpawned; 

        [SerializeField] private Ingredient[] _ingredientsPrefabs;
        [SerializeField] private Transform _ingredientsRoot;
        [SerializeField] private IngredientsSpawnZone _spawnZone;
        [SerializeField] private int _maxAmountOfIngredientsInSpawnZone;
        
        private Dictionary<string, ObjectPool<Ingredient>> _ingredientsPool;

        private List<Ingredient> _ingredientsInSpawnZone;

        public void Initialize()
        {
            _ingredientsPool = new Dictionary<string, ObjectPool<Ingredient>>();
            for (var i = 0; i < _ingredientsPrefabs.Length; i++)
            {
                var ingredient = _ingredientsPrefabs[i];
                _ingredientsPool.Add(ingredient.Data.Name, new ObjectPool<Ingredient>(() => CreateIngredient(ingredient.Data.Name), OnGetIngredient, OnReleaseIngredient));
            }
            
            _ingredientsInSpawnZone = new List<Ingredient>(_maxAmountOfIngredientsInSpawnZone);
            _spawnZone.IngredientEntered += OnIngredientEnteredSpawnZone;
            _spawnZone.IngredientLeft += OnIngredientLeftSpawnZone;
        }

        public void Activate()
        {
            SpawnRandomIngredient();
        }

        private void SpawnRandomIngredient()
        {
            var randomIndex = Random.Range(0, _ingredientsPrefabs.Length);

            var ingredientName = _ingredientsPrefabs[randomIndex].Data.Name;

            var randomIngredient = _ingredientsPool[ingredientName].Get();
            randomIngredient.transform.position = _spawnZone.GetRandomPositionInSpawnZone();

            IngredientSpawned?.Invoke(randomIngredient);
        }

        private Ingredient CreateIngredient(string ingredientName)
        {
            for (int i = 0; i < _ingredientsPrefabs.Length; i++)
            {
                var ingredient = _ingredientsPrefabs[i];
                if (ingredient.Data.Name == ingredientName)
                {
                    var ingredientInstance = Instantiate(ingredient, _ingredientsRoot);
                    ingredientInstance.WasDestroyed += OnIngredientDestroyed;

                    return ingredientInstance;
                }
            }

            throw new Exception("Can't create an ingredient with name: " + ingredientName);
        }
        
        private void OnReleaseIngredient(Ingredient ingredient)
        {
            ingredient.gameObject.SetActive(false);
        }

        private void OnGetIngredient(Ingredient ingredient)
        {
            ingredient.gameObject.SetActive(true);
            ingredient.Rigidbody.velocity = Vector3.zero;
        }
        
        private void OnIngredientEnteredSpawnZone(Ingredient ingredient)
        {
            _ingredientsInSpawnZone.Add(ingredient);

            if (_ingredientsInSpawnZone.Count < _maxAmountOfIngredientsInSpawnZone)
            {
                SpawnRandomIngredient();
            }
        }
        
        private void OnIngredientLeftSpawnZone(Ingredient ingredient)
        {
            _ingredientsInSpawnZone.Remove(ingredient);
            
            if (_ingredientsInSpawnZone.Count < _maxAmountOfIngredientsInSpawnZone)
            {
                SpawnRandomIngredient();
            }
        }
        
        private void OnIngredientDestroyed(Ingredient ingredient)
        {
            _ingredientsPool[ingredient.Data.Name].Release(ingredient);
            _ingredientsInSpawnZone.Remove(ingredient);
        }
    }
}