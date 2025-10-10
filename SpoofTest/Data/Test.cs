using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SpoofTest.Data;

public partial class Test : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private string _title = null!;
    [ObservableProperty]
    private string _name = null!;
    [ObservableProperty]
    private string _lastName = null!;
    [ObservableProperty]
    private string _patronymic = null!;
    [ObservableProperty]
    private string _sessionId = null!;
    [ObservableProperty]
    private string _group = null!;
    [ObservableProperty]
    private ObservableCollection<Question> _questions = [];
    [ObservableProperty]
    private TimeSpan _limit;
}
