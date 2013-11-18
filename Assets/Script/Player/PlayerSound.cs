using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
	
	public AudioClip SwordSwing;
	public AudioClip SwordHit;

	public void PlaySound(string _soundToPlay)
	{
		if(_soundToPlay == "SwordSwing")
		{
			audio.PlayOneShot(SwordSwing);	
		}
		else if(_soundToPlay == "SwordHit")
		{
			audio.PlayOneShot(SwordHit);
		}
	}
}
