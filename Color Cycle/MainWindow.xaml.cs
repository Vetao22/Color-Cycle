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
using System.Windows.Threading;

namespace Color_Cycle
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        SolidColorBrush initialColor, changedColor;
        Random rand;
        bool isStopped = true;
        int increment;
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;

            rand = new Random();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int r, g, b, maxValue, minValue, incrementValue;
                     
            r = changedColor.Color.R;
            g = changedColor.Color.G;
            b = changedColor.Color.B;

            incrementValue = (bool)chkRandom.IsChecked ? rand.Next(Math.Min(0, increment), Math.Max(0, increment)) : increment;            
            r += incrementValue;
            incrementValue = (bool)chkRandom.IsChecked ? rand.Next(Math.Min(0, increment), Math.Max(0, increment)) : increment;
            g += incrementValue;
            incrementValue = (bool)chkRandom.IsChecked ? rand.Next(Math.Min(0, increment), Math.Max(0, increment)) : increment;
            b += incrementValue;

            r = r > 255 ? 255 : r;
            g = g > 255 ? 255 : g;
            b = b > 255 ? 255 : b;

            r = r < 0 ? 0 : r;
            g = g < 0 ? 0 : g;
            b = b < 0 ? 0 : b;
            
            maxValue = Math.Max(r, Math.Max(g, b));
            minValue = Math.Min(r, Math.Min(g, b));

            if (maxValue >= 255)
            {
                increment *= -1;
            }
            else if (minValue <= 0)
            {
                increment *= -1;
            }

            SliderR.Value = r;
            SliderG.Value = g;
            SliderB.Value = b;
            
            changedColor = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            rectMain.Fill = changedColor;
        }

        string KeepOnlyNumbers(string input)
        {
            string result = "";

            foreach (Char c in input)
            {
                if (Char.IsNumber(c))
                {
                    result += c;
                }               
            }
            result = String.IsNullOrEmpty(result) ? "0" : result;

            return result;
        }

        private void TextBox_RGB_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            string text = KeepOnlyNumbers(txtBox.Text);
            double value = Convert.ToDouble(text);
            value = Math.Min(Math.Max(0, value), 255);
            text = value.ToString();

            txtBox.Text = text;
            txtBox.CaretIndex = text.Length;

            if (isStopped)
            {
                Color c = Color.FromRgb((Byte)SliderR.Value, (Byte)SliderG.Value, (Byte)SliderB.Value);
                initialColor = new SolidColorBrush(c);
            }

            rectInitialColor.Fill = initialColor;
           
         }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            isStopped = !isStopped;

            if(isStopped)
            {
                btnStart.Content = "Start";

                timer.Stop();
            }
            else
            {
                btnStart.Content = "Stop";

                string intervalText = txtInterval.Text;
                string incrementText = txtIncrement.Text;

                if(!String.IsNullOrEmpty(intervalText) && !String.IsNullOrEmpty(incrementText))
                {
                    int interval = Convert.ToInt32(intervalText);
                    increment = Convert.ToInt32(incrementText);

                    timer.Interval = TimeSpan.FromMilliseconds(interval);
                    timer.Start();

                    Color c = Color.FromRgb((Byte)SliderR.Value, (Byte)SliderG.Value, (Byte)SliderB.Value);
                    initialColor = new SolidColorBrush(c);
                    changedColor = initialColor;

                    rectMain.Fill = initialColor;
                }
            }
        }

        private void txtIncrement_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            string text = KeepOnlyNumbers(txtBox.Text);

            if (!String.IsNullOrEmpty(text))
            {
                int value = Convert.ToInt32(text);
                value = Math.Max(0, value);
                txtBox.Text = value.ToString();
            }
        }

        private void txtInterval_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            string text = KeepOnlyNumbers(txtBox.Text);
           
            if (!String.IsNullOrEmpty(text))
            {
                int value = Convert.ToInt32(text);
                value = Math.Max(0, value);
                text = value.ToString();

                txtBox.Text = text;
                txtBox.CaretIndex = text.Length;
            }

        }
    }
}
