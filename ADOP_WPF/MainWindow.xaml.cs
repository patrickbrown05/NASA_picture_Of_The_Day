using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace ADOP_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //init file name strings, used to preserve UI values between sessions
        const string SettingToday = "DateToday";
        const string SettingShowOnStartUp = "show on start up";
        const string SettingImageCountToday = "images today";
        const string SettingLimitRange = "limit range";

        //full path to the init file
        string initFilePath;

        //a char used to divide the name from the value in the init file
        const char SettingDivider = ':';

        // The objective of the NASA API portal is to make NASA data, including imagery, 
        //eminently accessible to application developers. 
        const string EndpointURL = "https://api.nasa.gov/planetary/apod";

        // June 16, 1995: the APOD launch date.
        DateTime LaunchDate = new DateTime(1995, 6, 16);

        //a count of images downloaded today
        int ImageCountToday;

        //flag to ignore a strange double event in WPF
        bool IgnoreDoubleEvent = false;
        public MainWindow()
        {
            InitializeComponent();

            //store the init file in the same folder as the application
            initFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "init_apod.txt");

            // Set the maximum date to today and the minimum date to the APOD launch date.
            MonthCalender.DisplayDateEnd = DateTime.Today;
            MonthCalender.DisplayDateStart = LaunchDate;

            readinitfile();
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            // Make sure the full range of dates is available.
            // This might invoke a call to LimitRangeCheckBox_CheckedChanged.
            LimitRangeCheckBox.IsChecked = false;

            // This will not load up the image, just sets the calendar to the APOD launch date.
            // This will fire a double event, the first of which needs to be ignored.
            IgnoreDoubleEvent = true;
            MonthCalender.SelectedDate = LaunchDate;
        }

        private void LimitRangeCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (LimitRangeCheckBox.IsChecked == true)
            {
                //set the minimum date of the calender to beginning of the year
                var thisYear = new DateTime(DateTime.Today.Year, 1, 1);
                MonthCalender.DisplayDateStart = thisYear;
            } else 
            {
                //set the minimum date of the calender to the year of the launch of APOD
                MonthCalender.DisplayDateStart = LaunchDate;
            }
        }

        private async void MonthCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //retrive an image, unless a double event has occured, in which case ignore the first event.
            if (!IgnoreDoubleEvent)
            {
                await RetrievePhoto();
            } else
            {
                IgnoreDoubleEvent = false;
            }
        }

        private bool IsSupportedFormat(string photoURL)
        {
            string ext = System.IO.Path.GetExtension(photoURL).ToLower();

            return (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".bmp" ||
                    ext == ".wmf" || ext == ".tif" || ext == ".ico");
        }

        private async Task RetrievePhoto()
        {
            var client = new HttpClient();
            JObject jresult = null;
            string responseContent = null;
            string description = null;
            string photoURL = null;
            string copyright = null;

            //set the UI elements to default 
            ImageCopyrightTextbox.Text = "NASA";
            DescriptionTextBox.Text = "";

            // Build the date parameter string for the date selected, or the last date 
            //if a range is specified.
            DateTime dt = (DateTime)MonthCalender.SelectedDate;

            string dateSelected = $"{dt.Year.ToString()}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}";
            string urlParameters = $"?date={dateSelected}&api_key=DEMO_KEY";

            //populate the http client appropriately
            client.BaseAddress = new Uri(EndpointURL);
            client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));

            //the critical call: send GET request with appropriate headers
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                //be ready to catch any other data from the server
                try
                {
                    //parse the content using newton soft api
                    responseContent = await response.Content.ReadAsStringAsync();

                    //parse the response object for the details we need
                    jresult = JObject.Parse(responseContent);

                    //now get the image
                    photoURL = (string)jresult["url"];
                    var photoURI = new Uri(photoURL);
                    var BMI = new BitmapImage(photoURI);

                    ImagePictureBox.Source = BMI;

                    if (IsSupportedFormat(photoURL))
                    {
                        //get the photo copyright information, but fill with "NASA" if no name is provided
                        copyright = (string)jresult["copyright"];
                        if (copyright != null && copyright.Length > 0)
                        {
                            ImageCopyrightTextbox.Text = copyright;
                        }
                        description = (string)jresult["description"];
                        DescriptionTextBox.Text = description;
                    }
                    else
                    {
                        DescriptionTextBox.Text = $"image type not supported, URL is {photoURL}";
                    }
                }
                catch (Exception ex)
                {
                    DescriptionTextBox.Text = $"image data not supported, URL is {ex.Message}";
                }

                ++ImageCountToday;
                ImagesTodayTextBox.Text = ImageCountToday.ToString();
            }
            else
            {
                DescriptionTextBox.Text = "We were unable to Retreive the NASA image for the day:" +
                $"{response.StatusCode.ToString()} { response.ReasonPhrase}";
            }
        }

        private void APOD_WPF_Close(object sender, EventArgs e)
        {
            writeinitfile();
        }

        private void writeinitfile()
        {
            using (var sw = new StreamWriter(initFilePath))
            {
                //write out todays date, to keep track of downloads per day
                sw.WriteLine(SettingToday + SettingDivider + DateTime.Today.ToShortDateString());

                //write out the number of images we have downloaded today
                sw.WriteLine(SettingImageCountToday + SettingDivider + ImageCountToday.ToString());

                //write out the UI settings we want to preserve for next time
                sw.WriteLine(SettingShowOnStartUp + SettingDivider + ShowTodaysImageCheckbox.IsChecked.ToString());
                sw.WriteLine(SettingLimitRange + SettingDivider + LimitRangeCheckBox.IsChecked.ToString());
                               
            }
        }
        private void readinitfile()
        {
            //check that the init file exists
            if (File.Exists(initFilePath))
            {
                string line = null;
                string[] part;
                bool isToday = false;

                using (var sr = new StreamReader(initFilePath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        //split the line into the part before the colo (the name), and the part after (the value).
                        part = line.Split(SettingDivider);

                        //switch on the "name" part, and then process the "value" part.
                        switch (part[0])
                        {
                            //read the date and, if it's not today's date, read the number of images already downloaded.
                            //if it's not today's date set the number of downloads back to zero.
                            case SettingToday:
                                var dt = DateTime.Parse(part[1]);
                                if (dt.Equals(DateTime.Today))
                                {
                                    isToday = true;
                                }
                                break;

                            case SettingImageCountToday:

                                //if the last time the app was used was earlier today, the
                                //image count stored is valid against the 50-per-day maximum.
                                if (isToday)
                                {
                                    ImageCountToday = int.Parse(part[1]);
                                } else
                                {
                                    ImageCountToday = 0;
                                }
                                break;

                            case SettingShowOnStartUp:
                                ShowTodaysImageCheckbox.IsChecked = bool.Parse(part[1]);
                                break;

                            case SettingLimitRange:

                                // This statement might invoke a call to LimitRangeCheckBox_CheckedChanged.
                                LimitRangeCheckBox.IsChecked = bool.Parse(part[1]);
                                break;


                            default:
                                throw new Exception("Unknown init file entry: " + line);
                        }
                    }
                }
            }
            else
            {
                //no init file exists yet so set defaults.
                ImageCountToday = 0;
                ShowTodaysImageCheckbox.IsChecked = true;
                LimitRangeCheckBox.IsChecked = false;
            }

            ImagesTodayTextBox.Text = ImageCountToday.ToString();

            //make a call to retrieve a picture on startup, if required by the setting
            if (ShowTodaysImageCheckbox.IsChecked == true)
            {
                //note that, in WPF, this fire a double event the first of which should be ignored.\
                IgnoreDoubleEvent = true;
                MonthCalender.SelectedDate = DateTime.Today;
            }
        }
    }
}
