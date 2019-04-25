namespace HSLibrary
{
    public enum CardsRarity
    {
        Free,
        Common,
        Rare,
        Epic,
        Legendary
    }

    public enum CardsVersion
    {
        Golden,
        Common
    }

    public enum CardsCategorize
    {
        Druid,
        Hunter,
        Mage,
        Paladin,
        Priest,
        Rogue,
        Shaman,
        Warlock,
        Warrior,
        Neuteral //all classes
    }

    interface ICards
    {
        int ManaCoast { get; set; }
        CardsRarity rarityOfCard { get; set; }
        CardsCategorize categorizeOfCard { get; set; }
        CardsVersion versionOfCard { get; set; }
    }
}
