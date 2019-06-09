using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterAI
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "AI/Attack")]

    public class Attack : AIAction
    {
        [SerializeField] private float AttackDistance = 2.0f;
        [SerializeField] private float TimeBetweenAttacks = 2.0f;
        [SerializeField] private bool MustBeSeenToAttack;
        [SerializeField] private bool ShouldRunAwayAfterAttackIfNotAggressive;

        private float LastAttackTime = 0.0f;
        private AIDecisionMaker DecisionMaker;
        private bool Attacked;

        public override bool CanActivate(AIDecisionMaker decisionMaker)
        {
            if (!DecisionMaker.IsSeen && MustBeSeenToAttack && !decisionMaker.IsAggressive) return false;

            if ((decisionMaker.transform.position - PlayerCharacter.position).sqrMagnitude <= AttackDistance)
                return true;

            if (!decisionMaker.IsAggressive) return false;            

            return false;
        }

        public override void AIPreInitialize(AIDecisionMaker decisionMaker)
        {
            base.AIPreInitialize(decisionMaker);
            DecisionMaker = decisionMaker;
        }

        public override void AIUpdate()
        {
            if (!Attacked)
            {
                PlayActionSound();
                animator.SetTrigger("Attack");
                LastAttackTime = Time.time;
                Attacked = true;
            }
            else if (Time.time >= LastAttackTime + TimeBetweenAttacks)
            {
                OnActionFinished = true;
            }
        }

        public override void Activate()
        {
            base.Activate();
            DecisionMaker.transform.LookAt(DecisionMaker.Player);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Attacked = false;
            LastAttackTime = 0.0f;
            if (!DecisionMaker.IsAggressive && ShouldRunAwayAfterAttackIfNotAggressive)
            {
                DecisionMaker.IsSeen = true;
                DecisionMaker.RunAway = true;
                int DefaultPriority = PRIORITY;
                PRIORITY = 0; // We're making sure he won't attack again
                DecisionMaker.StartCoroutine(ResetPriority(DefaultPriority));
            }
        }

        IEnumerator ResetPriority(int n)
        {
            yield return new WaitForSeconds(0.1f);
            PRIORITY = n;
        }
    }
}