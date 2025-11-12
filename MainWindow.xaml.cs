using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;

namespace TRPO_Prakt7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Doctor register_doctor;
        public Doctor login_doctor;

        public Pacient create_pacient;
        public Pacient found_pacient;
        public Pacient edit_pacient;

        public Pacient appointment_pacient;

        public MainWindow()
        {
            InitializeComponent();

            register_doctor = (Doctor)Resources["register_doctor"];
            login_doctor = (Doctor)Resources["login_doctor"];

            create_pacient = (Pacient)Resources["create_pacient"];
            found_pacient = (Pacient)Resources["found_pacient"];
            edit_pacient = (Pacient)Resources["edit_pacient"];

            appointment_pacient = (Pacient)Resources["appointment_pacient"];

            UpdateCountLabels();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                register_doctor.Register();
                MessageBox.Show($"Вы зарегистрированы. Ваш ID: {register_doctor.Id}", "Успешно");
                UpdateCountLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                login_doctor.Login();
                MessageBox.Show("Вход выполнен", "Успешно");
                
                CreatePacientBorder.IsEnabled = true;
                FindPacientBorder.IsEnabled = true;
                DoctorInfoId.Visibility = Visibility.Visible;

                IDTextBox.Clear();
                PassLoginTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PacientCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                create_pacient.Create();
                MessageBox.Show($"Пациент был добавлен. ID: {create_pacient.Id}", "Успешно");
                UpdateCountLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PacientFindButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                found_pacient.TryLoadPacient();
                MessageBox.Show("Пациент найден", "Успешно");

                LoadEditStudent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PacientEditButton_Click(object sender, RoutedEventArgs e)
        {
            edit_pacient.Save();
            MessageBox.Show("Файл пациента был изменён");
        }

        private void LoadEditStudent()
        {
            edit_pacient.LoadFromObject(found_pacient);
            appointment_pacient.LoadFromObject(found_pacient);
            EditPacientBorder.IsEnabled = true;
            AppointmentBorder.IsEnabled = true;

            appointment_pacient.LastAppointment = "";
            appointment_pacient.LastDoctor = login_doctor.Id;
            appointment_pacient.Diagnosis = "";
            appointment_pacient.Recomendations = "";
        }
        private void PacientEditResetButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEditStudent();
        }

        private void PacientAppApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                appointment_pacient.MakeAppointment();
                MessageBox.Show("Приём отмечен", "Успешно");

                found_pacient.TryLoadPacient();
                LoadEditStudent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCountLabels()
        {
            string[] doctor_files = Directory.GetFiles(Doctor.DOCTORS_FILEPATH);
            string[] pacient_files = Directory.GetFiles(Pacient.PACIENTS_FILEPATH);

            DoctorCountLabel.Content = $"Докторов: {doctor_files.Length}";
            PacientCountLabel.Content = $"Пациентов: {pacient_files.Length}";
        }
    }
}