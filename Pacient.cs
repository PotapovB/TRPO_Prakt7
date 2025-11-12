using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TRPO_Prakt7
{
    public class Pacient : INotifyPropertyChanged
    {
        public const string PACIENTS_FILEPATH = "../../../pacients";
        public event PropertyChangedEventHandler? PropertyChanged;
        private Random rnd = new Random();

        private Dictionary<int, Pacient> pacients = new Dictionary<int, Pacient>();

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

        private string birthday;
        public string Birthday
        {
            get { return birthday; }
            set
            {
                if (value != birthday)
                {
                    birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }

        private string last_appointment;
        public string LastAppointment
        {
            get { return last_appointment; }
            set
            {
                if (value != last_appointment)
                {
                    last_appointment = value;
                    OnPropertyChanged(nameof(LastAppointment));
                }
            }
        }

        private int last_doctor;
        public int LastDoctor
        {
            get { return last_doctor; }
            set
            {
                if (value != last_doctor)
                {
                    last_doctor = value;
                    OnPropertyChanged(nameof(LastDoctor));
                }
            }
        }

        private string diagnosis;
        public string Diagnosis
        {
            get { return diagnosis; }
            set
            {
                if (value != diagnosis)
                {
                    diagnosis = value;
                    OnPropertyChanged(nameof(Diagnosis));
                }
            }
        }

        private string recomendations;
        public string Recomendations
        {
            get { return recomendations; }
            set
            {
                if (value != recomendations)
                {
                    recomendations = value;
                    OnPropertyChanged(nameof(Recomendations));
                }
            }
        }

        public void LoadPacients()
        {
            string[] pacient_files = Directory.GetFiles(PACIENTS_FILEPATH, "D_*.json");

            foreach (string file in pacient_files)
            {
                string json_string = File.ReadAllText(file);
                Pacient pacient = JsonSerializer.Deserialize<Pacient>(json_string);
                pacients[pacient.Id] = pacient;
            }
        }

        public void TryLoadPacient()
        {
            LoadPacients();

            if (pacients.ContainsKey(Id))
            {
                Pacient found = pacients[Id];

                this.FirstName = found.FirstName;
                this.LastName = found.LastName;
                this.MiddleName = found.MiddleName;
                this.Birthday = found.Birthday;
                this.LastAppointment = found.LastAppointment;
                this.LastDoctor = found.LastDoctor;
                this.Diagnosis = found.Diagnosis;
                this.Recomendations = found.Recomendations;
            }
            else
            {
                throw new FileNotFoundException("Пациент по данному ID не найден!");
            }
        }

        public void LoadFromObject(Pacient other)
        {
            this.Id = other.Id;
            this.FirstName = other.FirstName;
            this.LastName = other.LastName;
            this.MiddleName = other.MiddleName;
            this.Birthday = other.Birthday;
            this.LastAppointment = other.LastAppointment;
            this.LastDoctor = other.LastDoctor;
            this.Diagnosis = other.Diagnosis;
            this.Recomendations = other.Recomendations;
        }

        public void Create()
        {
            LoadPacients();

            if (first_name == "" || last_name == "" || middle_name == "" || birthday == "")
                throw new ArgumentException("Все поля должны быть заполнены");

            int id;
            do
            { id = rnd.Next(1000000, 10000000); }
            while (pacients.ContainsKey(id));
            Id = id;

            Save();
        }

        public void MakeAppointment()
        {
            if (last_appointment == "" || last_doctor == 0 || diagnosis == "" || recomendations == "")
                throw new ArgumentException("Все поля должны быть заполнены");

            Save();
        }

        public void Save()
        {
            var jsonString = JsonSerializer.Serialize(this);
            var path = $"{PACIENTS_FILEPATH}/D_{_id}.json";
            File.WriteAllText(path, jsonString, Encoding.UTF8);
        }

        private void OnPropertyChanged(string property_name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
