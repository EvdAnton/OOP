namespace HSLibrary
{
    public enum HeroTypes
    {
        Druid,
        Hunter,
        Mage,
        Paladin,
        Priest,
        Rogue,
        Shaman,
        Warlock,
        Warrior
    }

    interface HeroInterface
    {
        HeroTypes Hero { get; set; }

        int Health { get; set; }

    }
}
