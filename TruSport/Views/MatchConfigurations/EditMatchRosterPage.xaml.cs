using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using Syncfusion.XForms.Buttons;
using TruSport.Model;
using TruSport.ViewModel;
using Xamarin.Forms;

namespace TruSport.Views.MatchConfigurations
{
    public partial class EditMatchRosterPage : ContentPage
    {
        MatchRosterViewModel matchRosterViewModel;

        public EditMatchRosterPage()
        {
            InitializeComponent();
        }

        public EditMatchRosterPage(string FixtureID, string TeamID, string TeamName)
        {
            matchRosterViewModel = new MatchRosterViewModel(Navigation, FixtureID, TeamID, TeamName);

            this.BindingContext = matchRosterViewModel;

            InitializeComponent();


            RosterItemListView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "IsStarter",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Player);
                    return item.IsStarter;
                }
            });
        }

        public EditMatchRosterPage(Fixture fixture, string teamID)
        {
            matchRosterViewModel = new MatchRosterViewModel(Navigation, fixture, teamID);

            this.BindingContext = matchRosterViewModel;

            InitializeComponent();


            RosterItemListView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "IsStarter",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Player);
                    return item.IsStarter;
                }
            });
        }

        public void StarterChanged(object sender, ToggledEventArgs e)
        { //Get ConfigurationItem (listview item) //save switch value Switch toggledSwitch = (Switch)sender; }
            var item = (SfSwitch)sender;

            var SelectedParent = (ListView)item.BindingContext;

            var isStarter = e.Value;
        }
    }
}
