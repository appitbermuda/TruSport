using System;
using System.Collections.Generic;
using TruSport.Data;
using TruSport.Model;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.Views.Admin
{
    public partial class EditUserTypePage : ContentPage
    {
        UserType thisUserType;
        public EditUserTypePage()
        {
            this.BindingContext = new UserTypeAdminPageViewModel();
            InitializeComponent();
        }

        public EditUserTypePage(UserType userType)
        {
            thisUserType = userType;
            
            this.BindingContext = new UserTypeAdminPageViewModel(userType);
            InitializeComponent();
        }

        async void SaveClicked(object sender, System.EventArgs e)
        {
            try
            {
                UserType userType = new UserType();
                if (thisUserType.ID == null || thisUserType.ID == "")
                {
                    userType = new UserType
                    {
                        Name = userTypeName.Text,
                    };
                }
                else
                {
                    userType = new UserType
                    {
                        Name = userTypeName.Text,
                        ID = thisUserType.ID,
                    };
                }

                
                await DisplayAlert("Success", "The user type was saved successfully!", "Okay");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "There was an issue saving this user type, please try again.", "Okay");
            }
        }
    }
}
