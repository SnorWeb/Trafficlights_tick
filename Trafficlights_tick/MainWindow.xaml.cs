using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Trafficlights_tick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //TIMERS
        DispatcherTimer timerlights = new DispatcherTimer();
        DispatcherTimer timerCar1 = new DispatcherTimer();
        DispatcherTimer timerCar2 = new DispatcherTimer();
        DispatcherTimer timerCar3 = new DispatcherTimer();
        DispatcherTimer timerCar4 = new DispatcherTimer();

        //DEFINE TIMER INTEGER
        int timeTrafficLights = 0;

        //CAR START POSITION 
        double positionCar1 = 0;
        double positionCar2 = 0;
        double positionCar3 = 0;
        double positionCar4 = 0;

        //CHECK IF ITS RED
        bool boolredLightVertical = false;
        bool boolredLightHorizontal = true;

        //DEFINE BRUSHCOLOR
        SolidColorBrush lightGreen = new SolidColorBrush(Colors.LightGreen);
        SolidColorBrush darkGreen = new SolidColorBrush(Colors.DarkGreen);
        SolidColorBrush orange = new SolidColorBrush(Colors.Orange);
        SolidColorBrush darkGoldenrod = new SolidColorBrush(Colors.DarkGoldenrod);
        SolidColorBrush red = new SolidColorBrush(Colors.Red);
        SolidColorBrush darkRed = new SolidColorBrush(Colors.DarkRed);

        

        //MEDIA COLORS
        private HashSet<System.Windows.Media.Color> usedColors = new HashSet<System.Windows.Media.Color>();

        //DEFINE CAR COLORS
        System.Windows.Media.Color colorCarOne = Colors.Transparent;
        System.Windows.Media.Color colorCarTwo = Colors.Transparent;
        System.Windows.Media.Color colorCarThree = Colors.Transparent;
        System.Windows.Media.Color colorCarFour = Colors.Transparent;



        public MainWindow()
        {
            InitializeComponent();

            //SETUPTIMERS
            //TRAFICLIGHTS
            timerlights.Tick += timerChangeTrafficLights;
            timerlights.Interval = TimeSpan.FromSeconds(1);
            timerlights.Start();
            //DEFINE TIMER CAR ONE
            timerCar1.Tick += timerCarOneDrive;
            timerCar1.Interval = TimeSpan.FromMilliseconds(5);
            timerCar1.Start();
            //DEFINE TIMER CAR TWO
            timerCar2.Tick += timerCarTwoDrive;
            timerCar2.Interval = TimeSpan.FromMilliseconds(5);
            timerCar2.Start();
            //DEFINE TIMER CAR Three
            timerCar3.Tick += timerCarThreeDrive;
            timerCar3.Interval = TimeSpan.FromMilliseconds(5);
            timerCar3.Start();
            //DEFINE TIMER CAR FOUR
            timerCar4.Tick += timerCarFourDrive;
            timerCar4.Interval = TimeSpan.FromMilliseconds(5);
            timerCar4.Start();

            //ADDING COLORS TO COMBOBOX
            comCarOneColor.ItemsSource = typeof(System.Windows.Media.Colors).GetProperties(BindingFlags.Static | BindingFlags.Public).Where(prop => prop.PropertyType == typeof(System.Windows.Media.Color)).Select(prop => prop.Name).ToList();
            comCarTwoColor.ItemsSource = typeof(System.Windows.Media.Colors).GetProperties(BindingFlags.Static | BindingFlags.Public).Where(prop => prop.PropertyType == typeof(System.Windows.Media.Color)).Select(prop => prop.Name).ToList();
            comCarThreeColor.ItemsSource = typeof(System.Windows.Media.Colors).GetProperties(BindingFlags.Static | BindingFlags.Public).Where(prop => prop.PropertyType == typeof(System.Windows.Media.Color)).Select(prop => prop.Name).ToList();
            comCarFourColor.ItemsSource = typeof(System.Windows.Media.Colors).GetProperties(BindingFlags.Static | BindingFlags.Public).Where(prop => prop.PropertyType == typeof(System.Windows.Media.Color)).Select(prop => prop.Name).ToList();

            // CHANGE COLOR
            comCarOneColor.SelectionChanged += ColorComboBox_SelectionChanged;
            comCarTwoColor.SelectionChanged += ColorComboBox_SelectionChanged;
            comCarThreeColor.SelectionChanged += ColorComboBox_SelectionChanged;
            comCarFourColor.SelectionChanged += ColorComboBox_SelectionChanged;

            //ADD CURRENT CAR COLORS
            colorCarOne = ((SolidColorBrush)CarOne.Fill).Color;
            colorCarOne = ((SolidColorBrush)CarTwo.Fill).Color;
            colorCarOne = ((SolidColorBrush)CarThree.Fill).Color;
            colorCarOne = ((SolidColorBrush)CarFour.Fill).Color;

        }


        //ON WINDOW LOAD
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //GETTING HEIGHTS
            double canvasHeight = canDownUP.ActualHeight;
            double canvasWidth = canLeftRight.ActualWidth;

            //SETTING START POSTITION
            positionCar1 = 0 + CarOne.Height;
            positionCar2 = canvasHeight - (CarTwo.Height * 2);
            positionCar3 = 0 + CarThree.Width;
            positionCar4 = canvasWidth - (CarFour.Width * 2);

        }

        //TIMER CAR ONE AND DRIVE
        private void timerCarOneDrive(object sender, EventArgs e)
        {
            if (positionCar1 >= canDownUP.ActualHeight)
            {
                positionCar1 = 0 - CarOne.ActualHeight;
            }
            if (boolredLightVertical == true && positionCar1 == Math.Round(grdNorthStreet.ActualHeight - 50))
            {
                positionCar1 += 0;
            }
            else
            {
                positionCar1 += 1;
                Canvas.SetTop(CarOne, positionCar1);
            }
            
        }
        //TIMER CAR TWO AND DRIVE
        private void timerCarTwoDrive(object sender, EventArgs e)
        {
            if (positionCar2 <= 0 - CarTwo.ActualHeight)
            {
                positionCar2 = canDownUP.ActualHeight + CarTwo.ActualHeight;
            }
            if (boolredLightVertical == true && positionCar2 == Math.Round(grdSouthStreet.ActualHeight + 90))
            {
                positionCar2 += 0;
            }
            else
            {
                positionCar2 -= 1;
                Canvas.SetTop(CarTwo, positionCar2);
            }
        }
        //TIMER CAR THREE AND DRIVE
        private void timerCarThreeDrive(object sender, EventArgs e)
        {
            if (positionCar3 >= canLeftRight.ActualWidth + CarThree.Width )
            {
                positionCar3 = 0 - CarThree.ActualWidth;
            }
            if (boolredLightHorizontal == true && positionCar3 == Math.Round(grdEastStreet.ActualWidth - 50))
            {
                positionCar3 += 0;
            }
            else
            {
                positionCar3 += 1;
                Canvas.SetLeft(CarThree, positionCar3);
            }
        }
        //TIMER CAR FOUR AND DRIVE
        private void timerCarFourDrive(object sender, EventArgs e)
        {
            if (positionCar4 <= 0 - CarFour.ActualWidth)
            {
                positionCar4 = canLeftRight.ActualWidth + CarFour.ActualWidth;
            }
            if (boolredLightHorizontal == true && positionCar4 == Math.Round(grdStreetWest.ActualWidth + 90))
            {
                positionCar4 += 0;
            }
            else
            {
                positionCar4 -= 1;
                Canvas.SetLeft(CarFour, positionCar4);
            }
        }


        //TRAFICLIGHTSTIMER AND CHANGE BOOL
        public void timerChangeTrafficLights(object sender, EventArgs e)
        {

            timeTrafficLights++;
            if (timeTrafficLights >= 0 && timeTrafficLights < 10)
            {
                TimerLightsEvent1();
                boolredLightVertical = false;
                boolredLightHorizontal = true;
            }
            else if (timeTrafficLights >= 10 && timeTrafficLights < 15)
            {
                TimerLightsEvent2();
                boolredLightVertical = true;
                boolredLightHorizontal = true;
            }
            else if (timeTrafficLights >= 15 && timeTrafficLights < 25)
            {
                TimerLightsEvent3();
                boolredLightVertical = true;
                boolredLightHorizontal = false;
            }
            else if (timeTrafficLights >= 25 && timeTrafficLights < 30)
            {
                TimerLightsEvent4();
                boolredLightVertical = true;
                boolredLightHorizontal = true;
            }
            else if (timeTrafficLights >= 30)
            {
                timeTrafficLights = 0;
            }
        }
        //TIMER CASE1
        private void TimerLightsEvent1()
        {
            // Case 1 
            GreenLightN.Fill = lightGreen;
            GreenLightS.Fill = lightGreen;
            OrangeLightE.Fill = darkGoldenrod;
            OrangeLightW.Fill = darkGoldenrod;
            RedLightN.Fill = darkRed;
            RedLightE.Fill = red;
            RedLightS.Fill = darkRed;
            RedLightW.Fill = red;
        }
        //TIMER CASE2
        private void TimerLightsEvent2()
        {
            // Case 2
            GreenLightN.Fill = darkGreen;
            GreenLightS.Fill = darkGreen;
            OrangeLightN.Fill = orange;
            OrangeLightS.Fill = orange;
        }
        //TIMER CASE3
        private void TimerLightsEvent3()
        {
            // Case 3
            GreenLightE.Fill = lightGreen;
            GreenLightW.Fill = lightGreen;
            OrangeLightN.Fill = darkGoldenrod; ;
            OrangeLightS.Fill = darkGoldenrod;
            RedLightN.Fill = red;
            RedLightE.Fill = darkRed;
            RedLightS.Fill = red;
            RedLightW.Fill = darkRed;
        }
        //TIMER CASE4
        private void TimerLightsEvent4()
        {
            // Case 4
            GreenLightE.Fill = darkGreen;
            GreenLightW.Fill = darkGreen;
            OrangeLightE.Fill = orange;
            OrangeLightW.Fill = orange;
        }


        //SLIDER SPEED CAR ONE
        private void SliderCarOne_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timerCar1.Interval = TimeSpan.FromMilliseconds(slSliderCarOne.Value);
        }
        //SLIDER SPEED CAR TWO
        private void SliderCarTwo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timerCar2.Interval = TimeSpan.FromMilliseconds(slSliderCarTwo.Value);
        }
        //SLIDER SPEED CAR ONE
        private void SliderCarThree_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timerCar3.Interval = TimeSpan.FromMilliseconds(slSliderCarThree.Value);
        }
        //SLIDER SPEED CAR ONE
        private void SliderCarFour_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timerCar4.Interval = TimeSpan.FromMilliseconds(slSliderCarFour.Value);
        }

        

        //CHANGE COLOR FUNCTION
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DECLINING VARIABLE
            var comboBox = sender as ComboBox;
            var selectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(comboBox.SelectedItem.ToString());

            //CONTROL IF COLOR IS IN USE AND SEND ALERT
            if (usedColors.Contains(selectedColor) || colorCarOne == selectedColor || colorCarTwo == selectedColor || colorCarThree == selectedColor || colorCarFour == selectedColor)
            {
                MessageBox.Show("This color has already been chosen.");
                return;
            }

            //PREVENT TO USE THE COLOR
            usedColors.Add(selectedColor);

            //SWITCHING THE  SELECTED COLOR FOR EVERY CAR
            switch (comboBox.Name)
            {
                case "comCarOneColor":
                    CarOne.Fill = new SolidColorBrush(selectedColor);
                    break;
                case "comCarTwoColor":
                    CarTwo.Fill = new SolidColorBrush(selectedColor);
                    break;
                case "comCarThreeColor":
                    CarThree.Fill = new SolidColorBrush(selectedColor);
                    break;
                case "comCarFourColor":
                    CarFour.Fill = new SolidColorBrush(selectedColor);
                    break;
            }
        }



        //DE DATABASE NIET KUNNEN AFWERKEN
        ////SAVE TO DATABASE
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
        //    string CNstr = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Examen2022_2023;Data Source=LAPTOP-NIELS\SQLEXPRESS";
        //    SqlConnection CN = new SqlConnection(CNstr);

        //    using (SqlCommand insertValues = new SqlCommand("INSERT into Examen2022_2023(CarOneName, CarOneSpeed, CarOneColor, CarTwoName, CarTwoSpeed, CarTwoColor, CarThreeName, CarThreeSpeed, CarThreeColor, CarFourName, CarFourSpeed, CarFourColor, UserName, DateTimeModified, ComputerName) Insert (@Name1,"))
        //    {
        //        insertValues.Parameters.AddWithValue("@Name1", CarOne.Name);
        //        insertValues.Parameters.AddWithValue("@value1", CarOne.Name);

        //    }

        }
    }
}
