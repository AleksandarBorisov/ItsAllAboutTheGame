namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class LockedOutViewModel
    {
        public LockedOutViewModel(int remainingHours)
        {
            this.RemainingHours = remainingHours;
        }

        public int RemainingHours { get; set; }
    }
}
