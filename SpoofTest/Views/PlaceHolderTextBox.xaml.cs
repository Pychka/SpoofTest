using System.Windows;
using System.Windows.Controls;

namespace SpoofTest.Views;

public partial class PlaceHolderTextBox : UserControl
{
    public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.Register(nameof(PlaceHolder), typeof(string), typeof(PlaceHolderTextBox), null);

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(PlaceHolderTextBox), null);

    public string PlaceHolder
    {
        get => (string)GetValue(PlaceHolderProperty);
        set => SetValue(PlaceHolderProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public int MaxLetters { get; set; }

    public PlaceHolderTextBox()
    {
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox text)
            Place.Visibility = string.IsNullOrEmpty(text.Text) ? Visibility.Visible : Visibility.Collapsed;
    }
}
