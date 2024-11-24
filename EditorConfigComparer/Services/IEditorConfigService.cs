using EditorconfigComparer.Models;

namespace EditorconfigComparer.Services
{
    internal interface IEditorConfigService
    {
        IEnumerable<EditorConfigRulePair> CreatePairs(EditorConfig leftConfig, EditorConfig rightConfig);
    }
}