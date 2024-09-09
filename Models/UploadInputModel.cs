namespace BPMPlus.ViewModels
{
    public class UploadInputModel
    {
        public string Tel { get; set; }

        public string Content { get; set; }

        public DateTime Enddate { get; set; } = DateTime.Now;

        public string Id { get; set; }

        public List<IFormFile> Files { get; set; } = null;


    }
}
