using System;
using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour {
	public float m_speed = 0.2f;
	public float m_speedBalistic = 5f;
	public int m_damage = 10;
	
	private Rigidbody m_rigidbody;
	
	private bool m_isKinematic;

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Launch(Vector3 velocity, bool isKinematic)
	{
		m_isKinematic = isKinematic;

		if (m_isKinematic)
		{
			m_rigidbody.isKinematic = true;
		}
		else
		{
			m_rigidbody.isKinematic = false;
			m_rigidbody.velocity = velocity;
		}
	}

	void Update () 
	{
		if (m_isKinematic)
		{
			var translation = transform.forward * m_speed *Time.deltaTime;
			transform.Translate (translation,Space.World);
		}
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.m_hp -= m_damage;
		if (monster.m_hp <= 0) {
			
			Destroy (monster.gameObject);
		}
		Destroy (gameObject);
	}
}
