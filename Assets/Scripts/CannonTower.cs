using UnityEngine;

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

    public FireMode m_fireMode;
    public float m_parabolicAngle = 45f;


    private float m_lastShotTime = -0.5f;
    private Monster m_currentTarget;

    private void Update()
    {
        if (m_projectilePrefab == null || m_shootPoint == null || m_cannonWeapon == null || m_hub == null)
            return;

        UpdateTarget();

        if(m_currentTarget == null)
            return;
        
        if(m_fireMode == FireMode.Direct)
            UpdateDirect();
        else
            UpdateBallistic();
    }

    #region DirectShooting
    private void UpdateDirect()
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
        float currentAngle = Vector3.Angle(m_cannonWeapon.forward, weaponToAimDirection);

        if (currentAngle < AllowedFireAngle && m_lastShotTime + m_shootInterval <= Time.time)
        {
            FireDirect();
            m_lastShotTime = Time.time;
        }
    }

    private void FireDirect()
    {
        var projectileObj = Instantiate(m_projectilePrefab, m_shootPoint.position, Quaternion.identity);
        var projectile = projectileObj.GetComponent<CannonProjectile>();

        projectileObj.transform.rotation = m_shootPoint.rotation;
        projectile.Launch(m_shootPoint.forward * projectile.m_speed, true);
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
        if (t < 0f) return targetPosition;

        return targetPosition + targetVelocity * t;
    }

    #endregion
    #region Ballistic
    private void UpdateBallistic()
    {
        float angleRad = m_parabolicAngle * Mathf.Deg2Rad;

        float launchSpeed = CalculateLaunchSpeed(m_currentTarget.transform.position, angleRad);
        if (launchSpeed <= 0f) return;

        Vector3 toTargetXZ = new Vector3(
            m_currentTarget.transform.position.x - m_shootPoint.position.x,
            0,
            m_currentTarget.transform.position.z - m_shootPoint.position.z
        );

        float x = toTargetXZ.magnitude;
        float t = x / (launchSpeed * Mathf.Cos(angleRad));

        Vector3 predicted = m_currentTarget.transform.position + m_currentTarget.Velocity * t;

        Vector3 flatDirection = new Vector3(predicted.x - transform.position.x, 0, predicted.z - transform.position.z);
        if (flatDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotY = Quaternion.LookRotation(flatDirection);
            m_hub.rotation = Quaternion.RotateTowards(
                m_hub.rotation,
                targetRotY,
                m_rotationSpeed * Time.deltaTime
            );
        }

        Quaternion targetRotX = Quaternion.Euler(-m_parabolicAngle, 0, 0);
        m_cannonWeapon.localRotation = Quaternion.RotateTowards(
            m_cannonWeapon.localRotation,
            targetRotX,
            m_rotationSpeed * Time.deltaTime
        );

        if (m_lastShotTime + m_shootInterval <= Time.time)
        {
            FireBallistic(predicted, m_parabolicAngle);
            m_lastShotTime = Time.time;
        }
    }
    
    private void FireBallistic(Vector3 predicted, float angleDeg)
    {
        var projectileObj = Instantiate(m_projectilePrefab, m_shootPoint.position, Quaternion.identity);
        var projectile = projectileObj.GetComponent<CannonProjectile>();

        float angleRad = angleDeg * Mathf.Deg2Rad;
        
        float launchSpeed = CalculateLaunchSpeed(predicted, angleRad);
        if (launchSpeed <= 0f) return;

        Vector3 dirXZ = new Vector3(predicted.x - m_shootPoint.position.x, 0, predicted.z - m_shootPoint.position.z).normalized;

        Vector3 velocity = dirXZ * (launchSpeed * Mathf.Cos(angleRad));
        velocity.y = launchSpeed * Mathf.Sin(angleRad);

        projectile.Launch(velocity, false);
    }
    
    private float CalculateLaunchSpeed(Vector3 target, float angle)
    {
        Vector3 dir = target - m_shootPoint.position;
        Vector3 dirXZ = new Vector3(dir.x, 0, dir.z);

        float x = dirXZ.magnitude; 
        float y = dir.y;          
        float g = Mathf.Abs(Physics.gravity.y);

        float cos = Mathf.Cos(angle);
        float tan = Mathf.Tan(angle);

        float numerator = g * x * x;
        float denominator = 2 * cos * cos * (x * tan - y);

        if (denominator <= 0f) return 0f;

        return Mathf.Sqrt(numerator / denominator);
    }
    #endregion
    
    private void UpdateTarget()
    {
        if (m_currentTarget != null)
        {
            if (m_currentTarget.m_hp <= 0 ||
                Vector3.Distance(transform.position, m_currentTarget.transform.position) > m_range)
                m_currentTarget = null;
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

    public enum FireMode
    {
        Direct,
        Parabolic
    }
}