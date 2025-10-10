using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SpoofTest.Data;

public partial class Question : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private string _title = null!;
    [ObservableProperty]
    private ObservableCollection<Answer> _answers = [];
    [ObservableProperty]
    private Answer? _userAnswer;
}
