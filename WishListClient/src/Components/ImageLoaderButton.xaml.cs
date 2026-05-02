using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.Windows.Input;

namespace WishListClient.src.Components;

public partial class ImageLoaderButton : ContentView
{
    #region Bindable Properties

    public static readonly BindableProperty PhotoSourceProperty =
        BindableProperty.Create(
            nameof(PhotoSource), typeof(ImageSource),
            typeof(ImageLoaderButton), null,
            BindingMode.TwoWay, 
            propertyChanged: OnChangedPhoto);

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

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(
        nameof(DeleteCommand),
        typeof(ICommand),
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

    public static readonly BindableProperty WidthProperty =
        BindableProperty.Create(
            nameof(Width),
            typeof(double),
            typeof(ImageLoaderButton));

    public static readonly BindableProperty HeightProperty =
        BindableProperty.Create(
            nameof(Height),
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

    public ICommand DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
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

    public double Width
    {
        get => (double)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public double Height
    {
        get => (double)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    #endregion

    public ImageLoaderButton()
    {
        InitializeComponent();
        BindingContext = this;

        SetValue(DeleteCommandProperty, new Command(() => {
            PhotoSource = null;
        }));
    }

    private static void OnChangedPhoto(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ImageLoaderButton button && button.Icon != null)
        {
            if (newValue != null)
            {
                button.Icon.IsVisible = false;
                button.DeleteButton.IsVisible = true;
            }
            else
            {
                button.Icon.IsVisible = true;
                button.DeleteButton.IsVisible = false;
            }
        }
    }
}