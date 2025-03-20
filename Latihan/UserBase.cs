// Derived Class (Child Class)-mengurangi duplikasi kode
namespace Formulatrix
{
    public abstract class UserBase : InterfaceUser //abstraction
    {
        // Hanya bisa diakses dari class ini dan turunannya
        public string Name { get; protected set; }
        public string Address { get; protected set; }
        public int? Age { get; protected set; } // Nullable Int
        public double? Height { get; protected set; } // Nullable Double
        public bool Intern { get; protected set; }
        public char Gender { get; protected set; }

        
        protected UserBase(string name, string address, int? age, double? height, bool intern, char gender)
        {
            Name = name;
            Address = address;
            Age = age;
            Height = height;
            Intern = intern;
            Gender = char.ToUpper(gender);
            
        }

        public abstract void ShowInfo(); //abstraction
    }
}
