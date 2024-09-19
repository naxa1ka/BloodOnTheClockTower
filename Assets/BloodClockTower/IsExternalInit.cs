#if UNITY_5_3_OR_NEWER && ! UNITY_2021_OR_NEWER
using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}
#endif
