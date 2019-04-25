using System;


namespace HSLibrary
{
    class CardsAtribute: Attribute
    {
        public string NameCards { get; set; }

        public CardsAtribute() { }
        public CardsAtribute(string nameCards)
        {
            NameCards = nameCards;
        }

        public override string ToString()
        {
            return $"{NameCards}";
        }
    }
}
