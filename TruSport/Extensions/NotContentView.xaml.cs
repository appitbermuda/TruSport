using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TruSport.Extensions
{
    public partial class NotContentView
    {
        /// <summary>
        /// Gets or sets the ActionTextProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty ActionTextProperty =
            BindableProperty.Create(nameof(ActionText), typeof(string), typeof(NotContentView));

        public NotContentView()
        {
            InitializeComponent();
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the Action Text.
        /// </summary>
        public string ActionText
        {
            get { return (string)GetValue(ActionTextProperty); }
            set { this.SetValue(ActionTextProperty, value); }
        }

        #endregion
    }
}
