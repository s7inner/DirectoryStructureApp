using System.ComponentModel.DataAnnotations.Schema;

namespace DirectoryStructureApp.Models
{
    public class MyCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MyCatalogId { get; set; }

        [ForeignKey("MyCatalogId")]
        public List<MyCatalog> Children { get; set; }
    }
}
