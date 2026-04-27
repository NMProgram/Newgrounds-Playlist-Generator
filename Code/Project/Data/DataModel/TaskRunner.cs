using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

public record Step(string Key, Func<object> Action, Func<object, string>? Formatter = null);
[ExcludeFromCodeCoverage]
public class TaskRunner(params List<Step> steps)
{
    record StepResult(string Key, object Value, Func<object, string>? Formatter = null);
    readonly List<Step> _actions = steps;
    static string GetFormattedStep(StepResult x)
        => x is null ? "" : $"{x.Key}: {x.Formatter?.Invoke(x.Value).Bold() ?? x.Value.ToString()?.Bold()}";
    static void PrintCurrentSteps(StepResult[] results, bool print, int index)
    {
        if (print && index > 0)
        {
            IEnumerable<string> res = results.Select(GetFormattedStep).Take(index);
            Console.WriteLine(string.Join("; ", res));
            
        }
    }
    public object[] RunTasks(bool print = true)
    {
        int index = 0;
        StepResult[] results = new StepResult[_actions.Count];
        while (index < _actions.Count)
        {
            PrintCurrentSteps(results, print, index);
            (string Key, Func<object> Action, Func<object, string>? Formatter) = _actions[index];
            if (results.Length != _actions.Count) { Array.Resize(ref results, _actions.Count); }
            try { results[index] = new(Key, Action(), Formatter); index++; }
            catch (TracebackException)
            {
                Console.Clear();
                if (index > 0) { index--; }
                else { throw new ReturnedException(""); }
            }
            catch (ReturnedException) {}
        }
        return [.. results.Select(x => x.Value)];
    }
    public TaskRunner Add(Step step) 
    {
        if (_actions.Contains(step)) { return this; }
        _actions.Add(step);
        return this;
    }
    public TaskRunner Add(string key, Func<object> action, Func<object, string>? format = null)
    {
        Add(new Step(key, action, format));
        return this;
    }
}