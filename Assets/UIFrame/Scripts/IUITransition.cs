using System.Threading.Tasks;

public interface IUITransition
{
    Task PlayOpenTransitionAsync();
    Task PlayCloseTransitionAsync();
    Task PlayPauseTransitionAsync();
    Task PlayResumeTransitionAsync();
} 