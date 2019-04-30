﻿using AgencyApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgencyApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAgencyUsersPage : ContentPage
	{
        User user;
        public AddAgencyUsersPage ()
		{
			InitializeComponent ();
            user = new User();
            stackLayoutContainer.BindingContext = user;
            
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var agencies = await Agency.Read();

            if (agencies.Count > 0)
            {
                pickerAgencyName.ItemsSource = agencies;
                pickerAgencyName.ItemDisplayBinding = new Binding("Name");
            }
        }

        private string id;
        private string name;
        private void PickerAgencyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var agencies = pickerAgencyName.SelectedItem as Agency;
            id = agencies.Id;
            name = agencies.Name;
        }
        
        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            bool isNotSelected = pickerAgencyName.SelectedIndex < 0;
            bool isEmailEmpty = string.IsNullOrEmpty(entryEmail.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(entryPassword.Text);
            bool isConfPasswordEmpty = string.IsNullOrEmpty(entryConfirmPassword.Text);

            if (isNotSelected || isEmailEmpty || isPasswordEmpty || isConfPasswordEmpty)
            {
                DisplayAlert("Error", "All fields are required", "Ok");
            }
            else
            {
                if (entryPassword.Text != entryConfirmPassword.Text)
                {
                    DisplayAlert("Error", "Passwords don't match", "Ok");
                }
                else
                {
                    user.UserType = "Agency";
                    user.Agency = id;
                    Register(user);
                    DisplayAlert("Successful", "Added user to " + name, "Ok");
                }
            }
        }

        private static async void Register(User user)
        {
            await App.MobileService.GetTable<User>().InsertAsync(user);
        }
    }
}