namespace Projeto_Final.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string Nick { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Game { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Nick} - {FirstName} - {LastName} - {Game}";
        }

        public Player Clone()
        {
            return (Player)MemberwiseClone();
        }
    }
}
