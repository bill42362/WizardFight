using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VortexParticle : MonoBehaviour {
	ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;

	private void LateUpdate() {
		InitializeIfNeeded();
		// GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles(m_Particles);

		// Change only the particles that are alive
		for (int i = 0; i < numParticlesAlive; i++) {
			Vector3 position = m_Particles[i].position;
			Vector3 center = Vector3.zero;
			center.z = position.z;
			Vector3 tengent = Vector3.Cross(Vector3.forward, (center - position));
			m_Particles[i].velocity += 0.5f*(i%2 + 1)*tengent;
		}

		// Apply the particle changes to the particle system
		m_System.SetParticles(m_Particles, numParticlesAlive);
	}

	void InitializeIfNeeded() {
		if (m_System == null) {
			m_System = GetComponent<ParticleSystem>();
		}

		if (m_Particles == null || m_Particles.Length < m_System.maxParticles) {
			m_Particles = new ParticleSystem.Particle[m_System.maxParticles]; 
		}
	}
}
