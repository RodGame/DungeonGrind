using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

public class FireBat : MonoBehaviour {
	
	private float _timeLived = 0.0f;
	private float _timeLife = 0.8f;
	private List<int> _monsterHit = new List<int>(); // A list of all the Id of monster already hit;
	
	// Update is called once per frame
	void Update () {
		_timeLived += Time.deltaTime;
		if(_timeLived >= _timeLife)
		{
			Destroy(transform.gameObject);
		}
		else
		{
			//transform.position += transform.forward*(_speed);
		}
	}
	
	void OnCollisionEnter(Collision _Collision)
	{
		if(_Collision.collider.tag == "Monster" && !_monsterHit.Contains(_Collision.collider.GetInstanceID()))
		{
			_monsterHit.Add (_Collision.collider.GetInstanceID());
			
			int spellDamage = Character.SpellList[(int)SpellName.FireBat].Damage;
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
			MagicBook.RewardSpell(Character.SpellList[(int)SpellName.FireBat], RewardForSkill);
		}
	}
}
