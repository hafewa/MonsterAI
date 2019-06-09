using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAI
{
    [CreateAssetMenu(fileName = "Stand Still", menuName = "AI/Stand Still")]
    public class StandStill : AIAction
    {
        [SerializeField] private float DistanceToRun = 3.0f;

        private AIDecisionMaker DecisionMaker;

        public override bool CanActivate(AIDecisionMaker decisionMaker)
        {
            return true;
        }
        
        public override void AIPreInitialize(AIDecisionMaker decisionMaker)
        {
            base.AIPreInitialize(decisionMaker);
            DecisionMaker = decisionMaker;
        }

        public override void AIUpdate()
        {
            base.AIUpdate();
            float distance = (DecisionMaker.transform.position - PlayerCharacter.position).sqrMagnitude;
            if (distance <= DistanceToRun)
            {
                Debug.Log("stalking lost 3");
                DecisionMaker.Stalking = false;
                if (DecisionMaker.IsSeen)
                {
                    DecisionMaker.RunAway = true;
                }
                OnActionFinished = true;
            }

            OnActionFinished = true;
        }
    }
}