﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using bullyPoop2.Services;
using bullyPoop2.Views;

namespace bullyPoop2
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
