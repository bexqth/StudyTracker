using Microsoft.Identity.Client;
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
using TrackerLibrary;
using Task = TrackerLibrary.Task;

namespace Tracker
{
    /// <summary>
    /// Interaction logic for ToDoUserControl.xaml
    /// </summary>
    public partial class ToDoUserControl : UserControl
    {
        public User User { get; set; }
        public SolidColorBrush pickedColor;
        public SolidColorBrush colorRed;
        public SolidColorBrush colorBlue;
        public SolidColorBrush colorGreen;
        public SolidColorBrush colorYellow;
        public SolidColorBrush colorPurple;
        public SolidColorBrush colorOrange;
        public List<Border> colorBorders;
        public List<Border> checkBoxBorders;
        private bool Editing = false;
        private CheckBox EditingCheckBox;
        private Border EditingBorder;
        private string? oldTitle;
        public int ToDoTasksDone { get; set; }
        public ToDoUserControl()
        {
            InitializeComponent();
            this.colorRed = new SolidColorBrush(Color.FromArgb(255, 249, 110, 110));
            this.colorBlue = new SolidColorBrush(Color.FromArgb(255, 91, 202, 249));
            this.colorGreen = new SolidColorBrush(Color.FromArgb(255, 157, 245, 126));
            this.colorYellow = new SolidColorBrush(Color.FromArgb(255, 253, 235, 112));
            this.colorPurple = new SolidColorBrush(Color.FromArgb(255, 92, 150, 245));
            this.colorOrange = new SolidColorBrush(Color.FromArgb(255, 247, 165, 82));
            this.colorBorders = new List<Border>();
            this.checkBoxBorders = new List<Border>();
            colorBorders.Add(BorderBlueColor);
            colorBorders.Add(BorderRedColor);
            colorBorders.Add(BorderGreenColor);
            colorBorders.Add(BorderYellowColor);
            colorBorders.Add(BorderPurpleColor);
            colorBorders.Add(BorderOrangeColor);
        }

        private void BtAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (!Editing) {
                AddNewCheckBox();
            } else {
                EditCheckBox();
            }
        }

        public void AddNewCheckBox() {

            if (TbNewItemTitle.Text != "")
            {
                CheckBox checkBox = new()
                {
                    Content = TbNewItemTitle.Text,
                    Padding = new Thickness(5, -8, 0, 0),
                    Margin = new Thickness(10),
                    FontSize = 22,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 86, 101, 115)),

                };


                Border border = new()
                {
                    Height = 45,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = pickedColor,
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(10, 15, 10, 3),
                    Child = checkBox
                };

                this.checkBoxBorders.Add(border);

                checkBox.Checked += (s, e) =>
                {
                    checkBox.IsChecked = true;
                    SpList.Children.Remove(border);
                    RemoveToDoTask(checkBox.Content.ToString());
                    User.ToDoTasksDone++;
                    
                };

                border.MouseLeftButtonDown += (s, e) =>
                {
                    ChangeFocusCheckBoxBorder(border, checkBox);
                    this.Editing = true;
                    this.EditingBorder = border;
                    this.EditingCheckBox = checkBox;
                    BtAddItem.Content = "Edit";
                    oldTitle = EditingCheckBox.Content.ToString();
                    SetToEdit();
                };

                User.AllToDoTaks++;
                SpList.Children.Add(border);
                AddToDoTask(TbNewItemTitle.Text);
                ResetText();
            }
        }

        public void EditCheckBox() {
            string newTitle = TbNewItemTitle.Text;

            EditingCheckBox.Content = newTitle;
            EditingBorder.Background = pickedColor;

            for (int i = 0; i < User.ToDos.Count; i++)
            {
                if (User.ToDos[i].Title == oldTitle)
                {
                    User.ToDos[i].Title = newTitle;
                    break;
                }
            }

        }

        public void SetToEdit() {
            BtAddItem.Content = "Edit";
            LbPrintAction.Content = "Edit item";
            TbNewItemTitle.Text = EditingCheckBox.Content.ToString();
            for (int i = 0; i < this.colorBorders.Count; i++) {
                if (colorBorders[i].Background.ToString() == EditingBorder.Background.ToString()) {
                    ChangePickedButton(colorBorders[i]);
                    SolidColorBrush c = (SolidColorBrush)colorBorders[i].Background;
                    pickedColor = c;
                }
            }   
        }

        public void SetToAdd() {
            BtAddItem.Content = "Add";
            TbNewItemTitle.Text = "Title";
            LbPrintAction.Content = "Create an item";
            for (int i = 0; i < this.colorBorders.Count; i++)
            {
                colorBorders[i].BorderThickness = new Thickness(0);
                colorBorders[i].BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }

        public void ChangeFocusCheckBoxBorder(Border border, CheckBox checkBox) {
            foreach (var b in checkBoxBorders) {
                if (b == border) {
                    b.BorderThickness = new Thickness(2);
                    b.BorderBrush = new SolidColorBrush(Colors.White);
                    checkBox.Padding = new Thickness(5, -11, 0, 0);
                }
                else {
                    b.BorderThickness = new Thickness(0);
                    checkBox.Padding = new Thickness(5, -8, 0, 0);
                }
            }
        }

        public void AddToDoTask(string title) {
            User.ToDos.Add(new Task(title));
        }

        public void RemoveToDoTask(string? title) {
            foreach (var todo in User.ToDos)
            {
                if (todo.Title == title) {
                    User.ToDos.Remove(todo);
                    break;
                }
            }
        }

        public void ResetText() {
            TbNewItemTitle.Text = "Title";
        }

        public void ChangePickedButton(Border border) {
            for (int i = 0; i < this.colorBorders.Count; i++) {
                if (colorBorders[i] == border)
                {
                    colorBorders[i].BorderThickness = new Thickness(3);
                    colorBorders[i].BorderBrush = new SolidColorBrush(Colors.White);
                }
                else {
                    colorBorders[i].BorderThickness = new Thickness(0);
                }
            }
        }

        private void BtOrange_Click(object sender, RoutedEventArgs e)
        {
            pickedColor = colorOrange;
            ChangePickedButton(BorderOrangeColor);
        }

        private void BtRed_Click(object sender, RoutedEventArgs e)
        {
            pickedColor = colorRed;
            ChangePickedButton(BorderRedColor);
        }

        private void BtYellow_Click(object sender, RoutedEventArgs e)
        {
            pickedColor = colorYellow;
            ChangePickedButton(BorderYellowColor);
        }

        private void BtGreen_Click(object sender, RoutedEventArgs e)
        {
            pickedColor = colorGreen;
            ChangePickedButton(BorderGreenColor);
        }

        private void BtBlue_Click(object sender, RoutedEventArgs e)
        {
            pickedColor = colorBlue;
            ChangePickedButton(BorderBlueColor);
        }

        private void BtPurple_Click(object sender, RoutedEventArgs e)
        {
            pickedColor = colorPurple;
            ChangePickedButton(BorderPurpleColor);
        }

        private void BrOutside_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Editing == true && e.OriginalSource == sender)
            {
                this.Editing = false;
                SetToAdd();
            }
        }

    }
}
