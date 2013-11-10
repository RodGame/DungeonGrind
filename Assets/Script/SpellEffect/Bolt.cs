using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {
	
	private float _speed = 0.25f;
	private float _timeLived = 0.0f;
	private float _timeLife = 0.5f;
	// Update is called once per frame
	void Update () {
		_timeLived += Time.deltaTime;
		if(_timeLived >= _timeLife)
		{
			Destroy(transform.gameObject);
		}
		else
		{
			transform.position += transform.forward*(_speed);
		}
	}
	
	void OnCollisionEnter(Collision _Collision)
	{
		if(_Collision.collider.tag == "Monster")
		{
			
			int spellDamage = Character.SpellList[(int)SpellName.IceBolt].Damage;
			MonsterProfile _MonsterProfile;
			if(_Collision.collider.gameObject.GetComponent<MonsterProfile>() != null)
			{
					_MonsterProfile = _Collision.collider.gameObject.GetComponent<MonsterProfile>();
			}
			else
			{
					_MonsterProfile = _Collision.collider.gameObject.transform.parent.GetComponent<MonsterProfile>();
			}
			
			// Damage Monster
			_MonsterProfile.DamageMonster(spellDamage);
			
			// Give Reward for skill usage
			float RewardForSkill = (float)spellDamage * _MonsterProfile.SkillReward; // Reward = Damage dealt * multiplier
			MagicBook.RewardSpell(Character.SpellList[(int)SpellName.IceBolt], RewardForSkill);
		}
		
		if(_Collision.collider.tag != "Player" && _Collision.collider.tag != "EquippedItem" && _Collision.collider.tag != "Spell")
		{
			Destroy (transform.gameObject);
		}
	}
}
