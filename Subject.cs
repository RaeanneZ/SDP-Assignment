using System.Collections.Generic;

public interface Subject
{
    void RegisterObserver(Observer observer);
    void RemoveObserver(Observer observer);
    void NotifyObservers(string message);
}