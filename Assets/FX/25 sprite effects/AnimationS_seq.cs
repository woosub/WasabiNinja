using UnityEngine;
using System.Collections;

public class AnimationS_seq: MonoBehaviour, IEffect
{
    public float fps = 24.0f;
	public bool loop = false; 
	public SpriteRenderer rendererMy;
	public Sprite[] frames ; 


    private int frameIndex;




	private bool isEnd ; 

    void Start()
    {
		frameIndex = 0; 
		isEnd = false; 

		StartEffect (); 
    }

    void NextFrame()
    {
        
		rendererMy.sprite = frames[frameIndex] ; 
		frameIndex = (frameIndex + 0001);// % frames.Length;

		if (frameIndex >= frames.Length) {
			if (loop)
				frameIndex = frameIndex % frames.Length;
			else {
				StopEffect (); 
				frameIndex =  frames.Length - 1;
			}
		}
    }

	public bool IsEnd (){
		return isEnd;
	}

	public void StartEffect()
	{
		isEnd = false;
		frameIndex = 0; 

		NextFrame();
		InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
	}


	public void StopEffect() 
	{
		isEnd = true; 
		CancelInvoke (); 
		gameObject.SetActive (false); 
	}




}