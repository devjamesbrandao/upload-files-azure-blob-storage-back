namespace Azure.Core.Model
{
    public class ImageView
    {
        public ImageView() { }
        
        public ImageView(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set;}
        public string Url { get; set; }
    }
}