﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : TabbedPage
    {
        public InfoPage ()
        {
            InitializeComponent();
        }
    }
}