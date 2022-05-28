using System.Collections.ObjectModel;
using System.Linq;
using PsychoPortal;

namespace PsychonautsTools;

public class DomainEditorViewModel : EditorViewModel
{
    public DomainEditorViewModel(Domain domain)
    {
        Domain = domain;
        Entities = new ObservableCollection<EntityViewModel>(domain.DomainEntityInfos.Select(x => new EntityViewModel(x)));
    }

    public Domain Domain { get; }
    public ObservableCollection<EntityViewModel> Entities { get; }
    public EntityViewModel? SelectedEntity { get; set; }

    public class EntityViewModel : BaseViewModel
    {
        public EntityViewModel(DomainEntityInfo entityInfo)
        {
            EntityInfo = entityInfo;
            DisplayName = entityInfo.Name;
            InfoItems = new ObservableCollection<InfoItem>()
            {
                new("Name", entityInfo.Name),
                new("Script Class", entityInfo.ScriptClass),
                new("Edit Vars", entityInfo.EditVars),
                new("Position", $"{entityInfo.Position}"),
                new("Rotation", $"{entityInfo.Rotation}"), // TODO: Normalize rotation
                new("Scale", $"{entityInfo.Scale}"),
            };
        }

        public DomainEntityInfo EntityInfo { get; }
        public string DisplayName { get; }
        public ObservableCollection<InfoItem> InfoItems { get; }
    }
}