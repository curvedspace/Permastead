using Avalonia;

namespace Permastead.Themes;

public interface IThemeManager
{
    void Initialize(Application application);

    void Switch(int index);
}
