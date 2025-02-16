using GameEntities.Player;
using UnityEngine;
using Zenject;

namespace Services.PlayerSpawnService
{
    public class PlayerSpawnService
    {
        private readonly GameObject _playerPrefab;
        private readonly DiContainer _container;
        private PlayerMovement _playerInstance;

        public PlayerMovement Player => _playerInstance;

        public PlayerSpawnService(GameObject playerPrefab, DiContainer container)
        {
            _playerPrefab = playerPrefab;
            _container = container;
        }

        public void SpawnPlayer(Vector3 spawnPosition)
        {
            if (_playerInstance == null)
            {
                _playerInstance = _container.InstantiatePrefabForComponent<PlayerMovement>(_playerPrefab);
                _playerInstance.transform.position = spawnPosition;
            }
        }
    }
}