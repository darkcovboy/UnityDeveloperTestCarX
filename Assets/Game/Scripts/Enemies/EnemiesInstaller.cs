using Game.Scripts.Enemies.Data;
using Game.Scripts.Enemies.Factory;
using Game.Scripts.Enemies.Movement;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Enemies
{
    public class EnemiesInstaller : MonoInstaller
    {
        [SerializeField] private MonsterConfig _monsterConfig;
        [SerializeField] private Monster _monsterPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<Monster>().FromInstance(_monsterPrefab).AsSingle();
            Container.Bind<MonsterConfig>().FromInstance(_monsterConfig).AsSingle();

            Container.Bind<MonsterFactory>().AsSingle();
        }
    }
}