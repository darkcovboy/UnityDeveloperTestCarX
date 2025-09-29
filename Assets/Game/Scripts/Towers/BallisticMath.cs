using UnityEngine;

namespace Game.Scripts.Towers
{
    public static class BallisticMath
    {
        public static Vector3 GetPredictedPosition(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVelocity, float projectileSpeed)
        {
            Vector3 directionToTarget = targetPos - shooterPos;

            float a = Vector3.Dot(targetVelocity, targetVelocity) - projectileSpeed * projectileSpeed;
            float b = 2f * Vector3.Dot(targetVelocity, directionToTarget);
            float c = Vector3.Dot(directionToTarget, directionToTarget);

            float discriminant = b * b - 4f * a * c;
            if (discriminant < 0 || Mathf.Abs(a) < 0.001f)
                return targetPos;

            float sqrtDiscriminant = Mathf.Sqrt(discriminant);
            float t1 = (-b + sqrtDiscriminant) / (2f * a);
            float t2 = (-b - sqrtDiscriminant) / (2f * a);

            float t = Mathf.Min(t1, t2);
            if (t < 0f) t = Mathf.Max(t1, t2);
            if (t < 0f) return targetPos;

            return targetPos + targetVelocity * t;
        }
        
        public static float CalculateLaunchSpeed(Vector3 shooterPos, Vector3 targetPos, float angleRad)
        {
            Vector3 dir = targetPos - shooterPos;
            Vector3 dirXZ = new Vector3(dir.x, 0, dir.z);

            float x = dirXZ.magnitude;
            float y = dir.y;
            float g = Mathf.Abs(Physics.gravity.y);

            float cos = Mathf.Cos(angleRad);
            float tan = Mathf.Tan(angleRad);

            float numerator = g * x * x;
            float denominator = 2 * cos * cos * (x * tan - y);

            if (denominator <= 0f) return 0f;

            return Mathf.Sqrt(numerator / denominator);
        }
        
        public static Vector3 CalculateBallisticVelocity(Vector3 shooterPos, Vector3 targetPos, float angleRad)
        {
            float launchSpeed = CalculateLaunchSpeed(shooterPos, targetPos, angleRad);
            if (launchSpeed <= 0f)
                return Vector3.zero;

            Vector3 dir = targetPos - shooterPos;
            Vector3 dirXZ = new Vector3(dir.x, 0, dir.z).normalized;

            Vector3 velocity = dirXZ * (launchSpeed * Mathf.Cos(angleRad));
            velocity.y = launchSpeed * Mathf.Sin(angleRad);

            return velocity;
        }
    }
}