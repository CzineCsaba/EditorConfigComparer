using EditorConfigComparer.Models;

namespace EditorConfigComparer.Services
{
    internal interface IEditorConfigReader
    {
        EditorConfig Read(string filePath);
    }
}
