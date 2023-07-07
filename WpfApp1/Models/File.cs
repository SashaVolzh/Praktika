

namespace WpfApp1.Models;

internal class File
{
    public int FileSize { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public byte[] FileData { get; set; }
    public object ID { get; set; }
    public string UploadedBy { get; set; }
    public DateTime UploadedAt { get; set;}
}
