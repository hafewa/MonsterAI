using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MonsterAI
{
    [CreateAssetMenu(fileName = "New Random Patrol", menuName = "AI/Random Patrol")]

    public class RandomPatrol : AIAction
    {
        [SerializeField] private bool WalkRandomly;
        [SerializeField] private string WaypointsTag;
        [SerializeField] private float delay = 1.0f;
        [SerializeField] private float StoppingDistance = 0.1f;

        private Transform[] AllWaypoints;
        private int TransformID = -1;
        private float switchTime = 0.0f;
        private bool IsWalkingAnimation;

        public override bool CanActivate(AIDecisionMaker decisionMaker)
        {
            if (decisionMaker.IsSeen) return false;

            return true;
        }

        public override void AIUpdate()
        {
            if(!IsWalkingAnimation)
            {
                IsWalkingAnimation = true;
               // animator.SetBool("Patrol", true);
            }

            if(navMeshAgent.remainingDistance <= StoppingDistance && Time.time > switchTime)
            {
                animator.SetBool("Patrol", false);
                switchTime = Time.time + delay;
                SelectWaypoint();
                OnActionFinished = true;
            }
        }

        public override void AIPreInitialize(AIDecisionMaker decisionMaker)
        {
            base.AIPreInitialize(decisionMaker);
            TransformID = -1;
            AllWaypoints = GameObject.FindGameObjectsWithTag(WaypointsTag).Select(go => go.transform).ToArray();               
        }

        public override void Activate()
        {
            base.Activate();
            animator.SetBool("Patrol", true);
            SelectWaypoint();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            navMeshAgent.destination = navMeshAgent.transform.position;
            animator.SetBool("Patrol", false);
            IsWalkingAnimation = false;
        }

        private void SelectWaypoint()
        {
            TransformID = SelectNextWalkingPoint();
            navMeshAgent.SetDestination(AllWaypoints[TransformID].position);
            animator.SetBool("Patrol", true);
        }

        private int SelectNextWalkingPoint()
        {
            if (WalkRandomly)
            {
                return Random.Range(0, AllWaypoints.Length);
            }

            if (TransformID == AllWaypoints.Length - 1)
            {
                return 0;
            }

            return TransformID + 1;
        }
    }
}