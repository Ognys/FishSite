public class NewsModel {
    
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; }
    public List<NewsImgModel> ImagesNews { get; set; } = new();
    
}