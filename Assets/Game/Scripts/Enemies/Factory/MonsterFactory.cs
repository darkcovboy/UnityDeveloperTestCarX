using Game.Scripts.Enemies.Data;
using Game.Scripts.Enemies.Movement;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Enemies.Factory
{
    public class MonsterFactory
    {
        private readonly MonsterConfig _config;
        private readonly Monster _prefab;

        public MonsterFactory(MonsterConfig config, Monster prefab)
        {
            _config = config;
            _prefab = prefab;
        }

        public Monster Create(Vector3 position, IMovementStrategy strategy)
        {
            var monster = Object.Instantiate(_prefab, position, Quaternion.identity, null);
            
            monster.Initialize(_config, strategy);
            
            return monster;
        }
    }
}