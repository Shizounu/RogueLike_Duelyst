using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public abstract class ActorManager : MonoBehaviour {
        public bool isEnabled;
        public List<CardDefinition> Deck = new();
        public Cards.CardDefinition[] Hand = new Cards.CardDefinition[6];

        public abstract void Enable();
        public abstract void Disable();

    
    
    }

}
