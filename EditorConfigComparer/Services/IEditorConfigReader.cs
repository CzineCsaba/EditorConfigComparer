using EditorconfigComparer.Models;

namespace EditorconfigComparer.Services
{
    internal interface IEditorConfigReader
    {
        EditorConfig Read(string filePath);
    }
}
