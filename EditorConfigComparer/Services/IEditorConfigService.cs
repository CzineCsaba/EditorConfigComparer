using EditorConfigComparer.Models;

namespace EditorConfigComparer.Services
{
    internal interface IEditorConfigService
    {
        IEnumerable<EditorConfigRulePair> CreatePairs(EditorConfig leftConfig, EditorConfig rightConfig);
    }
}