using CommunityToolkit.Mvvm.ComponentModel;

namespace SpoofTest.Data;

public partial class Answer : ObservableObject
{
    [ObservableProperty]
    private string _title = null!;
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private bool _selected;
    public bool IsCorrect { get; set; }
}
