using System;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;


namespace HSLibrary
{

    public enum Cards_MinionsEffects
    {
        Taunt,
        Deathrattle,
        Poison,
        Stelth,
        Buttelcry
    }

    public enum Cards_MinionsType
    {
        Beast,
        Demon,
        Dragon,
        Murloc,
        Pirate,
        Totem
    }

    [CardsAtribute("Minion card")]
    [DataContract]
    public class Minion : Cards
    {
        [DataMember]
        int health;

        [DataMember]
        public int MinionHealth
        {
            get
            { 
                return health;
            }   
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                else health = value;
            }
        }

        [DataMember]
        public Cards_MinionsEffects minionsEffects { get; set; }


        [DataMember]
        public Cards_MinionsType minionType { get; set; }

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
                if (value < 0)
                    throw new ArgumentException();
                else
                    attack = value;
            }
        }
    }
}
