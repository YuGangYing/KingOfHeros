using UnityEngine;
using System.Collections;

public class RealTimeParticle : MonoBehaviour {

	private void Awake()
	{
//		particle = GetComponent<ParticleSystem>();
		emitter = GetComponent<ParticleEmitter>();
	}
	
	// Use this for initialization
	void Start ()
	{
//		Time.timeScale = 0;
		lastTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		float deltaTime = Time.realtimeSinceStartup - (float)lastTime;
		emitter.Simulate(deltaTime);
//		particle.Simulate(deltaTime, true, false); //last must be false!!
		lastTime = Time.realtimeSinceStartup;
	}
	ParticleEmitter emitter;
	private double lastTime;
//	private ParticleSystem particle;
}
