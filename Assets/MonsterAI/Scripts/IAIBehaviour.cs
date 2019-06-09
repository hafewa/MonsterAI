using UnityEngine;
using System;
namespace MonsterAI
{
    public interface IAIBehaviour
    {
        // Preset variables for the AI Behaviour - be it animations, state,
        // assigning start-up variables (like animator) etc
        void AIPreInitialize(AIDecisionMaker decisionMaker);

        // Whether or not the AI can be activated. Mostly based on a bool in the brain
        bool CanActivate(AIDecisionMaker decisionMaker);

        // Update happens every frame. That's where the main behaviour code needs to happen
        void AIUpdate();

    }

}