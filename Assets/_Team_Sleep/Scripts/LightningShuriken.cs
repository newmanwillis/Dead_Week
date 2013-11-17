/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;

public class LightningShuriken : MonoBehaviour
{
	public Transform target;
	public int zigs = 100;
	public ParticleSystem particle_system;
	public float speed = 1f;
	public float scale = 1f;
	public int length = 100;
//	public Light startLight;
//	public Light endLight;
	
	Perlin noise;
	float oneOverZigs;
	
	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
	
	void Start()
	{
		oneOverZigs = 1f / (float)zigs;
	}
	
	void Update ()
	{
		int length = particle_system.GetParticles(particles);
		if (noise == null)
			noise = new Perlin();
			
		float timex = Time.time * speed * 0.1365143f;
		float timey = Time.time * speed * 1.21688f;
		float timez = Time.time * speed * 2.5564f;
		
		for (int i=0; i < length; i++)
		{
			Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
			Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
										noise.Noise(timey + position.x, timey + position.y, timey + position.z),
										noise.Noise(timez + position.x, timez + position.y, timez + position.z));
			position += (offset * scale * ((float)i * oneOverZigs));
			
			particles[i].position = position;
//			particles[i].color = Color.white;
//			particles[i].energy = 1f;
		}
		
		particle_system.SetParticles(particles, length);
		
	}	
}