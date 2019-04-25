using System;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Drawing;

namespace HSLibrary
{
    

    [DataContract]
    public abstract class Cards : ICards
    {

        [DataMember]
        int manacoast;

        [DataMember]
        public int ManaCoast
        {
            get
            {
                return manacoast;
            }
            set
            {
                if (value > 10)
                    throw new ArgumentException();
                else
                    manacoast = value;
            }
        }

        [DataMember]
        public CardsRarity rarityOfCard{ get; set; }

        [DataMember]
        public CardsCategorize categorizeOfCard { get; set; }

        [DataMember]
        public CardsVersion versionOfCard { get; set; }
    }


    
}
