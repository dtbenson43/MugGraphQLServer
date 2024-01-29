namespace Mug.Services.OpenAI.Models
{
    public class ChooseGameBranchData
    {
        public string Text { get; set; } = null!;

        public string FirstOption { get; set; } = null!;

        public string SecondOption { get; set; } = null!;
    }
}
