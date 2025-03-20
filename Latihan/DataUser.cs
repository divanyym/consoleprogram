using System;

namespace Formulatrix
{
    public class DataUser : IComparable<DataUser>
    {
        // Static Field untuk melacak jumlah user
        public static int UserCount { get; private set; } = 0;

        // Enum Gender
        public enum GenderType
        {
            LakiLaki,
            Perempuan
        }

        public enum JobLevel
        {
            Junior,
            Mid,
            Senior
        }

        // Properti
        public string Name { get; }
        public string Address { get; }
        public int? Age { get; protected set; } // Nullable
        public double? Height { get; protected set; } // Nullable
        public bool Intern { get; }
        public GenderType Gender { get; } // Menggunakan Enum
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public JobLevel Level { get; set; }
        public int HourlyRate { get; set; }  // Gaji per jam otomatis dihitung

        // Properti tambahan untuk perhitungan jam kerja dan gaji
        public double TotalWorkHours => (ClockOutTime.HasValue && ClockInTime.HasValue) 
            ? (ClockOutTime.Value - ClockInTime.Value).TotalHours 
            : 0;

        public double TotalSalary => TotalWorkHours * HourlyRate; // Perhitungan gaji total

        // Constructor
        public DataUser(JobLevel level, string name, string address, int? age, double? height, bool intern, char gender, DateTime? clockInTime, DateTime? clockOutTime)
        {
            Name = name;
            Level = level;
            Address = address;
            Age = age;
            Height = height;
            Intern = intern;
            Gender = ConvertToGender(gender); // Konversi char ke enum
            UserCount++; // Tambah jumlah user
            ClockInTime = clockInTime;
            ClockOutTime = clockOutTime;
            HourlyRate = GetHourlyRate(level); // Mengisi gaji per jam berdasarkan level kerja
        }

        // Metode Konversi Char ke Enum Gender
        private static GenderType ConvertToGender(char gender)
        {
            gender = char.ToUpper(gender); // Pastikan uppercase
            if (gender == 'L') return GenderType.LakiLaki;
            if (gender == 'P') return GenderType.Perempuan;

            throw new ArgumentException("Gender harus 'L' atau 'P'");
        }

        // Implementasi IComparable untuk perbandingan
        public int CompareTo(DataUser? other)
        {
            if (other == null) return 1;
            return (this.Age ?? 0).CompareTo(other.Age ?? 0);
        }

        // Operator > dan < menggunakan CompareTo()
        public static bool operator >(DataUser a, DataUser b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(DataUser a, DataUser b)
        {
            return a.CompareTo(b) < 0;
        }

        // Operator + untuk menjumlahkan umur
        public static int operator +(DataUser a, DataUser b)
        {
            return (a.Age ?? 0) + (b.Age ?? 0);
        }

        // Operator + untuk menambahkan string tambahan pada nama
        public static string operator +(DataUser a, string additionalText)
        {
            return $"{a.Name} {additionalText}";
        }

        // Method untuk menampilkan informasi user
        public void ShowInfo()
        {
            Console.WriteLine("\n===== User Info =====");
            Console.WriteLine($"Name         : {Name}");
            Console.WriteLine($"Address      : {Address}");
            Console.WriteLine($"Age          : {(Age.HasValue ? Age + " tahun" : "Belum diisi")}");
            Console.WriteLine($"Height       : {(Height.HasValue ? Height + " cm" : "Belum diisi")}");
            Console.WriteLine($"Intern       : {(Intern ? "Ya" : "Tidak")}");
            Console.WriteLine($"Gender       : {Gender}");
            Console.WriteLine($"Job Level    : {Level}");
            Console.WriteLine($"Hourly Rate  : Rp {HourlyRate:N0} per jam");
            Console.WriteLine($"Total Salary : Rp {TotalSalary:N0}");

           if (ClockInTime.HasValue && ClockOutTime.HasValue)
            {
                TimeSpan duration = ClockOutTime.Value - ClockInTime.Value;
                int hours = (int)duration.TotalHours;
                int minutes = duration.Minutes;

                Console.WriteLine($"Clock In     : {ClockInTime.Value}");
                Console.WriteLine($"Clock Out    : {ClockOutTime.Value}");
                Console.WriteLine($"Total Work Hours: {hours} jam {minutes} menit");
            }

            else
            {
                Console.WriteLine("Clock In / Clock Out data tidak lengkap.");
            }
        }

        // Fungsi untuk menentukan gaji per jam berdasarkan level kerja
        private static int GetHourlyRate(JobLevel jobLevel)
        {
            return jobLevel switch
            {
                JobLevel.Junior => 75000,  // Junior: Rp 75.000/jam
                JobLevel.Mid => 50000,     // Mid: Rp 50.000/jam
                JobLevel.Senior => 100000, // Senior: Rp 100.000/jam
                _ => 0
            };
        }
    }
}
