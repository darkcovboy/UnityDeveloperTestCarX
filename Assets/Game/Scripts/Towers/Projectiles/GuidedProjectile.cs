using Game.Scripts.Enemies.Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Towers.Projectiles
{
	public class GuidedProjectile : ProjectileBase 
	{
		[FormerlySerializedAs("m_speed")]
		[SerializeField] private float _speed = 5f;
		private Transform _target;

		public void SetTarget(Transform target)
		{
			_target = target;
		}

		private void Update()
		{
			if (_target == null)
			{
				Destroy(gameObject);
				return;
			}

			Vector3 toTarget = _target.position - transform.position;
			Vector3 step = toTarget.normalized * _speed * Time.deltaTime;
			transform.Translate(step);
		}
	}
}
