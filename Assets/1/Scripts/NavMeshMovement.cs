using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine.Networking;
using System.Runtime.InteropServices;


public enum Action
{
    Idle,
    petrol,
    chasing,
    Cought
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NavMeshMovement : NetworkBehaviour
{
    //public


    public float PetrolDistance = 10;
    public float searchingPlayerDistance = 10;
    public float TimeBetweenPetrols = 5;
    public float distanceToRun = 50;
    public float FieldOfView = 90;
    private float Speed = 0.11f;

    //private
    private NavMeshAgent nav;
    private NavMeshController control;
    public Action action = Action.Idle;
    //[SyncVar]
    public GameObject Player;
    private float timeBetweenPetrols;
    private Animator anim;
    private bool found = false;
    private Rigidbody rig;
    //[SyncVar]
    private bool isIdle;
    //[SyncVar]
    private bool isWalking;
    //[SyncVar]
    private bool isChasing;
    //[SyncVar]
    private bool isChatting;
    private float Timer;
    private bool Multiplayer;
    private bool movementAllowed;
    public GameObject[] obj;
    public int Attention;

    public Action actionType
    {
        get{ return action; }
        set{ action = value; }
    }

    public bool MovementAllowed
    {
        get{ return movementAllowed; }
        set
        {
            movementAllowed = value;
            if (movementAllowed == false)
                ChangeState(Action.Idle);
        }
    }
	
    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        control = GetComponent<NavMeshController>();
        rig = GetComponent<Rigidbody>();
        if (GameObject.Find("NetworkManager") == null)
        {
            Multiplayer = false;
            //MovementAllowed = GameManager.instance.MovementAllowed;
        }
        else
        {
            Multiplayer = true;
            MovementAllowed = true;
        }
        nav.speed = Speed;
        if (!CheckServer())
            return;
        //StartCoroutine("Get_Closest");


        ChangeState(action);
    }

	
    void Update()
    {
        if (!CheckServer())
            return;
        Get_Closest();

    }

	
    void  Get_Closest()
    {
        float Min = 10000;
        int OBJNUMBER = -1;

        obj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < obj.Length; i++)
        {
            if (!obj[i].GetComponent<PlayerHealth>().Dead)
            {
                if (Mathf.Abs(transform.position.magnitude - obj[i].transform.position.magnitude) < 2.5f)
                {
                    obj[i].gameObject.GetComponent<PlayerSound>().Play("HeartBeat");
                }
                //print("alive");
                if (Mathf.Abs(transform.position.magnitude - obj[i].transform.position.magnitude) < Min)
                {

                    //Get angle between enemy sight and player
                    float Angle = Mathf.Abs(Vector3.Angle(transform.forward, (obj[i].transform.position - transform.position).normalized));
                    //If angle is greater than field of view, we cannot see player
                    if (Angle > FieldOfView)
                        continue;
                    //Check with raycast- make sure player is not on other side of 
                    // 8 refer to the floor layar , you have to ignore the floor layer so that 
                    //the agent could see only the houses and player and not distracted by ground 
                    if (Physics.Linecast(transform.position, obj[i].transform.position, 8))
                        continue;
                    Min = Mathf.Abs(transform.position.magnitude - obj[i].transform.position.magnitude);

                    OBJNUMBER = i;
                    //	print(234);
                }
            }
        }
        if (OBJNUMBER == -1)
        {
            Player = null;
            if (action == Action.Cought || action == Action.chasing)
                ChangeState(Action.Idle);
            return;
        }
        if (Player != null)
        {
            if ((Vector3.Distance(transform.position, Player.transform.position) < searchingPlayerDistance || !Player.GetComponent<PlayerHealth>().Dead) && Player == obj[OBJNUMBER])
                return;
            else
                Player = null;
        }
        {
            //	Player = 
            Player = obj[OBJNUMBER] as GameObject;
            print(obj[OBJNUMBER].name);
            ChangeState(Action.chasing);
        }
    }

	
    void  refreshAnimatorState()
    {
        if (action == Action.Idle)
        {
            isIdle = true;
            isChasing = false;
            isWalking = false;
        }
        else if (action == Action.petrol)
        {
            isIdle = false;
            isChasing = false;
            isWalking = true;
        }
        else if (action == Action.chasing)
        {
            isIdle = false;
            isChasing = true;
            isWalking = false;
        }

        anim.SetBool("isIdle", isIdle);
        anim.SetBool("isChasing", isChasing);
        anim.SetBool("isWalking", isWalking);
    }

    #region States

	
    public  void ChangeState(Action act)
    {
        nav.isStopped = true;
       
        StopAllCoroutines();
        action = act;
        control.actionType = action;

        refreshAnimatorState();
        switch (act)
        {
            case Action.petrol:
                {
                    StartCoroutine("Petrol");
                    break;
                }
            case Action.Cought:
                {
                    StartCoroutine("Cought");
                    break;
                }
            case Action.chasing:
                {
                    StartCoroutine("Chasing");
                    break;
                }
            case Action.Idle:
                {
                    StartCoroutine("Idle");
                    break;
                }
        }
    }

	
    IEnumerator Petrol()
    {
        NavMeshHit hit;
        Vector3 area = Random.insideUnitSphere * PetrolDistance;
        area += transform.position;
        NavMesh.SamplePosition(area, out hit, PetrolDistance, 1);
        nav.SetDestination(hit.position);

        nav.Resume();
        //to know the things that the agent hits

        while (!nav.isStopped)
        {
            //Searching_For_Player();

            if (Vector3.Distance(transform.position, hit.position) < nav.stoppingDistance)
                ChangeState(Action.Idle);

            yield return null;
        }
    }

	
    IEnumerator Idle()
    {
        //if (!CheckServer())
        //	yield return null;
        if (MovementAllowed)
        {
            timeBetweenPetrols = TimeBetweenPetrols;
            while (timeBetweenPetrols > 0)
            {
                //	Searching_For_Player();
                timeBetweenPetrols -= Time.deltaTime;
                yield return null;
            }
            ChangeState(Action.petrol);
        }
        else
        {
            //MovementAllowed = GameManager.instance.MovementAllowed;
            yield return null;
            StartCoroutine("Idle");

        }
    }

	
    IEnumerator Chasing()
    {
        //if (!CheckServer())
        //	yield return null;
        Vector3 v = Vector3.Project(Player.transform.position, transform.transform.position);
        nav.Resume();
        Quaternion player = Quaternion.LookRotation(v);
        transform.rotation = Quaternion.Lerp(transform.rotation, player, 0.15f);




        while (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) < searchingPlayerDistance)
        {
            nav.SetDestination(Player.transform.position);

            if (Vector3.Distance(transform.position, Player.transform.position) <= nav.stoppingDistance)
            {

                ChangeState(Action.Cought);
            }
            yield return null;
        }
        ChangeState(Action.Idle);
    }

	
    IEnumerator Cought()
    {
        //	if (!CheckServer())
        //	yield return null;
        Vector3 player = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
        while (Vector3.Distance(transform.position, Player.transform.position) <= nav.stoppingDistance)
        {
            if (player == null)
                yield return null;
            transform.LookAt(player);
            if (Player.GetComponent<PlayerHealth>().Dead)
            {
                Player = null;
                ChangeState(Action.Idle);
            }
            else
            {
                if (anim.GetAnimatorTransitionInfo(0).IsName("Punch_R"))
                {
                    //nav.isStopped = true;
                    nav.speed = 0;
                }
                else
                {
                    nav.Resume();
                    nav.speed = Speed;
                    anim.SetTrigger("Punch");

                }
            }


            
            yield return null;
        }
        ChangeState(Action.chasing);
    }

    #endregion

    #region Eye Agent

	
    void Searching_For_Player()
    {


        if (Player == null)
            return;

        //Get angle between enemy sight and player
        float Angle = Mathf.Abs(Vector3.Angle(transform.forward, (Player.transform.position - transform.position).normalized));
        //If angle is greater than field of view, we cannot see player
        if (Angle > FieldOfView)
            return;
        //Check with raycast- make sure player is not on other side of 
        // 8 refer to the floor layar , you have to ignore the floor layer so that 
        //the agent could see only the houses and player and not distracted by ground 
        if (Physics.Linecast(transform.position, Player.transform.position, 8))
            return;
        //We can see player
        ChangeState(Action.chasing);
    }

    #endregion


    bool CheckServer()
    {
        if (Multiplayer)
        {
            if (isServer)
                return true;
            else
                return false;
        }
        else
            return true;
    }
}


