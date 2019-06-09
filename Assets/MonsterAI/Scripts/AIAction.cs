using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonsterAI
{
    [Serializable]
    public class AIAction : ScriptableObject, IAIBehaviour
    {
        public int PRIORITY = 7;
        [SerializeField] private AudioClip[] ActionSound;

        [HideInInspector] public Transform PlayerCharacter;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool OnActionFinished;
        [HideInInspector] protected NavMeshAgent navMeshAgent;

        private AudioSource audioSource;

        protected Transform AI;

        public virtual void AIPreInitialize(AIDecisionMaker decisionMaker)
        {
            PlayerCharacter = decisionMaker.Player;
            animator = decisionMaker.GetComponent<Animator>();
            OnActionFinished = false;
            AI = decisionMaker.transform;
            navMeshAgent = decisionMaker.GetComponent<NavMeshAgent>();
            audioSource = decisionMaker.GetComponent<AudioSource>();
        }

        public virtual bool CanActivate(AIDecisionMaker decisionMaker)
        {
            return false;
        }

        public virtual void AIUpdate()
        {

        }

        public virtual void Activate()
        {

        }

        public virtual void Deactivate()
        {

        }

        protected void PlayActionSound()
        {
            if(ActionSound.Length > 0)
            {
                audioSource.PlayOneShot(ActionSound[UnityEngine.Random.Range(0, ActionSound.Length)]);
            }
        }

    }
}