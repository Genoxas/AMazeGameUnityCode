using UnityEngine;
using System.Collections;

public class MonsterTransmitDamage : MonoBehaviour {

    [SerializeField]
    private GameObject monster;

    
	public void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            monster.GetComponent<MonsterAI>().DamagePlayer(obj.gameObject);
        }
    }
}
