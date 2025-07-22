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

namespace CrudVietSteam.View.UserControls
{
    /// <summary>
    /// Interaction logic for HintTextBox.xaml
    /// </summary>
    public partial class HintTextBox : UserControl
    {
        public HintTextBox()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Cổng gợi ý cho HintTextBox.
        /// </summary>
        public string HintText
        {
            get
            {
                return (string)GetValue(HintTextProperty);
            }
            set
            {
                SetValue(HintTextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //// đăng kí HintText trên wpf
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(HintTextBox), new PropertyMetadata("..."));
        //Tên cổng  , Kiểu dữ liệu cổng , tên chủ của cổng này , giá trị mặc định của cổng


        /// <summary>
        /// Cổng giao tiếp cho nội dung nhập liệu của người dùng.
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc... 
        // đăng kí text trên wpf 
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HintTextBox), new PropertyMetadata(""));



    }
}
