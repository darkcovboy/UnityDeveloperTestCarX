using System;
using Game.Scripts.Enemies.Data;
using Game.Scripts.Enemies.Health;
using Game.Scripts.Enemies.Movement;
using UnityEngine;

namespace Game.Scripts.Enemies
{
	public class Monster : MonoBehaviour 
	{
		[SerializeField] private MonsterHealth _health;
		[SerializeField] private MonsterMovement _movement;

		public void Initialize(MonsterConfig config, IMovementStrategy strategy)
		{
			_health.Initialize(config.MaxHealth);
			_movement.Initialize(config, strategy);
		}

		public MonsterHealth Health => _health;
		public MonsterMovement Movement => _movement;

	}
}
