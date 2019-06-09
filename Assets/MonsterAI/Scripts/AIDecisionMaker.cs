using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAI
{
    public class AIDecisionMaker : MonoBehaviour
    {
        public Transform Player;
        [SerializeField]
        private AIAction[] AIBehaviours;
        [SerializeField]
        private float SightRadius = 50.0f;
        [SerializeField]
        private float HearingRadius = 80.0f;
        [SerializeField] private float ActivationDistance = 250.0f;
        [SerializeField] private float FreezeDistance = 25.0f;
        [SerializeField] private Transform RaycastController;
        [SerializeField] private LayerMask playerlayer;

        private AIAction currentAction;
        private AIAction[] AIactions;
        private Collider m_collider;
        private Camera m_camera;

        [HideInInspector] public bool IsAggressive;
        [HideInInspector] public bool RunAway;
        [HideInInspector] public bool Stalking;
        [HideInInspector] public bool HitByLight;
        [HideInInspector] public bool IsSeen;

        private void Awake()
        {
            AIactions = new AIAction[AIBehaviours.Length];

            for (int i = 0; i < AIBehaviours.Length; i++)
            {
                AIactions[i] = Instantiate(AIBehaviours[i]);
                AIactions[i].AIPreInitialize(this);
            }

            m_collider = GetComponent<Collider>();
            m_camera = Camera.main;
            
            Replan();

            InvokeRepeating("ActiveBasedOnDistance", 5.0f, 45.0f);
        }

        private void Update()
        {
            if(currentAction)
            {
                Debug.Log(currentAction);
                if (currentAction.OnActionFinished)
                {
                    currentAction.OnActionFinished = false;
                    Replan();
                }
                else currentAction.AIUpdate();
            }

            if (!Stalking && !RunAway)
            {
                CheckDistanceToPlayer();
            }

            if(IsSeen != IsVisible() && !RunAway)
            {
                IsSeen = IsVisible();
                Replan();
            }
        }

        private void Replan()
        {
            if (currentAction)
                currentAction.Deactivate();

            currentAction = null;

            List<AIAction> PotentialBehaviour = new List<AIAction>();
            for(int i=0; i< AIactions.Length; i++)
            {
                if(AIactions[i].CanActivate(this))
                {
                    PotentialBehaviour.Add(AIactions[i]);
                }
            }

            if (PotentialBehaviour.Count == 1)
            {
                currentAction = PotentialBehaviour[0];
            }
            else
            {
                int HighestPriority = -1;
                for (int i = 0; i < PotentialBehaviour.Count; i++)
                {
                    if (PotentialBehaviour[i].PRIORITY > HighestPriority)
                    {
                        currentAction = PotentialBehaviour[i];
                        HighestPriority = currentAction.PRIORITY;
                    }
                }
            }

            if (currentAction) currentAction.Activate();
        }

        public void SeenByPlayer(bool seen)
        {
            IsSeen = seen;
            Replan();
        }

        public void OnAnimationDamage()
        {
            RaycastHit hit;
            if(Physics.Raycast(RaycastController.position, transform.forward, out hit, 10.0f, playerlayer))
            {
                Debug.Log("check for whether or not player's been hit");
            }
        }
        private void CheckDistanceToPlayer()
        {
            float distance = DistanceToPlayer();
            if(distance <= HearingRadius)
            {
                Stalk();
            }
            else if (distance <= SightRadius)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (Player.position - transform.position), out hit, SightRadius))
                {
                    if (hit.collider.gameObject == Player.gameObject)
                    {
                        Stalk();
                    }
                }
            }
        }

        private void Stalk()
        {
            Stalking = true;
            Replan();
        }

        private bool IsVisible()
        {
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(m_camera), m_collider.bounds) && DistanceToPlayer() < FreezeDistance)
            {
                return true;
            }
            else
                return false;
        }

        private float DistanceToPlayer()
        {
            return (transform.position - Player.position).sqrMagnitude; 
        }

        private bool CloseEnoughToPlayer()
        {
            return DistanceToPlayer() < ActivationDistance;
        }

        private void ActiveBasedOnDistance()
        {
            //gameObject.SetActive(CloseEnoughToPlayer());
        }
    }

}