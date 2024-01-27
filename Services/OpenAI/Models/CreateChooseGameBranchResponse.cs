namespace Mug.Services.OpenAI.Models
{
    public class CreateChooseGameBranchResponse
    {
        public string Text { get; set; } = null!;

        public string FirstOption { get; set; } = null!;

        public string SecondOption { get; set; } = null!;
    }
}
