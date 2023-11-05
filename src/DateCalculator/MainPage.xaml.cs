using System.Globalization;

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

            YearEntry.IsVisible = false;
            MonthEntry.IsVisible = false;
            DayEntry.IsVisible = false;
            DateDiffDatePicker.IsVisible = true;
        }

        private void OnCalculateBtnClicked(object sender, EventArgs e)
        {
            string result;

            if (isCalculateDays)
            {
                result = CalculateDays();
            } 
            else
            {
                result = CalculateDate();
            }

            ResultLabel.Text = result;
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
            defaultADBtnBackgroundColor = ADBtn.BackgroundColor;
            defaultTWBtnBackgroundColor = TWBtn.BackgroundColor;
            defaultFrontBtnBackgroundColor = FrontBtn.BackgroundColor;
            defaultBackBtnBackgroundColor = BackBtn.BackgroundColor;
            defaultDiffBtnBackgroundColor = DiffBtn.BackgroundColor;
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

            if (!string.IsNullOrEmpty(year))
            {
                now = now.AddYears(isFront ? -Convert.ToInt16(year) : Convert.ToInt16(year));
            }

            if (!string.IsNullOrEmpty(month))
            {
                now = now.AddMonths(isFront ? -Convert.ToInt16(month) : Convert.ToInt16(month));
            }

            if (!string.IsNullOrEmpty(day))
            {
                now = now.AddDays(isFront ? -Convert.ToInt16(day) : Convert.ToInt16(day));
            }

            if (isTwYear)
            {
                var calendar = new TaiwanCalendar();

                return $"{calendar.GetYear(now)} 年 {now:MM} 月 {now:dd} 日";
            }

            return $"{now:yyyy 年 MM 月 dd 日}";
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
            ADBtn.BackgroundColor = defaultADBtnBackgroundColor;
            TWBtn.BackgroundColor = defaultTWBtnBackgroundColor;
            FrontBtn.BackgroundColor = defaultFrontBtnBackgroundColor;
            BackBtn.BackgroundColor = defaultBackBtnBackgroundColor;
            DiffBtn.BackgroundColor = defaultDiffBtnBackgroundColor;
            YearEntry.Text = null;
            MonthEntry.Text = null;
            DayEntry.Text = null;
            ResultLabel.Text = null;
        }
    }
}