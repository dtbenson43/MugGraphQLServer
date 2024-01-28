using Mug.Services.CosmosDb.Models.ChooseGame;

namespace Mug.Utilities
{
    public static class ChooseGameUtilities
    {
        public static ChooseGameBranch? GetNextBranch(ChooseGame game, ChooseGameBranch? currentBranch)
        {
            if (currentBranch == null) return game.InitialBranch;

            foreach (var branch in game.Branches)
            {
                var nextBranchId = currentBranch.UserChoice == UserChoiceOption.FirstOption
                    ? currentBranch.FirstOption.NextBranchId
                    : currentBranch.SecondOption.NextBranchId;
                if (branch.Id == nextBranchId) return branch;
            }
            return null;
        }

        public static Boolean GetNextBranch(ChooseGame game, ChooseGameBranch? currentBranch, out ChooseGameBranch? nextBranch)
        {
            nextBranch = GetNextBranch(game, currentBranch);
            if (nextBranch == null) return false;
            return true;
        }

        public static ChoiceOption? GetUserChoice(ChooseGameBranch branch)
        {
            if (branch.UserChoice == null) return null;
            return branch.UserChoice == UserChoiceOption.FirstOption
                ? branch.FirstOption
                : branch.SecondOption;
        }

        public static ChoiceOption? GetOptionById(ChooseGameBranch branch, string id)
        {
            if (branch.FirstOption.NextBranchId == id) return branch.FirstOption;
            if (branch.SecondOption.NextBranchId == id) return branch.SecondOption;
            return null;
        }

        public static UserChoiceOption? GetUserChoiceOptionById(ChooseGameBranch branch, string id)
        {
            if (branch.FirstOption.NextBranchId == id) return UserChoiceOption.FirstOption;
            if (branch.SecondOption.NextBranchId == id) return UserChoiceOption.SecondOption;
            return null;
        }

        public static ChooseGameBranch? GetBranchById(ChooseGame game, string branchId)
        {
            foreach (var branch in game.Branches)
            {
                if (branch.Id == branchId) return branch;
            }
            return null;
        }
    }
}
