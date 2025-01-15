using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
