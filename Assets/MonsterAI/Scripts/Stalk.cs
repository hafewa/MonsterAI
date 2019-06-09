using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterAI
{
    [CreateAssetMenu(fileName = "New Stalk", menuName = "AI/Stalk")]
    public class Stalk : AIAction
    {
        [SerializeField] private float DistanceToStop = 5.0f;
        [SerializeField] private float DistanceToRun = 2.0f;
        [SerializeField] private float TimeBeforeGivingUp = 5.0f;
        [SerializeField] private float EyeSightRange = 8.0f;
        [SerializeField] private bool CanSneakAttack;

        private AIDecisionMaker DecisionMaker;
        private bool SeePlayer;
        private float TimeLostPlayer = 0.0f;

        public override bool CanActivate(AIDecisionMaker decisionMaker)
        {
            if (decisionMaker.IsSeen) return false;

            if (decisionMaker.Stalking)
                return true;

            return false;
        }

        public override void AIUpdate()
        {
            PlayerLost();
            SetupPlayerLose();
        }

        public override void AIPreInitialize(AIDecisionMaker decisionMaker)
        {
            base.AIPreInitialize(decisionMaker);
            DecisionMaker = decisionMaker;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            navMeshAgent.destination = navMeshAgent.transform.position;
            animator.SetBool("Stalk", false);
        }

        private void PlayerLost()
        {
            if (TimeLostPlayer > 0 && TimeLostPlayer + TimeBeforeGivingUp >= Time.time)
            {
                DecisionMaker.Stalking = false;
                OnActionFinished = true;
            }
        }

        private void SetupPlayerLose()
        {
            float distance = (DecisionMaker.transform.position - PlayerCharacter.position).sqrMagnitude;
            if (distance <= DistanceToStop)
            {
                navMeshAgent.destination = navMeshAgent.transform.position;
                animator.SetBool("Stalk", false);

                if (distance <= DistanceToRun && DecisionMaker.IsSeen)
                {
                    DecisionMaker.Stalking = false;
                    DecisionMaker.RunAway = true;
                    OnActionFinished = true;
                }
                else if (distance <= DistanceToRun && CanSneakAttack)
                {
                    DecisionMaker.Stalking = false;
                    DecisionMaker.IsAggressive = true;
                }
            }  else
            {
                animator.SetBool("Stalk", true);
                navMeshAgent.destination = PlayerCharacter.position;
            }

            if (distance > EyeSightRange && SeePlayer)
            {
                TimeLostPlayer = Time.time;
                SeePlayer = false;
            } else if(!SeePlayer && distance < EyeSightRange)
            {
                SeePlayer = true;
            }
        }
    }
}