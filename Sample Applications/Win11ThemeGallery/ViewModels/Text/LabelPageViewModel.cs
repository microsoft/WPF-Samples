﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.ViewModels
{
    public partial class LabelPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "Label";

	[ObservableProperty]
	private string _pageDescription = "";

    }
}
