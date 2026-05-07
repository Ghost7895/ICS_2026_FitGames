using FitGames.app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitGames.app.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAsync(string title, string message)
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page is null)
        {
            return;
        }

        await page.DisplayAlertAsync(title, message, "OK");
    }
}
