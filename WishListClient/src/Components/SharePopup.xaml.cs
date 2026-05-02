using CommunityToolkit.Maui.Views;
using WishListClient.src.ViewModels;

namespace WishListClient.src.Components;

public partial class SharePopup : Popup
{
    public ShareTab CurrentTab
    {
        get => (ShareTab)GetValue(CurrentTabProperty);
        set => SetValue(CurrentTabProperty, value);
    }

    public static readonly BindableProperty CurrentTabProperty =
        BindableProperty.Create(
            nameof(CurrentTab), 
            typeof(ShareTab), 
            typeof(SharePopup), 
            ShareTab.Link, 
            propertyChanged: OnTabChanged
            );

    public SharePopup(SharePopupViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }

    private static void OnTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SharePopup popup && newValue is ShareTab newTab)
            popup.UpdateUI(popup, newTab);
    }

    private void UpdateUI(SharePopup popup, ShareTab tab)
    {
        if (popup.BtnLinkTab == null || popup.BtnQrTab == null || popup.LinkEntry == null || popup.QrImage == null)
            return;

        bool isLink = tab == ShareTab.Link;

        popup.LinkEntry.IsVisible = isLink;
        popup.QrImage.IsVisible = !isLink;

        if (isLink)
        {
            popup.BtnLinkTab.BackgroundColor = Color.FromArgb("#512BD4");
            popup.BtnLinkTab.TextColor = Colors.White;
            popup.BtnQrTab.BackgroundColor = Color.FromArgb("#2C2C2C");
            popup.BtnQrTab.TextColor = Colors.Gray;
        }
        else
        {
            popup.BtnQrTab.BackgroundColor = Color.FromArgb("#512BD4");
            popup.BtnQrTab.TextColor = Colors.White;
            popup.BtnLinkTab.BackgroundColor = Color.FromArgb("#2C2C2C");
            popup.BtnLinkTab.TextColor = Colors.Gray;
        }
    }

    private void OnLinkTabClicked(object sender, EventArgs e) {
        CurrentTab = ShareTab.Link;
    }

    private void OnQrTabClicked(object sender, EventArgs e) {
        CurrentTab = ShareTab.QrCode;
    }
}

public enum ShareTab
{
    Link,
    QrCode
}