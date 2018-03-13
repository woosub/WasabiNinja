using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour, IEffect
{
	public bool OnlyDeactivate;
	public bool unScaleTime; 
	public float endTime; 


	private float _deltaTime;
	private ParticleSystem _particle ; 

	private bool bEnd ; 
	
	void OnEnable()
	{
		
		bEnd = false; 
		if (unScaleTime) {
			_deltaTime = 0; 
			_particle = GetComponent<ParticleSystem> ();
			_particle.time = 0; 
			_particle.Play (); 
			//_particle.Clear (); 
			//_particle.Play (); 
			//_particle.playOnAwake = false; 
		}
		else
			StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if (OnlyDeactivate) {
					#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
					#else
					this.gameObject.SetActive (false);
					#endif

					bEnd = true; 
				} else
					bEnd = true; 
					EffectPoolManager.Instance.DespawnEffect (gameObject.transform); 
					//GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}

	void Update()
	{
		if (unScaleTime) {


			_deltaTime += Time.unscaledDeltaTime; 
			_particle.Simulate(Time.unscaledDeltaTime, true, false);
			if (_deltaTime >= endTime) {
				bEnd = true; 
				if (OnlyDeactivate) {
					#if UNITY_3_5
					this.gameObject.SetActiveRecursively(false);
					#else
					this.gameObject.SetActive (false);
					#endif
				} else
					EffectPoolManager.Instance.DespawnEffect (gameObject.transform); 
			}
			/*
			if (_particle.isPlaying == false) {
				StopAllCoroutines (); 
				_particle.Stop (); 

				if (OnlyDeactivate) {
					#if UNITY_3_5
					this.gameObject.SetActiveRecursively(false);
					#else
					this.gameObject.SetActive (false);
					#endif
				} else
					EffectPoolManager.Instance.DespawnEffect (gameObject.transform); 

			}
			*/
			/*
			_deltaTime = Time.realtimeSinceStartup - _lastInterval;
			_lastInterval = Time.realtimeSinceStartup;

			_particle.Simulate (_deltaTime); 
			_particle.Play (); 
			*/
		}
	}

	public bool IsEnd ()
	{
		return bEnd;
	}
	public void StartEffect() 
	{
	}
	public void StopEffect() 
	{
		bEnd = true; 
		StopAllCoroutines (); 
		gameObject.SetActive (false); 
	}
}
