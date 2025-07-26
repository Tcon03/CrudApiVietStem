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
    /// Interaction logic for HeaderHintTbl.xaml
    /// </summary>
    public partial class HeaderHintTbl : UserControl
    {
        public HeaderHintTbl()
        {
            InitializeComponent();
        }



        // SỬA LẠI FILE .XAML.CS CỦA BẠN NHƯ SAU:

        // Sửa tên property từ "Lablel" thành "Label"
        public string TextHeader
        {
            get { return (string)GetValue(TextHeaderProperty); }
            set { SetValue(TextHeaderProperty, value); }
        }

        public static readonly DependencyProperty TextHeaderProperty =
            // Dùng nameof(Label) để đảm bảo an toàn, thay vì chuỗi "Label"
            DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(HeaderHintTbl), new PropertyMetadata("Default Title"));

        // --- Các property khác giữ nguyên ---

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register(nameof(HintText), typeof(string), typeof(HeaderHintTbl), new PropertyMetadata("Default hint..."));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HeaderHintTbl), new PropertyMetadata(""));


    
    }
}
