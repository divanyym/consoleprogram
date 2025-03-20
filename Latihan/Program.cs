using System;

namespace Formulatrix
{
    class Program
    {
        // Delegate untuk menampilkan pesan
        public delegate void MessageDelegate(string message);

        static void Main()
        {
            Console.WriteLine("=== FORMULATRIX INTERN INFORMATION CENTER ===");

            // Delegate untuk menampilkan pesan selamat datang
            MessageDelegate showMessage = DisplayMessage;
            showMessage("Program dimulai, silakan masukkan data!");

            const int user = 5;
            DataControl author = new DataControl(user);

            // Subscribe ke event UserAdded
            author.UserAdded += OnUserAdded;

             while (true)
            {
                Console.WriteLine("\n=== MENU ===");
                Console.WriteLine("1. Tambah User");
                Console.WriteLine("2. Tampilkan Semua User");
                Console.WriteLine("3. Cari User");
             /* Console.WriteLine("4. Input Jam Kerja"); */
                Console.WriteLine("4. Keluar");
                Console.Write("Pilih menu: "); 

                string? pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        author.InputData();
                        break;

                    case "2":
                        author.ShowData();
                        break;

                    case "3":
                        Console.Write("Masukkan nama yang dicari: ");
                        string? searchName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(searchName))
                        {
                            author.SearchUser(searchName);
                        }
                        else
                        {
                            Console.WriteLine("Nama tidak boleh kosong.");
                        }
                        break;
                    
                    case "4":
                        Console.WriteLine("Keluar...");
                        return;

                    default:
                        Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                        break;
                }
                 
                 // Simpan dan baca data kerja dari CSV
                author.SaveWorkTimeToCSV("work_time.csv");
                author.ReadWorkTimeFromCSV("work_time.csv");

            }
               
        }
    
        
         // Event handler untuk menangkap event UserAdded
        static void OnUserAdded(object? sender, DataUser user)
        {
            Console.WriteLine($"[Succsess] User baru ditambahkan: {user.Name}, {user.Age} tahun.");
        }
        
        // Metode yang digunakan delegate untuk menampilkan pesan
        static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
