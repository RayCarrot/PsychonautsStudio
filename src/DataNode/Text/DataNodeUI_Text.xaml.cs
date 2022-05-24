using System.Windows.Controls;

namespace PsychonautsTools
{
    /// <summary>
    /// Interaction logic for DataNodeUI_Text.xaml
    /// </summary>
    public partial class DataNodeUI_Text : UserControl
    {
        public DataNodeUI_Text()
        {
            InitializeComponent();
        }

        public DataNode_TextViewModel? ViewModel
        {
            get => DataContext as DataNode_TextViewModel;
            set => DataContext = value;
        }
    }
}