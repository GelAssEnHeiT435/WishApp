using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls.Shapes;

namespace WishListClient.src.Components;

public partial class PasswordEntry : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(PasswordEntry), default(string), BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(PasswordEntry), "Пароль");

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType), typeof(ReturnType), typeof(PasswordEntry), ReturnType.Default);

    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    public static readonly BindableProperty IsPasswordVisibleProperty = BindableProperty.Create(
        nameof(IsPasswordVisible), typeof(bool), typeof(PasswordEntry), false,
        propertyChanged: OnIsPasswordVisibleChanged);

    public bool IsPasswordVisible
    {
        get => (bool)GetValue(IsPasswordVisibleProperty);
        set => SetValue(IsPasswordVisibleProperty, value);
    }

    public static readonly BindableProperty ToggleIconDataProperty = BindableProperty.Create(
        nameof(ToggleIconData), typeof(Geometry), typeof(PasswordEntry), null);

    public Geometry ToggleIconData
    {
        get => (Geometry)GetValue(ToggleIconDataProperty);
        private set => SetValue(ToggleIconDataProperty, value);
    }

    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
        nameof(IconColor), typeof(Color), typeof(PasswordEntry), Colors.Gray);

    public Color IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(
        nameof(IconSize), typeof(double), typeof(PasswordEntry), 24.0);

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public static readonly BindableProperty NextElementProperty =
        BindableProperty.CreateAttached(
            "NextElement",
            typeof(VisualElement),
            typeof(PasswordEntry),
            null,
            BindingMode.OneWay,
            propertyChanged: OnNextElementChanged);

    public static VisualElement? GetNextElement(BindableObject view)
        => (VisualElement?)view.GetValue(NextElementProperty);

    public static void SetNextElement(BindableObject view, VisualElement? value)
        => view.SetValue(NextElementProperty, value);

    public PasswordEntry()
    {
        InitializeComponent();
        BindingContext = this;
        UpdateIcon();
    }

    private static void OnNextElementChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PasswordEntry passwordEntry &&
            passwordEntry._entry is Entry internalEntry)
        {
            SetFocusOnEntryCompletedBehavior.SetNextElement(internalEntry, (VisualElement?)newValue);
        }
    }

    private void OnToggleTapped(object sender, TappedEventArgs e) {
        IsPasswordVisible = !IsPasswordVisible;
    }

    private static void OnIsPasswordVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PasswordEntry control)
        {
            control.UpdatePasswordState();
            control.UpdateIcon();
        }
    }

    private void UpdatePasswordState() {
        if (_entry != null) _entry.IsPassword = !IsPasswordVisible;
    }

    private void UpdateIcon()
    {
        var resources = Application.Current?.Resources;

        if (resources == null) return;

        var key = IsPasswordVisible ? "Visible" : "NotVisible";

        if (resources.TryGetValue(key, out var resource) && resource is PathGeometry geometry)
            ToggleIconData = geometry;
    }
}