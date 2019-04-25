using System;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace HSLibrary
{

    public enum DKHeroTypes
    {
        DKDruid,
        DKHunter,
        DKMage,
        DKPaladin,
        DKPriest,
        DKRogue,
        DKShaman,
        DKWarlock,
        DKWarrior
    }

    
    [CardsAtribute("Hero Card")]
    [DataContract]
    public class HeroCard: Cards, HeroInterface
    {
        [DataMember]
        public int Armor { set; get; }

        [DataMember]
        public HeroTypes Hero { get; set; }

        [DataMember]
        public DKHeroTypes HeroAbility { get; set; }

        [DataMember]
        public int Health { get; set; }

        public string ChangeHeroAbility(HeroCard hero)
        {
            return $"Ability of {hero.Hero} changed";
        }

        public HeroCard()
        {
            rarityOfCard = CardsRarity.Legendary;
            Health = 30;
            Armor = 5;
        }
    }
}
