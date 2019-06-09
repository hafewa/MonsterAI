using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterAI
{
    [CreateAssetMenu(fileName = "New Chase", menuName = "AI/Chase")]

    public class Chase : AIAction
    {
        [SerializeField] private float ScareDistance = 2.5f;
        [SerializeField] private float RunModifier = 2.0f;
        [SerializeField] private float StoppingDistance = 1.5f;

        private AIDecisionMaker DecisionMaker;
        private bool ModifiedSpeed;

        public override bool CanActivate(AIDecisionMaker decisionMaker)
        {
            if(decisionMaker.IsAggressive)
                return true;

            if ((decisionMaker.transform.position - PlayerCharacter.position).sqrMagnitude <= ScareDistance && decisionMaker.HitByLight)
                return true;

            return false;
        }

        public override void AIUpdate()
        {
            if (navMeshAgent.remainingDistance <= StoppingDistance)
            {
                EndChase();
            }

            else ChasePlayer();
        }

        public override void AIPreInitialize(AIDecisionMaker decisionMaker)
        {
            base.AIPreInitialize(decisionMaker);
            DecisionMaker = decisionMaker;
        }

        public override void Activate()
        {
            PlayActionSound();
            base.Activate();
            DecisionMaker.IsAggressive = true;
            navMeshAgent.destination = PlayerCharacter.position;
        }

        private void ChasePlayer()
        {
            animator.SetBool("Chase", true);
            navMeshAgent.destination = PlayerCharacter.position;
            if (!ModifiedSpeed)
            {
                ModifiedSpeed = true;
                navMeshAgent.speed *= RunModifier;
            }
        }

        private void EndChase()
        {
            animator.SetBool("Chase", false);
            navMeshAgent.destination = navMeshAgent.transform.position;
            if (ModifiedSpeed)
            {
                ModifiedSpeed = false;
                navMeshAgent.speed /= RunModifier;
            }
            OnActionFinished = true;
        }
    }
}