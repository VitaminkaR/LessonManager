using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Core.Enums;
using LessonManager.Model;
using LessonManager.Model.Database;
using LessonManager.Model.Database.Entities;
using LessonManager.Model.Database.Repositories;
using LessonManager.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LessonManager.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        private ApplicationContext m_ApplicationContext;
        private IActivityRepository m_ActivityRepository;
        private ISubjectRepository m_SubjectRepository;

        private IISCImport m_ISCImport;

        [ObservableProperty]
        private SubjectEntity m_ChoosenSubject;

        [ObservableProperty]
        private TabItem m_ChoosenTab;

        [RelayCommand]
        private void OpenISCFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Document";
            dialog.DefaultExt = ".ics";
            dialog.Filter = "Файл календаря(.ics)|*.ics";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                    m_ISCImport.Init(fs);

                ICollection<SubjectEntity> subjects = m_ISCImport.GetSubjects();

                foreach (SubjectEntity subject in subjects)
                {
                    m_SubjectRepository.AddAsync(subject);
                }
            }
        }

        [RelayCommand]
        private void ReloadDB()
        {
            MessageBoxResult rsltMessageBox = MessageBox.Show(
                "Вы действительно хотите полностью удалить информацию о занятиях?",
                "Потверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    m_ApplicationContext.ClearDB();
                    break;
            }

            CurrentActivities.Clear();
        }

        [RelayCommand]
        private void AddSubject()
        {
            new SubjectAddWindow().ShowDialog();

            SetSubjects();
        }

        [RelayCommand]
        private void GetStatistics()
        {
            new StatisticsWindow().Show();
        }

        public void SetActivities()
        {
            if (ChoosenSubject == null)
            {
                MessageBox.Show("Не выбрана дисциплина");
                return;
            }

            ActivityType activityType = GetActivityTypeByTab();

            SettingActivities(ChoosenSubject, activityType);
        }

        private ActivityType GetActivityTypeByTab()
        {
            ActivityType activityType = ActivityType.Lec;
            switch ((string)ChoosenTab.Header)
            {
                case "Лекции":
                    activityType = ActivityType.Lec;
                    break;
                case "Практики":
                    activityType = ActivityType.Prac;
                    break;
                case "Лабораторные":
                    activityType = ActivityType.Lab;
                    break;
                default:
                    throw new System.Exception("Неизвестный тип занятия");
            }

            return activityType;
        }

        // все дисциплины
        public ObservableCollection<SubjectEntity> Subjects { get; set; }
        // выбранные по дисциплине занятия
        public ObservableCollection<ActivityEntity> CurrentActivities { get; set; }

        // удаляет дисциплину из меню
        public async void RemoveSubjectElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubject == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            await m_SubjectRepository.RemoveAsync(ChoosenSubject.Name);

            SetSubjects();
        }

        // редактирует дисциплину из меню
        public async void EditSubjectElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubject == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            SubjectEntity s = await m_SubjectRepository.GetAsync(ChoosenSubject.Name);
            new SubjectEditWindow(s).ShowDialog();

            SetSubjects();
        }

        // непосредственно обновляет коллекцию занятий
        private void SettingActivities(SubjectEntity subject, ActivityType type)
        {
            var activities = m_ActivityRepository.GetAllActivitiesOfTypeFromSubject(subject, type);
            foreach (var item in CurrentActivities)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }
            CurrentActivities.Clear();
            foreach (var item in activities)
            {
                CurrentActivities.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        // редактирвоание занятия
        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var serviceProvider = App.CurrentApplication.services.BuildServiceProvider();
            IActivityRepository activities = serviceProvider.GetRequiredService<IActivityRepository>();
            m_ActivityRepository.EditActivity((ActivityEntity)sender);
        }

        public MainViewModel()
        {
            var serviceProvider = App.CurrentApplication.services.BuildServiceProvider();
            m_ApplicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
            m_ActivityRepository = serviceProvider.GetRequiredService<IActivityRepository>();
            m_SubjectRepository = serviceProvider.GetRequiredService<ISubjectRepository>();

            Subjects = new ObservableCollection<SubjectEntity>();
            CurrentActivities = new ObservableCollection<ActivityEntity>();
            CurrentActivities.CollectionChanged += CurrentActivities_CollectionChanged;

            m_ISCImport = new OGUICSImport();
        }

        public async void SetSubjects()
        {
            Subjects.Clear();
            foreach (var item in await m_SubjectRepository.GetAsync())
                Subjects.Add(item);
        }

        private async void CurrentActivities_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // удаление активности
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                ActivityEntity activity = (ActivityEntity)e.OldItems[0];
                m_ActivityRepository.RemoveActivity(activity);
            }
            // добавление активности
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems.Count == 1)
            {
                ActivityEntity activity = (ActivityEntity)e.NewItems[0];
                if(activity.Name == null)
                {
                    if (ChoosenSubject == null)
                    {
                        MessageBox.Show("Не выбран ни один элемент");
                        return;
                    }

                    ActivityType activityType = GetActivityTypeByTab();
                    SubjectEntity s = await m_SubjectRepository.GetAsync(ChoosenSubject.Name);

                    activity.Name = "";
                    activity.Subject = s;
                    activity.Type = activityType;
                    activity.ActivityTime = DateTime.Now;
                    m_ActivityRepository.AddActivity(activity);

                    activity.PropertyChanged += Item_PropertyChanged;
                }
            }
        }
    }
}
