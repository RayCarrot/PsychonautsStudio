using System.Windows.Controls;
using System.Windows.Input;

namespace PsychonautsTools
{
    /// <summary>
    /// Interaction logic for DataNodeUI_GameTexture.xaml
    /// </summary>
    public partial class DataNodeUI_GameTexture : UserControl
    {
        public DataNodeUI_GameTexture()
        {
            InitializeComponent();
        }

        public DataNode_GameTextureViewModel? ViewModel
        {
            get => DataContext as DataNode_GameTextureViewModel;
            set => DataContext = value;
        }

        private void Frames_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel != null)
                ViewModel.SelectedFrame = null;
        }
    }
}