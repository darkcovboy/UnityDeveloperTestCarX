using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour 
{
	public float m_shootInterval = 0.5f;
	public float m_range = 4f;
	public GameObject m_projectilePrefab;
	public Transform m_shootPoint;

	private float m_lastShotTime = -0.5f;

	private void Update () 
	{
		if (m_projectilePrefab == null || m_shootPoint == null)
			return;

		foreach (var monster in FindObjectsOfType<Monster>()) 
		{
			if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
				continue;

			if (m_lastShotTime + m_shootInterval > Time.time)
				continue;

			Vector3 aimPoint = GetPredictedPosition(monster.transform.position, monster.Velocity, m_projectilePrefab.GetComponent<CannonProjectile>().m_speed);
			
			Vector3 direction = (aimPoint - m_shootPoint.position).normalized;
			m_shootPoint.rotation = Quaternion.LookRotation(direction);
			
			// shot
			Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);

			m_lastShotTime = Time.time;
			break;
		}
	}
	
	private Vector3 GetPredictedPosition(Monster monster, float projectileSpeed)
	{
		Vector3 targetPos = monster.transform.position;
		Vector3 targetVel = (monster.m_moveTarget.transform.position - monster.transform.position).normalized * monster.m_speed;

		float distance = Vector3.Distance(m_shootPoint.position, targetPos);
		float time = distance / projectileSpeed;

		return targetPos + targetVel * time;
	}


	private Vector3 GetPredictedPosition(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
	{
		Vector3 directionToTarget = targetPosition - m_shootPoint.position;
		
		float a = Vector3.Dot(targetVelocity, targetVelocity) - projectileSpeed * projectileSpeed;
		float b = 2f * Vector3.Dot(targetVelocity, directionToTarget);
		float c = Vector3.Dot(directionToTarget, directionToTarget);
		
		float discriminant = b * b - 4f * a * c;
		if (discriminant < 0 || Mathf.Abs(a) < 0.001f)
		{
			return targetPosition;
		}
		
		float sqrtDiscriminant = Mathf.Sqrt(discriminant);
		float t1 = (-b + sqrtDiscriminant) / (2f * a);
		float t2 = (-b - sqrtDiscriminant) / (2f * a);
		
		float t = Mathf.Min(t1, t2);

		if (t < 0f) t = Mathf.Max(t1, t2);
		if(t < 0f) return targetPosition;

		return targetPosition + targetVelocity * t;
	}
}
