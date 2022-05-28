using System.Collections.ObjectModel;
using System.Linq;
using PsychoPortal;

namespace PsychonautsTools;

public class SceneEditorViewModel : EditorViewModel
{
    public SceneEditorViewModel(Scene scene)
    {
        Scene = scene;
        Textures = new ObservableCollection<string>(scene.TextureTranslationTable.Select(x => x.FileName.Value));
    }

    public Scene Scene { get; }
    public ObservableCollection<string> Textures { get; }
}