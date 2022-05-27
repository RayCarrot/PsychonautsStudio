using System.Windows.Controls;

namespace PsychonautsTools
{
    /// <summary>
    /// Interaction logic for DataNodeUI_Image.xaml
    /// </summary>
    public partial class DataNodeUI_Image : UserControl
    {
        public DataNodeUI_Image()
        {
            InitializeComponent();
        }

        public DataNode_ImageViewModel? ViewModel
        {
            get => DataContext as DataNode_ImageViewModel;
            set => DataContext = value;
        }
    }
}