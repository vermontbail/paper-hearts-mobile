using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
enum EnemyState
{
    Stationary,
    Moving,
    PrepAttack,
    Hitstop
}

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update\
    [SerializeField]
    private float timeBetweenShots = 8f;
    [SerializeField] // speed of enemy
    private float movementSpeed = 2f;
    [SerializeField] // health of enemy
    private int health = 2;
    [SerializeField] // projectile summoned
    GameObject enemyProjectile;

    // time enemy freezes for attack
    private float shotWindup = 1f;
    private float currentWindupTimer = 0f;
    private float currentShotTimer = 0f;
    // hitfreeze
    private float hitStop = 1f;
    private float currentHitstop = 0f;

    // array of two points
    private GameObject point1;
    private GameObject point2;
    private float distanceUntilSwitch = 0.1f;
    private bool canPatrol = false;

    private bool movingToPointOne = true;

    private EnemyState es = EnemyState.Stationary;

    private SpriteRenderer sr;

    // Update is called once per frame
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // find children that are considered patrol points
        List<Transform> array = new List<Transform>();

        foreach (Transform child in transform.parent)
        {
            if (child.gameObject.tag == "Patrol")
            {
                array.Add(child);
            }

            if (array.Count == 2) // if two, set up patrol system
            {
                point1 = array[0].gameObject;
                point2 = array[1].gameObject;
                canPatrol = true;
                es = EnemyState.Moving;
            }
        }
    }

    void Update()
    {
        switch (es)
        {
            case EnemyState.Hitstop:
                currentHitstop += Time.deltaTime;
                if (currentHitstop >= hitStop)
                {
                    ResetEnemyState();
                }
                break;
            case EnemyState.PrepAttack:
                currentWindupTimer += Time.deltaTime;
                if (currentWindupTimer >= shotWindup)
                {
                    CreateProjectile();
                    ResetEnemyState();
                }
                break;

            case EnemyState.Moving:
            case EnemyState.Stationary:
            default:
                if (es == EnemyState.Moving) Move();
                currentShotTimer += Time.deltaTime;
                if (currentShotTimer >= timeBetweenShots)
                {
                    ResetEnemyState();
                    es = EnemyState.PrepAttack;
                }
                break;
        }

        // debug colors no matter what
        DebugColors();
    }
    private void DebugColors()
    {
        switch (es) 
        {
            case EnemyState.Hitstop:
                sr.color = Color.red;
                break;
            case EnemyState.Moving:
                sr.color = Color.green;
                break;
            case EnemyState.PrepAttack:
                sr.color = Color.yellow;
                break;
            case EnemyState.Stationary:
                sr.color = Color.blue;
                break;
        }

    }
    public void TakeDamage()
    {
        health--;
        if (health <= 0) // remove from game
        {
            if(TutorialManager.tutState == TutorialManager.TutorialState.enemy)
            {
                TutorialManager.AdvanceTutorial();
            }
            this.transform.parent.gameObject.SetActive(false);
            FindObjectOfType<GameManager>().AddScore();
        }
        else
        {
            ResetEnemyState();
            es = EnemyState.Hitstop;
        }
    }
    public void ResetEnemyState()
    {
        switch (canPatrol)
        {
            case true:
                es = EnemyState.Moving;
                break;
            case false:
                es = EnemyState.Stationary;
                break;
        }
        currentHitstop = 0f;
        currentShotTimer = 0f;
        currentWindupTimer = 0f;
    }
    private void CreateProjectile()
    {
        // create projectile here
        GameObject instance = Instantiate(enemyProjectile);
        instance.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
    }
    private void Move()
    {
        // guard code
        if (!canPatrol) return;
        
        
        switch (movingToPointOne)
        {
            case true:
                transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, movementSpeed * Time.deltaTime);

                // check for direction switch
                if (Vector3.Distance(transform.position, point1.transform.position) <= distanceUntilSwitch)
                {
                    movingToPointOne = false;
                }
                break;
            case false:
                transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, movementSpeed * Time.deltaTime);

                // check for direction switch
                if (Vector3.Distance(transform.position, point2.transform.position) <= distanceUntilSwitch)
                {
                    movingToPointOne = true;
                }
                break;
        }
    }
}
