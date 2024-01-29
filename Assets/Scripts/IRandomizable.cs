using System.Collections.Generic;

public interface IRandomizable
{
    public float GetChance();
    public void ChangeChance(float value);

    public float GetChanceStep();

    public int GetIndex();

    public List<string> GetCorrectIndex();
}
