using DreamQuiz.Player;
using System;
using System.Collections.Generic;

public static class AnalyticsHelper
{
    public static TDto Map<TModel, TDto>(TModel model) where TDto : new()
    {
        TDto dto = new TDto();

        foreach (var modelProp in typeof(TModel).GetProperties())
        {
            var dtoProp = typeof(TDto).GetProperty(modelProp.Name);

            if (dtoProp != null && dtoProp.PropertyType.IsAssignableFrom(modelProp.PropertyType))
            {
                dtoProp.SetValue(dto, modelProp.GetValue(model));
            }
        }

        return dto;
    }

    //TODO: Fetch data relationship from server
    //https://ocarinastudios.atlassian.net/browse/DQG-2091
    public static char ParseResult(PlayerStageGoal.PlayerStageGoalState playerStageGoalState)
    {
        char parsed = 'Q';

        switch (playerStageGoalState)
        {
            case PlayerStageGoal.PlayerStageGoalState.Lose:
                parsed = 'L';
                break;
            case PlayerStageGoal.PlayerStageGoalState.Win:
                parsed = 'W';
                break;
        }

        return parsed;
    }

    //TODO: Fetch data relationship from server
    //https://ocarinastudios.atlassian.net/browse/DQG-2091
    public static List<int> ParseTeam(List<Collectible> team)
    {
        List<int> parsed = new List<int>();

        foreach (var collectible in team)
        {
            parsed.Add(Convert.ToInt32(collectible.Data.Type));
        }

        return parsed;
    }
}