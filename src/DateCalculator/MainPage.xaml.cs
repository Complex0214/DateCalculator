using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Windows.Devices.HumanInterfaceDevice;

namespace DateCalculator
{
    public partial class MainPage : ContentPage
    {
        private bool isTwYear;
        private bool isCalculateDays;
        private bool isFront;
        private Color defaultADBtnBackgroundColor;
        private Color defaultTWBtnBackgroundColor;
        private Color defaultFrontBtnBackgroundColor;
        private Color defaultBackBtnBackgroundColor;
        private Color defaultDiffBtnBackgroundColor;
        private Color toggledBackgroundColor;

        public MainPage()
        {
            InitializeComponent();
            SetDefault();

            if (Application.Current != null) 
                Application.Current.UserAppTheme = Application.Current.RequestedTheme;

            if (Application.Current.RequestedTheme == AppTheme.Light)
            {
                DarkModeSwitch.IsToggled = false;
            }
            else
            {
                DarkModeSwitch.IsToggled = true;
            }
        }

        private void OnADBtnClicked(object sender, EventArgs e)
        {
            isTwYear = false;
            
            ADBtn.BackgroundColor = toggledBackgroundColor;
            TWBtn.BackgroundColor = defaultTWBtnBackgroundColor;
        }

        private void OnTWBtnClicked(object sender, EventArgs e)
        {
            isTwYear = true;
            
            ADBtn.BackgroundColor = defaultADBtnBackgroundColor;
            TWBtn.BackgroundColor = toggledBackgroundColor;
        }

        private void OnFrontBtnClicked(object sender, EventArgs e)
        {
            isCalculateDays = false;
            isFront = true;

            FrontBtn.BackgroundColor = toggledBackgroundColor;
            BackBtn.BackgroundColor = defaultBackBtnBackgroundColor;
            DiffBtn.BackgroundColor = defaultDiffBtnBackgroundColor;

            YearEntry.IsVisible = true;
            MonthEntry.IsVisible = true;
            DayEntry.IsVisible = true;
            DateDiffDatePicker.IsVisible = false;
        }

        private void OnBackBtnClicked(object sender, EventArgs e)
        {
            isCalculateDays = false;
            isFront = false;

            FrontBtn.BackgroundColor = defaultFrontBtnBackgroundColor;
            BackBtn.BackgroundColor = toggledBackgroundColor;
            DiffBtn.BackgroundColor = defaultDiffBtnBackgroundColor;

            YearEntry.IsVisible = true;
            MonthEntry.IsVisible = true;
            DayEntry.IsVisible = true;
            DateDiffDatePicker.IsVisible = false;
        }

        private void OnDiffBtnClicked(object sender, EventArgs e)
        {
            isCalculateDays = true;

            FrontBtn.BackgroundColor = defaultFrontBtnBackgroundColor;
            BackBtn.BackgroundColor = defaultBackBtnBackgroundColor;
            DiffBtn.BackgroundColor = toggledBackgroundColor;

            YearEntry.Text = null;
            MonthEntry.Text = null;
            DayEntry.Text = null;

            YearEntry.IsVisible = false;
            MonthEntry.IsVisible = false;
            DayEntry.IsVisible = false;
            DateDiffDatePicker.IsVisible = true;
        }

        private void OnCalculateBtnClicked(object sender, EventArgs e)
        {
            string result = string.Empty;

            if (isCalculateDays)
            {
                result = CalculateDays();
            } 
            else
            {
                if (IsInputField() && IsValidType())
                {
                    result = CalculateDate();
                }             
            }

            ResultLabel.Text = result;
        }

        private bool IsInputField()
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(YearEntry.Text) || !string.IsNullOrEmpty(MonthEntry.Text) || !string.IsNullOrEmpty(DayEntry.Text))
            {
                isValid = true;
            }
            else
            {
                DisplayAlert("錯誤", "年月日至少輸入一項!!", "確定");
            }

