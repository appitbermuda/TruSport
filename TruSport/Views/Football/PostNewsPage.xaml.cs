using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Football
{
    public partial class PostNewsPage : ContentPage
    {
        PushNoti pushNoti;
        PostNewsViewModel postNewsViewModel;

        public PostNewsPage()
        {
            pushNoti = new PushNoti();

            postNewsViewModel = new PostNewsViewModel();
            BindingContext = postNewsViewModel;

            InitializeComponent();
        }

        async void PostNewsClicked(object sender, System.EventArgs e)
        {
            if (OBOEntry.Text.Length > 0 && TitleEntry.Text.Length > 0 && DescriptionEntry.Text.Length > 0)
            {
                var postNews = await DisplayAlert("Post News", "Are you sure you want to post this blog to OnTrack News? Please verify spelling before posting.", "Post", "Cancel");

                if (postNews)
                {
                    //var response = await pushNoti.Notify(TitleEntry.Text, TitleEntry.Text, DescriptionEntry.Text);

                    //var response = await pushNoti.Notify("Push from Mobile", "Push from Mobile", "This is a push from Mobile");

                    //await DisplayAlert("Notification", response, "Okay");

                    OBOEntry.Text = String.Empty;
                    TitleEntry.Text = String.Empty;
                    DescriptionEntry.Text = String.Empty;
                }
            }
        }
    }
}
