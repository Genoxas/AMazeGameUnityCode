using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: Script used to control the monster in order for the monster to attack players. Monster has points on map which
 * he will travel to but if a player is close to the monster he will begin searching. After a certain amount of time he will stop
 * searching for the player (deaggro player) and begin to go on his path once again until he finds another in front of him or 'senses'
 * another player.
 */

public class MonsterAI : NetworkBehaviour
{
    [SerializeField]
    private Collider monsterLeftHandDamage;
    [SerializeField]
    private Collider monsterRightHandDamage;
    [SerializeField]
    private Transform raycastPoint;
    private Animator anim;
    private MonsterSoundController monsterSoundController;
    private GameObject[] monsterPaths;
    private GameObject[] players;
    private GameObject lastFoundPlayer;
    private GameObject chosenPath;
    private NavMeshAgent monster;
    private string storedPlayerID;
    private float playerDistance;
    private float aggroTimer = 6;
    private float playerImmunityTimer = 12;
    private float foundPlayerDistance;
    private int indexOfPath;
    private bool monsterAttackCooldown = false;
    private bool foundInitialPlayer = false;
    private bool executeOnce = false;
    private RaycastHit findPlayer;
    private monsterState monsterCurrentState = monsterState.chooseFirstPath;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        monsterSoundController = GetComponent<MonsterSoundController>();
        monster = GetComponent<NavMeshAgent>();
        monsterPaths = GameObject.FindGameObjectsWithTag("MonsterPathing");
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public override void OnStartServer()
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }

    //enum which determines the monsters current status
    private enum monsterState
    {
        onPath = 0,
        chooseFirstPath = 1,
        foundNewPlayer = 2,
        foundNewPath = 3,
        killedPlayer = 4,
    };

    // Update is called once per frame
    [Server]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Destroy(this.gameObject);
            RpcDisableMonster();
        }
        //Debug.Log(monsterCurrentState);
        //Perform a switch on the enum declared above to check the current state of the monster
        switch (monsterCurrentState)
        {
            case (monsterState.chooseFirstPath):
                {
                    indexOfPath = Random.Range(0, monsterPaths.Length);
                    monster.SetDestination(monsterPaths[indexOfPath].transform.position);
                    chosenPath = monsterPaths[indexOfPath];
                    monsterCurrentState = monsterState.onPath;
                    break;
                }
            case (monsterState.onPath):
                {
                    //When the monsters current position is close to the current path block by 2 blocks then find another path
                    if ((monster.transform.position - chosenPath.transform.position).magnitude < 2)
                    {
                        indexOfPath = Random.Range(0, monsterPaths.Length);
                        monster.SetDestination(monsterPaths[indexOfPath].transform.position);
                        chosenPath = monsterPaths[indexOfPath];
                    }
                    //Find all players in the players array and locate distance between monster and player
                    foreach (GameObject player in players)
                    {
                        playerDistance = Vector3.Distance(this.transform.position, player.transform.position);
                        //if distance is under 8 blocks then execute the following
                        if (playerDistance < 8)
                        {
                            //executes when the monster first locates a player and sets the unique player id/gameobject as well as the monster state
                            if (foundInitialPlayer == false)
                            {
                                storedPlayerID = player.name;
                                lastFoundPlayer = player;
                                monsterCurrentState = monsterState.foundNewPlayer;
                                foundInitialPlayer = true;
                            }
                            //executes when a second player is found after the fact a monster has found the first player. Overwrites the existing unique id/gameobject for monster and sets monster state
                            else if (foundInitialPlayer == true && storedPlayerID != player.name)
                            {
                                storedPlayerID = player.name;
                                lastFoundPlayer = player;
                                monsterCurrentState = monsterState.foundNewPlayer;
                                break;
                            }
                        }
                    }
                    //while monster is still on monster path, if player is found in the monsters sights he will set his current state to found player and continue to chase
                    if (Physics.Raycast(raycastPoint.transform.position, raycastPoint.transform.TransformDirection(Vector3.forward), out findPlayer, 10))
                    {
                        if (findPlayer.transform.tag == "Player")
                        {
                            storedPlayerID = findPlayer.transform.gameObject.name;
                            lastFoundPlayer = findPlayer.transform.gameObject;
                            monsterCurrentState = monsterState.foundNewPlayer;
                        }
                    }
                    //if monster can still sense old player after another new player is found then give him immunity for 12 seconds and when timer hits 0 then make him able to reaggro player again
                    if (foundInitialPlayer == true && playerImmunityTimer >= 0)
                    {
                        playerImmunityTimer -= Time.deltaTime;
                    }
                    else if (playerImmunityTimer < 0)
                    {
                        foundInitialPlayer = false;
                        playerImmunityTimer = 12;
                    }
                    break;
                }
            case (monsterState.foundNewPlayer):
                {
                    //if a new player is found the keep setting the destination of the monster to the player but also - time from the aggro timer
                    monster.SetDestination(lastFoundPlayer.transform.position);
                    aggroTimer -= Time.deltaTime;
                    foundPlayerDistance = Vector3.Distance(this.gameObject.transform.position, lastFoundPlayer.transform.position);
                    //if monster and player are two blocks close to eachother then damage player. Once attacked monster attack will be on cooldown.
                    if (foundPlayerDistance < 2 && monsterAttackCooldown == false)
                    {
                        RpcPlayMonsterGrowl();
                        //perform a slerp here to look at the player and perform an attack once spotted
                        if (Physics.Raycast(raycastPoint.transform.position, transform.TransformDirection(Vector3.forward), out findPlayer, 10))
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lastFoundPlayer.transform.position - transform.position), Time.deltaTime * 10);
                            if (findPlayer.transform.tag == "Player")
                            {
                                anim.SetInteger("AttackMixer", Random.Range(0, 2));
                                GetComponent<NetworkAnimator>().SetTrigger("Punch");
                                monsterAttackCooldown = true;
                            }
                        }
                    }
                    //if aggro timer hits 0 then set monster state to foundNewPath where he will find another path to follow
                    if (aggroTimer <= 0)
                    {
                        aggroTimer = 6;
                        monsterCurrentState = monsterState.foundNewPath;
                    }
                    //if monster sees a player then set aggro timer to 8 and continue following player    
                    if (Physics.Raycast(raycastPoint.transform.position, transform.TransformDirection(Vector3.forward), out findPlayer, 10))
                    {
                        if (findPlayer.transform.tag == "Player")
                        {
                            aggroTimer = 8;
                            if (findPlayer.transform.gameObject != lastFoundPlayer.transform.gameObject)
                            {
                                storedPlayerID = findPlayer.transform.gameObject.name;
                                lastFoundPlayer = findPlayer.transform.gameObject;
                            }
                        }
                    }
                    break;
                }
            //find another path for monster once monster finds a path point
            case (monsterState.foundNewPath):
                {
                    indexOfPath = Random.Range(0, monsterPaths.Length);
                    monster.SetDestination(monsterPaths[indexOfPath].transform.position);
                    chosenPath = monsterPaths[indexOfPath];
                    monsterCurrentState = monsterState.onPath;
                    break;
                }
            case (monsterState.killedPlayer):
                {
                    if (executeOnce == false)
                    {
                        executeOnce = true;
                        anim.SetBool("Roar", true);
                        monster.SetDestination(this.transform.position);
                        break;
                    }
                    break;
                }
        }
    }

    [Server]
    void OnCollisionEnter(Collision player)
    {
        if (player.transform.tag == "Player")
        {
            storedPlayerID = player.transform.gameObject.name;
            lastFoundPlayer = player.transform.gameObject;
            monsterCurrentState = monsterState.foundNewPlayer;
        }
    }

    [Server]
    public void ResetMonsterAttackCoolDown()
    {
        monsterAttackCooldown = false;
    }

    [Server]
    public void EnableMonsterLeftHandDamage()
    {
        monsterLeftHandDamage.enabled = true;
    }

    [Server]
    public void DisableMonsterLeftHandDamage()
    {
        monsterLeftHandDamage.enabled = false;
    }

    [Server]
    public void EnableMonsterRightHandDamage()
    {
        monsterRightHandDamage.enabled = true;
    }

    [Server]
    public void DisableMonsterRightHandDamage()
    {
        monsterRightHandDamage.enabled = false;
    }

    [Server]
    public void DamagePlayer(GameObject player)
    {
        RpcPlayMonsterImpact();
        player.GetComponent<PlayerActions>().CmdRegisterHit(player, 50);
        StartCoroutine(CheckPlayerHealth(player));
    }

    [Server]
    private IEnumerator CheckPlayerHealth(GameObject player)
    {
        yield return new WaitForSeconds(0.3f);
        if (player.GetComponent<PlayerStats>().GetHealth() <= 0)
        {
            monsterCurrentState = monsterState.killedPlayer;
        }
    }

    public void PlayKilledPlayer()
    {
        anim.SetBool("Roar", false);
        monsterSoundController.PlayMonsterRoar();
    }

    [ClientRpc]
    private void RpcPlayMonsterGrowl()
    {
        monsterSoundController.PlayMonsterGrowl();
    }

    [ClientRpc]
    private void RpcPlayMonsterImpact()
    {
        monsterSoundController.PlayMonsterImpact();
    }

    public void Continue()
    {
        monsterCurrentState = monsterState.foundNewPath;
        executeOnce = false;
    }

    [ClientRpc]
    private void RpcDisableMonster()
    {
        if (isServer)
            return;
        Destroy(this.gameObject);
    }
}