namespace Scripts.OOP.Perks
{
    public abstract class Perk
    { 
        private string name;
        public string Name => name ??
            (name = GetType().Name.Replace('_', ' '));

        private string description;

        public string Description => description ??
            (description = GetDescription());

        private int level; //Owned level; If <= 0: Perk lost when decharged
        public int Level { get => level; }
        private int buff; //Temporary buffs lost when charge ends
        public int Buff { get => buff; }

        public int Intensity => level + buff;

        private float charge; //Current buff charge
        public float Charge { get => charge; }

        public PerkUIHandler ui;

        protected virtual int ToBuffCharge => 5;

        public Perk()
        {
            Start();
        }

        protected virtual void Start() { }

        //Get\Sets
        protected abstract string GetDescription();

        //Actions
        public void ChargeBuff(int buff, float charge)
        {
            this.buff += buff;
            this.charge += charge;

            description = null;
        }

        public void Add(Perk perk)
        {
            level += perk.level;
            ChargeBuff(perk.buff, perk.charge);
        }

        public void LevelUp(int levels = 1)
        {
            level += levels;
            if (ui)
            {
                if (buff <= 1) ui.SetLevel(level);
                else ui.SetBuff(Intensity, charge);
            }
            description = null;
        }

        public void ToBuff()
        {
            buff = level;
            charge = ToBuffCharge;
            level = 0;
            description = null;
        }

        public void AsBuff(int level, int chargeMultiplier)
        {
            buff = level;
            charge = ToBuffCharge * chargeMultiplier;
            this.level = 0;
            description = null;
        }

        public bool Consume(float consumed)
        {
            if (buff > 0)
            {
                charge -= consumed;
                //When depleted, empty buff
                if (charge <= 0)
                {
                    charge = 0;
                    buff = 0;

                    if (ui) ui.SetLevel(level);
                }
                else if (ui) ui.SetCharge(charge);
            }
            //If buff and level empty: Perk should be removed
            return Intensity == 0;
        }
    }
}
