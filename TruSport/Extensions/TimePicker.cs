using System;
using System.Collections.ObjectModel;
using Syncfusion.SfPicker.XForms;
using Xamarin.Forms;

namespace TruSport.Extensions
{
    public class TimePicker : SfPicker
    {
        #region Public Properties

        /// <summary>
        /// Date is the acutal DataSource for SfPicker control which will holds the collection of Day ,Month and Year
        /// </summary>
        /// <value>The time.</value>
        public ObservableCollection<object> Time { get; set; }

        //Hour is the collection of Hours in Railway time format
        public ObservableCollection<object> Hour { get; set; }

        //Minute is the collection of Minutes from 00 to 59
        public ObservableCollection<object> Minute { get; set; }

        //public ObservableCollection<object> Format;


        /// <summary>
        /// Headers api is holds the column name for every column in date picker
        /// </summary>
        /// <value>The Headers.</value>
        public ObservableCollection<string> Headers { get; set; }

        #endregion

        public TimePicker()
        {
            Time = new ObservableCollection<object>();
            Hour = new ObservableCollection<object>();
            Minute = new ObservableCollection<object>();
            //Format = new ObservableCollection<object>();
            Headers = new ObservableCollection<string>();

            if (Device.RuntimePlatform == Device.Android)
            {
                Headers.Add("HOUR");
                Headers.Add("MINUTE");
                Headers.Add("FORMAT");
            }
            else
            {
                Headers.Add("Hour");
                Headers.Add("Minute");
                Headers.Add("Format");
            }

            HeaderText = "Time Picker";

            PopulateTimeCollection();

            this.ColumnHeaderText = Headers;
            ShowFooter = true;
            ShowHeader = true;
            ShowColumnHeader = true;
            

            this.ItemsSource = Time;
        }

        private void PopulateTimeCollection()
        {

            //Populate Hour
            for (int i = 0; i <= 24; i++)
            {
                if (i < 10)
                {
                    Hour.Add("0" + i.ToString());
                }
                else
                    Hour.Add(i.ToString());

            }

            //Populate Minute

            for (int j = 0; j < 60; j++)
            {

                if (j < 10)
                {
                    Minute.Add("0" + j);
                }
                else
                    Minute.Add(j.ToString());
            }

            //Populate Format

            //Format.Add("AM");
            //Format.Add("PM");
            Time.Add(Hour);
            Time.Add(Minute);
            //Time.Add(Format);

        }
    }
}
