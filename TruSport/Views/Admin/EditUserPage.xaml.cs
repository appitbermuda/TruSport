using System;
using System.Collections.Generic;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class EditUserPage : ContentPage
    {
        UserListView user;
        
        UserPageViewModel userPageViewModel;

        string userTypeID;

        public EditUserPage()
        {
            userPageViewModel = new UserPageViewModel(Navigation);

            this.BindingContext = userPageViewModel;
            InitializeComponent();
        }

        public EditUserPage(User thisUser)
        {
            userPageViewModel = new UserPageViewModel(Navigation, thisUser);
            this.BindingContext = userPageViewModel;

            //user = new UserListView();
            //user = thisUser;


            InitializeComponent();
        }

        //void OpenPicker(object sender, System.EventArgs e)
        //{
        //    UserTypePicker.IsOpen = true;
        //}

        //async void SaveClicked(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        User updateUser = new User
        //        {
        //            ID = user.ID,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            Email = user.Email,
        //            //Password = user.Password,
        //            UserTypeID = userTypeID
        //        };

        //        //await databaseManager.SaveUser(updateUser);

        //        await DisplayAlert("Success", "User saved successfully.", "Okay");

        //        await Navigation.PopAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Error", "There was an issue saving the user, please try again.", "Okay");
        //    }
        //}

        //void UserTypeSelected(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    var userTypeValue =  UserTypePicker.SelectedItem as String;
        //    UserTypeLabel.Text = userTypeValue;

        //    userTypeID = userPageViewModel.UserTypeCollection.Where(x => x.Name == userTypeValue).FirstOrDefault().ID;
        //}
    }
}
