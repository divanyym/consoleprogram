using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace Formulatrix
{
    public class DataControl : IEnumerable<DataUser>
    {
        public event EventHandler<DataUser>? UserAdded; // Event ketika user baru ditambahkan
        private readonly List<DataUser> _input = new List<DataUser>(); // List jumlah user
        public static int TotalUsers { get; private set; } = 0; // Menyimpan total jumlah user yang dibuat

        private int _maxUsers;

        public DataControl(int userCount)
        {
            _maxUsers = userCount > 5 ? 5 : userCount; // Batasi max 5 user
        }

        // Enum Job Level
        public enum JobLevel
        {
            Junior,
            Mid,
            Senior
        }
     

        public void InputData()
        {
            while (TotalUsers < 5) // Membatasi hingga 5 user saj
            {
                Console.WriteLine ($"\nData ke-{TotalUsers + 1}:");

                string name = InputString("Name");
                string address = InputString("Address");
                int? age = InputInt("Age"); // Menghindari null
                double? height = InputDouble("Tinggi Badan (cm)"); // Menghindari null
                bool intern = InputBool("Apakah Anda sedang intern di Formulatrix? (true/false)");
                char gender = InputChar("Jenis Kelamin (L/P)");
               

                // FIX: Tambahkan input untuk JobLevel
                JobLevel level = InputJobLevel(); 

         
                // Input Jam Kerja
                DateTime? clockInTime = InputTime("Clock In Time");
                DateTime? clockOutTime = InputTime("Clock Out Time");
                
                var levelConverted = (Formulatrix.DataUser.JobLevel)(int)level;
                var newUser = new DataUser(levelConverted, name, address, age, height, intern, gender, clockInTime, clockOutTime);

                _input.Add(newUser);
                TotalUsers++;

                // Menampilkan informasi user
                newUser.ShowInfo();

                // Panggil event jika ada subscriber
                UserAdded?.Invoke(this, newUser);

                if (TotalUsers >= 5)
                {
                    Console.WriteLine("\n[INFO] Maksimal 5 user telah tercapai. Tidak bisa menambahkan lebih banyak user.");
                    break;
                }

                

                Console.Write("\nTambahkan user lain? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y") break;
            }
        }

        public void ShowData()
        {
            Console.WriteLine("\n=== DATA TELAH TERSIMPAN ===");
            foreach (var input in _input)
            {
                input.ShowInfo();
            }
             if (_input.Count >= 2)
        {
            var user1 = _input[0];
            var user2 = _input[1];

            Console.WriteLine("\n=== PERBANDINGAN DATA ===");

            if (user1 > user2)
            {
                Console.WriteLine($"{user1.Name} lebih tua dari {user2.Name}");
            }
            else if (user1 < user2)
            {
                Console.WriteLine($"{user2.Name} lebih tua dari {user1.Name}");
            }
            else
            {
                Console.WriteLine($"{user1.Name} dan {user2.Name} memiliki umur yang sama.");
            }

            int totalUmur = user1 + user2;
            Console.WriteLine($"Total umur mereka: {totalUmur} tahun");

            string userInfo = user1 + "(User dari input)";
            Console.WriteLine(userInfo);
            }
        }   

        // Implementasi Enumerator agar DataControl bisa digunakan dalam foreach
        public IEnumerator<DataUser> GetEnumerator()
        {
            return _input.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); //mengembalikan objek dari enumerator
        }

        public void SearchUser(string name)
        {
            var foundUser = _input.Find(user => user.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (foundUser != null)
            {
                Console.WriteLine("\n=== USER DITEMUKAN ===");
                foundUser.ShowInfo();
            }
            else
            {
                Console.WriteLine("User tidak ditemukan.");
            }
        }

        private static string InputString(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine()?.Trim().ToLower() ?? "empty"; 
        }

        private static int? InputInt(string prompt)
        {
            Console.Write($"{prompt}: ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input)) return null; // Jika kosong, kembalikan null
            
            return int.TryParse(input, out int value) ? value : null;
        }

        private static double? InputDouble(string prompt)
        {
            Console.Write($"{prompt}: ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input)) return null; // Jika kosong, kembalikan null
            
            return double.TryParse(input, out double value) ? value : null;
        }

        private static bool InputBool(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string? input = Console.ReadLine();

                try
                {
                    return bool.Parse(input ?? throw new FormatException()); // Paksa FormatException jika null
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input tidak valid! Harap masukkan 'true' atau 'false'.");
                }
            }
        }

        private static char InputChar(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                char input = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine(); // Untuk pindah ke baris baru setelah input

                try
                {
                    if (input == 'L' || input == 'P')
                        return input;
                    else
                        throw new FormatException();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input tidak valid! Harap masukkan 'L' untuk Laki-laki atau 'P' untuk Perempuan.");
                }
            }
        }

        //csv
        public void SaveWorkTimeToCSV(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("Name,ClockIn, ClockOut, Total Work Hours, Total Salary");
                foreach (var user in _input)
                {
                    writer.WriteLine($"{user.Name},{user.ClockInTime},{user.ClockOutTime},{user.TotalWorkHours},{user.TotalSalary}");
                }
            }
            Console.WriteLine($"Data work time saved to {filename}");
        }

        public void ReadWorkTimeFromCSV(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found!");
                return;
            }

            using (StreamReader reader = new StreamReader(filename))
            {
                string? header = reader.ReadLine(); // Skip header
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    if (line != null)
                    {
                        var parts = line.Split(',');
                        if (parts.Length >= 3) 
                        {
                            Console.WriteLine($"User: {parts[0]}, Clock In: {parts[1]}, Clock Out: {parts[2]}");
                        }
                        else
                        {
                            Console.WriteLine("Skipping invalid line: " + line);
                        }
                    }
                }
            }
        } 
        private static DateTime? InputTime(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (format: HH:mm): ");
                string? input = Console.ReadLine();

                if (DateTime.TryParseExact(input, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime time))
                {
                    return time;
                }
                else
                {
                    Console.WriteLine("Format waktu tidak valid. Harap masukkan waktu dalam format HH:mm.");
                }
            }
        }


    // Fungsi untuk meminta input kategori pekerjaan
    private static JobLevel InputJobLevel()
    {
        while (true)
        {
            Console.WriteLine("Pilih level pekerjaan: (1) Intern, (2) Junior, (3) Senior");
            Console.Write("Masukkan angka (1-3): ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1": return JobLevel.Mid;
                case "2": return JobLevel.Junior;
                case "3": return JobLevel.Senior;
                default:
                    Console.WriteLine("Input tidak valid! Pilih 1, 2, atau 3.");
                    break;
            }
        }}
    
 }}



