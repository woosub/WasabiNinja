using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using PathologicalGames;


public interface IEffect
{
	bool IsEnd ();
	void StartEffect() ; 
	void StopEffect() ; 

}

public class EffectPoolManager : MonoBehaviour {

	[Serializable]
	public struct EffectPrefab
	{
		public GameObject _prefap; 
		public int _preloadCount; 
	}


	private static EffectPoolManager _instance = null ; 
	public static EffectPoolManager Instance{ get { return _instance; } }


	public EffectPrefab[] _prefapMap ; 
	//private Dictionary<string, List<EffectPrefab>> _preLoadMap;  
	//private List<GameObject> _effectPoolList; 
	private Dictionary<string, List<GameObject>> __effectPoolMap ; 
	//private Dictionary<string, List<GameObject>> _preLoadMap;  
	private GameObject _gameObject ; 


	void Awake(){

		if (_instance != null) {
			Destroy (this.gameObject); 
			return; 
		}

		_instance = this; 



	}



	// Use this for initialization
	void Start () {
		
		_gameObject = this.gameObject; 
		__effectPoolMap = new Dictionary<string, List<GameObject>> (); 


		foreach (var prefap in _prefapMap) {

			for (int i = 0; i < prefap._preloadCount; ++i) {
				string prefapName = prefap._prefap.name;
				GameObject go = GameObject.Instantiate (prefap._prefap); 
				go.transform.parent = _gameObject.transform; 
				go.SetActive (false); 

				List<GameObject> effectList; 
				if (__effectPoolMap.ContainsKey (prefapName)) {
					effectList = __effectPoolMap[prefapName] ; 
				} else {
					effectList = new List<GameObject> (); 
					__effectPoolMap [prefapName] = effectList; 
				}
				effectList.Add (go); 
			}
		}



	}


	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddEffect(string effectName, Vector3 pos)
	{
		if (__effectPoolMap.ContainsKey (effectName) == false) {
			Debug.LogWarning (effectName + " is not in preLoad."); 
			return; 
		}

		var poolList = __effectPoolMap [effectName]; 

		int index = poolList.FindIndex (e => e.activeSelf == false); 

		if (index < 0) {
			int prefapIndex = System.Array.FindIndex (_prefapMap, p => p._prefap.name.CompareTo (effectName) == 0); 
			if (prefapIndex < 0) {
				Debug.LogWarning (effectName + " is not in preLoad."); 
				return; 
			}
			var prefap = _prefapMap [prefapIndex]; 
			string prefapName = prefap._prefap.name;
			GameObject go = GameObject.Instantiate (prefap._prefap); 
			go.transform.parent = _gameObject.transform; 


			List<GameObject> effectList; 
			if (__effectPoolMap.ContainsKey (prefapName)) {
				effectList = __effectPoolMap [prefapName]; 
			} else {
				effectList = new List<GameObject> (); 
			}
			effectList.Add (go); 

			go.SetActive (true); 

			go.transform.position = pos; 
			go.GetComponent<IEffect> ().StartEffect (); 

		} else {
			poolList [index].SetActive (true); 
			poolList [index].transform.position = pos; 
			poolList [index].GetComponent<IEffect> ().StartEffect (); 

		}

	}


	public void DespawnEffect(Transform transform)
	{
		//_pool.Despawn (transform); 
		transform.GetComponent<IEffect>().StopEffect() ; 
		transform.gameObject.SetActive (false); 
	}
}
