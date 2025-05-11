using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPresenter : IMoneySetting
{
    private Money data;
    private MoneyView view;

    public MoneyPresenter(Money data, MoneyView view)
    {
        this.data = data;
        this.view = view;

        view.UpdateText(data.TotalMoney);
    }

    public void Reset()
    {
        data.ReSetTotalMoney();

        view.UpdateText(data.TotalMoney);
    }

    public void Add(int value)
    {
        data.AddMoney(value);

        view.UpdateText(data.TotalMoney);
        view.UpdatecashOutText(value);
    }

    public void Remove(int value)
    {
        data.MinusMoney(value);
        view.UpdateText(data.TotalMoney);
    }

    public int GetMoney() => data.TotalMoney;
}

