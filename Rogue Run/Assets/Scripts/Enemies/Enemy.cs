using System.Collections;

public interface IEnemy
{
    //applies vigilant debuff onto enemy
    public IEnumerator ApplyGraviton(int seconds);
}