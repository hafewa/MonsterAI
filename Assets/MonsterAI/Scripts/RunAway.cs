using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterAI
{
    [CreateAssetMenu(fileName = "Run Away", menuName = "AI/Run Away")]
    public class RunAway : AIAction
    {
        [SerializeField] private float DistanceToRunAway = 6.0f;
        [SerializeField] private float SafeDistance = 50.0f;
        [SerializeField] private float RunSpeedModifier = 2.0f;
        [SerializeField] private float StoppingDistance = 0.1f;

        private bool RunningAway;
        private AIDecisionMaker DecisionMaker;
        private bool ModifiedSpeed;

        public override bool CanActivate(AIDecisionMaker decisionMaker)
        {
            if (DecisionMaker.IsAggressive) return false;

            if((decisionMaker.RunAway || decisionMaker.HitByLight) && DecisionMaker.IsSeen && (DecisionMaker.transform.position - PlayerCharacter.position).sqrMagnitude <= DistanceToRunAway)
                return true;

            return false;
        }

        public override void AIUpdate()
        {
            float distance = (DecisionMaker.transform.position - PlayerCharacter.position).sqrMagnitude;
            if (distance > SafeDistance && RunningAway) {
                RunningAway = false;
                DecisionMaker.RunAway = false;
                OnActionFinished = true;
                if(ModifiedSpeed)
                {
                    ModifiedSpeed = false;
                    navMeshAgent.speed /= RunSpeedModifier;
                }
            }

            if (!RunningAway || navMeshAgent.remainingDistance <= StoppingDistance)
            {
                animator.SetBool("RunAway", false);
                GetPointAwayFromPlayer();
            }
        }

        public override void AIPreInitialize(AIDecisionMaker decisionMaker)
        {
            base.AIPreInitialize(decisionMaker);
            DecisionMaker = decisionMaker;
        }

        public override void Activate()
        {
            base.Activate();
            PlayActionSound();
            if (!ModifiedSpeed)
            {
                ModifiedSpeed = true;
                navMeshAgent.speed *= RunSpeedModifier;
            }
            DecisionMaker.IsAggressive = false;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            animator.SetBool("RunAway", false);
            if (ModifiedSpeed)
            {
                navMeshAgent.speed /= RunSpeedModifier;
                ModifiedSpeed = false;
            }
        }

        private void GetPointAwayFromPlayer()
        {
            RunningAway = true;
            if (!ModifiedSpeed)
            {
                ModifiedSpeed = true;
                navMeshAgent.speed *= RunSpeedModifier;
            }
            DecisionMaker.transform.LookAt(AI.position - (PlayerCharacter.position - AI.position));
            navMeshAgent.destination = (AI.position - (PlayerCharacter.position - AI.position) * 10f);
            animator.SetBool("RunAway", true);
        }
    }
}