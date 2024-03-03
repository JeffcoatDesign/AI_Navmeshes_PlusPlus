using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityStandardAssets.Characters.ThirdPerson;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public UnityEvent<EnemyController> OnDie = new ();
    private MyThirdPersonCharacter m_character;
    private NavMeshAgent m_agent;
    [SerializeField] float m_attackRange = 1.5f;
    int hp = 10;
    [SerializeField] float m_pursueRange = 10f;
    private PlayerController m_player;

    void Awake()
    {
        m_character = GetComponent<MyThirdPersonCharacter>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.SetDestination(transform.position);

        m_player = FindAnyObjectByType<PlayerController>();
        m_player.OnDie.AddListener(OnPlayerDied);
    }

    private void OnPlayerDied()
    {
        m_player = null;
    }

    public void GetHit(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            OnDie.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        m_agent.updateRotation = false;
    }

    void Update()
    {
        bool inRange = false;

        if (m_player == null) return;

        float distance = Vector3.Distance(transform.position, m_player.transform.position);
        if (distance <= m_attackRange)
            inRange = true;
        if (distance < m_pursueRange)
            m_agent.SetDestination(m_player.transform.position);

        if (inRange)
        {
            m_character.Face(m_player.transform.position);
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