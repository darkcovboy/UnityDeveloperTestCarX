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
		//public FlyingShield m_flyingShieldPrefab;

		private Coroutine _spawnCoroutine;
		private MonsterFactory _monsterFactory;

		[Inject]
		public void Construct(MonsterFactory factory)
		{
			_monsterFactory = factory;
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
			return new DirectMovement(_target);
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
