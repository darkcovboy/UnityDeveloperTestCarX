using Game.Scripts.Enemies;
using Game.Scripts.Towers.Projectiles;
using UnityEngine;

namespace Game.Scripts.Towers
{
	public class SimpleTower : TowerBase 
	{
		protected override void Shoot(Monster target)
		{
			var projectile = (GuidedProjectile)Factory.GetProjectile(ProjectileType.Guided,
				transform.position + Vector3.up * 1.5f, Quaternion.identity);
			projectile.SetTarget(target.transform);
		}
	}
}
