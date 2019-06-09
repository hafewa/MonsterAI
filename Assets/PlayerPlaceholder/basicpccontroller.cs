using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DialogSystem
{
    public class basicpccontroller : MonoBehaviour
    {
        [SerializeField] private float speed = 2.0f;

        private float startTime;
        private float journeyLength;
        private Vector3 startPos;
        private Vector3 goToPosition;

        private void Start()
        {
            goToPosition = transform.position;
        }

        private void Update()
        {
            if ((goToPosition - transform.position).sqrMagnitude > 1.0f)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startPos, goToPosition, fracJourney);
            }

            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50f))
                {
                    startPos = transform.position;
                    startTime = Time.time;
                    Vector3 temp = hit.point;
                    goToPosition = new Vector3(temp.x, transform.position.y, temp.z);
                    journeyLength = Vector3.Distance(transform.position, goToPosition);
                }
            }
        }
        
    }
}