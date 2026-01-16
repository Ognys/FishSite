public class NewsImgModel
{
    public int Id { get; set; }
    public string ImagePath { get; set; }
    public int NewsModelId { get; set; }
    public NewsModel News { get; set; }
}