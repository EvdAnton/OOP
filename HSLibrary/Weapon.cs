using System;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace HSLibrary
{
    [CardsAtribute("Weapon card")]
    [DataContract]
    public class Weapon : Cards
    {
        [DataMember]
        int attack;

        [DataMember]
        public int Attack
        {
            get
            {
                return attack;
            }
            set
            {
                if (value > 0)
                    attack = value;
                else
                    throw new ArgumentException();
            }
        }

        [DataMember]
        int durability;

        [DataMember]
        public int Durability
        {
            get
            {
                return durability;
            }
            set
            {
                if (value > 0)
                    durability = value;
                else
                    throw new ArgumentException();
            }
        }
    }
}
