using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;

namespace WishListClient.src.Components;

public partial class ImageLoaderButton : ContentView
{
    #region Bindable Properties

    public static readonly BindableProperty PhotoSourceProperty =
        BindableProperty.Create(
            nameof(PhotoSource),
            typeof(ImageSource),
            typeof(ImageLoaderButton),
            null,
            BindingMode.TwoWay);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(ImageLoaderButton),
            null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(ImageLoaderButton),
            null);

    public static readonly BindableProperty IconDataProperty =
        BindableProperty.Create(
            nameof(IconData),
            typeof(Geometry),
            typeof(ImageLoaderButton),
            null);

    public static readonly BindableProperty IconColorProperty =
        BindableProperty.Create(
            nameof(IconColor),
            typeof(Color),
            typeof(ImageLoaderButton),
            Colors.White);

    public static readonly BindableProperty ButtonSizeProperty =
        BindableProperty.Create(
            nameof(ButtonSize),
            typeof(double),
            typeof(ImageLoaderButton),
            120.0);

    #endregion

    #region Public Properties

    public ImageSource? PhotoSource
    {
        get => (ImageSource?)GetValue(PhotoSourceProperty);
        set => SetValue(PhotoSourceProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public Geometry? IconData
    {
        get => (Geometry?)GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public Color IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public double ButtonSize
    {
        get => (double)GetValue(ButtonSizeProperty);
        set => SetValue(ButtonSizeProperty, value);
    }

    #endregion

    public ImageLoaderButton()
    {
        InitializeComponent();
        BindingContext = this;
    }

}