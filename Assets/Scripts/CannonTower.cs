using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour 
{
	private const float AllowedFireAngle = 5f;

	public float m_shootInterval = 0.5f;
	public float m_range = 4f;
	public GameObject m_projectilePrefab;
	public Transform m_shootPoint;
	
	public float m_rotationSpeed = 180f;
	public Transform m_cannonWeapon;
	public Transform m_hub;


	private float m_lastShotTime = -0.5f;
	private Monster m_currentTarget;

	private void Update () 
	{
		if (m_projectilePrefab == null || m_shootPoint == null || m_cannonWeapon == null || m_hub == null)
			return;
		
		UpdateTarget();

		if (m_currentTarget != null)
		{
			Vector3 aimPoint = GetPredictedPosition(
				m_currentTarget.transform.position,
				m_currentTarget.Velocity,
				m_projectilePrefab.GetComponent<CannonProjectile>().m_speed);

			Vector3 direction = (aimPoint - m_shootPoint.position).normalized;

			Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
			if (flatDirection.sqrMagnitude > 0.001f)
			{
				Quaternion targetRotY = Quaternion.LookRotation(flatDirection);
				m_hub.rotation = Quaternion.RotateTowards(
					m_hub.rotation,
					targetRotY,
					m_rotationSpeed * Time.deltaTime
				);
			}

			Vector3 localDirection = m_hub.InverseTransformDirection(direction);
			Quaternion targetRotX = Quaternion.LookRotation(localDirection);
			m_cannonWeapon.localRotation = Quaternion.RotateTowards(
				m_cannonWeapon.localRotation,
				targetRotX,
				m_rotationSpeed * Time.deltaTime
			);
			
			Vector3 weaponToAimDirection = (aimPoint - m_shootPoint.position).normalized;
			float angle = Vector3.Angle(m_cannonWeapon.forward, weaponToAimDirection);
			
			if (angle < AllowedFireAngle && m_lastShotTime + m_shootInterval <= Time.time)
			{
				Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
				m_lastShotTime = Time.time;
			}
		}
	}

	private void UpdateTarget()
	{
		if (m_currentTarget != null)
		{
			if (m_currentTarget.m_hp <= 0 ||
			    Vector3.Distance(transform.position, m_currentTarget.transform.position) > m_range) m_currentTarget = null;
		}
		else
		{
			Monster nearestTarget = null;
			float nearestDist = float.MaxValue;

			foreach (var monster in FindObjectsOfType<Monster>())
			{
				float dist = Vector3.Distance(transform.position, monster.transform.position);
				if (dist < nearestDist && dist <= m_range)
				{
					nearestTarget = monster;
					nearestDist = dist;
				}
			}
			
			m_currentTarget = nearestTarget;
		}
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
