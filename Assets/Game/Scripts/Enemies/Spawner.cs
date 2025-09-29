using System;
using System.Collections;
using Game.Scripts.Enemies.Data;
using Game.Scripts.Enemies.Factory;
using Game.Scripts.Enemies.Movement;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Scripts.Enemies
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField] private MovementType _movementType;
		[FormerlySerializedAs("m_interval")]
		[SerializeField, Min(0.1f)] private float _interval = 3f;
		[FormerlySerializedAs("m_moveTarget")]
		[SerializeField] private Transform _target;

		[SerializeField] private Transform _towerPoint;
		//public FlyingShield m_flyingShieldPrefab;

		private Coroutine _spawnCoroutine;
		private MonsterFactory _monsterFactory;
		private MonsterConfig _monsterConfig;

		[Inject]
		public void Construct(MonsterConfig config,MonsterFactory factory)
		{
			_monsterFactory = factory;
			_monsterConfig = config;
		}

		private void Start()
		{
			_spawnCoroutine = StartCoroutine(Spawn());
		}

		private IEnumerator Spawn()
		{
			while (true)
			{
				IMovementStrategy strategy = CreateStrategy();
				_monsterFactory.Create(transform.position, strategy);
				yield return new WaitForSeconds(_interval);
			}
		}

		private IMovementStrategy CreateStrategy()
		{
			return _movementType switch
			{
				MovementType.Direct => new DirectMovement(_target,_monsterConfig.Speed),
				MovementType.Accelerating => new AcceleratingMovement(_target, _monsterConfig.Speed,
					_monsterConfig.Acceleration),
				MovementType.Circular => new CircularMovement(_towerPoint, _monsterConfig.Speed,_monsterConfig.Radius, _monsterConfig.AngularSpeed),
				_ => new DirectMovement(_target, _monsterConfig.Speed)
			};
		}

		private void OnDestroy()
		{
			if (_spawnCoroutine != null)
			{
				StopCoroutine(_spawnCoroutine);
				_spawnCoroutine = null;
			}
		}
	}
}
