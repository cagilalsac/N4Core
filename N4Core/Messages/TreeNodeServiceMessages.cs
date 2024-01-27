using N4Core.Enums;

namespace N4Core.Messages
{
    public class TreeNodeServiceMessages : ServiceMessages
    {
        public string NodeDetailFound { get; set; }
        public string NodeDetailTextRequired { get; set; }
        public string NodeNameRequired { get; set; }
        public string NodeFound { get; set; }
        public string NodeNotFound { get; set; }
        public string NodeDetailNotFound { get; set; }
        public string NoUpdateForRootNodeActive { get; set; }
        public string NoDeleteForRootNode { get; set; }
        public string NoDeleteForNodeDetailHasNodes { get; set; }
        public string NodeDetailDeleted { get; set; }

        public TreeNodeServiceMessages(Languages language = Languages.English)
        {
            NodeDetailFound = language == Languages.Türkçe ? "Nod detayı bulundu." : "Node detail found.";
            NodeDetailTextRequired = language == Languages.Türkçe ? "Nod detayı yazısı zorunludur." : "Node detail text is required.";
            NodeNameRequired = language == Languages.Türkçe ? "Nod adı zorunludur." : "Node name is required.";
            NodeFound = language == Languages.Türkçe ? "Nod bulundu." : "Node found.";
            NodeNotFound = language == Languages.Türkçe ? "Nod bulunamadı." : "Node not found.";
            NodeDetailNotFound = language == Languages.Türkçe ? "Nod detayı bulunamadı." : "Node detail not found.";
            NoUpdateForRootNodeActive = language == Languages.Türkçe ? "Kök nod aktif özelliği güncellenemez." : "Root node active property cannot be updated.";
            NoDeleteForRootNode = language == Languages.Türkçe ? "Kök nod silinemez." : "Root node cannot be deleted.";
            NoDeleteForNodeDetailHasNodes = language == Languages.Türkçe ? "Nod detayı silinemez çünkü nod detayının ilişkili nodları bulunmaktadır." : "Node detail cannot be deleted because node detail has relational nodes.";
            NodeDetailDeleted = language == Languages.Türkçe ? "Nod detayı başarıyla silindi." : "Node detail deleted successfully.";
        }
    }
}
