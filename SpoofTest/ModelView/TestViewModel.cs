using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpoofTest.Data;
using SpoofTest.Helpers;
using System.Windows;

namespace SpoofTest.ModelView;

public partial class TestViewModel : ObservableObject
{
    Networker networker = new();
    [ObservableProperty]
    private Test? _test = null!;
    [ObservableProperty]
    private int _currentQuestionNumber = 1;
    [ObservableProperty]
    private TimeSpan _currentTime;
    [ObservableProperty]
    private Question? currentQuestion;
    [ObservableProperty]
    private Visibility _visible = Visibility.Collapsed;
    public int Corrects = 0;
    public bool canWork;
    [RelayCommand]
    private void SelectAnswer(Answer? answer)
    {
        if (canWork && answer is null || CurrentQuestion is null)
            return;
        if(CurrentQuestion.UserAnswer is not null)
            CurrentQuestion.UserAnswer.Selected = false;
        CurrentQuestion.UserAnswer = answer;
    }
    [RelayCommand]
    private void SelectPage(int? number)
    {
        if (number is null)
            return;
        CurrentQuestionNumber = number.Value;
        CurrentQuestion = Test.Questions[CurrentQuestionNumber-1];
    }
    [RelayCommand]
    public async Task CheckWork()
    {
        if (Test.Questions.FirstOrDefault(x => x.UserAnswer is null) is not null)
            if (MessageBox.Show("У вас есть не решённые задания. Вы точно хотите отправить работу?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
        var result = await networker.Send(Test);
        MessageBox.Show($"Ваш результат: {result}");
        Environment.Exit(0);
    }
    [RelayCommand]
    public void NextQuestion()
    {
        if (CurrentQuestionNumber == Test.Questions.Count)
            return;
        CurrentQuestionNumber++;
        CurrentQuestion = Test.Questions[CurrentQuestionNumber - 1];
        if (CurrentQuestionNumber == Test.Questions.Count)
            Visible = Visibility.Visible;
    }
    [RelayCommand]
    private void LastQuestion()
    {
        if (CurrentQuestionNumber == 1)
            return;
        CurrentQuestionNumber--;
        CurrentQuestion = Test.Questions[CurrentQuestionNumber - 1];
        if (CurrentQuestionNumber == Test.Questions.Count-1)
            Visible = Visibility.Collapsed;
    }
    public async Task Start(string key, string name, string lastName, string patronymic, string group)
    {
        Test = await networker.GetWorkAsync(key);
        if (Test is null)
            return;
        Test.Name = name;
        Test.LastName = lastName;
        Test.Patronymic = patronymic;
        Test.Group = group;
        Test.SessionId = "";
        CurrentTime = Test.Limit;
        CurrentQuestion = Test.Questions[CurrentQuestionNumber - 1];
        Timer();
    }

    private async void Timer()
    {
        canWork = true;
        while (CurrentTime > TimeSpan.Zero)
        {
            CurrentTime -= TimeSpan.FromSeconds(1);
            await Task.Delay(1000);
        }
        canWork = false;
        await CheckWork();
    }
}
