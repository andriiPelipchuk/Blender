using System;
using System.Collections.Generic;
using Food.Flyer;
using Food.Spawner;
using Food.Swiper;
using UnityEngine;

namespace Food
{
    public class IngredientsManager : MonoBehaviour
    {
        [SerializeField] private IngredientsSpawner _spawner;
        [SerializeField] private IngredientsSwiper _swiper;
        [SerializeField] private IngredientsFlyer _flyer;
        [SerializeField] private Blender.Blender _blender;
        [SerializeField] private GameState _gameState;

        private List<Ingredient> _availableIngredients;

        public void Activate()
        {
            _spawner.Activate();
        }
        
        public void Restart()
        {
            for (int i = _availableIngredients.Count - 1; i >= 0; i--)
            {
                _availableIngredients[i].DestroyIngredient();
            }
        }
        
        private void Awake()
        {
            _availableIngredients = new List<Ingredient>();

            _flyer.Initialize();
            _spawner.Initialize();
            _swiper.Initialize(_availableIngredients);

            _spawner.IngredientSpawned += OnIngredientBecameAvailable;
        }

        private void OnIngredientBecameAvailable(Ingredient ingredient)
        {
            ingredient.WasDestroyed += OnIngredientDestroyed;
            ingredient.WasClicked += OnIngredientWasClicked;
            
            _availableIngredients.Add(ingredient);
        }

        private void OnIngredientDestroyed(Ingredient ingredient)
        {
            ingredient.WasDestroyed -= OnIngredientDestroyed;
            ingredient.WasClicked -= OnIngredientWasClicked;

            _availableIngredients.Remove(ingredient);
        }
        
        private void OnIngredientWasClicked(Ingredient ingredient)
        {
            if(_gameState.GameSateType != GameSateType.Ordering) return;
            
            _flyer.LaunchIngredientToTarget(ingredient);
            _blender.OpenLid();
        }
    }
}