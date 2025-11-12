using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TRPO_Prakt7
{
    public class Doctor : INotifyPropertyChanged
    {
        public const string DOCTORS_FILEPATH = "../../../doctors";

        bool loaded = false;

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        private string first_name;
        public string FirstName
        {
            get { return first_name; }
            set
            {
                if (value != first_name)
                {
                    first_name = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        private string last_name;
        public string LastName
        {
            get { return last_name; }
            set
            {
                if (value != last_name)
                {
                    last_name = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        private string middle_name;
        public string MiddleName
        {
            get { return middle_name; }
            set
            {
                if (value != middle_name)
                {
                    middle_name = value;
                    OnPropertyChanged(nameof(MiddleName));
                }
            }
        }

        private string specialization;
        public string Specialization
        {
            get { return specialization; }
            set
            {
                if (value != specialization)
                {
                    specialization = value;
                    OnPropertyChanged(nameof(specialization));
                }
            }
        }

        public string Password {get; set;}

        public string ConfirmPassword { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;


        private Dictionary<int, string> ids = new Dictionary<int, string>();
        public Doctor() { }

        private Random rnd = new Random();

        public void LoadDoctors()
        {
            string[] doctor_files = Directory.GetFiles(DOCTORS_FILEPATH, "D_*.json");

            foreach (string file in doctor_files)
            {
                string json_string = File.ReadAllText(file);
                Doctor doctor = JsonSerializer.Deserialize<Doctor>(json_string);
                ids[doctor.Id] = doctor.Password;
            }
        }
        public void Register()
        {
            if (first_name == "" || last_name == "" || middle_name == "" || specialization == "" || Password == "" || ConfirmPassword == "")
                throw new ArgumentException("Все поля должны быть заполнены");

            if (Password != ConfirmPassword)
                throw new ArgumentException("Пароли не совпадают");

            int id;
            do { id = rnd.Next(10000, 100000); }
            while (ids.ContainsKey(id));
            Id = id;

            var jsonString = JsonSerializer.Serialize(this);
            var path = $"{DOCTORS_FILEPATH}/D_{_id}.json";
            File.WriteAllText(path, jsonString, Encoding.UTF8);
        }

        public void Login()
        {
            LoadDoctors();

            if (Id == 0 && Password == "")
                throw new ArgumentException("Все поля должны быть заполнены");

            if (!ids.ContainsKey(Id) || ids[Id] != Password)
                throw new ArgumentException("Неверный логин или пароль");


            var path = $"{DOCTORS_FILEPATH}/D_{Id}.json";
            if (!File.Exists(path))
                throw new FileNotFoundException("Пользователь не найден");

            var jsonString = File.ReadAllText(path);
            Doctor doctor = JsonSerializer.Deserialize<Doctor>(jsonString);

            this.Id = doctor.Id;
            this.FirstName = doctor.FirstName;
            this.LastName = doctor.LastName;
            this.MiddleName = doctor.MiddleName;
            this.Specialization = doctor.Specialization;
            this.Password = doctor.Password;

            loaded = true;
        }

        private void OnPropertyChanged(string property_name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
