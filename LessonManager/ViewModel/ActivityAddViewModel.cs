using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model;
using System.Windows;

namespace LessonManager.ViewModel
{
    internal partial class ActivityAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? m_ActivityName;

        [ObservableProperty]
        private string? m_ActivityType;

        [ObservableProperty]
        private DateTime m_ActivityDate;

        [ObservableProperty]
        private Subject m_SubjectActivity;

        [RelayCommand]
        private void AddActivity()
        {
            if (m_ActivityName == null || m_ActivityType == null)
            {
                MessageBox.Show("Не все поля заполненные");
                return;
            }

            if(SubjectActivity == null)
            {
                MessageBox.Show("Ошибка, неправильно найдена дисциплина");
                throw new Exception("Error, SubjectActivity is null");
            }

            App.ApplicationContext.ActivityDB.AddActivity(m_ActivityName, SubjectActivity, (ActivityType)Enum.Parse(typeof(ActivityType), m_ActivityType), m_ActivityDate);
        }

        // автоматически устанавливает шаблон названия (вызывать после установки контента данных)
        public void AutoSetName()
        {
            if (ActivityType == "Lab")
                ActivityName = "Лабораторная работа №";
            else if (ActivityType == "Prac")
                ActivityName = "Практика №";
            else if (ActivityType == "Lec")
                ActivityName = "Лекция №";
            else
                throw new Exception("Неизвестный вид занятия");
        }
    }
}
