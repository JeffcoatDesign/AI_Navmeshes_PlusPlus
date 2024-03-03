using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public UnityEvent OnDie = new ();
    private MyThirdPersonCharacter m_character;
    private NavMeshAgent m_agent;
    [SerializeField] float m_attackRange = 1.5f;
    [SerializeField] int m_maxHP = 20;
    [SerializeField] int m_hp = 20;
    private List<EnemyController> m_enemies = new();
    EnemyController m_target;
    void Awake()
    {
        m_character = GetComponent<MyThirdPersonCharacter>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.SetDestination(transform.position);

        m_enemies = new(FindObjectsByType<EnemyController>(FindObjectsSortMode.None));
        foreach (EnemyController enemy in m_enemies)
        {
            enemy.OnDie.AddListener(RemoveEnemy);
        }

        m_hp = m_maxHP;

        m_character.OnAttack.AddListener(Attack);
    }

    private void Start()
    {
        m_agent.updateRotation = false;
    }
    private void RemoveEnemy (EnemyController enemy)
    {
        m_enemies.Remove(enemy);
    }

    public void GetHit(int amount)
    {
        m_hp -= amount;
        OnDie.Invoke();
        gameObject.SetActive(false);
    }

    private void Attack ()
    {
        if (m_agent != null)
        {
            m_target.GetHit(1);
        }
    }

    void Update()
    {
        bool inRange = false;

        foreach (EnemyController enemy in m_enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= m_attackRange) { 
                inRange = true;
                m_target = enemy;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                m_agent.SetDestination(hit.point);
            }
        }
        bool atDestination = Vector3.Distance(transform.position, m_agent.destination) <= m_attackRange;
        Camera.main.transform.parent.position = transform.position;
        if (inRange && atDestination)
        {
            m_agent.SetDestination(m_target.transform.position);
            m_character.Face(m_target.transform.position);
            m_character.HandleAttack(inRange);
        }
        else if (m_agent.remainingDistance > m_agent.stoppingDistance)
        {
            m_character.Move(m_agent.desiredVelocity, false, false);
        } else { 
            m_character.Move(Vector3.zero, false, false);
        }
    }
}