            return isValid;           
        }

        private bool IsValidType()
        {
            bool isType = false;

            if (int.TryParse(YearEntry.Text, out _) || int.TryParse(MonthEntry.Text, out _) || int.TryParse(DayEntry.Text, out _))
            {
                isType = true;
            }
            else
            {
                DisplayAlert("錯誤", "年月日只能輸入數字!!", "確定");
            }

            return isType;
        }

        private void OnClearBtnClicked(object sender, EventArgs e)
        {
            Reset();
        }

        private void SetDefault()
        {
            isTwYear = false;
            isCalculateDays = false;
            isFront = false;

            toggledBackgroundColor = Color.FromRgb(255, 82, 82);
        }

        private string CalculateDays()
        {
            var diffTimespan = DateSelectDatePicker.Date - DateDiffDatePicker.Date;

            return $"{Math.Abs(diffTimespan.TotalDays)}";
        }

        private string CalculateDate()
        {
            var now = DateSelectDatePicker.Date;
            var year = YearEntry.Text;
            var month = MonthEntry.Text;
            var day = DayEntry.Text;

            var yearsToAdd = ParseInput(year);
            var monthsToAdd = ParseInput(month);
            var daysToAdd = ParseInput(day);

            now = now.AddYears(isFront ? -yearsToAdd : yearsToAdd);
            now = now.AddMonths(isFront ? -monthsToAdd : monthsToAdd);
            now = now.AddDays(isFront ? -daysToAdd : daysToAdd);

            if (isTwYear)
            {
                var calendar = new TaiwanCalendar();

                return $"{calendar.GetYear(now)} 年 {now:MM} 月 {now:dd} 日";
            }

            return $"{now:yyyy 年 MM 月 dd 日}";
        }

        private int ParseInput(string input)
        {
            return string.IsNullOrEmpty(input) ? 0 : Convert.ToInt32(input);
        }

        private void Reset()
        {
            UIReset();

            isTwYear = false;
            isCalculateDays = false;
            isFront = false;
        }

        private void UIReset()
        {
            DateSelectDatePicker.Date = DateTime.Now;
            DateDiffDatePicker.Date = DateTime.Now;

            if(Application.Current.UserAppTheme == AppTheme.Light)
                LightMode();
            else
                DarkMode();

            YearEntry.Text = null;
            MonthEntry.Text = null;
            DayEntry.Text = null;
            ResultLabel.Text = null;

            YearEntry.IsVisible = true;
            MonthEntry.IsVisible = true;
            DayEntry.IsVisible = true;
            DateDiffDatePicker.IsVisible = false;
        }
        
        private void OnDarkModeToggled(object sender, ToggledEventArgs e)
        {
            if (Application.Current != null)
                Application.Current.UserAppTheme = Application.Current.RequestedTheme is AppTheme.Light ? AppTheme.Dark : AppTheme.Light;

            if (DarkModeSwitch.IsToggled)
            {
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                Application.Current.UserAppTheme = AppTheme.Light;
            }

            if (Application.Current.UserAppTheme == AppTheme.Light)
            {
                LightMode();
            }
            else
            {
                DarkMode();
            }
        }

        private void LightMode()
        { 
            var hasLightColor = App.Current.Resources.TryGetValue("Primary", out var color);

            if (hasLightColor)
            {
                var primaryColor = (Color)color;
                defaultADBtnBackgroundColor = primaryColor;
                defaultTWBtnBackgroundColor = primaryColor;
                defaultFrontBtnBackgroundColor = primaryColor;
                defaultBackBtnBackgroundColor = primaryColor;
                defaultDiffBtnBackgroundColor = primaryColor;
            }
            
            TWBtn.BackgroundColor = defaultTWBtnBackgroundColor;
            ADBtn.BackgroundColor = defaultADBtnBackgroundColor;
            FrontBtn.BackgroundColor = defaultFrontBtnBackgroundColor;
            BackBtn.BackgroundColor = defaultBackBtnBackgroundColor;
            DiffBtn.BackgroundColor = defaultDiffBtnBackgroundColor;
            DateSelectDatePicker.TextColor = defaultDiffBtnBackgroundColor;
            DateDiffDatePicker.TextColor = defaultDiffBtnBackgroundColor;            
        }

        private void DarkMode()
        { 
            var hasDarkColor = App.Current.Resources.TryGetValue("Secondary", out var color);

            if (hasDarkColor)
            {
                var secondaryColor = (Color)color;
                defaultADBtnBackgroundColor = secondaryColor;
                defaultTWBtnBackgroundColor = secondaryColor;
                defaultFrontBtnBackgroundColor = secondaryColor;
                defaultBackBtnBackgroundColor = secondaryColor;
                defaultDiffBtnBackgroundColor = secondaryColor;
            }

            TWBtn.BackgroundColor = defaultTWBtnBackgroundColor;
            ADBtn.BackgroundColor = defaultADBtnBackgroundColor;
            FrontBtn.BackgroundColor = defaultFrontBtnBackgroundColor;
            BackBtn.BackgroundColor = defaultBackBtnBackgroundColor;
            DiffBtn.BackgroundColor = defaultDiffBtnBackgroundColor;
            DateSelectDatePicker.TextColor = defaultDiffBtnBackgroundColor;
            DateDiffDatePicker.TextColor = defaultDiffBtnBackgroundColor; 
        }
    }
}